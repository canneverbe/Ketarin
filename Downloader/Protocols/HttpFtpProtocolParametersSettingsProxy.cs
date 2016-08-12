using System;
using System.Collections.Generic;
using System.Text;
using MyDownloader.Core;

namespace MyDownloader.Extension.Protocols
{
    class HttpFtpProtocolParametersSettingsProxy : IHttpFtpProtocolParameters
    {
        #region IHttpFtpProtocolParameters Members

        public string ProxyAddress
        {
            get
            {
                return Settings.ProxyAddress;
            }
            set
            {
                Settings.ProxyAddress = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return Settings.ProxyUserName;
            }
            set
            {
                Settings.ProxyUserName = value;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return Settings.ProxyPassword;
            }
            set
            {
                Settings.ProxyPassword = value;
            }
        }

        public string ProxyDomain
        {
            get
            {
                return Settings.ProxyDomain;
            }
            set
            {
                Settings.ProxyDomain = value;
            }
        }

        public bool UseProxy
        {
            get
            {
                return Settings.UseProxy;
            }
            set
            {
                Settings.UseProxy = value;
            }
        }

        public bool ProxyByPassOnLocal
        {
            get
            {
                return Settings.ProxyByPassOnLocal;
            }
            set
            {
                Settings.ProxyByPassOnLocal = value;
            }
        }

        public int ProxyPort
        {
            get
            {
                return Settings.ProxyPort;
            }
            set
            {
                Settings.ProxyPort = value;
            }
        }

        #endregion
    }
}
