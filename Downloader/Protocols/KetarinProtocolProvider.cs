using System;
using System.IO;
using System.Linq;
using System.Net;
using Ketarin;
using Ketarin.Forms;
using MyDownloader.Core;
using Settings = CDBurnerXP.Settings;
using WebClient = Ketarin.WebClient;

namespace MyDownloader.Extension.Protocols
{
    internal class KetarinProtocolProvider : HttpProtocolProvider
    {
        private static readonly string[] NoAutoReferer = {"sourceforge.net"};
        private readonly CookieContainer cookies;
        private readonly ApplicationJob job;

        public KetarinProtocolProvider(ApplicationJob job, CookieContainer cookies)
        {
            this.job = job;
            this.cookies = cookies;
        }

        public override RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream)
        {
            WebRequest request = this.GetRequest(rl);

            RemoteFileInfo result = new RemoteFileInfo();
            WebResponse response = null;

            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                response = request.GetResponse();
                result.MimeType = response.ContentType;
                result.LastModified = ((HttpWebResponse)response).LastModified;
                result.FileSize = response.ContentLength;
                result.AcceptRanges = string.Compare(response.Headers["Accept-Ranges"], "bytes", StringComparison.OrdinalIgnoreCase) == 0;
            }

            FtpWebRequest ftpRequest = request as FtpWebRequest;
            if (ftpRequest != null)
            {
                result.AcceptRanges = true;
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                
                using (FtpWebResponse sizeResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    result.FileSize = sizeResponse.ContentLength;
                }

                ftpRequest = (FtpWebRequest)this.GetRequest(rl);

                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                using (FtpWebResponse dateResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    result.LastModified = dateResponse.LastModified;
                }

                // For FTP: Don't create download stream yet.
            }

            if (!result.AcceptRanges || response == null)
            {
                LogDialog.Log(this.job, $"Server for {rl.Url} does not support segmented transfer");
            }

            stream = response?.GetResponseStream();

            return result;
        }

        public override Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
        {
            WebRequest request = this.GetRequest(rl);

            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                if (initialPosition != 0)
                {
                    if (endPosition == 0)
                    {
                        httpRequest.AddRange((int) initialPosition);
                    }
                    else
                    {
                        httpRequest.AddRange((int) initialPosition, (int) endPosition);
                    }
                }
            }

            FtpWebRequest ftpRequest = request as FtpWebRequest;
            if (ftpRequest != null)
            {
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpRequest.ContentOffset = initialPosition;
            }

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        protected override WebRequest GetRequest(ResourceLocation location)
        {
            return CreateRequest(new Uri(location.Url), this.job, this.cookies);
        }

        /// <summary>
        /// Determines the base host (TLD + server name) of an URI.
        /// </summary>
        /// <returns>Example: sourceforge.net</returns>
        private static string GetBaseHost(Uri uri)
        {
            string[] parts = uri.Host.Split('.');
            if (parts.Length <= 2)
            {
                return uri.Host;
            }

            return parts[parts.Length - 2] + "." + parts[parts.Length - 1];
        }

        public static WebRequest CreateRequest(Uri location, ApplicationJob job, CookieContainer cookies)
        {
            WebRequest req = WebRequest.CreateDefault(WebClient.FixNoProtocolUri(location));

            req.Timeout = Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10))*1000;
            // 10 seconds by default

            HttpWebRequest httpRequest = req as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.Accept = "*/*";

                // Store cookies for future requests. Some sites
                // check for previously stored cookies before allowing to download.
                if (httpRequest.CookieContainer == null)
                {
                    httpRequest.CookieContainer = cookies;
                }
                else
                {
                    httpRequest.CookieContainer.Add(cookies.GetCookies(httpRequest.RequestUri));
                }

                // If we have an HTTP request, some sites may require a correct referer
                // for the download.
                // If there are variables defined (from which most likely the download link
                // or version is being extracted), we'll just use the first variable's URL as referer.
                // The user still has the option to set a custom referer.
                // Note: Some websites don't "like" certain referers
                if (!NoAutoReferer.Contains(GetBaseHost(req.RequestUri)))
                {
                    foreach (UrlVariable urlVar in job.Variables.Values)
                    {
                        httpRequest.Referer = urlVar.Url;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(job.HttpReferer))
                {
                    httpRequest.Referer = job.Variables.ReplaceAllInString(job.HttpReferer);
                }

                LogDialog.Log(job,
                    "Using referer: " + (string.IsNullOrEmpty(httpRequest.Referer) ? "(none)" : httpRequest.Referer));
                httpRequest.UserAgent = string.IsNullOrEmpty(job.UserAgent)
                    ? WebClient.DefaultUserAgent
                    : job.Variables.ReplaceAllInString(job.UserAgent);

                // PAD files may be compressed
                httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return req;
        }
    }
}