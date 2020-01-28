using Newtonsoft.Json;
using RWS.IRC5.SubscriptionServices;
using RWS.OmniCore.ResponseTypes;

namespace RWS.OmniCore.ResponseTypes
{
    public class IOSignalsResource : SubscriptionEventHelper<IOEventArgs, int>
    {

        public event ValueChangedIOEventHandler OnValueChanged
        {
            add
            {
                var ioEventArgs = new IOEventArgs();

                ValueChangedEventHandler += value;
                StartSubscriptionAsync(Cs, $"/rw/iosystem/{Links.Self.Href};state", ioEventArgs);
            }
            remove
            {
                SubscriptionSockets[value].Abort();
                SubscriptionSockets.Remove(value);


                ValueChangedEventHandler -= value;

            }
        }

        [JsonProperty(PropertyName = "_links")]
        public _LinksRes7 Links { get; set; }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public string LValue { get; set; }
        public string LState { get; set; }
    }





}
