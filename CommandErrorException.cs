using System;
using System.Collections.Generic;
using System.Text;

namespace Ketarin
{
    /// <summary>
    /// An exception which is thrown when a custom
    /// command returns the code "1" to indicate an error
    /// and prevent downloading.
    /// </summary>
    class CommandErrorException : ApplicationException
    {
        public CommandErrorException()
            : base("The custom command failed (exit code 1), download aborted")
        {
        }
    }
}
