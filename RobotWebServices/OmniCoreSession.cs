using RobotWebServices.OmniCoreServices.RobotWareServices;
using RWS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RobotWebServices
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

            TemplateUri = TemplateUri.Replace("http:", "https:");
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
    }
}
