// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

namespace MyDownloader.Extension.Protocols
{
    public interface IHttpFtpProtocolParameters
    {
        string ProxyAddress { get; set; }

        string ProxyUserName { get; set; }

        string ProxyPassword { get; set; }

        string ProxyDomain { get; set; }

        bool UseProxy { get; set; }

        bool ProxyByPassOnLocal { get; set; }

        int ProxyPort { get; set; }
    }
}