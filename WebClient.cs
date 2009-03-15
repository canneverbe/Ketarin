using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using CDBurnerXP;

namespace Ketarin
{
    /// <summary>
    /// The modified WebClient used by Ketarin for 
    /// downloads. Submits a valid user agent by default.
    /// </summary>
    class WebClient : System.Net.WebClient
    {
        /// <summary>
        /// Gets a user agent. To prevent websites from
        /// blocking Ketarin, we'll just use some random
        /// Internet Explorer / Firefox user agents.
        /// </summary>
        public static string UserAgent
        {
            get
            {
                List<string> userAgents = new List<string>();
                userAgents.Add("Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 6.0; en-US)");
                userAgents.Add("Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0;)");
                userAgents.Add("Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)");
                userAgents.Add("Mozilla/4.0 (Windows; MSIE 6.0; Windows NT 6.0)");
                userAgents.Add("Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.6pre) Gecko/2009011606 Firefox/3.1");
                userAgents.Add("Mozilla/5.0 (Windows; U; Windows NT 7.0; en-US; rv:1.9.0.6) Gecko/2009011913 Firefox/3.0.6");
                userAgents.Add("Mozilla/5.0 (Windows; U; Windows NT 6.0; de; rv:1.9.0.6) Gecko/2009011913 Firefox/3.0.6 (.NET CLR 3.5.30729)");
                
                Random rand = new Random();
                int index = rand.Next() % userAgents.Count;
                return userAgents[index];
            }
        }

        public WebClient()
            : base()
        {
            Headers.Add("User-Agent", UserAgent);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            // Make sure that the user defined timeout is used for all web requests!
            request.Timeout = Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10)) * 1000; // 10 seconds by default
            return request;
        }

        /// <summary>
        /// Works around the HTTP to FTP redirection limitation.
        /// </summary>
        public static WebResponse GetResponse(WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null || ex.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }

                // Create a new WebRequest, starting with the FTP URL
                string nextUrl = ex.Response.Headers["location"];
                if (string.IsNullOrEmpty(nextUrl))
                {
                    throw;
                }

                WebRequest nextRequest = WebRequest.CreateDefault(new Uri(nextUrl));
                nextRequest.Timeout = request.Timeout;
                if (request.Credentials != null)
                {
                    nextRequest.Credentials = request.Credentials;
                }
                nextRequest.Proxy = request.Proxy;

                return nextRequest.GetResponse();
            }
        }
    }
}
