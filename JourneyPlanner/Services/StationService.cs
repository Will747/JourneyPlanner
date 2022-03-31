using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JourneyPlanner.Models;
using MySql.Data.MySqlClient;

namespace JourneyPlanner.Services
{
    public class StationService : IStationService
    {
        private readonly DatabaseService _database;
        private readonly Dictionary<string, Station> _stations;

        public StationService(DatabaseService database)
        {
            _database = database;
            _stations = new Dictionary<string, Station>();

            // Make SQL request for all the stations
            using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand("SELECT * FROM stations;", conn);
            var reader = command.ExecuteReader();
            
            // Transfer results to _stations dictionary
            while (reader.Read())
            {
                var station = ConvertToStation(reader);
                _stations.Add(station.CodeName, station);
            }
        }

        // Reads results from SQL command and creates a station from them
        private static Station ConvertToStation(IDataRecord reader)
        {
            return new Station
            {
                Name = Convert.ToString(reader["name"]), CodeName = Convert.ToString(reader["codeName"]),
                Longitude = Convert.ToDouble(reader["longitude"]), Latitude = Convert.ToDouble(reader["latitude"]), 
                LinkedStations = Convert.ToString(reader["linkedStations"])
            };
        }
        
        public Station[] GetAll()
        {
            return _stations.Select(stationPair => stationPair.Value).ToArray();
        }

        // Searches for a particular station in the database
        public async Task<List<Station>> Search(string value)
        {
            var stations = new List<Station>();
            
            // Make SQL request
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                    "SELECT * FROM stations WHERE name LIKE @name OR codeName LIKE @codeName ;", conn);
            command.Parameters.AddWithValue("@name", "%" + value + "%");
            command.Parameters.AddWithValue("@codeName", "%" + value + "%");
            var reader = await command.ExecuteReaderAsync();
            
            // Read results into a list
            while (await reader.ReadAsync())
            {
                stations.Add(ConvertToStation(reader));
            }
            
            return stations;
        }

        // Returns the station with matching crs
        public Station GetStation(string crs)
        {
            return _stations[crs];
        }

        // Returns all lines between stations in the database 
        public List<Line> GetAllLines()
        {
            var stations = GetAll();
            var completed = new List<string>();
            var lines = new List<Line>();
            
            foreach (var station in stations)
            {
                if (station.LinkedStations != "") {
                    // Go through each station this station is connected with
                    var links = station.LinkedStations.Split('.');
                    foreach (var link in links)
                    {
                        var dest= GetStation(link);

                        // Ensure a link between these two stations hasn't already been added
                        if (!completed.Exists(codeName => codeName == dest.CodeName))
                        {
                            Coordinates[] coords =
                            {
                                new Coordinates {Lat = station.Latitude, Lng = station.Longitude},
                                new Coordinates {Lat = dest.Latitude, Lng = dest.Longitude}
                            };

                            lines.Add(new Line {From = station.CodeName, To = dest.CodeName, Coordinates = coords});
                        }
                    }
                    completed.Add(station.CodeName);
                }
            }
            
            return lines;
        }
        
        // Methods used to generate the station information in the database
        // But not needed now the database has been filled
        /*
        //Used to create links between stations
        private async void CreateLink(string origin, string destination)
        {
            await using var conn = _database.GetConnection();
            //Get current links
            conn.Open();
            var command = new MySqlCommand(
                "SELECT linkedStations FROM stations WHERE codeName=@destination ;", conn);
            command.Parameters.AddWithValue("@destination", destination);
            var reader = await command.ExecuteReaderAsync();
            reader.Read();
            var currentLinks = Convert.ToString(reader["linkedStations"]);
            await conn.CloseAsync();
            
            //Update and add link to new station
            string links;
            if (currentLinks != "")
            {
                links = currentLinks + '.' + origin;
            }
            else
            {
                links = origin;
            }

            // Update the database with the new links to the station
            conn.Open();
            command = new MySqlCommand(
                "UPDATE stations SET linkedStations=@links WHERE codeName=@destination;", conn);
            command.Parameters.AddWithValue("@destination", destination);
            command.Parameters.AddWithValue("@links", links);
            await command.ExecuteNonQueryAsync();
        }
        
        // Creates a links between the stations from the array in the database
        public void Link(string[] link)
        {
            //Creates a link for both directions
            CreateLink(link[0], link[1]);
            CreateLink(link[1], link[0]);
        }

        // Used to fill the table with coordinates for each station
        public async void NoCoords()
        {
            var stations = new List<Station>();
            await using var conn = _database.GetConnection();
            conn.Open();
            var command = new MySqlCommand(
                "SELECT * FROM stations WHERE longitude='0';", conn);
                        
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                stations.Add(ConvertToStation(reader));
            }
            await conn.CloseAsync();
            
            var client = new RestClient("http://transportapi.com/v3/uk");
            for (var i = 0; i <= stations.Count; i++)
            {
                //Fetch long, lat
                var request = new RestRequest(
                    @"places.json?query=" + stations[i].CodeName + 
                    "&type=train_station&app_id=xxxxxxxxxxxxxxxxx",
                    DataFormat.Json);
                var response = await client.ExecuteGetAsync(request);
                var result = JsonConvert.DeserializeObject<StationApi>(response.Content);

                if (result.Member.Length > 0)
                {
                    for (int a = 0; a <= result.Member.Length - 1; a++)
                    {
                        if (result.Member[a].StationCode == stations[i].CodeName)
                        {
                            conn.Open();
                            command = new MySqlCommand(
                                "UPDATE stations SET longitude='" + 
                                result.Member[a].Longitude +
                                "', latitude='" + result.Member[a].Latitude + 
                                "' WHERE codeName='" + stations[i].CodeName + "' ;", conn);

                            await command.ExecuteNonQueryAsync();
                            await conn.CloseAsync();
                            Console.WriteLine("Updated " + stations[i].CodeName);
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("============");
                    Console.WriteLine(stations[i].CodeName + " NOT FOUND!");
                    Console.WriteLine("============");
                }
            }
        }
        */

        // Returns the distance between two stations in kilometers
        public int GetDistanceBetween(Station start, Station end)
        {
            // https://en.wikipedia.org/wiki/Haversine_formula
            // Haversine formula - Used to calculate the circular distance between two points

            const int earthDiameter = 12742; // 2 * 6371 (radius of earth) = 12742km
            var lat1 = start.Latitude * Math.PI / 180; // Convert to radians
            var lat2 = end.Latitude * Math.PI / 180;

            var changeInLat = (end.Latitude  - start.Latitude) * Math.PI / 180;
            var changeInLon = (end.Longitude - start.Longitude) * Math.PI / 180;

            return Convert.ToInt32(
                earthDiameter * Math.Asin(
                    Math.Sqrt(
                        Math.Pow(Math.Sin(changeInLat / 2), 2) 
                        + Math.Cos(lat1) 
                        * Math.Cos(lat2) 
                        * Math.Pow(Math.Sin(changeInLon / 2), 2)
                        )
                    )
                );
        }

        // Returns a the stations to go through to get between two stations
        public Path GetRoute(Station start, Station end)
        {
            var openStations = new List<Node>
            {
                new Node {StationCRS = start.CodeName, Score = 0, Cost = 0, ParentList = new List<string>()}
            };
            
            var closedStations = new Dictionary<string, Node>();
            var endId = end.CodeName;
            
            while (openStations.Count > 0)
            {
                // Sorts the openStations based on score
                openStations.Sort((node, node1) =>
                {
                    if (node.Score < node1.Score) return -1;
                    if (node.Score == node1.Score) return 0;
                    return 1;
                });
                
                // Adds open station with best score to closed stations
                closedStations[openStations[0].StationCRS] = openStations[0];
                
                var currentNode = openStations[0];
                var currentStation = _stations[currentNode.StationCRS];
                openStations.RemoveAt(0);
                
                // If the destination has been reached end the loop
                if (currentStation.CodeName == endId)
                {
                    break;
                }
                
                var linkedStations = currentStation.LinkedStations.Split(".");
                foreach (var linkedStation in linkedStations)
                {
                    // Calculates the cost to travel to the linked station from the current station
                    var cost = GetDistanceBetween(currentStation, _stations[linkedStation])
                               + currentNode.Cost;
                    
                    // Score based on distance from the linked station to the final destination and cost
                    var score = cost
                                    + GetDistanceBetween(_stations[linkedStation], end);
                    
                    var parentList = new List<string>(currentNode.ParentList) {currentNode.StationCRS};

                    // Ensure this linked node isn't already part of the route, if it is the path would be going backwards
                    if (linkedStation != currentNode.ParentCRS)
                    {
                        if (openStations.Exists(node => node.StationCRS == linkedStation))
                        {
                            var nodeIndex = openStations.FindIndex(node => node.StationCRS == linkedStation);
                        
                            // If the node already exists but this path offers a better score change the path on that node
                            if (openStations[nodeIndex].Score > score)
                            {
                                openStations[nodeIndex].Score = score;
                                openStations[nodeIndex].Cost = cost;
                                openStations[nodeIndex].ParentCRS = currentNode.StationCRS;
                                openStations[nodeIndex].ParentList = parentList;
                            }
                        }
                        else
                        {
                            // As this linked station hasn't been added create a new node with this path
                            openStations.Add(
                                new Node
                                {
                                    StationCRS= linkedStation, 
                                    Score = score,
                                    Cost = cost,
                                    ParentCRS = currentNode.StationCRS,
                                    ParentList = parentList
                                });
                        }   
                    }
                }
            }
            
            // Create list of stations the route goes through
            List<Station> stationRoute;
            if (closedStations[endId] != null)
            {
                stationRoute = closedStations[endId].ParentList.Select(stationId => _stations[stationId]).ToList();
                stationRoute.Add(_stations[endId]);
            }
            else
            {
                return new Path();
            }

            // Create a list of lines (edges) based on the stationRoute list.
            var lines = new List<Line>();
            for (var i = 1; i < stationRoute.Count; i++)
            {
                lines.Add(GetLine(stationRoute[i - 1], stationRoute[i]));   
            }

            // Create the completed path
            return new Path
            {
                _stations = stationRoute.ToArray(),
                lines = lines.ToArray(),
                TotalDistance = GetDistanceBetween(start, end)
            };
        }

        // Returns the coordinates of a line between two stations
        private Line GetLine(Station start, Station end)
        {
            return new Line
            {
                To = end.CodeName, 
                From = start.CodeName, 
                Distance = GetDistanceBetween(start, end),
                Coordinates = new []
                {
                    new Coordinates{ Lat = start.Latitude, Lng = start.Longitude}, 
                    new Coordinates{ Lat = end.Latitude, Lng = end.Longitude}, 
                }
            };
        }
    }
}
