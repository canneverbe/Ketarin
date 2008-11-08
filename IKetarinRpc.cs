using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace Ketarin
{
    [XmlRpcUrl("http://ketarin.canneverbe.com/rpc")]
    public interface IKetarinRpc : IXmlRpcProxy
    {
        [XmlRpcMethod("ketarin.GetApplications")]
        RpcApplication[] GetApplications(string searchSubject);        

        [XmlRpcMethod("ketarin.GetApplication")]
        string GetApplication(int shareId);

        [XmlRpcMethod("ketarin.SaveApplication")]
        int SaveApplication(string xml, string authorGuid); 
    }

    public struct RpcApplication
    {
        public static DateTime UnixToDotNet(int unixTimestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);
        }


        [XmlRpcMember("applicationname")]
        public string ApplicationName;
        [XmlRpcMember("updatedat")]
        public int UpdatedAt;
        [XmlRpcMember("shareid")]
        public int ShareId;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public DateTime UpdatedAtDate
        {
            get
            {
                return UnixToDotNet(UpdatedAt);
            }
        }
    }
}
