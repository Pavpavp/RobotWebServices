using RWS.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RWS.SubscriptionServices
{

    public abstract class SubscriptionEventHelper<T, TRet> where T : IEventArgs<TRet>
    {
        public Dictionary<object, ClientWebSocket> SubscriptionSockets { get; } = new Dictionary<object, ClientWebSocket>();
        protected ValueChangedIOEventHandler ValueChangedEventHandler { get; set; }
        public ControllerSession ControllerSession { get; set; }

        public delegate void ValueChangedIOEventHandler(object source, T args);
        private int Prio { get; set; } = 1;

        public async void StartSubscription7Async(ControllerSession cs, string resource, T eventArgs)
        {
            ControllerSession = cs;


            using (HttpClientHandler handler = new HttpClientHandler { Credentials = new NetworkCredential(cs?.UAS.User, cs?.UAS.Password) })
            {
                handler.Proxy = null;   // disable the proxy, the controller is connected on same subnet as the PC 
                handler.UseProxy = false;
                handler.CookieContainer = ControllerSession.CookieContainer;

                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };

                Tuple<string, string>[] dataParameters =
                {
                    Tuple.Create("resources", "1"),
                    Tuple.Create("1", resource),
                    Tuple.Create("1-p", Prio.ToString(CultureInfo.InvariantCulture))
                };

                using (HttpContent httpContent = new StringContent(ControllerSession.BuildDataParameters(dataParameters)))
                {
                    httpContent.Headers.Remove("Content-Type");
                    httpContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded;v=2.0");

                    using (HttpClient client = new HttpClient(handler))
                    {
                        await SocketThread7Async(client, httpContent, eventArgs).ConfigureAwait(true);

                    }
                }
            }
        }

        public async void StartSubscriptionAsync(ControllerSession cs, string resource, T eventArgs)
        {
            ControllerSession = cs;

            using (HttpClientHandler handler = new HttpClientHandler { Credentials = new NetworkCredential(cs?.UAS.User, cs?.UAS.Password) })
            {

                handler.Proxy = null;   // disable the proxy, the controller is connected on same subnet as the PC 
                handler.UseProxy = false;
                handler.CookieContainer = ControllerSession.CookieContainer;

                using (HttpClient client = new HttpClient(handler))
                {
                    var httpContent = new Dictionary<string, string>
                        {
                            { "resources", "1" },
                            { "1", resource },
                            { "1-p", Prio.ToString(CultureInfo.InvariantCulture) }
                        };

                    await SocketThreadAsync(client, httpContent, eventArgs).ConfigureAwait(true);

                }
            }

        }


        private async Task SocketThread7Async(HttpClient client, HttpContent httpContent, T eventArgs)
        {

            using CancellationTokenSource cancelToken = new CancellationTokenSource();

            var resp = await client.PostAsync(new Uri($"https://{ControllerSession.Address.Full}/subscription"), httpContent).ConfigureAwait(true);
            resp.EnsureSuccessStatusCode();

            var header = resp.Headers.FirstOrDefault(p => p.Key == "Set-Cookie");

            string abbCookie = header.Value.Last().Split('=')[1].Split(';')[0];

            string sessionCookie = header.Value.First().Split(':')[0].Split('=')[1];

            using ClientWebSocket wSock = new ClientWebSocket();

            wSock.Options.Credentials = new NetworkCredential(ControllerSession.UAS.User, ControllerSession.UAS.Password);

            wSock.Options.Proxy = null;

            CookieContainer cc = new CookieContainer();

            cc.Add(new Uri($"https://{ControllerSession.Address.Full}"), new Cookie("ABBCX", abbCookie, "/", ControllerSession.Address.IP));

            cc.Add(new Uri($"https://{ControllerSession.Address.Full}"), new Cookie("-http-session-", sessionCookie, "/", ControllerSession.Address.IP));

            wSock.Options.Cookies = cc;

#if NETCOREAPP3_1
            wSock.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };
#endif

            wSock.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);

            wSock.Options.AddSubProtocol("rws_subscription");

            await wSock.ConnectAsync(new Uri($"wss://{ControllerSession.Address.Full}/poll"), cancelToken.Token).ConfigureAwait(true);

            var bArr = new byte[1024];
            ArraySegment<byte> arr = new ArraySegment<byte>(bArr);

            SubscriptionSockets.Add(ValueChangedEventHandler, wSock);

            while (ValueChangedEventHandler != null)
            {
                try
                {
                    var res = await wSock.ReceiveAsync(arr, cancelToken.Token).ConfigureAwait(true);

                    if (ValueChangedEventHandler == null)
                        break;

                    var s = Encoding.ASCII.GetString(arr.Array);

                    eventArgs.SetValueChanged(s);
                    ValueChangedEventHandler(this, eventArgs);

                }
                catch (Exception ex)
                {
                    if (ex is WebSocketException && wSock.State == WebSocketState.Aborted)
                        break;
                }
            }
        }

        private async Task SocketThreadAsync(HttpClient client, Dictionary<string, string> httpContent, T eventArgs)
        {

            using CancellationTokenSource cancelToken = new CancellationTokenSource();

            using FormUrlEncodedContent fuec = new FormUrlEncodedContent(httpContent);

            var resp = await client.PostAsync(new Uri($"http://{ControllerSession.Address.IP}/subscription"), fuec).ConfigureAwait(true);

            resp.EnsureSuccessStatusCode();

            var header = resp.Headers.FirstOrDefault(p => p.Key == "Set-Cookie");

            var val = header.Value.Last();

            string abbCookie = val.Split('=')[1].Split(';')[0];

            using (ClientWebSocket wSock = new ClientWebSocket())
            {
                wSock.Options.Credentials = new NetworkCredential(ControllerSession.UAS.User, ControllerSession.UAS.Password);

                wSock.Options.Proxy = null;

                CookieContainer cc = new CookieContainer();

                cc.Add(new Uri($"http://{ControllerSession.Address.IP}"), new Cookie("ABBCX", abbCookie, "/", ControllerSession.Address.IP));

                wSock.Options.Cookies = cc;

                wSock.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);

                wSock.Options.AddSubProtocol("robapi2_subscription");

                await wSock.ConnectAsync(new Uri($"ws://{ControllerSession.Address.IP}/poll"), cancelToken.Token).ConfigureAwait(true);
                var bArr = new byte[1024];

                ArraySegment<byte> arr = new ArraySegment<byte>(bArr);

                SubscriptionSockets.Add(ValueChangedEventHandler, wSock);

                while (ValueChangedEventHandler != null)
                {
                    try
                    {
                        var res = await wSock.ReceiveAsync(arr, cancelToken.Token).ConfigureAwait(true);

                        if (ValueChangedEventHandler == null)
                            break;

                        var s = Encoding.ASCII.GetString(arr.Array);

                        eventArgs.SetValueChanged(s);
                        ValueChangedEventHandler(this, eventArgs);

                    }
                    catch (Exception ex)
                    {
                        if (ex is WebSocketException && wSock.State == WebSocketState.Aborted)
                            break;
                    }
                }
            }
        }
    }
}
