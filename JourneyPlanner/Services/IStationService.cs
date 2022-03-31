using System.Collections.Generic;
using System.Threading.Tasks;
using JourneyPlanner.Models;

namespace JourneyPlanner.Services
{
    // Interface for accessing details about stations
    public interface IStationService
    {
        // Returns all the stations in the database
        public Station[] GetAll();
        
        // Returns any stations that contain value in their name
        public Task<List<Station>> Search(string value);
        
        // Returns station with specified crs code
        public Station GetStation(string crs);
        
        // Returns all the lines between stations
        public List<Line> GetAllLines();
        
        // Returns direct distance in kilometers between two stations
        public int GetDistanceBetween(Station start, Station end);
        
        // Finds a path between two stations
        public Path GetRoute(Station start, Station end);
    }
}