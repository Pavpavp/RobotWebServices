using Newtonsoft.Json;
using RWS.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using RWS.SubscriptionServices.ElogSubscriber;
using RWS.RobotWareServices;
using RWS.UserServices;
using System.Xml;
using RWS.SubscriptionServices;
using System.Globalization;

namespace RWS
{
    public partial class ControllerSession
    {

        const string templateUri = "{0}/{1}";
        public string IP { get; private set; }
        public UAS UAS { get; private set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public ControllerService ControllerService { get; set; }
        public RobotWareService RobotWareService { get; set; }
        public FileService FileService { get; set; }
        public UserService UserService { get; set; }
        public SubscriptionService SubscriptionService { get; set; }
        public ControllerSession(string controllerIP, [Optional]UAS uas)
        {
            IP = controllerIP;

            UAS = uas ?? new UAS("Default User", "robotics");


            ControllerService = new ControllerService(this);
            RobotWareService = new RobotWareService(this);
            FileService = new FileService(this);
            SubscriptionService = new SubscriptionService(this);
            UserService = new UserService(this);
        }

        public void Connect(string ip, UAS uas)
        {
            IP = ip;
            UAS = uas;
        }

        public GenResponse<T> Call<T>(string method, string domain, Tuple<string, string>[] dataParameters, Tuple<string, string>[] urlParameters)
        {

            var uri = string.Format(CultureInfo.InvariantCulture, templateUri, "http://" + IP, domain);

            if (uri.EndsWith("/", StringComparison.InvariantCulture)) uri = uri.TrimEnd('/');

            if (urlParameters != null && urlParameters.Length > 0)
            {
                StringBuilder extraParameters = new StringBuilder();

                foreach (var item in urlParameters)
                {
                    extraParameters.Append((extraParameters.Length == 0 ? "?" : "&") + item.Item1 + "=" + item.Item2);
                }

                if (extraParameters.Length > 0)
                {
                    uri += extraParameters.ToString();
                }
            }

            Debug.WriteLine(uri);

            return CallWithJson<T>(new Uri(uri), method, dataParameters);

        }

        public GenResponse<T> CallWithJson<T>(Uri uri, string method, Tuple<string, string>[] dataParameters, params Tuple<string, string>[] headers)
        {
            var request = WebRequest.CreateHttp(uri);

            request.Credentials = new NetworkCredential(UAS.User, UAS.Password);

            if (CookieContainer != null)
                request.CookieContainer = CookieContainer;

            foreach (var header in headers)
            {
                request.Headers.Add(header.Item1, header.Item2);
            }

            request.Proxy = null;
            request.Method = method;
            //   request.PreAuthenticate = true;
            request.ContentType = "application/x-www-form-urlencoded";


            if (dataParameters != null && dataParameters.Length > 0 && method != "GET")
            {
                StringBuilder combinedParams = new StringBuilder();

                foreach (var item in dataParameters)
                {
                    combinedParams.Append((item.Item1 == dataParameters[0].Item1 ? "" : "&") + item.Item1 + "=" + item.Item2);
                }

                Stream stream = request.GetRequestStream();

                if (method == "PUT")
                {
                    using (FileStream fs = File.OpenRead(combinedParams.ToString().Split('=')[0]))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bb = br.ReadBytes((int)fs.Length);
                            stream.Write(bb, 0, bb.Length);
                        }
                    }
                }
                else
                    stream.Write(Encoding.ASCII.GetBytes(combinedParams.ToString()), 0, combinedParams.ToString().Length);

                stream.Close();
            }

            using (var httpResponse = (HttpWebResponse)request.GetResponse())
            {
                string cookieHeader = httpResponse.Headers[HttpResponseHeader.SetCookie];

                if (cookieHeader != null)
                {
                    _subscrCookie = HttpWebCommunication.GetAbbCookie(httpResponse, IP);
                    CookieContainer.SetCookies(new Uri("http://" + IP), cookieHeader);
                }

                //if (httpResponse.StatusCode == HttpStatusCode.OK)
                //{

                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var content = sr.ReadToEnd();

                    GenResponse<T> jsonResponse = default;

                    jsonResponse = JsonConvert.DeserializeObject<GenResponse<T>>(content);

                    return jsonResponse;

                }
                //   }

            }
        }

        private static string GetDebugCallDetails(string uri)
        {
            StringBuilder sb = new StringBuilder();
            var u = new Uri(uri);
            sb.Append(u.AbsolutePath);
            if (u.Query.StartsWith("?", StringComparison.InvariantCulture))
            {
                var queryParameters = u.Query.Substring(1).Split('&');
                foreach (var p in queryParameters)
                {

                    var kv = p.Split('=');
                    if (kv.Length == 2)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(", ");
                        }

                        sb.Append(kv[0]).Append(" = ").Append(kv[1]);
                    }

                }
            }
            return sb.ToString();
        }


        #region SubscriptionTests

        private Cookie _subscrCookie;
        public SimpleSafeList<string> MessageQueue { get; set; } = new SimpleSafeList<string>();



        public void RunSubscription()
        {

            SubscribeRAPID();
            // HttpWebResponse response = SubscribeDI();
            //// A subscribe request shall return created
            //if (response.StatusCode != HttpStatusCode.Created)
            //{
            //    // something was wrong
            //    return;
            //}
            // Get the created location url from header
            // The location url contains the subscription group. 
            // The location url shall be used if the subscription shall be removed or new subscriptions shall be added to the same group
            // string location = response.Headers["Location"];
            // It's a good idea to close the response stream when done with the response
            //response.GetResponseStream().Close();
            // Connect websocket
            RWSWebSockets ws = new RWSWebSockets();
            ws.WsConnect(MessageQueue, new Uri("http://" + IP), _subscrCookie);
            // Wait on messages on the message queue
            // Message to the message queue are sent from the websocket or the main thread
            string message = null;
            do
            {
                message = MessageQueue.WaitRead();
                if (message != null)
                {
                    if (message.Substring(0, 6) == "<info>")
                    {
                        ;
                        //  Console.WriteLine(message);
                    }
                    else if (message.Substring(0, 6) == "<data>") // event
                    {
                        ;
                        // Console.WriteLine(message);
                        // pick out the elog href from the event
                        string href = GetElogLink(message.Substring(6));
                        // get elog message from controller
                        if (href != null)
                        {
                            SubscribeRAPID2(href);
                            string elogMsg = GetElogMessage(href);
                            if (elogMsg != null)
                            {
                                Console.WriteLine(elogMsg);
                            }
                        }
                    }
                }
            } while (true);
        }

        string DeserializeElogResource(Stream elogXml)
        {
            string elogDescription = null;
            string elogCode = null;
            string elogTimeStamp = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(elogXml);
            // Create an XmlNamespaceManager for resolving namespaces.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://www.w3.org/1999/xhtml");

            XmlNodeList nodes = doc.SelectNodes("//ns:html/ns:body/ns:div/ns:ul/ns:li", nsmgr);
            foreach (XmlNode node in nodes)
            {
                XmlAttribute type = node.Attributes["class"];
                if (type != null)
                {
                    string classType = type.Value.ToString(CultureInfo.InvariantCulture);
                    switch (classType)
                    {
                        case "elog-message":
                            XmlNode elog = node.SelectSingleNode("//ns:span[@class='desc']", nsmgr);
                            if (elog != null)
                            {
                                elogDescription = elog.InnerText.ToString(CultureInfo.InvariantCulture);
                            }
                            elog = node.SelectSingleNode("//ns:span[@class='code']", nsmgr);
                            if (elog != null)
                            {
                                elogCode = elog.InnerText.ToString(CultureInfo.InvariantCulture);
                            }
                            elog = node.SelectSingleNode("//ns:span[@class='tstamp']", nsmgr);
                            if (elog != null)
                            {
                                elogTimeStamp = elog.InnerText.ToString(CultureInfo.InvariantCulture);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return elogCode + " " + elogTimeStamp + " " + elogDescription;
        }

        private string GetElogLink(string xmlResource)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResource);
            // Create an XmlNamespaceManager for resolving namespaces.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://www.w3.org/1999/xhtml");
            string elogHref = null;
            string seqNo = null;
            // Get type of event
            XmlNodeList nodes = doc.SelectNodes("//ns:html/ns:body/ns:div/ns:ul/ns:li", nsmgr);
            foreach (XmlNode node in nodes)
            {
                XmlAttribute type = node.Attributes["class"];
                if (type != null)
                {
                    string classType = type.Value.ToString(CultureInfo.InvariantCulture);
                    switch (classType)
                    {
                        case "elog-message-ev":
                            XmlNode elogNode = node.SelectSingleNode("ns:a[@rel='self']", nsmgr);
                            if (elogNode != null)
                            {
                                XmlAttribute rlink = elogNode.Attributes["href"];
                                elogHref = rlink.Value.ToString(CultureInfo.InvariantCulture);
                            }
                            XmlNode elogSeqNo = node.SelectSingleNode("//ns:span[@class='seqnum']", nsmgr);
                            if (elogSeqNo != null)
                            {
                                seqNo = elogSeqNo.InnerText.ToString(CultureInfo.InvariantCulture);
                            }
                            Console.WriteLine("Elog: seqNo={0} href={1}", seqNo, elogHref);
                            break;
                        default:
                            break;
                    }
                }
            }
            return elogHref;
        }


        private string GetElogMessage(string elogUrl)
        {
            // Get the elog message of language Program._language
            HttpWebResponse response = Program._webcon.DoWebRequest("GET", Program._host + elogUrl + Program._language, null);
            // A subscribe request shall return created
            if (response.StatusCode != HttpStatusCode.OK)
            {
                // something was wrong
                Console.WriteLine("Error get resource {0} error {1}", elogUrl, response.StatusCode.ToString());
                return null;
            }
            // Deserialize the elog XML string 
            string elogMessage = DeserializeElogResource(response.GetResponseStream());
            // Close the http response stream (if forgotten, it might not be possible to send more requests) 
            response.GetResponseStream().Close();

            return elogMessage;
        }

        public void SubscribeRAPID()
        {

            string method = "POST";

            //Tuple<string, string>[] dataParameters = { Tuple.Create("resources=1&1=" + "/rw/elog/0" + "&1-p", "1") };
            Tuple<string, string>[] dataParameters = { Tuple.Create("resources=1&1=/rw/rapid/symbol/data/RAPID/T_ROB1/uimsg/PNum;value&1-p", "1") };
            Tuple<string, string>[] urlParameters = null;


            var sdfg = this.Call<dynamic>(method, "subscription", dataParameters, urlParameters);

        }

        public void SubscribeRAPID2(string nr)
        {

            string method = "GET";

            Tuple<string, string>[] dataParameters = { Tuple.Create("resources=1&1=" + nr + "&1-p", "1") };
            //  Tuple<string, string>[] dataParameters = { Tuple.Create($"resources=1&1=/rw/rapid/symbol/data/RAPID/T_ROB1/uimsg/PNum;value&1-p", "1") };
            Tuple<string, string>[] urlParameters = null;


            var sdfg = this.Call<dynamic>(method, "subscription", dataParameters, urlParameters);

        }

        #endregion SubscriptionTests


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



}
