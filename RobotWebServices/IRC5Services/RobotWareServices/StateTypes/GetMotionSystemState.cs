using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.RobotWareServices.StateTypes
{
    public class GetMotionSystemState
    {

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        public string ChangeCount { get; set; }

        public string MechUnitName { get; set; }

        public string Pollrate { get; set; }

        public string ModalPayloadMode { get; set; }

        public string AbsAccActive { get; set; }

    }




}
