using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace SimplerExample
{
    public static class Program
    {
        // public static string IP = "localhost";
        public static void Main(string ip)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential("Default User", "robotics") };
            handler.Proxy = null;   // disable the proxy, the controller is connected on same subnet as the PC 
            handler.UseProxy = false;
            // handler.PreAuthenticate = true;

            var client = new HttpClient(handler);
            CancellationTokenSource cancelToken = new CancellationTokenSource();
            //    Console.WriteLine("will connect to host, press c to cancel the connection and q to quit the program");
            // socketThread(client, ip, cancelToken.Token, "/rw/rapid/symbol/data/RAPID/T_ROB1/uimsg/PNum;value"); 
            //socketThread(client, ip, cancelToken.Token, "/rw/iosystem/signals/Virtual1/Board1/diTest;state");
            //socketThread(client, ip, cancelToken.Token, "/rw/iosystem/signals/diTest;state");
            socketThread(client, ip, cancelToken.Token, "/rw/elog/0");

            bool exit = false;

            while (!exit)
            {
                //char v = (char)Console.Read();
                //switch (v)
                //{
                //    case 'q':
                //        exit = true;
                //        break;
                //    case 'c':
                //      //  Console.WriteLine("cancelling");
                //        cancelToken.Cancel();
                //        break;
                //    default:
                //        break;
                //}
                ;
            }
        }

        private static async void socketThread(HttpClient client, string ip, CancellationToken cancelToken, string resource)
        {
            //  "resources=1&1=" + "/rw/elog/0" + "&1-p", "1"

            var values = new Dictionary<string, string>
            {
                   { "resources", "1" },
                   { "1", resource },
                   { "1-p", "1" }
            };
            //post that you want to subscribe on values
            var resp = await client.PostAsync($"http://{ip}/subscription", new FormUrlEncodedContent(values)).ConfigureAwait(true); ;
            resp.EnsureSuccessStatusCode();

            //Get the ABB cookie, which will be used to connect to to the websocket
            var header = resp.Headers.First(p => p.Key == "Set-Cookie");
            var val = header.Value.Last();
            string ABBCookie = val.Split('=')[1].Split(';')[0];


            //Setup the websocket connection
            ClientWebSocket wsock = new ClientWebSocket();
            wsock.Options.Credentials = new NetworkCredential("Default User", "robotics");
            wsock.Options.Proxy = null;
            CookieContainer cc = new CookieContainer();
            cc.Add(new Uri($"http://{ip}"), new Cookie("ABBCX", ABBCookie, "/", ip));
            wsock.Options.Cookies = cc;
            wsock.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);
            wsock.Options.AddSubProtocol("robapi2_subscription");

            //Connect
            await wsock.ConnectAsync(new Uri($"ws://{ip}/poll"), cancelToken).ConfigureAwait(true);
            //  Console.WriteLine("connected to host");
            byte[] barr = new byte[1024];
            ArraySegment<byte> arr = new ArraySegment<byte>(barr);
            while (true) //we are now connected
            {
                try
                {
                    var res = await wsock.ReceiveAsync(arr, cancelToken).ConfigureAwait(true); ;
                }
                catch (OperationCanceledException cancelEx)
                {
                    //     Console.WriteLine("canceled");
                    break;
                }
                //parse message
                var s = Encoding.ASCII.GetString(arr.Array);
                var startIdx = s.IndexOf("lvalue");
                var endIdx = s.IndexOf('<', startIdx);
                string sub = s.Substring(startIdx + 6 + 2, endIdx - startIdx - 2 - 6);
                int value = int.Parse(sub);
                //   Console.WriteLine(value);
            }
        }
    }
}
