using System;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// This dialog serves as editor
    /// for more flexible editing of multi line text fields.
    /// </summary>
    public partial class MultilineEditorDialog : PersistentForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value which is to be edited.
        /// </summary>
        public string Value
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }

        #endregion

        public MultilineEditorDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void chkWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            txtValue.WordWrap = chkWordWrap.Checked;
        }

        /// <summary>
        /// Adds multiple collection of variable
        /// names to the text box shown in the dialog.
        /// </summary>
        public void SetVariableNames(params string[][] variableNames)
        {
            this.txtValue.SetVariableNames(variableNames);
        }
    }
}
