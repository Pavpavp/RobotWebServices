using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RWS.SubscriptionServices
{
    public struct SubscriptionService
    {
        public ControllerSession Controller { get; set; }

        public SubscriptionService(ControllerSession cs)
        {
            Controller = cs;
        }

        public void StartSubscription(string resource, int prio)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(Controller.UAS.User, Controller.UAS.Password) };
            handler.Proxy = null;   // disable the proxy, the controller is connected on same subnet as the PC 
            handler.UseProxy = false;

            var client = new HttpClient(handler);
            var cancelToken = new CancellationTokenSource();


            var httpContent = new Dictionary<string, string>
            {
                   { "resources", "1" },
                   { "1", resource },
                   { "1-p", prio.ToString() }
            };

            SocketThreadAsync(client, Controller.IP, cancelToken.Token, httpContent, Controller.UAS);
            //SocketThread(client, Controller.IP, cancelToken.Token, "/rw/panel/opmode", Controller.UAS);




        }

        private static async void SocketThreadAsync(HttpClient client, string ip, CancellationToken cancelToken, Dictionary<string, string> httpContent, UAS uas)
        {

            //post that you want to subscribe on values
            var resp = await client.PostAsync($"http://{ip}/subscription", new FormUrlEncodedContent(httpContent)).ConfigureAwait(true);
            resp.EnsureSuccessStatusCode();

            //Get the ABB cookie, which will be used to connect to to the websocket
            var header = resp.Headers.FirstOrDefault(p => p.Key == "Set-Cookie");
            var val = header.Value.Last();
            string abbCookie = val.Split('=')[1].Split(';')[0];


            //Setup the websocket connection
            using (ClientWebSocket wSock = new ClientWebSocket())
            {
                wSock.Options.Credentials = new NetworkCredential(uas.User, uas.Password);
                wSock.Options.Proxy = null;
                CookieContainer cc = new CookieContainer();
                cc.Add(new Uri($"http://{ip}"), new Cookie("ABBCX", abbCookie, "/", ip));
                wSock.Options.Cookies = cc;
                wSock.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);
                wSock.Options.AddSubProtocol("robapi2_subscription");

                //Connect
                await wSock.ConnectAsync(new Uri($"ws://{ip}/poll"), cancelToken).ConfigureAwait(true);
                //  Console.WriteLine("connected to host");
                var bArr = new byte[1024];
                ArraySegment<byte> arr = new ArraySegment<byte>(bArr);

                while (true) //we are now connected
                {
                    try
                    {
                        var res = await wSock.ReceiveAsync(arr, cancelToken).ConfigureAwait(true);

                        //parse message
                        var s = Encoding.ASCII.GetString(arr.Array);

                    }
                    catch (OperationCanceledException cancelEx)
                    {
                        return;
                    }
                }



            }
        }

    }
}
