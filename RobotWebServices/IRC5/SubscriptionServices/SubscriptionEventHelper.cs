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

namespace RWS.IRC5.SubscriptionServices
{

    public abstract class SubscriptionEventHelper<T, TRet> where T : IEventArgs<TRet>
    {
        public delegate void ValueChangedIOEventHandler(object source, T args);
        private string TemplateSocketUrl { get; set; } = "ws://{0}/poll";
        private string Protocol { get; set; } = "robapi2_subscription";
        public Dictionary<object, ClientWebSocket> SubscriptionSockets { get; } = new Dictionary<object, ClientWebSocket>();
        protected ValueChangedIOEventHandler ValueChangedEventHandler { get; set; }
        public int Prio { get; set; } = 1;
        public IRC5Session Cs { get; set; }


        public async void StartSubscriptionAsync(IRC5Session cs, string resource, T eventArgs)
        {
            Cs = cs;

            if (Cs.IsOmnicore)
            {
                Protocol = "rws_subscription";
                TemplateSocketUrl = "wss://{0}/poll";
            }

            using HttpClientHandler handler = new HttpClientHandler { Credentials = new NetworkCredential(cs?.UAS.User, cs?.UAS.Password) };
            handler.Proxy = null;
            handler.UseProxy = false;
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };

            Tuple<string, string>[] dataParameters =
            {
                    Tuple.Create("resources", "1"),
                    Tuple.Create("1", resource),
                    Tuple.Create("1-p", Prio.ToString(CultureInfo.InvariantCulture))
                };

            using HttpContent httpContent = new StringContent(IRC5Session.BuildDataParameters(dataParameters));
            httpContent.Headers.Remove("Content-Type");
            httpContent.Headers.Add("Content-Type", Cs.ContentTypeHeader);

            using HttpClient client = new HttpClient(handler);
            await SocketThreadAsync(client, httpContent, eventArgs).ConfigureAwait(true);
        }


        private async Task SocketThreadAsync(HttpClient client, HttpContent httpContent, T eventArgs)
        {

            using CancellationTokenSource cancelToken = new CancellationTokenSource();

            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, Cs.TemplateUrl, Cs.Address.Full, "subscription"));

            var resp = await client.PostAsync(uri, httpContent).ConfigureAwait(true);

            resp.EnsureSuccessStatusCode();

            var header = resp.Headers.FirstOrDefault(p => p.Key == "Set-Cookie");

            string abbCookie = header.Value.Last().Split('=')[1].Split(';')[0];

            string sessionCookie = header.Value.First().Split(':')[0].Split('=')[1];

            using ClientWebSocket wSock = new ClientWebSocket();

            wSock.Options.Credentials = new NetworkCredential(Cs.UAS.User, Cs.UAS.Password);

            wSock.Options.Proxy = null;

            CookieContainer cc = new CookieContainer();

            cc.Add(new Uri($"https://{Cs.Address.Full}"), new Cookie("ABBCX", abbCookie, "/", Cs.Address.IP));

            cc.Add(new Uri($"https://{Cs.Address.Full}"), new Cookie("-http-session-", sessionCookie, "/", Cs.Address.IP));

            wSock.Options.Cookies = cc;

#if NETCOREAPP3_1
            wSock.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };
#endif

            wSock.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);

            wSock.Options.AddSubProtocol(Protocol);

            await wSock.ConnectAsync(new Uri(string.Format(CultureInfo.InvariantCulture, TemplateSocketUrl, Cs.Address.Full)), cancelToken.Token).ConfigureAwait(true);

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
