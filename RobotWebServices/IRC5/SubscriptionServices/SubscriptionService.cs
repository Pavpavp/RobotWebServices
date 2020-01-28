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
    public class SubscriptionService
    {
        public IRC5Session ControllerSession { get; set; }

        public SubscriptionService(IRC5Session cs)
        {
            ControllerSession = cs;
        }




    }
}
