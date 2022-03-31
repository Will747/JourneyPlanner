using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.RTT;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace JourneyPlanner.Services
{
    // Used for getting data from the RealTimeTrains API
    public class RttService : IRealtimeService
    {
        private readonly RestClient _client;
        private readonly IStationService _stationService;

        public RttService(IStationService stationService, IConfiguration configuration)
        {
            _stationService = stationService;
            
            // Get RTT api authentication from the config file
            var rttUser = configuration.GetConnectionString("RttAPIUser");
            var rttPassword = configuration.GetConnectionString("RttAPIKey");;
            
            // Setup the api client so its ready to make requests
            _client = new RestClient("https://api.rtt.io/api/v1/json/")
            {
                Authenticator =
                    new HttpBasicAuthenticator(rttUser, rttPassword)
            };
        }

        // Returns the main values from a train service request
        public async Task<Service> GetServiceInfo(string rsid)
        {
            // Get the full service
            var service = await GetService(rsid);
            
            // Reformat to a simpler model with a more accessible data structure
            return ConvertToService(service);
        }

        // Converts an RTT location to the custom stop model
        private Stop CreateStop(Location stop)
        {
            var station = _stationService.GetStation(stop.Crs) as Stop;
            if (station != null)
            {
                station.Platform = stop.Platform;
                station.RealtimeArrivalTime = stop.RealtimeArrival;
                station.RealtimeDepartureTime = stop.RealtimeDeparture;
                station.PlatformChanged = stop.PlatformChanged;
                station.PlatformConfirmed = stop.PlatformConfirmed;
                return station;
            }

            return null;
        }

        // Converts an RTT service to the custom service model
        private Service ConvertToService(Models.RTT.RttService service)
        {
            var stops = new Stop[service.Locations.Length];
            for(var i = 0; i < service.Locations.Length; i++)
            {
                stops[i] = CreateStop(service.Locations[i]);
            }

            var outService = ConvertToBasicService(service) as Service;
            if (outService == null) return null;
            
            outService.Stops = stops;
            outService.PowerType = service.PowerType;
            return outService;
        }

        // Converts an Rtt service to a basic service model containing an overview of the service
        private BasicService ConvertToBasicService(Models.RTT.RttService service)
        {
            //Reformat departure time to a user readable string
            string departTime;
            if (service.LocationDetail.GbttBookedDeparture.ToString().Length == 2)
            {
                // If GbttBookedDeparture only contains two characters it's the number of minutes past mid-night
                departTime = "00:" + Convert.ToString(service.LocationDetail.GbttBookedDeparture)?.Substring(0, 2);
            }
            else
            {
                // Adds a colon between the hours and minutes
                departTime = Convert.ToString(service.LocationDetail.GbttBookedDeparture)?.Substring(0, 2) + ':' 
                    + Convert.ToString(service.LocationDetail.GbttBookedDeparture)?.Substring(2);
            }
            
            
            return new BasicService
            {
                ServiceUid = service.ServiceUid, 
                TrainId = service.TrainIdentity,
                AtocName = service.AtocName, 
                DepartureTime = departTime,
                Platform = Convert.ToInt32(service.LocationDetail.Platform),
                Origin = service.LocationDetail.Origin[0].Description,
                Destination = service.LocationDetail.Destination[0].Description
            };
        }

        // Returns any services that have stops at the two stations provided
        public async Task<BasicService[]> GetDirectServices(string startCrs, string endCrs)
        {
            var services = new List<BasicService>();
            
            // Create api request
            var request = new RestRequest("/search/" + startCrs + "/to/" + endCrs, Method.GET, DataFormat.Json);
            var response = await _client.GetAsync<RttServiceList>(request);

            if (response.Services != null)
            {
                // Convert all RTT services to basic services and add them to the list
                services.AddRange(response.Services.Select(ConvertToBasicService));
            }
            
            return services.ToArray();
        }
        
        // Returns a specific service based on its rsid code
        private async Task<Models.RTT.RttService> GetService(string rsid)
        {
            // Get the date and form parameters string
            var currentDate = DateTime.Now;
            var parameters = "service/" + rsid + currentDate.Date;
            
            // Make and return the request
            var request = new RestRequest(parameters, DataFormat.Json);
            return await _client.GetAsync<Models.RTT.RttService>(request);
        }
    }
}
