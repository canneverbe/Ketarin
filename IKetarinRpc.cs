using System;
using CookComputing.XmlRpc;

namespace Ketarin
{
    [XmlRpcUrl("http://ketarin.canneverbe.com/rpc")]
    public interface IKetarinRpc : IXmlRpcProxy
    {
        [XmlRpcMethod("ketarin.GetMostDownloadedApplications")]
        RpcApplication[] GetMostDownloadedApplications();

        [XmlRpcMethod("ketarin.GetApplications")]
        RpcApplication[] GetApplications(string searchSubject);

        [XmlRpcMethod("ketarin.GetSimilarApplications")]
        RpcApplication[] GetSimilarApplications(string searchSubject, string appGuid);

        [XmlRpcMethod("ketarin.GetUpdatedApplications")]
        string[] GetUpdatedApplications(RpcAppGuidAndDate[] existingApps); 

        [XmlRpcMethod("ketarin.GetApplication")]
        string GetApplication(int shareId);

        [XmlRpcMethod("ketarin.SaveApplication")]
        int SaveApplication(string xml, string authorGuid); 
    }

    public struct RpcAppGuidAndDate
    {
        [XmlRpcMember("applicationguid")]
        public string ApplicationGuid;
        [XmlRpcMember("updatedat")]
        public int UpdatedAt;

        public RpcAppGuidAndDate(Guid guid, DateTime? date)
        {
            ApplicationGuid = guid.ToString();
            UpdatedAt = date.HasValue ? RpcApplication.DotNetToUnix(date.Value) : 0;
        }
    }

    public struct RpcApplication
    {
        public static DateTime UnixToDotNet(int unixTimestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);
        }

        public static int DotNetToUnix(DateTime date)
        {
            return Convert.ToInt32((date - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        [XmlRpcMember("applicationname")]
        public string ApplicationName;
        [XmlRpcMember("updatedat")]
        public int UpdatedAt;
        [XmlRpcMember("shareid")]
        public int ShareId;
        [XmlRpcMember("downloadcount")]
        public int UseCount;

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
