using System;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Prompts for a name when saving a new user script.
    /// </summary>
    public partial class NewSnippetDialog : Form
    {
        #region Properties

        /// <summary>
        /// Gets the script name entered by the user.
        /// </summary>
        public string ScriptName
        {
            get
            {
                return txtScriptName.Text;
            }
        }

        #endregion

        public NewSnippetDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void txtScriptName_TextChanged(object sender, EventArgs e)
        {
            bOK.Enabled = !string.IsNullOrEmpty(txtScriptName.Text);
        }
    }
}
