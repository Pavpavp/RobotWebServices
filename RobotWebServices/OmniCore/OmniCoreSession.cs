using RobotWebServices.OmniCoreServices.RobotWareServices;
using RWS.OmniCore.ResponseTypes;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static RWS.Enums;

namespace RWS.OmniCore
{
    public class OmniCoreSession : IRC5Session
    {
        //public ControllerService ControllerService { get; set; }
        new public RobotWareService RobotWareService { get; set; }
        //public FileService FileService { get; set; }
        //public UserService UserService { get; set; }
        //public SubscriptionService SubscriptionService { get; set; }




        public OmniCoreSession(Address ip, [Optional] UAS uas) : base(ip, uas)
        {
            Address = ip;

            IsOmnicore = true;
            TemplateUrl = "https://{0}/{1}";
            AcceptHeader = "application/hal+json;v=2.0";
            ContentTypeHeader = "application/x-www-form-urlencoded;v=2.0";

            UAS = uas ?? new UAS("Default User", "robotics");

            InitServices();

        }
        private void InitServices()
        {
            //ControllerService = new ControllerService(this);
            RobotWareService = new RobotWareService(this);
            //FileService = new FileService(this);
            //SubscriptionService = new SubscriptionService(this);
            //UserService = new UserService(this);
        }


        public async Task<BaseResponse<TRes, TState>> CallAsync<TRes,TState>(RequestMethod requestMethod, string domain, Tuple<string, string>[] dataParameters, Tuple<string, string>[] urlParameters, params Tuple<string, string>[] headers)
        {

            HttpResponseMessage response;

            CreateHttpClient(requestMethod, domain, dataParameters, urlParameters, headers, out HttpClientHandler handler, out HttpClient client, out HttpRequestMessage requestMessage);

            response = await client.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();


            return await DeserializeJsonResponse<BaseResponse<TRes, TState>>(response).ConfigureAwait(true);

        }

    }
}
