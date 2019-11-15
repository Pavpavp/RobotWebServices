using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.SubscriptionServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.IO;
    using WebSocketSharp; // the websocket-sharp library from https://github.com/sta/websocket-sharp
    using System.Net;
    using System.Xml;
    // Example on how to subscribe and receive events on a websocket
    namespace ElogSubscriber
    {
        public class Program
        {
            public static string _host = "http://192.168.8.105"; // controller to connect to
            public static string _userName = "Default User";
            public static string _password = "robotics";
            public static string _language = "?lang=en"; // return elog descriptions in english
            public static HttpWebCommunication _webcon = new HttpWebCommunication();
            // A simple message queue between websocket thread and session thread
            public static SimpleSafeList<string> messageQueue = new SimpleSafeList<string>();
            static void Main(string[] args)
            {
                // Create a thread which will :
                //  Connect to the controller 
                //  Subscribe on elog
                //  Display elog events
                ControllerSession1 session = new ControllerSession1();
                Thread sessionThread = new Thread(new ThreadStart(session.Run));
                sessionThread.Start();
                //Console.WriteLine("Hit ENTER to exit...");
                //Console.ReadLine();
                // send exit message to session thread
                messageQueue.Add("<info>exit");
                // abort the thread
                sessionThread.Abort();
            }
        }
        // <summary>
        // Controller thread. Connect to the controller and subscribe on elog.
        // Waits on inbound events on the message queue
        // </summary>
        public class ControllerSession1
        {
            public void Run()
            {
                // Subscribe on elog domain 0
                string postData = "resources=1&1=" + "/rw/elog/0" + "&1-p=1";
                HttpWebResponse response = Program._webcon.DoWebRequest("POST", Program._host + "/subscription", postData);
                // A subscribe request shall return created
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    // something was wrong
                    Console.WriteLine("Error subscribe on resource {0} error {1}", postData, response.StatusCode.ToString());
                    return;
                }
                // Get the created location url from header
                // The location url contains the subscription group. 
                // The location url shall be used if the subscription shall be removed or new subscriptions shall be added to the same group
                string location = response.Headers["Location"];
                // It's a good idea to close the response stream when done with the response
                response.GetResponseStream().Close();
                // Connect websocket
                RWSWebSockets ws = new RWSWebSockets();
                ws.WsConnect(Program.messageQueue, new Uri(Program._host), Program._webcon.GetAbbCookie());
                // Wait on messages on the message queue
                // Message to the message queue are sent from the websocket or the main thread
                string message = null;
                do
                {
                    message = Program.messageQueue.WaitRead();
                    if (message != null)
                    {
                        if (message.Substring(0, 6) == "<info>")
                        {
                            Console.WriteLine(message);
                        }
                        else if (message.Substring(0, 6) == "<data>") // event
                        {
                            // Console.WriteLine(message);
                            // pick out the elog href from the event
                            string href = GetElogLink(message.Substring(6));
                            // get elog message from controller
                            if (href != null)
                            {
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

            // <summary>
            // Get elog message from controller
            // </summary>
            // <param name="elogUrl"></param>
            // <returns></returns>
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

            // <summary>
            // Deserialize Elog xml string
            // </summary>
            // <param name="elogXml"></param>
            // <returns></returns>
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
                        string classType = type.Value.ToString();
                        switch (classType)
                        {
                            case "elog-message":
                                XmlNode elog = node.SelectSingleNode("//ns:span[@class='desc']", nsmgr);
                                if (elog != null)
                                {
                                    elogDescription = elog.InnerText.ToString();
                                }
                                elog = node.SelectSingleNode("//ns:span[@class='code']", nsmgr);
                                if (elog != null)
                                {
                                    elogCode = elog.InnerText.ToString();
                                }
                                elog = node.SelectSingleNode("//ns:span[@class='tstamp']", nsmgr);
                                if (elog != null)
                                {
                                    elogTimeStamp = elog.InnerText.ToString();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                return elogCode + " " + elogTimeStamp + " " + elogDescription;
            }
            // <summary>
            // Get the link to the elog resource from the elog event
            // </summary>
            // <param name="xmlResource"></param>
            // <returns></returns>
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
                        string classType = type.Value.ToString();
                        switch (classType)
                        {
                            case "elog-message-ev":
                                XmlNode elogNode = node.SelectSingleNode("ns:a[@rel='self']", nsmgr);
                                if (elogNode != null)
                                {
                                    XmlAttribute rlink = elogNode.Attributes["href"];
                                    elogHref = rlink.Value.ToString();
                                }
                                XmlNode elogSeqNo = node.SelectSingleNode("//ns:span[@class='seqnum']", nsmgr);
                                if (elogSeqNo != null)
                                {
                                    seqNo = elogSeqNo.InnerText.ToString();
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
        }
        // <summary>
        // Wrapper class for http
        // </summary>
        public class HttpWebCommunication
        {
            CookieContainer _cookies = new CookieContainer();  // keep the cookies the same during multiple requests
            NetworkCredential _credentials = new NetworkCredential(Program._userName, Program._password);
            Cookie _abbCookie = null;
            public HttpWebCommunication()
            {
            }
            // <summary>
            // Do a web request
            // </summary>
            // <param name="method">GET, PUT, POST, DELETE</param>
            // <param name="url"></param>
            // <param name="body"></param>
            // <returns></returns>
            public HttpWebResponse DoWebRequest(string method, string url, string body)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Credentials = _credentials;
                request.Method = method;
                request.CookieContainer = _cookies;
                request.PreAuthenticate = true;
                request.Proxy = null;
                Uri uri = new Uri(url);
                if (request.Method == "PUT" || request.Method == "POST")
                {
                    request.ContentType = "application/x-www-form-urlencoded"; // use form data when sending update etc to controller
                    Stream s = request.GetRequestStream();
                    s.Write(Encoding.ASCII.GetBytes(body), 0, body.Length);
                    s.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (_abbCookie == null) // take the cookie from the first request
                {
                    // set the abb cookie
                    _abbCookie = GetAbbCookie(response,"");
                }
                return response;
            }
            // <summary>
            // Get the ABBCX cookie from the response
            // </summary>
            // <param name="response"></param>
            // <returns></returns>
            public static Cookie GetAbbCookie(HttpWebResponse response, string ip)
            {
                string abbcookiestr = null;
                // get the abb cookie
                string cookiesText = (response as HttpWebResponse).Headers[HttpResponseHeader.SetCookie];
                string[] lines = cookiesText.Split(';');
                foreach (string line in lines)
                {
                    string[] c = line.Split('=');
                    if (line.Contains("ABBCX"))
                    {
                        abbcookiestr = c[1];
                        break;
                    }
                }
                return new Cookie("ABBCX", abbcookiestr, "/", ip);
            }
            public Cookie GetAbbCookie()
            {
                return _abbCookie;
            }
        }

        // <summary>
        // Websocket interface towards websocket_sharp
        // </summary>
        class RWSWebSockets
        {
            WebSocket websocket = null;
            bool m_connected = false;
            SimpleSafeList<string> m_messageQueue;
            public void WsConnect(SimpleSafeList<string> msgQueue, Uri url, Cookie cookie)
            {
                m_messageQueue = msgQueue;
                string wsUrl = "ws://" + url.Authority + "/poll"; // Authority, the port number must be included in the url
                websocket = new WebSocket(wsUrl, "robapi2_subscription"); // create websocket using robapi2_subscription protocol
                                                                          // add cookies used in the web socket connection
                websocket.SetCookie(new WebSocketSharp.Net.Cookie(cookie.Name, cookie.Value));
                // define handles
                websocket.OnOpen += new EventHandler(WsOpened);
                websocket.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(WsError);
                websocket.SetCredentials(Program._userName, Program._password, false);
                websocket.OnMessage += new EventHandler<MessageEventArgs>(WsMessageReceived);
                websocket.OnClose += new EventHandler<CloseEventArgs>(WsClosed);
                // do the web socket connect, if anything goes wrong is an exception thrown
                websocket.Connect();
            }

            public void Close()
            {
                if (m_connected)
                    websocket.Close();
            }
            private void WsOpened(object sender, EventArgs e)
            {
                m_messageQueue.Add("<info>opened");
                m_connected = true;
            }
            private void WsClosed(object sender, CloseEventArgs e)
            {
                m_messageQueue.Add("<info>closed");
                m_connected = false;
            }
            private void WsError(object sender, WebSocketSharp.ErrorEventArgs e)
            {
                m_messageQueue.Add("<info>error");
            }
            // Send inbound events on the message queue
            private void WsMessageReceived(object sender, MessageEventArgs e)
            {
                m_messageQueue.Add("<data>" + e.Data.ToString());
            }
        }
        // <summary>
        // Simple thread safe list
        // </summary>
        // <typeparam name="T"></typeparam>
        public class SimpleSafeList<T>
        {
            private readonly List<T> _items = new List<T>();
            private EventWaitHandle _ewh = new EventWaitHandle(false, EventResetMode.ManualReset);
            // <summary>
            // Add an item to the list
            // </summary>
            // <param name="item"></param>
            public void Add(T item)
            {
                lock (this._items)
                {
                    this._items.Add(item);
                    _ewh.Set();
                }
            }
            // <summary>
            // Get number of items in the list
            // </summary>
            public int Count
            {
                get
                {
                    lock (this._items)
                    {
                        return this._items.Count;
                    }
                }
            }
            // <summary>
            // Return item from list
            // </summary>
            // <param name="index"></param>
            // <returns></returns>
            public T this[int index]
            {
                get
                {
                    lock (this._items)
                    {
                        return this._items[index];
                    }
                }
            }
            // <summary>
            // Wait on item to be inserted to the list
            // </summary>
            // <returns></returns>
            public T WaitRead()
            {
                if (Count == 0)
                {
                    _ewh.WaitOne(200);
                }
                _ewh.Reset();
                if (Count > 0)
                {
                    T val = this[0];
                    this._items.Remove(val);
                    return val;
                }
                return default(T);
            }
        }
    }
}
