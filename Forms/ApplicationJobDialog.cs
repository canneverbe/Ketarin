using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Ketarin.Forms
{
    public partial class ApplicationJobDialog : Form
    {
        private ApplicationJob m_ApplicationJob = new ApplicationJob();

        #region Properties

        /// <summary>
        /// Gets or sets the ApplicationJob object shown in the dialog.
        /// </summary>
        internal ApplicationJob ApplicationJob
        {
            get {
                WriteApplication();
                return m_ApplicationJob;
            }
            set {
                if (m_ApplicationJob == null)
                {
                    throw new ArgumentNullException("value");
                }

                m_ApplicationJob = value;
                ReadApplication();
                this.Text = "Edit " + m_ApplicationJob.Name;
            }
        }

        #endregion

        public ApplicationJobDialog()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;

            cboCategory.DataSource = DbManager.GetCategories();
        }

        /// <summary>
        /// Reads the ApplicationJob and fills the controls accordingly.
        /// </summary>
        private void ReadApplication()
        {
            txtApplicationName.Text = m_ApplicationJob.Name;
            txtFixedUrl.Text = m_ApplicationJob.FixedDownloadUrl;
            txtTarget.Text = m_ApplicationJob.TargetPath;
            txtFileHippoId.Text = m_ApplicationJob.FileHippoId;
            rbFileHippo.Checked = (m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FileHippo);
            rbFixedUrl.Checked = (m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FixedUrl);
            chkEnabled.Checked = m_ApplicationJob.Enabled;
            rbFolder.Checked = m_ApplicationJob.TargetIsFolder;
            chkDeletePrevious.Checked = m_ApplicationJob.DeletePreviousFile;
            txtCommand.Text = m_ApplicationJob.ExecuteCommand;
            cboCategory.Text = m_ApplicationJob.Category;
        }

        /// <summary>
        /// Modifies the ApplicationJob based on the control values.
        /// </summary>
        private void WriteApplication()
        {
            m_ApplicationJob.Name = txtApplicationName.Text;
            m_ApplicationJob.FixedDownloadUrl = txtFixedUrl.Text;
            m_ApplicationJob.TargetPath = txtTarget.Text;
            m_ApplicationJob.Enabled = chkEnabled.Checked;
            m_ApplicationJob.FileHippoId = txtFileHippoId.Text;
            m_ApplicationJob.DeletePreviousFile = chkDeletePrevious.Checked;
            m_ApplicationJob.ExecuteCommand = txtCommand.Text;
            m_ApplicationJob.DownloadSourceType = (rbFixedUrl.Checked) ? ApplicationJob.SourceType.FixedUrl : ApplicationJob.SourceType.FileHippo;
            m_ApplicationJob.Category = cboCategory.Text;
        }

        private void bBrowseFile_Click(object sender, EventArgs e)
        {
            // Depending on the save type, either open a file or directory.

            if (rbFileName.Checked)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.InitialDirectory = txtTarget.Text;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtTarget.Text = dialog.FileName;
                    }
                }
            }
            else
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    // Folder browser doesn't like file names
                    if (File.Exists(txtTarget.Text))
                    {
                        dialog.SelectedPath = Path.GetDirectoryName(txtTarget.Text);
                    }
                    else
                    {
                        dialog.SelectedPath = txtTarget.Text;
                    }

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtTarget.Text = dialog.SelectedPath;
                    }
                }
            }
        }

        private void rbFileName_CheckedChanged(object sender, EventArgs e)
        {
            txtTarget.AutoCompleteSource = AutoCompleteSource.FileSystem;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtTarget.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Check that a target location is given
            if (string.IsNullOrEmpty(txtTarget.Text))
            {
                MessageBox.Show(this, "You did not specify a target location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            // Check that name is not empty
            if (string.IsNullOrEmpty(txtApplicationName.Text))
            {
                MessageBox.Show(this, "The application name must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            // Check for valid URL
            if (rbFixedUrl.Checked && string.IsNullOrEmpty(txtFixedUrl.Text))
            {
                MessageBox.Show(this, "The URL must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
            else if (rbFileHippo.Checked && String.IsNullOrEmpty(txtFileHippoId.Text))
            {
                MessageBox.Show(this, "You did not specify a FileHippo ID.\r\nYou can paste the desired URL from the FileHippo.com website, the ID will be extracted automatically.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }
        }

        private void txtFileHippoId_TextChanged(object sender, EventArgs e)
        {
            rbFileHippo.Checked = true;
            // If someone pasted the full URL, extract the ID from it
            Regex regex = new Regex(@"filehippo\.com/download_([0-9a-z._-]+)/", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match id = regex.Match(txtFileHippoId.Text);
            if (id.Groups.Count > 1)
            {
                txtFileHippoId.Text = id.Groups[1].Value;
            }
        }

        private void txtFixedUrl_TextChanged(object sender, EventArgs e)
        {
            rbFixedUrl.Checked = true;
        }

        private void bVariables_Click(object sender, EventArgs e)
        {
            using (EditVariablesDialog dialog = new EditVariablesDialog(m_ApplicationJob))
            {
                dialog.ShowDialog(this);
            }
        }

    }
}
