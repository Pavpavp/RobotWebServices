using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.Data
{
    public class BaseResponseTemp<TRes, TState>
    {
        [JsonProperty(PropertyName = "_links")]
        public Links Links { get; set; }

        [JsonProperty(PropertyName = "_embedded")]
        public _Embedded<TState> Embedded { get; set; }
    }

    public class BaseResponse<TState>
    {
        [JsonProperty(PropertyName = "_links")]
        public Links Links { get; set; }

        [JsonProperty(PropertyName = "_embedded")]
        public _Embedded<TState> Embedded { get; set; }
    }

    public class Links
    {
        public Base Base { get; set; }
    }

    public class Base
    {
        public string Href { get; set; }
    }

    public class _Embedded<T>
    {
        public Status Status { get; set; }

        [JsonProperty(PropertyName = "_state")]
        public T[] State { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
    }

    public class Devices
    {
        public string Href { get; set; }
    }

    public class Self
    {
        public string Href { get; set; }
    }


    public class _Links1
    {
        public Self Self { get; set; }
        public Devices Devices { get; set; }
    }





}
