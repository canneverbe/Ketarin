using System;
using System.Collections.Generic;
using System.Text;

namespace CDBurnerXP
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;

        public T Value
        {
            get { return m_value; }
        }
    }
}
