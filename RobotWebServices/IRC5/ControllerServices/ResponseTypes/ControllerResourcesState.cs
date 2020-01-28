using Newtonsoft.Json;

namespace RWS.IRC5.ResponseTypes
{

    public class ControllerResourcesState
    {
        [JsonProperty(PropertyName = "_links")]
        public _Links1 _links { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        public string DateTime { get; set; }

        [JsonProperty(PropertyName = "ctrl-name")]
        public string ControllerName { get; set; }

        [JsonProperty(PropertyName = "ctrl-id")]
        public string ControllerID { get; set; }

    }


}
