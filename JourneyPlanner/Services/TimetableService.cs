using System.Threading.Tasks;
using JourneyPlanner.Models;
using JourneyPlanner.Models.User;

namespace JourneyPlanner.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly IRealtimeService _realtimeService;
        
        public TimetableService(IRealtimeService realtimeService)
        {
            _realtimeService = realtimeService;
        }
        
        public async Task<BasicService[]> GetDirectRoutes(UserRoute input)
        {
            return await _realtimeService.GetDirectServices(input.Stops[0].CodeName, input.Stops[1].CodeName);
        }
    }
}
