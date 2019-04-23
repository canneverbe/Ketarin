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
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject != null)
                    return dataObject.GetDataPresent(format);
            }
            catch { }
            return false;
        }

        public static object GetData(string format)
        {
            try
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject != null)
                    return dataObject.GetData(format);
            }
            catch { }
            return null;
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
