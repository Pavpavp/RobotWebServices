using Newtonsoft.Json;

namespace RWS.IRC5.UserServices.StateData
{
    public class GetRmmpState
    {

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public long UserID { get; set; }
        public string Alias { get; set; }
        public string Location { get; set; }
        public string Application { get; set; }
        public string Privilege { get; set; }
        public bool RmmpHeldByMe { get; set; }

    }



}
