using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
#if !MONO
using System.Runtime.InteropServices;
#endif

namespace CDBurnerXP.Forms
{
    public class ControlRedrawLock : IDisposable
    {
#if !MONO
        [DllImport("user32.dll", EntryPoint="SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam); 

        public static int WM_SETREDRAW = 0x000B;
#endif

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
#if MONO
		    _control.SuspendLayout();
#else
                    SendMessage(_control.Handle, WM_SETREDRAW, 0, 0);
#endif
                    _disabledRedraw = true;
                }
            }
            catch
            { }
        }

        private bool IsValidControl()
        {
            return ((_control != null)
#if !MONO
		    && (_control.IsHandleCreated)
#endif
		   );
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (IsValidControl() && _disabledRedraw)
                {
                    // Unlock drawing
#if MONO
		    _control.ResumeLayout();
#else
                    SendMessage(_control.Handle, WM_SETREDRAW, 1, 0);
#endif
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
