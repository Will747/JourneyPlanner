
namespace JourneyPlanner.Models
{
    // All service information including the stops
    public class Service : BasicService
    {
        public string PowerType { get; set; }
        public Stop[] Stops { get; set; }
    }
}
