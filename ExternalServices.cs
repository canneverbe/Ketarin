using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Ketarin
{
    static class ExternalServices
    {
        public static string FileHippoDownloadUrl(string fileId)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/", fileId);

            // On the overview page, find the link to the
            // download page of the latest version
            string overviewPage = string.Empty;
            using (WebClient client = new WebClient())
            {
                overviewPage = client.DownloadString(url);
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
                downloadPage = client.DownloadString(downloadUrl);
            }

            findUrl = "/download/file/";
            pos = downloadPage.IndexOf(findUrl);
            if (pos < 0) return string.Empty;
            pos += findUrl.Length;
            string redirectUrl = string.Format("http://www.filehippo.com/download/file/{0}", downloadPage.Substring(pos, 64));

            return redirectUrl;
        }

        public static string FileHippoMd5(string fileId)
        {
            fileId = fileId.ToLower();
            string url = string.Format("http://www.filehippo.com/download_{0}/tech/", fileId);

            
            string md5 = string.Empty;
            using (WebClient client = new WebClient())
            {
                md5 = client.DownloadString(url);
                string find = "MD5 Checksum:</b></td><td>";
                int pos = md5.IndexOf(find);
                if (pos < 0) return string.Empty;

                return md5.Substring(pos + find.Length, 32);
            }
        }
    }
}
