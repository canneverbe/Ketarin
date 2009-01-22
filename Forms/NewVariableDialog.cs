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

        private bool m_Updating = false;

        public NewVariableDialog()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtVariableName.SetVariableNames(DbManager.GetMostUsedVariableNames());
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVariableName.Text))
            {
                MessageBox.Show(this, "The variable name must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }

        private void txtVariableName_TextChanged(object sender, EventArgs e)
        {
            if (m_Updating) return;

            int selStart = txtVariableName.SelectionStart;
            
            m_Updating = true;
            // Do not allow colons and braces within variable names
            txtVariableName.Text = txtVariableName.Text.Replace("{", "");
            txtVariableName.Text = txtVariableName.Text.Replace("}", "");
            txtVariableName.Text = txtVariableName.Text.Replace(":", "");
            m_Updating = false;

            txtVariableName.SelectionStart = selStart;
        }
    }
}
