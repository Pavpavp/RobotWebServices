using RWS;
using RWS.Data;
using RWS.SubscriptionServices;
using System;
using System.Linq;
using Zeroconf;
using System.Threading.Tasks;

namespace Test
{
    class TestConsole
    {
        static void Main(string[] args)
        {
            ResolveBonjourServiceConitnous();
            Console.ReadKey();
        }

        public static void ResolveBonjourServiceConitnous(){
             var sub = ZeroconfResolver.ResolveContinuous("_http._tcp.local.");
            var listenSubscription = sub.Subscribe(resp => Console.WriteLine(resp.ToString()));
            
            var sub2 = ZeroconfResolver.ResolveContinuous("rws._sub._http._tcp.local.");
            var listenSubscription2 = sub2.Subscribe(resp => {
                Console.WriteLine(resp.ToString() + string.Join(",",resp.Services.Select(s => s.Value)));
            });
        }


    }
}
