using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Ketarin.Forms;

namespace Ketarin
{
    /// <summary>
    /// Provides a couple of functions for external services like FileHippo.
    /// </summary>
    internal static class ExternalServices
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

            // Consider multi-language versions too
            Regex regexLang = new Regex(@"filehippo\.com/([a-z]+)/download_([0-9a-z._-]+)?", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match idLang = regexLang.Match(url);
            if (idLang.Groups.Count > 2)
            {
                return idLang.Groups[1].Value + ":" + idLang.Groups[2].Value;
            }

            return url;
        }

        /// <summary>
        /// Builds the base download URL for a given file ID.
        /// </summary>
        public static string GetFileHippoBaseDownloadUrl(string fileId)
        {
            if (fileId.Contains(":"))
            {
                string[] splitData = fileId.Split(':');
                return string.Format("https://filehippo.com/{0}/download_{1}/", splitData);
            }
            
            return string.Format("https://filehippo.com/download_{0}/", fileId);
        }

        /// <summary>
        /// Returns the FileHipp ID without the language identifier.
        /// </summary>
        public static string GetFileHippoCleanFileId(string fileId)
        {
            if (fileId.Contains(":"))
            {
                return fileId.Substring(fileId.IndexOf(':') + 1);
            }
            else
            {
                return fileId;
            }
        }

        /// <summary>
        /// Determines the download URL for a FileHippo application with the given ID.
        /// </summary>
        /// <param name="avoidBeta">Whether or not to avoid beta versions. If only beta versions are available, they will be downloaded anyways.</param>
        public static string FileHippoDownloadUrl(string fileId, bool avoidBeta)
        {
            fileId = fileId.ToLower();
            string url = GetFileHippoBaseDownloadUrl(fileId);

            // On the overview page, find the link to the
            // download page of the latest version
            string overviewPage;
            using (WebClient client = new WebClient())
            {
                overviewPage = client.DownloadString(url);

                // If FileHippo redirects to a new name, extract it the actual ID.
                if (client.ResponseUri != null)
                {
                    string newId = GetFileHippoIdFromUrl(client.ResponseUri.ToString());
                    if (!string.IsNullOrEmpty(newId) && GetFileHippoBaseDownloadUrl(newId) != url && newId != client.ResponseUri.ToString())
                    {
                        LogDialog.Log(string.Format("FileHippo ID '{0}' has been renamed to '{1}'.", fileId, newId));
                        fileId = newId;
                    }
                }
            }

            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId, false);
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    overviewPage = client.DownloadString($"https://filehippo.com/download_{GetFileHippoCleanFileId(fileId)}/post_download/");
                }
            }
            catch (Exception)
            {
                throw new WebException("FileHippo ID '" + fileId + "' does not exist.", WebExceptionStatus.ReceiveFailure);
            }

            // Now on the download page, find the link which redirects to the latest file
            // setTimeout(function() { downloadIframe.src = 'https://filehippo.com/launch_download/?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
            string searchText = "downloadIframe.src";
            int pos = overviewPage.IndexOf(searchText);
            int urlEndPos = overviewPage.IndexOf("'", pos + 30);
            if (pos < 0)
            {
                throw new WebException("Download URL for FileHippo ID '" + fileId + "' cannot be found.", WebExceptionStatus.ReceiveFailure);
            }

            string downloadUrl = overviewPage.Substring(pos + searchText.Length, urlEndPos - pos - searchText.Length).Trim().Trim('\'', '=', ' ');            
            return downloadUrl;
        }

        /// <summary>
        /// Determines the content of the most recent version of an application's
        /// overview page on FileHippo, which is not a beta version.
        /// </summary>
        /// <param name="overviewPage">Starting point of an application (most recent version overview page)</param>
        private static string GetNonBetaPageContent(string overviewPage, string fileId, bool techTab)
        {
            // Find the most recent version which is not a beta
            string[] otherUrls = FileHippoGetAllVersions(overviewPage, fileId);

            foreach (string altUrl in otherUrls)
            {
                string downloadUrl = altUrl;

                if (techTab)
                {
                    downloadUrl = downloadUrl.Replace(fileId, fileId + "/tech");
                }

                using (WebClient altUrlDownloader = new WebClient())
                {
                    string newPage = altUrlDownloader.DownloadString(downloadUrl);
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

            string url = GetFileHippoBaseDownloadUrl(fileId);

            string overviewPage;
            using (WebClient client = new WebClient())
            {
                overviewPage = client.DownloadString(url);
            }
            
            if (avoidBeta && FileHippoIsBeta(overviewPage))
            {
                overviewPage = GetNonBetaPageContent(overviewPage, fileId, false);
            }

            // Extract version from page like: <span style="font-weight: normal">&nbsp12.4.3</span>
            Regex regex = new Regex(@"<span style=""font-weight: normal"">(([\d]+\.|[\d]+\s).*)</", RegexOptions.IgnoreCase);
            Match match = regex.Match(overviewPage);
            if (!match.Success) return null;

            return match.Groups[1].Value.Trim();
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
                string mainPage = client.DownloadString(GetFileHippoBaseDownloadUrl(fileId));

                // It will match almost anything from FileHippo (except drivers without version numbers...)
                Regex regex = new Regex(@"<meta itemprop=""softwareVersion"" content=""(.+) [\.\dab]+(\s.+)?""/>", RegexOptions.IgnoreCase);
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
            string url = GetFileHippoBaseDownloadUrl(fileId);
            
            string md5Page;
            using (WebClient client = new WebClient())
            {
                md5Page = client.DownloadString(url);

                // If FileHippo redirects to a new name, extract it the actual ID.
                if (client.ResponseUri != null)
                {
                    string newId = GetFileHippoIdFromUrl(client.ResponseUri.ToString());
                    if (!string.IsNullOrEmpty(newId) && GetFileHippoBaseDownloadUrl(newId) != url && newId != client.ResponseUri.ToString())
                    {
                        return FileHippoMd5(newId, avoidBeta);
                    }
                }

                if (avoidBeta && FileHippoIsBeta(md5Page))
                {
                    md5Page = GetNonBetaPageContent(md5Page, fileId, true);
                }

                Regex validMd5 = new Regex("(?si)md5.+?([0-9a-f]{32})", RegexOptions.IgnoreCase);
                Match match = validMd5.Match(md5Page);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Determines whether or not a page refers to a beta version.
        /// </summary>
        private static bool FileHippoIsBeta(string pageContent)
        {
            return pageContent.Contains("<span class=\"beta-text hidden-xs\">BETA</span>");
        }

        /// <summary>
        /// Returns a list links to all versions of an application on FileHippo.
        /// </summary>
        /// <param name="pageContent">Starting point of an application (most recent version overview page)</param>
        private static string[] FileHippoGetAllVersions(string pageContent, string fileId)
        {
            int historypage = 1;
            int historypagemax = 1;
            List<string> urls = new List<string>();
            string FileHippoBaseDownloadUrl = GetFileHippoBaseDownloadUrl(fileId);

            while (historypage <= historypagemax)
            {
                string historyUrl = FileHippoBaseDownloadUrl + "/history/" + historypage + "/";
                using (WebClient client = new WebClient())
                {
                    pageContent = client.DownloadString(historyUrl);
                }

                if (historypage == 1)
                {
                    Regex pagecount = new Regex(@"class=""pager-page-link"">(?!.*class=""pager-page-link"">)(\d+)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match countmatch = pagecount.Match(pageContent);
                    if (countmatch.Success)
                    {
                        historypagemax = Convert.ToInt32(countmatch.Groups[1].Value);
                    }
                }

                Regex regex = new Regex(string.Format(@"/download_{0}/(tech/)?(\d+)", GetFileHippoCleanFileId(fileId)), RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(pageContent);

                foreach (Match match in matches)
                {
                    string idPart = fileId;
                    if (!string.IsNullOrEmpty(match.Groups[1].Value))
                    {
                        idPart += "/tech";
                    }

                    string url = GetFileHippoBaseDownloadUrl(idPart) + string.Format("{0}/", match.Groups[2].Value);
                    urls.Add(url);
                    using (WebClient client = new WebClient())
                    {
                        pageContent = client.DownloadString(url);
                    }

                    if (!FileHippoIsBeta(pageContent))
                    {
                        urls = new List<string> {url};
                        return urls.ToArray();
                    }
                }

                historypage++;
            }

            return urls.ToArray();
        }
    }
}
