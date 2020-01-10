using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.RobotWareServices.StateData
{


    public class SystemInformationState7
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Build { get; set; }
        public string Revision { get; set; }
        public string Subrevision { get; set; }
        public string BuildTag { get; set; }
        public string RobApiCompatibilityRevision { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string RwVersion { get; set; }
        public string SysId { get; set; }
        public string StartTm { get; set; }
        public string RWVersionName { get; set; }
    }








}
