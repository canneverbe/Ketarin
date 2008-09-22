using System;
using System.Collections.Generic;
using System.Text;

namespace Ketarin
{
    class ApplicationJobError
    {
        private ApplicationJob m_ApplicationJob;
        private Exception m_Error;

        #region Properties

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

        #endregion

        public ApplicationJobError(ApplicationJob job, Exception error)
        {
            m_ApplicationJob = job;
            m_Error = error;
        }
    }
}
