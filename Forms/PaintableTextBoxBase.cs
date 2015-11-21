using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Allows custom drawing within a text box.
    /// </summary>
    class PaintableTextBoxBase : TextBox
    {
        public new event PaintEventHandler Paint;

        private const int WM_Paint = 15;
        private const int WM_SETFOCUS = 0x7;
        private const int WM_LBUTTONDOWN = 0x201;

        [DebuggerStepThrough()]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_Paint || m.Msg == WM_SETFOCUS || m.Msg == WM_LBUTTONDOWN)
            {
                using (Graphics gfx = base.CreateGraphics())
                {
                    PaintEventArgs pe = new PaintEventArgs(gfx, base.ClientRectangle);

                    this.OnPaint(pe);

                    if (Paint != null)
                    {
                        Paint(this, pe);
                    }

                    pe.Dispose();
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            this.Invalidate();
        }
    }
}
