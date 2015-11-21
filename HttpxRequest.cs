using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace Ketarin
{
    /// <summary>
    /// Represents an HTTP requests, with further functionality
    /// encoded in the URL (like setting Accept headers).
    /// </summary>
    public class HttpxRequestCreator : IWebRequestCreate
    {
        #region IWebRequestCreate Member

        public WebRequest Create(Uri uri)
        {
            Uri httpUri = new Uri(uri.ToString().Replace("httpx://", "http://"));

            CookieContainer cookies = new CookieContainer();
            string accept = null;
            string referer = null;

            NameValueCollection parameters = HttpUtility.ParseQueryString(uri.Query);

            foreach (string key in parameters.AllKeys)
            {
                if (key == null) continue;

                if (key.StartsWith("cookie:"))
                {
                    string name = key.Substring("cookie:".Length);
                    string value = parameters[key];
                    cookies.Add(new Cookie(name, value, "/", uri.Host));

                    parameters.Remove(key);
                }
                else if (key.StartsWith("header:accept"))
                {
                    accept = parameters[key];
                }
                else if (key.StartsWith("header:referer"))
                {
                    referer = parameters[key];
                }
            }

            // Remove cookie parameters from URL
            string newPathAndQuery = httpUri.PathAndQuery.Substring(0, httpUri.PathAndQuery.Length - httpUri.Query.Length);
            string newQuery = string.Empty;
            foreach (string key in parameters.AllKeys)
            {
                newQuery += "&" + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(parameters[key]);
            }
            if (newQuery.Length > 0)
            {
                newQuery = "?" + newQuery.TrimStart('&');
                newPathAndQuery = newPathAndQuery + newQuery;
            }
            httpUri = new Uri(httpUri.Scheme + "://" + httpUri.Host + newPathAndQuery);

            HttpWebRequest req = WebRequest.CreateDefault(httpUri) as HttpWebRequest;
            req.CookieContainer = cookies;
            if (accept != null)
            {
                req.Accept = accept;
            }

            if (referer != null)
            {
                req.Referer = referer;
            }

            return req;
        }

        #endregion
    }
}
