using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.RobotWareServices.StateTypes
{
    public class SetMechUnitState
    {

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "change-count")]
        public string ChangeCount { get; set; }

        [JsonProperty(PropertyName = "mechunit-name")]
        public string MechUnitName { get; set; }

        [JsonProperty(PropertyName = "poll-rate")]
        public string Pollrate { get; set; }

        [JsonProperty(PropertyName = "modal-payload-mode")]
        public string ModalPayLoadMode { get; set; }

        [JsonProperty(PropertyName = "absacc-active")]
        public string AbsAccActive { get; set; }
        //public _Links1 _links { get; set; }

    }
}
