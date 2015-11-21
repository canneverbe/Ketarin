using System;
using System.Net;

namespace Ketarin
{
    /// <summary>
    /// This exception should be thrown, if the file to download is
    /// not a binary file type.
    /// </summary>
    class NonBinaryFileException : Exception
    {
        private static string m_Message = "The downloaded file is not a binary file type ({0}). Possibly there is an error page. Status code: {1} ({2})";

        /// <summary>
        /// Creates a new exception based on the content type and status code.
        /// </summary>
        public static NonBinaryFileException Create(string contentType, HttpStatusCode code)
        {
            if (code == HttpStatusCode.NotFound)
            {
                return new NonBinaryFileException("The file at the specified URL could not be found.");
            }
            else if (code == HttpStatusCode.Unauthorized)
            {
                return new NonBinaryFileException("You do not have permission to use access the URL.");
            }
            return new NonBinaryFileException(contentType, code);
        }

        private NonBinaryFileException(string msg)
            : base(msg)
        {
        }

        private NonBinaryFileException(string contentType, HttpStatusCode code)
            : base(string.Format(m_Message, contentType, (int)code, code))
        {
        }
    }
}
