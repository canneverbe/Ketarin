using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace Ketarin
{
    /// <summary>
    /// The modified WebClient used by Ketarin for 
    /// downloads. Submits a valid user agent by default.
    /// </summary>
    class WebClient : System.Net.WebClient
    {
        /// <summary>
        /// Gets Ketarin's default user agent (includes application name and version)
        /// </summary>
        public static string UserAgent
        {
            get
            {
                return "Ketarin/" + Application.ProductVersion + "(Windows; N) MSIE (.NET CLR 2.0)";
            }
        }

        public WebClient()
            : base()
        {
            Headers.Add("User-Agent", UserAgent);
        }
    }
}
