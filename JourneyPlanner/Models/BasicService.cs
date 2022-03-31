namespace JourneyPlanner.Models
{
    // Overview of service information without going into details
    // of where the service stops or power type
    public class BasicService
    {
        public string ServiceUid { get; set; }
        public string TrainId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string AtocName { get; set; }
        public int Platform { get; set; }
        public string DepartureTime { get; set; }
    }
}