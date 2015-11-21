using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// A modified TextBox for all dialogs in Ketarin.
    /// Currently implements: Ctrl+A
    /// </summary>
    class TextBox : System.Windows.Forms.TextBox
    {
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                // Normalise line endings
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Replace("\r\n", "\n");
                    value = value.Replace("\n", "\r\n");
                }
                base.Text = value;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.A:
                    SelectAll();
                    return true;

                case Keys.Control | Keys.Back:
                    // Find non-space char
                    int i = SelectionStart - 1;
                    while (i > 0 && Text[i] == ' ') i--;
                    // Delete everything to the space
                    while (i >= 0 && Text[i] != ' ') i--;
                    // Update text
                    this.Text = Text.Substring(0, ++i);
                    SelectionStart = i;
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
