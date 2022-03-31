using System.Threading.Tasks;
using JourneyPlanner.Models;

namespace JourneyPlanner.Services
{
    // Interface for getting data about current train services
    public interface IRealtimeService
    {
        // Returns the service with the specified rsid code.
        public Task<Service> GetServiceInfo(string rsid);
        
        // Returns array of all services going between the two stations
        public Task<BasicService[]> GetDirectServices(string startCrs, string endCrs);
    }
}