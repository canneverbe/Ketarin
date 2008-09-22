using System;
using System.Collections.Generic;
using System.Text;

namespace Ketarin
{
    class NonBinaryFileException : Exception
    {
        private static string m_Message = "The downloaded file is not a binary file type ({0}). Possibly there is an error page.";

        public static string MessageText
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public NonBinaryFileException(string contentType) : base(string.Format(MessageText, contentType))
        {
        }
    }
}
