using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.Data
{

    public class ClockResourceState
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string DateTime { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public _Links1 Links { get; set; }
    }


}
