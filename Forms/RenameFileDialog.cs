using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CDBurnerXP.IO;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which allows the user
    /// to rename a file.
    /// </summary>
    public partial class RenameFileDialog : Form
    {
        private string m_Directory = string.Empty;

        #region Properties

        /// <summary>
        /// Gets or sets the file which is to be renamed.
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.Combine(m_Directory, txtFileName.Text);
            }
            set
            {
                m_Directory = Path.GetDirectoryName(value);
                txtFileName.Text = Path.GetFileName(value);
            }
        }

        #endregion

        public RenameFileDialog()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                MessageBox.Show(this, "Empty file names are not allowed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
            else if (PathEx.IsInvalidFileName(txtFileName.Text))
            {
                MessageBox.Show(this, "The file name contains invalid characters.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
            else if (File.Exists(FileName))
            {
                string msg = string.Format("The file '{0}' already exists.", FileName);
                MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }
    }
}
