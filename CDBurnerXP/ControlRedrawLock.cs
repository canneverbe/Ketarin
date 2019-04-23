using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CDBurnerXP.Forms
{
    public class ControlRedrawLock : IDisposable
    {
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
                    _control.SuspendLayout();
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
                    _control.ResumeLayout();
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
