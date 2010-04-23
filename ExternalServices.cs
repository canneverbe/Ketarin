using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Ketarin.Forms;

namespace Ketarin
{
    /// <summary>
    /// Provides a couple of functions for external services like FileHippo.
    /// </summary>
    static class ExternalServices
    {
        /// <summary>
        /// If the given URL contains a FileHippo ID, it is returned.
        /// </summary>
        /// <returns>The original URL if no ID is found</returns>
        public static string GetFileHippoIdFromUrl(string url)
        {
            // If someone pasted the full URL, extract the ID from it
            Regex regex = new Regex(@"filehippo\.com/download_([0-9a-z._-]+)/", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match id = regex.Match(url);
            if (id.Groups.Count > 1)
            {
                return id.Groups[1].Value;
            }

            return url;
        }

        /// <summary>
        /// Determines the download URL for a FileHippo application with the given ID.
        /// </summary>
        /// <param name="avoidBeta">Whether or not to avoid beta versions. If only beta versions are available, they will be downloaded anyways.</param>
        public static string FileHippoDownloadUrl(string fileId, bool avoidBeta)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/", fileId);

            // On the overview page, find the link to the
            // download page of the latest version
            string overviewPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                overviewPage = client.DownloadString(url);

                // If FileHippo redirects to a new name, extract it the actual ID.
                if (client.ResponseUri != null)
                {
                    string newId = GetFileHippoIdFromUrl(client.ResponseUri.ToString());
                    if (!string.IsNullOrEmpty(newId) && fileId != newId && newId != client.ResponseUri.ToString())
                    {
                        LogDialog.Log(string.Format("FileHippo ID '{0}' has been renamed to '{1}'.", fileId, newId));
                        fileId = newId;
                    }
                }
            }

            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId);
            }

            string findUrl = string.Format("/download_{0}/download/", fileId);
            int pos = overviewPage.IndexOf(findUrl);
            if (pos < 0)
            {
                throw new WebException("FileHippo ID '" + fileId + "' does not exist.", WebExceptionStatus.ReceiveFailure);
            }
            pos += findUrl.Length;

            string downloadUrl = string.Format("http://www.filehippo.com/download_{0}/download/{1}/", fileId, overviewPage.Substring(pos, 32));
            
            // Now on the download page, find the link which redirects to the latest file
            string downloadPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                downloadPage = client.DownloadString(downloadUrl);
            }

            findUrl = "/download/file/";
            pos = downloadPage.IndexOf(findUrl);
            if (pos < 0) return string.Empty;
            pos += findUrl.Length;
            string redirectUrl = string.Format("http://www.filehippo.com/download/file/{0}", downloadPage.Substring(pos, 64));

            return redirectUrl;
        }

        /// <summary>
        /// Determines the content of the most recent version of an application's
        /// overview page on FileHippo, which is not a beta version.
        /// </summary>
        /// <param name="overviewPage">Starting point of an application (most recent version overview page)</param>
        private static string GetNonBetaPageContent(string overviewPage, string fileId)
        {
            // Find the most recent version which is not a beta
            string[] otherUrls = FileHippoGetAllVersions(overviewPage, fileId);

            foreach (string altUrl in otherUrls)
            {
                using (WebClient altUrlDownloader = new WebClient())
                {
                    string newPage = altUrlDownloader.DownloadString(altUrl);
                    if (!FileHippoIsBeta(newPage))
                    {
                        return newPage;
                    }
                }
            }

            return overviewPage;
        }

        /// <summary>
        /// Determines the version of a given application on FileHippo.
        /// </summary>
        public static string FileHippoVersion(string fileId, bool avoidBeta)
        {
            if (string.IsNullOrEmpty(fileId)) return null;

            string url = string.Format("http://www.filehippo.com/download_{0}/tech/", fileId);

            string overviewPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                overviewPage = client.DownloadString(url);
            }
            
            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId);
            }

            // Extract version from title like: <title>Download Firefox 3.0.4 - FileHippo.com</title>
            Regex regex = new Regex(@"((?<=\>Title:\<.*?\s)(?:\(?\d+?\.\d+?.*?)(?=\</td\>)|(?:[a-z]+?\s\d{1,2},\s\d{4}))", RegexOptions.IgnoreCase);
            Match match = regex.Match(overviewPage);
            if (!match.Success) return null;

            return match.Groups[1].Value;
        }

        /// <summary>
        /// Returns the application name for a given FileHippo ID.
        /// </summary>
        /// <returns>Returns string.empty if the name could not be determined</returns>
        public static string FileHippoAppName(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) return string.Empty;

            using (WebClient client = new WebClient())
            {
                string mainPage = client.DownloadString("http://www.filehippo.com/download_" + fileId);

                // It will match almost anything from FileHippo (except drivers without version numbers...)
                Regex regex = new Regex(@"<h1>(.+) [\.\dab]+(\s.+)?<\/h1>", RegexOptions.IgnoreCase);
                Match match = regex.Match(mainPage);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Determines the MD5 hash of a given application.
        /// </summary>
        /// <returns>null if no hash has been calculated on the FileHippo website</returns>
        public static string FileHippoMd5(string fileId, bool avoidBeta)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/tech/", fileId);

            
            string md5Page = string.Empty;
            using (WebClient client = new WebClient())
            {
                md5Page = client.DownloadString(url);

                // If FileHippo redirects to a new name, extract it the actual ID.
                if (client.ResponseUri != null)
                {
                    string newId = GetFileHippoIdFromUrl(client.ResponseUri.ToString());
                    if (!string.IsNullOrEmpty(newId) && fileId != newId && newId != client.ResponseUri.ToString())
                    {
                        return FileHippoMd5(newId, avoidBeta);
                    }
                }

                if (avoidBeta && FileHippoIsBeta(md5Page))
                {
                    md5Page = GetNonBetaPageContent(md5Page, fileId);
                }

                string find = "MD5 Checksum:</b></td><td>";
                int pos = md5Page.IndexOf(find);
                if (pos < 0) return string.Empty;

                string result = md5Page.Substring(pos + find.Length, 32);
                Regex validMd5 = new Regex("[0-9a-f]{32}", RegexOptions.IgnoreCase);
                if (!validMd5.IsMatch(result)) return null;

                return result;
            }
        }

        /// <summary>
        /// Determines whether or not a page refers to a beta version.
        /// </summary>
        private static bool FileHippoIsBeta(string pageContent)
        {
            return pageContent.Contains("filehippo.com/img/beta.gif");
        }

        /// <summary>
        /// Returns a list links to all versions of an application on FileHippo.
        /// </summary>
        /// <param name="pageContent">Starting point of an application (most recent version overview page)</param>
        private static string[] FileHippoGetAllVersions(string pageContent, string fileId)
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
