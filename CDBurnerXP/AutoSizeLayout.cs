using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// This Table-Layout Panel does not AutoSize (thus keeps the defined width)
    /// but dynamically adjusts the height as needed.
    /// </summary>
    public class AutoSizeLayout : TableLayoutPanel
    {
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Size prefSize = GetPreferredSize(this.Size);
            Height = prefSize.Height;
        }
    }
}
