using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace Ketarin
{
    static class ExternalServices
    {
        public static string FileHippoDownloadUrl(string fileId, bool avoidBeta)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/", fileId);

            // On the overview page, find the link to the
            // download page of the latest version
            string overviewPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", DbManager.UserAgent);
                overviewPage = client.DownloadString(url);
            }

            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId);
            }

            string findUrl = string.Format("/download_{0}/download/", fileId);
            int pos = overviewPage.IndexOf(findUrl);
            if (pos < 0) return string.Empty;
            pos += findUrl.Length;

            string downloadUrl = string.Format("http://www.filehippo.com/download_{0}/download/{1}/", fileId, overviewPage.Substring(pos, 32));
            
            // Now on the download page, find the link which redirects to the latest file
            string downloadPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", DbManager.UserAgent);
                downloadPage = client.DownloadString(downloadUrl);
            }

            findUrl = "/download/file/";
            pos = downloadPage.IndexOf(findUrl);
            if (pos < 0) return string.Empty;
            pos += findUrl.Length;
            string redirectUrl = string.Format("http://www.filehippo.com/download/file/{0}", downloadPage.Substring(pos, 64));

            return redirectUrl;
        }

        private static string GetNonBetaPageContent(string overviewPage, string fileId)
        {
            // Find the most recent version which is not a beta
            string[] otherUrls = FileHippoGetAllVersions(overviewPage, fileId);

            foreach (string altUrl in otherUrls)
            {
                using (WebClient altUrlDownloader = new WebClient())
                {
                    altUrlDownloader.Headers.Add("User-Agent", DbManager.UserAgent);
                    string newPage = altUrlDownloader.DownloadString(altUrl);
                    if (!FileHippoIsBeta(newPage))
                    {
                        return newPage;
                    }
                }
            }

            return overviewPage;
        }

        public static string FileHippoVersion(string fileId, bool avoidBeta)
        {
            if (string.IsNullOrEmpty(fileId)) return null;

            string url = string.Format("http://www.filehippo.com/download_{0}/", fileId);

            string overviewPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", DbManager.UserAgent);
                overviewPage = client.DownloadString(url);
            }
            
            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId);
            }

            // Extract version from title like: <title>Download Firefox 3.0.4 - FileHippo.com</title>
            Regex regex = new Regex(@"<title>.+?(\d[\d\.]+.*) - File", RegexOptions.IgnoreCase);
            Match match = regex.Match(overviewPage);
            if (!match.Success) return null;

            return match.Groups[1].Value;
        }

        public static string FileHippoMd5(string fileId, bool avoidBeta)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/tech/", fileId);

            
            string md5Page = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", DbManager.UserAgent);
                md5Page = client.DownloadString(url);

                if (avoidBeta && FileHippoIsBeta(md5Page))
                {
                    md5Page = GetNonBetaPageContent(md5Page, fileId);
                }

                string find = "MD5 Checksum:</b></td><td>";
                int pos = md5Page.IndexOf(find);
                if (pos < 0) return string.Empty;

                return md5Page.Substring(pos + find.Length, 32);
            }
        }

        public static bool FileHippoIsBeta(string pageContent)
        {
            return pageContent.Contains("http://i.filehippo.com/img/beta.gif");
        }

        public static string[] FileHippoGetAllVersions(string pageContent, string fileId)
        {
            Regex regex = new Regex(string.Format(@"/download_{0}/(tech/)?(\d+)", fileId), RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(pageContent);

            List<string> urls = new List<string>();

            foreach (Match match in matches)
            {
                string idPart = fileId;
                if (!string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    idPart += "/tech";
                }
                string url = string.Format("http://filehippo.com/download_{0}/{1}/", idPart, match.Groups[2].Value);
                urls.Add(url);
            }

            return urls.ToArray();
        }
    }
}
