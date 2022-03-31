using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.User;

namespace JourneyPlanner.Services
{
    // Interface for getting timetable based information
    public interface ITimetableService
    {
        // Returns direct train services between the start and end stations
        public Task<BasicService[]> GetDirectRoutes(UserRoute input);
    }
}