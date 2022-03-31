using System;

namespace JourneyPlanner.Models
{
    // Defines a stop in a train service
    public class Stop : Station
    {
        public long? RealtimeArrivalTime { get; set; }
        public long? RealtimeDepartureTime { get; set; }
        public long? Platform { get; set; }
        public bool PlatformConfirmed { get; set; }
        public bool PlatformChanged { get; set; }
    }
}
