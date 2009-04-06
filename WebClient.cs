using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using CDBurnerXP;
using System.Web;
using System.IO;

namespace Ketarin
{
    /// <summary>
    /// The modified WebClient used by Ketarin for 
    /// downloads. Submits a valid user agent by default.
    /// </summary>
    class WebClient : System.Net.WebClient
    {
        private string m_PostData = string.Empty;
        private string m_ReplacementString = string.Empty;
        private static string m_UserAgent = null;

        #region Properties

        /// <summary>
        /// Gets the plain POST data which is being ursed for a request.
        /// </summary>
        public string PostData
        {
            get { return m_PostData; }
        }

        /// <summary>
        /// Gets a user agent. To prevent websites from
        /// blocking Ketarin, we'll just use some random
        /// Internet Explorer / Firefox user agents.
        /// </summary>
        public static string UserAgent
        {
            get
            {
                if (m_UserAgent == null)
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
                    m_UserAgent = userAgents[index];
                }

                return m_UserAgent;
            }
        }

        #endregion

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

            // Need to append POST data?
            if (!string.IsNullOrEmpty(m_PostData))
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                Stream newStream = request.GetRequestStream();
                byte[] bytes = Encoding.ASCII.GetBytes(m_PostData);
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            return request;
        }

        public new string DownloadString(string address)
        {
            return DownloadString(new Uri(address));
        }

        public new string DownloadString(Uri address)
        {
            m_ReplacementString = string.Empty;

            try
            {
                return base.DownloadString(address);
            }
            catch (WebException)
            {
                if (!string.IsNullOrEmpty(m_ReplacementString))
                {
                    return m_ReplacementString;
                }

                throw;
            }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            
            HttpWebResponse httpResponse = response as HttpWebResponse;

            // If binary contents are sent, output information about the download
            if (httpResponse != null && response.ContentType == "application/octet-stream" && response.ContentLength > 100000)
            {
                m_ReplacementString = "ResponseUri: " + httpResponse.ResponseUri + "\r\n";
                m_ReplacementString += httpResponse.Headers.ToString();
                return null;
            }
            return response;
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

        /// <summary>
        /// Sets the POST data which is sent
        /// along with the request. Replaces variables.
        /// </summary>
        /// <param name="variable">The variable, which sends the request</param>
        public void SetPostData(UrlVariable variable)
        {
            string[][] pairs = GetKeyValuePairs(variable.PostData);
            if (variable.Parent != null)
            {
                foreach (string[] keyValue in pairs)
                {
                    keyValue[0] = variable.Parent.ReplaceAllInString(keyValue[0]);
                    keyValue[1] = variable.Parent.ReplaceAllInString(keyValue[1]);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (string[] keyValue in pairs)
            {
                sb.Append(HttpUtility.UrlEncode(keyValue[0]) + "=" + HttpUtility.UrlEncode(keyValue[1]) + "&");
            }

            m_PostData = sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// Determines the key-value pairs from a post data string.
        /// </summary>
        internal static string[][] GetKeyValuePairs(string postData)
        {
            List<string[]> results = new List<string[]>();
            // No data, no efforts
            if (postData == null) return results.ToArray();

            string[] pairs = postData.Split('&');
            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    keyValue[0] = HttpUtility.UrlDecode(keyValue[0]);
                    keyValue[1] = HttpUtility.UrlDecode(keyValue[1]);
                    results.Add(keyValue);
                }
            }

            return results.ToArray();
        }
    }
}
