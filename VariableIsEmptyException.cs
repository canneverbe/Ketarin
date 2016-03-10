using System;

namespace Ketarin
{
    public class VariableIsEmptyException : ApplicationException
    {
        public VariableIsEmptyException(string msg) : base(msg)
        {
        }

        public VariableIsEmptyException()
        {
        }
    }
}