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
    public class SubscriptionService
    {
        public ControllerSession ControllerSession { get; set; }

        public SubscriptionService(ControllerSession cs)
        {
            ControllerSession = cs;
        }




    }
}
