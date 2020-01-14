using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.Data
{


    public class BaseResponse7<TRes, TState>
    {
        [JsonProperty(PropertyName = "_links")]
        public _Links7 Links { get; set; }

        [JsonProperty(PropertyName = "_embedded")]
        public _Embedded7<TRes> Embedded { get; set; }
        public TState[] State { get; set; }

    }

    public class _Links7
    {
        public Base7 Base { get; set; }
        public Self7 Self { get; set; }
    }

    public class _LinksRes7
    {
        public Self7 Self { get; set; }
    }

    public class Base7
    {
        public string Href { get; set; }
    }

    public class Self7
    {
        public string Href { get; set; }
    }

    public class _Embedded7<TRes>
    {
        public TRes[] Resources { get; set; }
    }

    public class Resource7
    {

        [JsonProperty(PropertyName = "_links")]
        public _LinksRes7 Links { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public Option7[] Options { get; set; }
    }


    public class Option7
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string Option { get; set; }
    }


}
