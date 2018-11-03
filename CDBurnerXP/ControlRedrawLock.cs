using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CDBurnerXP.Forms
{
    public class ControlRedrawLock : IDisposable
    {
        [DllImport("user32.dll", EntryPoint="SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam); 

        public static int WM_SETREDRAW = 0x000B;

        private Control _control;
        private bool _invalidate;
        private bool _disabledRedraw = false;

        public ControlRedrawLock(Control control) : this(control, true)
        {
        }

        public ControlRedrawLock(Control control, bool invalidate)
        {
            try
            {
                _control = control;
                _invalidate = invalidate;

                if (IsValidControl())
                {
                    // Lock drawing
                    SendMessage(_control.Handle, WM_SETREDRAW, 0, 0);
                    _disabledRedraw = true;
                }
            }
            catch
            { }
        }

        private bool IsValidControl()
        {
            return ((_control != null) && (_control.IsHandleCreated));
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (IsValidControl() && _disabledRedraw)
                {
                    // Unlock drawing
                    SendMessage(_control.Handle, WM_SETREDRAW, 1, 0);
                    if (_invalidate)
                    {
                        // Invalidate the control to trigger a re-paint.
                        _control.Invalidate(true);
                    }
                }
            }
            catch
            { }
        }

        #endregion
    }
}