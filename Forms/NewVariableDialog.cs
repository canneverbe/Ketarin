using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    public partial class NewVariableDialog : Form
    {
        #region Properties

        public string VariableName
        {
            get
            {
                return txtVariableName.Text;
            }
        }

        #endregion

        public NewVariableDialog()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVariableName.Text))
            {
                MessageBox.Show(this, "The variable name must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }
    }
}
