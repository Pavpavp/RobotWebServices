using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.RobotWareServices.StateData
{
    public class GetRobtargetState
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string CF1 { get; set; }
        public string CF4 { get; set; }
        public string CF6 { get; set; }
        public string CFX { get; set; }
        public string EAXa { get; set; }
        public string EAXb { get; set; }
        public string EAXc { get; set; }
        public string EAXd { get; set; }
        public string EAXe { get; set; }
        public string EAXf { get; set; }
    }

}
