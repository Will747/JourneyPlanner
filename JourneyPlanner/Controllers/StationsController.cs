using System.Collections.Generic;
using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.User;
using JourneyPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace JourneyPlanner.Controllers
{
    // Controller for providing information on stations and routes between them
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/stations")]
    public class StationsController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly ITimetableService _timetableService;
        
        public StationsController(IStationService stationService, ITimetableService timetableService)
        {
            _stationService = stationService;
            _timetableService = timetableService;
        }

        // GET /api/v1/stations
        // Returns all the stations in the UK
        [HttpGet]
        public Station[] GetStations()
        {
            return _stationService.GetAll();
        }
        
        // GET /api/v1/stations/station/crs
        // Returns a specific station based on its three letter code
        [HttpGet("station/{crs}")]
        public Station GetStation(string crs)
        {
            return _stationService.GetStation(crs);
        }

        // GET /api/v1/stations/search/{data}
        // Searches for any stations containing the input in their name or crs
        [HttpGet("search/{value}")]
        public async Task<ActionResult<List<Station>>> GetStationsSearch(string value)
        {
            return await _stationService.Search(value);
        }

        // POST /api/v1/stations/route
        // Returns a path of stations to get between the input stations
        [HttpPost("route")]
        public Path[] GetRoutes([FromBody] Station[] input)
        {
            var paths = new List<Path>();
            
            // Gets a path between each station in the array
            for (var i = 1; i < input.Length; i++)
            {
                if (input[i] != null && input[i].Name != null)
                {
                    paths.Add(_stationService.GetRoute(input[i - 1], input[i]));
                }
            }
            return paths.ToArray();
        }
        
        // GET /api/v1/stations/lines
        // Returns details of one section of track between two stations
        [HttpGet("lines")]
        public List<Line> GetLines()
        {
            return _stationService.GetAllLines();
        }
        
        // POST /api/v1/stations/distance
        // Returns the direct distance between two stations in kilometers
        [HttpPost("distance")]
        public int GetDistance([FromBody] Station[] stations)
        {
            return stations.Length < 2 ? -1 : _stationService.GetDistanceBetween(stations[0], stations[1]);
        }
        
        // POST /api/v1/stations/directService
        // Returns any direct services between the start and end stations in the route
        [HttpPost("directService")]
        public async Task<BasicService[]> GetDirect([FromBody] UserRoute input)
        {
            return await _timetableService.GetDirectRoutes(input);
        }
    }
}
