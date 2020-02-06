using RWS.IRC5.RobotWareServices;
using RWS.IRC5.UserServices;
using RWS.IRC5.SubscriptionServices;
using static RWS.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using Zeroconf;
using Newtonsoft.Json;
using RWS.IRC5;
using RWS.IRC5.ResponseTypes;

namespace RWS
{

    //requires bonjour discovery deamon to run on the VC - computer
    //Download Bonjour Print Services for Windows v2.0.2 https://support.apple.com/kb/DL999?locale=en_US
    //https://learn.adafruit.com/bonjour-zeroconf-networking-for-windows-and-linux
    public static class ControllerDiscovery
    {

        //  private static string resolvePort = "dns-sd -L "RobotWebServices_ABB_Testrack" _http._tcp";
        public static async Task<List<IZeroconfHost>> Discover()
        {
            var serviceList = new List<IZeroconfHost>();
            ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);
            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key)).ConfigureAwait(false);

            foreach (var resp in responses)
                serviceList.Add(resp);

            var results = await ZeroconfResolver.ResolveAsync("_http._tcp.local.");


            ;

            return serviceList;
        }

    }

    public class IRC5Session
    {
        internal string TemplateUrl { get; set; } = "http://{0}/{1}";
        internal string AcceptHeader { get; set; } = "application/x-www-form-urlencoded";
        internal string ContentTypeHeader { get; set; } = "application/x-www-form-urlencoded";
        public Address Address { get; set; }
        public UAS UAS { get; set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public ControllerService ControllerService { get; set; }
        public RobotWareService RobotWareService { get; set; }
        public FileService FileService { get; set; }
        public UserService UserService { get; set; }
        public SubscriptionService SubscriptionService { get; set; }
        public bool IsOmnicore { get; set; } = false;
        public string CtrlName { get; set; }
        public string Version { get; set; }


        public IRC5Session(Address ip, [Optional] UAS uas)
        {
            Address = ip;

            UAS = uas ?? new UAS("Default User", "robotics");

            InitServices();


        }
        private void InitServices()
        {
            ControllerService = new ControllerService(this);
            RobotWareService = new RobotWareService(this);
            FileService = new FileService(this);
            SubscriptionService = new SubscriptionService(this);
            UserService = new UserService(this);
        }

        public void Connect(Address ip, UAS uas)
        {
            Address = ip;
            UAS = uas;
        }



        public async Task<BaseResponse<T>> CallAsync<T>(RequestMethod requestMethod, string domain, Tuple<string, string>[] dataParameters, Tuple<string, string>[] urlParameters, params Tuple<string, string>[] headers)
        {


            HttpResponseMessage response;

            CreateHttpClient(requestMethod, domain, dataParameters, urlParameters, headers, out HttpClientHandler handler, out HttpClient client, out HttpRequestMessage requestMessage);

            response = await client.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();


            return await DeserializeJsonResponse<BaseResponse<T>>(response).ConfigureAwait(true);

        }

        protected void CreateHttpClient(RequestMethod requestMethod, string domain, Tuple<string, string>[] dataParameters, Tuple<string, string>[] urlParameters, Tuple<string, string>[] headers, out HttpClientHandler handler, out HttpClient client, out HttpRequestMessage requestMessage)
        {
            var method = new HttpMethod(requestMethod.ToString());
            handler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(UAS.User, UAS.Password),
                CookieContainer = CookieContainer,
                UseCookies = true,
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; }
            };
            client = new HttpClient(handler);
            requestMessage = new HttpRequestMessage(method, BuildUri(domain, urlParameters));
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Item1, header.Item2);
            }
            requestMessage.Headers.Accept.ParseAdd(AcceptHeader);



            switch (requestMethod)
            {
                case RequestMethod.GET:


                    break;
                default:
                    if (dataParameters != null)
                    {
                        //requestMessage.Content = new StringContent(BuildDataParameters(dataParameters));
                        //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        requestMessage.Content = new StringContent(BuildDataParameters(dataParameters), Encoding.UTF8, AcceptHeader);

                    }
                    break;
            }
        }


        protected static async Task<T> DeserializeJsonResponse<T>(HttpResponseMessage resp1)
        {
            using var sr = new StreamReader(await resp1.Content.ReadAsStreamAsync().ConfigureAwait(true));

            var content = sr.ReadToEnd();
            T jsonResponse = default;
            jsonResponse = JsonConvert.DeserializeObject<T>(content);
            return jsonResponse;
        }

        public static string BuildDataParameters(Tuple<string, string>[] dataParameters)
        {
            StringBuilder combinedParams = new StringBuilder();

            foreach (var param in dataParameters)
            {
                combinedParams.Append((param.Item1 == dataParameters[0].Item1 ? "" : "&") + param.Item1 + "=" + param.Item2);
            }

            return combinedParams.ToString();
        }
        protected Uri BuildUri(string domain, Tuple<string, string>[] urlParameters)
        {
            var url = string.Format(CultureInfo.InvariantCulture, TemplateUrl, Address.Full, domain);

            if (url.EndsWith("/", StringComparison.InvariantCulture)) url = url.TrimEnd('/');

            StringBuilder extraParameters = new StringBuilder();

            if (urlParameters != null)
            {
                foreach (var param in urlParameters)

                    extraParameters.Append((extraParameters.Length == 0 ? "?" : "&") + param.Item1 + "=" + param.Item2);


                if (extraParameters.Length > 0)
                {
                    url += extraParameters.ToString();
                }
            }

            Debug.WriteLine(url);

            return new Uri(url);
        }

    }

    public class UAS
    {
        public string User { get; set; }
        public string Password { get; set; }

        public UAS(string user, string password)
        {
            User = user;
            Password = password;
        }


    }

    public class Address
    {
        public string IP { get; set; }
        public string Port { get; set; }

        /// <summary>
        /// IP and port separated by ':' 
        /// </summary>
        public string Full { get; set; }
        public Address(string address)
        {
            address = address?.Trim();

            IP = address;
            Full = address;

            if (address.Contains(':'))
            {
                IP = address.Split(':')[0];
                Port = address.Split(':')[1];
            }


        }

    }
}
