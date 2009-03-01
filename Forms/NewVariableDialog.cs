using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog for creating or rename a variable.
    /// </summary>
    public partial class NewVariableDialog : Form
    {
        private ApplicationJob.UrlVariableCollection m_ExistingVariables = null;

        #region Properties

        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                return txtVariableName.Text;
            }
            set
            {
                txtVariableName.Text = value;
            }
        }

        #endregion

        private bool m_Updating = false;

        public NewVariableDialog(ApplicationJob.UrlVariableCollection variables)
        {
            InitializeComponent();
            m_ExistingVariables = variables;

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Provide a list of most used variable names
            txtVariableName.SetVariableNames(DbManager.GetMostUsedVariableNames());
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Make sure, that the user entered a value (if OK has been chosen)
            if (string.IsNullOrEmpty(txtVariableName.Text))
            {
                MessageBox.Show(this, "The variable name must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
            else if (m_ExistingVariables != null && m_ExistingVariables.ContainsKey(txtVariableName.Text))
            {
                string msg = string.Format("A variable with the name '{0}' already exists.", txtVariableName.Text);
                MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
