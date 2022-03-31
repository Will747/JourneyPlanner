using System;
using Newtonsoft.Json;

//This is the data structure of the API request for each station
//Generated from quicktype.io
namespace JourneyPlanner.Models
{
    public partial class StationApi
        {
            [JsonProperty("request_time")]
            public DateTimeOffset RequestTime { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("acknowledgements")]
            public string Acknowledgements { get; set; }

            [JsonProperty("member")]
            public Member[] Member { get; set; }
        }

        public partial class Member
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            [JsonProperty("accuracy")]
            public long Accuracy { get; set; }

            [JsonProperty("station_code")]
            public string StationCode { get; set; }

            [JsonProperty("tiploc_code")]
            public string TiplocCode { get; set; }
        }
}