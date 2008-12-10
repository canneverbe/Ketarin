using System;
using System.Collections.Generic;
using System.Text;

namespace Ketarin
{
    class ApplicationJobError
    {
        private ApplicationJob m_ApplicationJob;
        private Exception m_Error;
        private string m_RequestedUrl = string.Empty;

        #region Properties

        public string RequestedUrl
        {
            get { return m_RequestedUrl; }
            set { m_RequestedUrl = value; }
        }

        internal ApplicationJob ApplicationJob
        {
            get { return m_ApplicationJob; }
            set { m_ApplicationJob = value; }
        }

        public Exception Error
        {
            get { return m_Error; }
            set { m_Error = value; }
        }

        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(m_RequestedUrl))
                {
                    return m_Error.Message;
                }
                return m_Error.Message + " (" + m_RequestedUrl + ")";
            }
        }

        #endregion

        public ApplicationJobError(ApplicationJob job, Exception error)
        {
            m_ApplicationJob = job;
            m_Error = error;
        }

        public ApplicationJobError(ApplicationJob job, Exception error, string requestedUrl)
        {
            m_ApplicationJob = job;
            m_Error = error;
            m_RequestedUrl = requestedUrl;
        }
    }
}
