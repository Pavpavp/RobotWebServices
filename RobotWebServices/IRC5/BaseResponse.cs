using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.IRC5.ResponseTypes
{


    public class TransportRwsResponse
    {
        public TransportRwsResponse()
        {
        }

        public TransportRwsResponse(IRwsResponse response)
        {
            Items = response.Items?.Select(item => new TransportRwsResponseItem(item)).ToList();
            Error = (response.Error != null) ? new TransportRwsError(response.Error) : null;
            Location = response.Location;
        }

        public List<TransportRwsResponseItem> Items { get; set; }
        public TransportRwsError Error { get; set; }
        public string Location { get; set; }
    }



    public class TransportRwsResponseItem
    {
        public TransportRwsResponseItem()
        {
        }

        public TransportRwsResponseItem(IRwsResponseItem item)
        {
            Title = item.Title;
            Type = item.Type;
            SubItems = item.SubItems?.Select(i => new TransportRwsResponseSubItem(i)).ToList();
            Data = item.Data?.Select(kv => new TransportRwsResponseDataItem
            {
                Key = kv.Key,
                Values = (kv.Value is List<string> list) ? list : new List<string> { kv.Value as string }
            }).ToList();
            Links = item.Links?.ToList();
        }

        public string Title { get; set; }
        public string Type { get; set; }
        public List<TransportRwsResponseSubItem> SubItems { get; set; }
        public List<TransportRwsResponseDataItem> Data { get; set; }
        public List<RwsResponseLink> Links { get; set; }
    }





    public class RwsResponseLink
    {
        public string Href;
        public string Rel;
    }

    public interface IRwsResponseItem
    {
        IReadOnlyDictionary<string, object> Data { get; }
        string this[string key] { get; }
        string Title { get; }
        string Type { get; }
        IList<IRwsResponseItem> SubItems { get; } // usually null

        IEnumerable<RwsResponseLink> Links { get; }

        bool ThrowForMissingData { get; set; }
    }

    public interface IRwsResponse
    {
        IReadOnlyList<IRwsResponseItem> Items { get; }
        IRwsError Error { get; }
        string Location { get; }
    }

    public interface IRwsResponseRaw
    {
        //Task<Stream> GetStreamAsync();
        Task<byte[]> GetBytesAsync(int numBytes = -1);
        Task<string> GetTextAsync();
        IRwsError Error { get; }
    }

    public interface IRwsError
    {
        //HttpStatusCode HttpStatusCode { get; }
        //RobHResult ErrorCode { get; }
        string Message { get; }
    }


    public class TransportRwsResponseSubItem
    {
        public TransportRwsResponseSubItem()
        {
        }

        public TransportRwsResponseSubItem(IRwsResponseItem item)
        {
            Title = item.Title;
            Type = item.Type;
            Data = item.Data?.Select(kv => new TransportRwsResponseDataItem
            {
                Key = kv.Key,
                Values = (kv.Value is List<string> list) ? list : new List<string> { kv.Value as string }
            }).ToList();
            Links = item.Links?.ToList();
        }

        public string Title { get; set; }
        public string Type { get; set; }
        public List<TransportRwsResponseDataItem> Data { get; set; }
        public List<RwsResponseLink> Links { get; set; }
    }



    public class TransportRwsResponseDataItem
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }



    public class TransportRwsError
    {
        public TransportRwsError()
        {
        }

        public TransportRwsError(IRwsError error)
        {
            //HttpStatusCode = error.HttpStatusCode;
            //ErrorCode = error.ErrorCode;
            Message = error.Message;
        }

        //public HttpStatusCode HttpStatusCode { get; set; }
        //public RobHResult ErrorCode { get; set; }
        public string Message { get; set; }
    }






































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
