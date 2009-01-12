using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// A modified TextBox for all dialogs in Ketarin.
    /// Currently implements: Ctrl+A
    /// </summary>
    class TextBox : System.Windows.Forms.TextBox
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    SelectAll();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
