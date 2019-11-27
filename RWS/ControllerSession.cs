using Newtonsoft.Json;
using RWS.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using RWS.RobotWareServices;
using RWS.UserServices;
using RWS.SubscriptionServices;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using static RWS.Enums;
using System.Net.Http.Headers;
using RWS.RobotWareServices.StateData;

namespace RWS
{
    public partial class ControllerSession
    {

        const string templateUri = "{0}/{1}";
        public Address Address { get; private set; }
        public UAS UAS { get; private set; }
        public BaseResponse<GetSystemInformationState> SystemInformation { get; set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public ControllerService ControllerService { get; set; }
        public RobotWareService RobotWareService { get; set; }
        public FileService FileService { get; set; }
        public UserService UserService { get; set; }
        public SubscriptionService SubscriptionService { get; set; }
        public ControllerSession(Address ip, [Optional]UAS uas)
        {
            Address = ip;

            UAS = uas ?? new UAS("Default User", "robotics");


            ControllerService = new ControllerService(this);
            RobotWareService = new RobotWareService(this);
            FileService = new FileService(this);
            SubscriptionService = new SubscriptionService(this);
            UserService = new UserService(this);

            SystemInformation = RobotWareService.GetSystemInformationAsync().Result;
        }

        public void Connect(Address ip, UAS uas)
        {
            Address = ip;
            UAS = uas;
        }

        public async Task<BaseResponse<T>> CallAsync<T>(RequestMethod requestMethod, string domain, Tuple<string, string>[] dataParameters, Tuple<string, string>[] urlParameters, params Tuple<string, string>[] headers)
        {

            HttpResponseMessage response;
            var method1 = new HttpMethod(requestMethod.ToString());

            using (var handler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(UAS.User, UAS.Password),
                CookieContainer = CookieContainer
            })
            {

                using (HttpClient client = new HttpClient(handler))
                using (var requestMessage = new HttpRequestMessage(method1, BuildUri(domain, urlParameters)))
                {

                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Item1, header.Item2);
                    }
                    requestMessage.Headers.Accept.ParseAdd("application/x-www-form-urlencoded");

                    switch (requestMethod)
                    {
                        case RequestMethod.GET:

                            break;
                        default:
                            if (dataParameters != null)
                            {
                                //requestMessage.Content = new StringContent(BuildDataParameters(dataParameters));
                                //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                                requestMessage.Content = new StringContent(BuildDataParameters(dataParameters), Encoding.UTF8, "application/x-www-form-urlencoded");

                            }
                            break;
                    }

                    response = await client.SendAsync(requestMessage).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    return await DeserializeJsonResponse<T>(response).ConfigureAwait(true);
                }
            }

        }

        private static async Task<BaseResponse<T>> DeserializeJsonResponse<T>(HttpResponseMessage resp1)
        {
            using (var sr = new StreamReader(await resp1.Content.ReadAsStreamAsync().ConfigureAwait(true)))
            {
                var content = sr.ReadToEnd();
                BaseResponse<T> jsonResponse = default;
                jsonResponse = JsonConvert.DeserializeObject<BaseResponse<T>>(content);
                return jsonResponse;
            }
        }

        private static string BuildDataParameters(Tuple<string, string>[] dataParameters)
        {
            StringBuilder combinedParams = new StringBuilder();

            foreach (var param in dataParameters)
            {
                combinedParams.Append((param.Item1 == dataParameters[0].Item1 ? "" : "&") + param.Item1 + "=" + param.Item2);
            }



            return combinedParams.ToString();
        }

        private Uri BuildUri(string domain, Tuple<string, string>[] urlParameters)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, templateUri, "http://" + Address.Full, domain);

            if (uri.EndsWith("/", StringComparison.InvariantCulture)) uri = uri.TrimEnd('/');

            StringBuilder extraParameters = new StringBuilder();

            foreach (var param in urlParameters)
            {
                extraParameters.Append((extraParameters.Length == 0 ? "?" : "&") + param.Item1 + "=" + param.Item2);
            }

            if (extraParameters.Length > 0)
            {
                uri += extraParameters.ToString();
            }

            Debug.WriteLine(uri);

            return new Uri(uri);
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
