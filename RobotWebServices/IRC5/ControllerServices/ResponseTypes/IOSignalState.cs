using Newtonsoft.Json;
using RWS.IRC5.ResponseTypes;

namespace RWS.IRC5.SubscriptionServices
{
    public class IOSignalState : SubscriptionEventHelper<IOEventArgs, int>
    {

        public event IOValueChangedEventHandler OnValueChanged
        {
            add
            {
                var ioEventArgs = new IOEventArgs();

                ValueChangedEventHandler += value;
                StartSubscriptionAsync(Controller, $"/rw/iosystem/{Title}".Replace("?json=1", ";state"), ioEventArgs);
            }
            remove
            {
                SubscriptionSockets[value].Abort();
                SubscriptionSockets.Remove(value);

                ValueChangedEventHandler -= value;

            }
        }

        public IRC5Session Controller { get; set; }

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
