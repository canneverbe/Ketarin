using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using CDBurnerXP;

namespace Ketarin
{
    /// <summary>
    /// The modified WebClient used by Ketarin for 
    /// downloads. Submits a valid user agent by default.
    /// </summary>
    internal class WebClient : System.Net.WebClient
    {
        private static string defaultUserAgent;
        private string replacementString = string.Empty;
        
        #region Properties

        /// <summary>
        /// If the WebClient has been redirected after a request,
        /// this specifies the new URL.
        /// </summary>
        public Uri ResponseUri { get; private set; }

        /// <summary>
        /// Gets the plain POST data which is being ursed for a request.
        /// </summary>
        public string PostData { get; private set; } = string.Empty;

        /// <summary>
        /// Default user agent for all requests.
        /// </summary>
        public static string DefaultUserAgent
        {
            get { return defaultUserAgent ?? (defaultUserAgent = Settings.GetValue("DefaultUserAgent", "Mozilla/4.0 (compatible; Ketarin; +https://ketarin.org/)") as string); }
            set { defaultUserAgent = value; }
        }

        #endregion

        public WebClient()
            : this(null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public WebClient(string userAgent)
        {
            this.Headers.Add("User-Agent", string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent);

            // MS Bugfix - https://connect.microsoft.com/VisualStudio/feedback/details/386695/system-uri-incorrectly-strips-trailing-dots?wa=wsignin1.0#
            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (string scheme in new[] { "http", "https" })
                {
                    UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
                    if (parser != null)
                    {
                        int flagsValue = (int)flagsField.GetValue(parser);
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }
                }
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            address = FixNoProtocolUri(address);
            WebRequest request = base.GetWebRequest(address);

            HttpWebRequest httpReq = request as HttpWebRequest;
            if (httpReq != null)
            {
                // DownloadString will not decompress automatically
                httpReq.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

                httpReq.Accept = "*/*";
            }

            // Make sure that the user defined timeout is used for all web requests!
            request.Timeout = Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10)) * 1000; // 10 seconds by default
            Updater.AddRequestToCancel(request);

            // Need to append POST data?
            if (!string.IsNullOrEmpty(this.PostData))
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                Stream newStream = request.GetRequestStream();
                byte[] bytes = Encoding.ASCII.GetBytes(this.PostData);
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            return request;
        }

        public new string DownloadString(string address)
        {
            try
            {
                return this.DownloadString(new Uri(address));
            }
            catch (UriFormatException)
            {
                throw new UriFormatException("The format of the URI \"" + address + "\" cannot be determined.");
            }
        }

        public new string DownloadString(Uri address)
        {
            this.replacementString = string.Empty;

            try
            {
                return base.DownloadString(address);
            }
            catch (WebException ex)
            {
                // If only SSL3 is supported, use this temporarily.
                if (ex.Status == WebExceptionStatus.SecureChannelFailure && ServicePointManager.SecurityProtocol != SecurityProtocolType.Ssl3)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    try
                    {
                        return base.DownloadString(address);
                    }
                    finally
                    {
                        ServicePointManager.SecurityProtocol = Updater.DefaultHttpProtocols;
                    }
                }

                if (!string.IsNullOrEmpty(this.replacementString))
                {
                    return this.replacementString;
                }

                throw;
            }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            
            HttpWebResponse httpResponse = response as HttpWebResponse;

            if (httpResponse != null)
            {
                this.ResponseUri = httpResponse.ResponseUri;
            }

            // If binary contents are sent, output information about the download
            if (httpResponse != null && response.ContentType == "application/octet-stream" && response.ContentLength > 100000)
            {
                this.replacementString = "ResponseUri: " + httpResponse.ResponseUri + "\r\n";
                this.replacementString += httpResponse.Headers.ToString();
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
                // If only SSL3 is supported, use this temporarily.
                if (ex.Status == WebExceptionStatus.SecureChannelFailure && ServicePointManager.SecurityProtocol != SecurityProtocolType.Ssl3)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                    try
                    {
                        return GetResponse(request);
                    }
                    finally
                    {
                        ServicePointManager.SecurityProtocol = Updater.DefaultHttpProtocols;
                    }
                }

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

                WebRequest nextRequest = WebRequest.CreateDefault(FixNoProtocolUri(new Uri(nextUrl)));
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

            this.PostData = sb.ToString().TrimEnd('&');
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

        /// <summary>
        /// Not using a protocol will default to file:// which is looking for a network resource.
        /// This won't work properly in Ketarin's context, but looks like missing protocols
        /// have become hip http://www.paulirish.com/2010/the-protocol-relative-url/ lately.
        /// </summary>
        public static Uri FixNoProtocolUri(Uri urlToRequest)
        {
            if (urlToRequest.OriginalString.StartsWith("//") && urlToRequest.Scheme == "file")
            {
                return new Uri("http:" + urlToRequest.OriginalString);
            }

            return urlToRequest;
        }
    }
}
