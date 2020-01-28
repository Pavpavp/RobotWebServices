using Newtonsoft.Json;
using RWS.IRC5.SubscriptionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RWS.IRC5.Data
{
    public class IOSignalsState : SubscriptionEventHelper<IOEventArgs, int>
    {

        public event ValueChangedIOEventHandler OnValueChanged
        {
            add
            {
                var ioEventArgs = new IOEventArgs();

                ValueChangedEventHandler += value;
                StartSubscriptionAsync(ControllerSession, $"/rw/iosystem/{Links.Self.Href}".Replace("?json=1", ";state"), ioEventArgs);
            }
            remove
            {
                SubscriptionSockets[value].Abort();
                SubscriptionSockets.Remove(value);


                ValueChangedEventHandler -= value;

            }
        }
        //public ControllerSession ControllerSession { get; set; }

        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public _Links1 Links { get; set; }
        public string Name { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        public string Category { get; set; }
        public int LValue { get; set; }
        public string LState { get; set; }

    }

}
