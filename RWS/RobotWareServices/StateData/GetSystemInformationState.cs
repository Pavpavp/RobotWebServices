using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.RobotWareServices.StateData
{

    public class GetSystemInformationState
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string Name { get; set; }
        public string RWVersion { get; set; }
        public string SysID { get; set; }
        public string StartTm { get; set; }
        public string RWVersionName { get; set; }
        public Option[] Options { get; set; }

    }




    public class Option
    {
        public string _type { get; set; }
        public string _title { get; set; }
        public string option { get; set; }
    }



}
