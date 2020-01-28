using Newtonsoft.Json;

namespace RWS.IRC5.RobotWareServices.StateTypes
{

    public class GetJointTargetState
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string RAX_1 { get; set; }
        public string RAX_2 { get; set; }
        public string RAX_3 { get; set; }
        public string RAX_4 { get; set; }
        public string RAX_5 { get; set; }
        public string RAX_6 { get; set; }
        public string EAX_a { get; set; }
        public string EAX_b { get; set; }
        public string EAX_c { get; set; }
        public string EAX_d { get; set; }
        public string EAX_e { get; set; }
        public string EAX_f { get; set; }
    }




}
