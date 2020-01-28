using Newtonsoft.Json;
using RWS.IRC5.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.FileServices.Data
{
    public class GetDirectoryListingState
    {
        [JsonProperty(PropertyName = "_links")]
        public _Links1 Links { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "fs-cdate")]
        public string FSCDate { get; set; }

        [JsonProperty(PropertyName = "fs-mdate")]
        public string FSCMDate { get; set; }

        [JsonProperty(PropertyName = "fs-size")]
        public string FSCSize { get; set; }

        [JsonProperty(PropertyName = "fs-readonly")]
        public string FSCReadonly { get; set; }
    }


}
