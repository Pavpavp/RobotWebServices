using Newtonsoft.Json;

namespace RWS.IRC5.ResponseTypes
{
    public class TimeZoneResourceState
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string TimeZone { get; set; }
    }

}
