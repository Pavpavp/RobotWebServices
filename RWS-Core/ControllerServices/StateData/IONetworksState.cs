using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RWS.Data
{


    public class IONetworksState
    {

        [JsonProperty(PropertyName = "_links")]
        public _Links1 Links { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }


        public string Name { get; set; }

        public string PState { get; set; }
    }



}




