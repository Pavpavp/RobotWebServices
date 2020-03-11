using Newtonsoft.Json;
using RWS.IRC5.SubscriptionServices;
using RWS.OmniCore.ResponseTypes;
using System.Threading.Tasks;

namespace RWS.OmniCore.ResponseTypes
{
    public class IOSignalsResource : SubscriptionEventHelper<IOEventArgs, int>
    {
        private int _lvalue;


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
        public _LinksRes7 Links
        {
            get;

            set;
        }

        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public int LValue
        {
            get
            {
                return _lvalue;
            }

            set
            {
                if (Cs != null) 
                    UpdateSignal(Name, value);

                _lvalue = value;
            }
        }

        private async void UpdateSignal(string signal, int value)
        {
            await Cs.RobotWareService.UpdateIOSignalValueAsync(signal, value.ToString());
        }

        public string LState { get; set; }


    }





}
