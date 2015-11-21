using System;

namespace Ketarin
{
    /// <summary>
    /// This exception should be thrown, if the download target
    /// location of an application is invalid.
    /// </summary>
    class TargetPathInvalidException : Exception
    {
        public TargetPathInvalidException(string target)
            : base(string.Format("The specified target path '{0}' is not valid.", target))
        {
        }
    }
}
