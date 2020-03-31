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

        public delegate void IOValueChangedEventHandler(object source, T args);
        private string TemplateSocketUrl { get; set; } = "ws://{0}/poll";
        private string Protocol { get; set; } = "robapi2_subscription";
        public Dictionary<object, ClientWebSocket> SubscriptionSockets { get; set; } = new Dictionary<object, ClientWebSocket>();
        protected IOValueChangedEventHandler ValueChangedEventHandler { get; set; }
        public int Prio { get; private set; } = 1;
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

            string prioAndResourcePath = Prio + "|" + resource.Replace(resource.Split('/').Last(), "");

            int resourceCount = 1;
            Cs.SubscriptionService.SubscriptionSessions.TryGetValue(prioAndResourcePath, out var outSubscData);
            resourceCount += outSubscData == null ? 0 : outSubscData.ResourceCount;

            Tuple<string, string>[] dataParameters =
            {
                    Tuple.Create("resources", resourceCount.ToString()),
                    Tuple.Create(resourceCount.ToString(), resource),
                    Tuple.Create(resourceCount + "-p", Prio.ToString(CultureInfo.InvariantCulture))
                };


            string combinedParams = IRC5Session.BuildDataParameters(dataParameters);

            if (outSubscData != null)
            {
                outSubscData.CombinedParameters = (outSubscData.CombinedParameters.Contains(resource) ?
                outSubscData.CombinedParameters : (outSubscData.CombinedParameters + "&" + combinedParams).TrimStart('&'));

                outSubscData.ResourceCount = resourceCount;
            }
            else
            {
                Cs.SubscriptionService.SubscriptionSessions.AddOrOverwrite(prioAndResourcePath, new OpenSubscriptionData()
                {
                    ResourceCount = resourceCount,
                    CombinedParameters = combinedParams,
                    ResourcePath = prioAndResourcePath.Split('|')[1],
                    GroupID = "1",
                });
            }
            outSubscData = Cs.SubscriptionService.SubscriptionSessions[prioAndResourcePath];


            using HttpContent httpContent = new StringContent(outSubscData.CombinedParameters);
            httpContent.Headers.Remove("Content-Type");
            httpContent.Headers.Add("Content-Type", Cs.ContentTypeHeader);

            using HttpClient client = new HttpClient(handler);

            outSubscData.RequestQueue.Add(new Task(() => { StartSubscriptionAsync(Cs, resource, eventArgs); }));

            if (outSubscData.RequestQueue.Count == 1)
            {
                outSubscData.CombinedParameters = "";

                await SocketThreadAsync(client, httpContent, eventArgs, prioAndResourcePath);
            }
        }



        private async Task SocketThreadAsync(HttpClient client, HttpContent httpContent, T eventArgs, string prioAndResourcePath)
        {
            using CancellationTokenSource cancelToken = new CancellationTokenSource();

            //await Cs.SubscriptionService.SemaphoreSlim.WaitAsync();

            HttpResponseMessage resp;

            Cs.SubscriptionService.SubscriptionSessions.TryGetValue(prioAndResourcePath, out var outSubscData);

            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, Cs.TemplateUrl, Cs.Address.Full, "subscription"));
            resp = await client.PostAsync(uri, httpContent).ConfigureAwait(true);

            resp.EnsureSuccessStatusCode();

            var header = resp.Headers.FirstOrDefault(p => p.Key == "Set-Cookie");

            string abbCookie = header.Value.Last().Split('=')[1].Split(';')[0];

            string sessionCookie = header.Value.First().Split(':')[0].Split('=')[1];

            if (outSubscData.RequestQueue.Count == 1)
                sessionCookie = outSubscData.Session;

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

            //If user tried to open more sockets while we were opening this one async, they will be combined in the last Task of the RequestQueue
            //and subscribed to in the same session as this one
            if (outSubscData.RequestQueue.Count > 1)
            {
                outSubscData.Session = sessionCookie;
                outSubscData.RequestQueue.Last().Start();
            }
            outSubscData.RequestQueue.Clear();

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

    public class OpenSubscriptionData
    {
        public string Resource { get; set; }
        public int ResourceCount { get; set; }
        public string ResourcePath { get; set; }
        public string CombinedParameters { get; set; }
        public string Session { get; set; }
        public string GroupID { get; set; }
        public List<Task> RequestQueue { get; set; } = new List<Task>();

    }
}
