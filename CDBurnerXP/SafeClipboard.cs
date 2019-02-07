using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CDBurnerXP.IO
{
    /// <summary>
    /// Accesses the Clipboard without throwing exceptions.
    /// </summary>
    public static class SafeClipboard
    {
        public static bool IsDataPresent(string format)
        {
            try
            {
                return Clipboard.GetDataObject().GetDataPresent(format);
            }
            catch (NullReferenceException)
	    {
		return false;
	    }
            catch (ExternalException)
            {
                return false;
            }
        }

        public static object GetData(string format)
        {
            try
            {
                return Clipboard.GetDataObject().GetData(format);
            }
            catch (ExternalException)
            {
                return null;
            }
        }

        public static bool SetData(object value, bool copy)
        {
            try
            {
                Clipboard.SetDataObject(value, copy, 2, 50);
                return true;
            }
            catch (ExternalException)
            {
                return false;
            }
        }
    }
}
