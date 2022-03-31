using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JourneyPlanner.Models.RTT
{
    //File Generated by quicktype.io

    public partial class RttService
    {
        public RttService()
        {
            
        }
        [JsonProperty("serviceUid")] public string ServiceUid { get; set; }

        [JsonProperty("runDate")] public DateTimeOffset RunDate { get; set; }

        [JsonProperty("serviceType")] public string ServiceType { get; set; }

        [JsonProperty("isPassenger")] public bool IsPassenger { get; set; }

        [JsonProperty("trainIdentity")] public string TrainIdentity { get; set; }

        [JsonProperty("powerType")] public string PowerType { get; set; }

        [JsonProperty("trainClass")] public string TrainClass { get; set; }

        [JsonProperty("atocCode")] public string AtocCode { get; set; }

        [JsonProperty("atocName")] public string AtocName { get; set; }

        [JsonProperty("performanceMonitored")] public bool PerformanceMonitored { get; set; }

        [JsonProperty("origin")] public List<Destination> Origin { get; set; }

        [JsonProperty("destination")] public List<Destination> Destination { get; set; }

        [JsonProperty("locations")] public Location[] Locations { get; set; }

        [JsonProperty("realtimeActivated")] public bool RealtimeActivated { get; set; }

        [JsonProperty("runningIdentity")] public string RunningIdentity { get; set; }
        
        //For some request with many services, only basic data is provided 
        [JsonProperty("locationDetail")] public Location LocationDetail { get; set; }
    }

    public partial class Destination
    {
        [JsonProperty("tiploc")] public string Tiploc { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("workingTime")] public long WorkingTime { get; set; }

        [JsonProperty("publicTime")] public long PublicTime { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("realtimeActivated")] public bool RealtimeActivated { get; set; }

        [JsonProperty("tiploc")] public string Tiploc { get; set; }

        [JsonProperty("crs")] public string Crs { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("gbttBookedDeparture", NullValueHandling = NullValueHandling.Ignore)]
        public long? GbttBookedDeparture { get; set; }

        [JsonProperty("origin")] public List<Destination> Origin { get; set; }

        [JsonProperty("destination")] public List<Destination> Destination { get; set; }

        [JsonProperty("isCall")] public bool IsCall { get; set; }

        [JsonProperty("isPublicCall")] public bool IsPublicCall { get; set; }

        [JsonProperty("realtimeDeparture", NullValueHandling = NullValueHandling.Ignore)]
        public long? RealtimeDeparture { get; set; }

        [JsonProperty("realtimeDepartureActual", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RealtimeDepartureActual { get; set; }

        [JsonProperty("realtimeGbttDepartureLateness")]
        public long? RealtimeGbttDepartureLateness { get; set; }

        [JsonProperty("platform")] public long Platform { get; set; }

        [JsonProperty("platformConfirmed")] public bool PlatformConfirmed { get; set; }

        [JsonProperty("platformChanged")] public bool PlatformChanged { get; set; }

        [JsonProperty("line", NullValueHandling = NullValueHandling.Ignore)]
        public string Line { get; set; }

        [JsonProperty("lineConfirmed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? LineConfirmed { get; set; }

        [JsonProperty("displayAs")] public string DisplayAs { get; set; }

        [JsonProperty("gbttBookedArrival", NullValueHandling = NullValueHandling.Ignore)]
        public long? GbttBookedArrival { get; set; }

        [JsonProperty("realtimeArrival", NullValueHandling = NullValueHandling.Ignore)]
        public long? RealtimeArrival { get; set; }

        [JsonProperty("realtimeArrivalActual", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RealtimeArrivalActual { get; set; }

        [JsonProperty("realtimeGbttArrivalLateness")]
        public long? RealtimeGbttArrivalLateness { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("pathConfirmed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PathConfirmed { get; set; }

        [JsonProperty("associations", NullValueHandling = NullValueHandling.Ignore)]
        public Association[] Associations { get; set; }
    }

    public partial class Association
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("associatedUid")] public string AssociatedUid { get; set; }

        [JsonProperty("associatedRunDate")] public string AssociatedRunDate { get; set; }
    }
}
    