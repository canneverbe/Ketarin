using System;

namespace Ketarin
{
    public class ApplicationJobError
    {
        #region Properties

        public string RequestedUrl { get; set; }

        internal ApplicationJob ApplicationJob { get; set; }

        public Exception Error { get; set; }

        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this.RequestedUrl))
                {
                    return this.Error.Message;
                }
                return this.Error.Message + " (" + this.RequestedUrl + ")";
            }
        }

        #endregion

        public ApplicationJobError(ApplicationJob job, Exception error)
        {
            this.RequestedUrl = string.Empty;
            this.ApplicationJob = job;
            this.Error = error;
        }

        public ApplicationJobError(ApplicationJob job, Exception error, string requestedUrl)
        {
            this.ApplicationJob = job;
            this.Error = error;
            this.RequestedUrl = requestedUrl;
        }
    }
}
