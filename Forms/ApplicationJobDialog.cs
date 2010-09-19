using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using CookComputing.XmlRpc;
using CDBurnerXP;
using CDBurnerXP.IO;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    public partial class ApplicationJobDialog : PersistentForm
    {
        private ApplicationJob m_ApplicationJob = new ApplicationJob();

        #region Properties

        /// <summary>
        /// Gets or sets the ApplicationJob object shown in the dialog.
        /// </summary>
        internal ApplicationJob ApplicationJob
        {
            get {
                return m_ApplicationJob;
            }
            set {
                if (m_ApplicationJob == null)
                {
                    throw new ArgumentNullException("value");
                }

                m_ApplicationJob = value;
                bSaveAsDefault.Visible = false;
                ReadApplication();
                this.Text = "Edit " + m_ApplicationJob.Name;
            }
        }

        /// <summary>
        /// Gets or sets whether or not the dialog is in read-only mode.
        /// </summary>
        public bool ReadOnly 
        {
            get
            {
                return txtApplicationName.ReadOnly;
            }
            set
            {
                bool enable = !value;
                txtApplicationName.ReadOnly = value;
                txtExecuteAfter.ReadOnly = value;
                txtExecuteBefore.ReadOnly = value;
                txtNotes.ReadOnly = value;
                txtWebsite.ReadOnly = value;
                txtFixedUrl.ReadOnly = value;
                txtTarget.ReadOnly = value;
                txtSpoofReferer.ReadOnly = value;
                txtUserAgent.ReadOnly = value;
                txtFileHippoId.ReadOnly = value;
                txtUseVariablesForChanges.Enabled = value;
                cboCategory.Enabled = enable;
                chkDownloadExclusively.Enabled = enable;
                chkDeletePrevious.Enabled = enable;
                chkEnabled.Enabled = enable;
                chkShareOnline.Enabled = enable;
                chkCheckForUpdatesOnly.Enabled = enable;
                chkIgnoreFileInformation.Enabled = enable;
                rbAlwaysDownload.Enabled = enable;
                rbBetaAvoid.Enabled = enable;
                rbBetaDefault.Enabled = enable;
                bBrowseFile.Enabled = enable;
                bOK.Enabled = enable;
                bOK.Visible = enable;
            }
        }

        #endregion

        public ApplicationJobDialog()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;

            cboCategory.DataSource = DbManager.GetCategories();

            string defaultXml = Settings.GetValue("DefaultApplication", "") as string;
            if (!string.IsNullOrEmpty(defaultXml))
            {
                m_ApplicationJob = ApplicationJob.LoadOneFromXml(defaultXml);
                ReadApplication();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RefreshVariables();
            SetAutocompleteSource();

            this.BeginInvoke((MethodInvoker)delegate()
            {
                // Do not set focus to TabControl 
                txtApplicationName.Focus();
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Prevent an invalid operation exception because of automcomplete
            txtTarget.Dispose();
        }

        /// <summary>
        /// Refreshes the variable menu items in the context menus.
        /// </summary>
        private void RefreshVariables()
        {
            if (m_ApplicationJob == null) return;

            // Adjust context menus
            List<string> appVarNames = new List<string>();
            foreach (UrlVariable var in m_ApplicationJob.Variables.Values)
            {
                appVarNames.Add(var.Name);
            }

            // Add global  variables
            foreach (UrlVariable gVar in UrlVariable.GlobalVariables.Values)
            {
                appVarNames.Add(gVar.Name);
            }

            // Add "version" variable to context menu if filehippo ID is present
            if (rbFileHippo.Checked && !string.IsNullOrEmpty(txtFileHippoId.Text) && !appVarNames.Contains("version"))
            {
                appVarNames.Add("version");
            }

            txtExecuteAfter.SetVariableNames(new string[] { "file", "root", "category", "appname" }, appVarNames.ToArray());
            txtExecuteBefore.SetVariableNames(new string[] { "file", "root", "category", "appname" }, appVarNames.ToArray());
            txtFixedUrl.SetVariableNames(new string[] { "category", "appname" }, appVarNames.ToArray());
            txtTarget.SetVariableNames(new string[] { "category", "appname" }, appVarNames.ToArray());
            txtSpoofReferer.SetVariableNames(new string[] { "category", "appname" }, appVarNames.ToArray());
            txtUseVariablesForChanges.Items.Clear();
            txtUseVariablesForChanges.Items.AddRange(appVarNames.ToArray());

            foreach (SetupInstructionListBoxPanel panel in this.instructionsListBox.Panels)
            {
                panel.VariableNames = txtExecuteAfter.VariableNames;
            }
        }

        /// <summary>
        /// Reads the ApplicationJob and fills the controls accordingly.
        /// </summary>
        private void ReadApplication()
        {
            txtApplicationName.Text = m_ApplicationJob.Name;
            txtFixedUrl.Text = m_ApplicationJob.FixedDownloadUrl;
            txtTarget.Text = m_ApplicationJob.TargetPath;
            txtUserAgent.Text = m_ApplicationJob.UserAgent;
            txtFileHippoId.Text = m_ApplicationJob.FileHippoId;
            rbFileHippo.Checked = (m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FileHippo);
            rbFixedUrl.Checked = (m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FixedUrl);
            chkEnabled.Checked = m_ApplicationJob.Enabled;
            
            rbFolder.Checked = m_ApplicationJob.TargetIsFolder;
            // One of the two must be checked (always)
            if (!rbFolder.Checked) rbFileName.Checked = true;

            chkDeletePrevious.Checked = m_ApplicationJob.DeletePreviousFile;
            txtExecuteAfter.Text = m_ApplicationJob.ExecuteCommand;
            txtExecuteBefore.Text = m_ApplicationJob.ExecutePreCommand;
            cboCategory.SelectedIndex = -1;
            cboCategory.Text = string.IsNullOrEmpty(m_ApplicationJob.Category) ? null : m_ApplicationJob.Category;
            chkShareOnline.Checked = m_ApplicationJob.ShareApplication;
            chkShareOnline.Enabled = m_ApplicationJob.CanBeShared;
            chkDownloadExclusively.Checked = m_ApplicationJob.ExclusiveDownload;
            chkCheckForUpdatesOnly.Checked = m_ApplicationJob.CheckForUpdatesOnly;
            txtSpoofReferer.Text = m_ApplicationJob.HttpReferer;
            rbBetaAvoid.Checked = (ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.Avoid);
            rbBetaDefault.Checked = (ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.Default);
            rbAlwaysDownload.Checked = (ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.AlwaysDownload);
            txtUseVariablesForChanges.Text = m_ApplicationJob.VariableChangeIndicator;
            chkIgnoreFileInformation.Checked = m_ApplicationJob.IgnoreFileInformation;

            txtWebsite.Text = m_ApplicationJob.WebsiteUrl;
            txtNotes.Text = m_ApplicationJob.UserNotes;

            // Setup instructions
            foreach (SetupInstruction instruction in m_ApplicationJob.SetupInstructions)
            {
                instructionsListBox.Panels.Add(new SetupInstructionListBoxPanel(instruction.Clone() as SetupInstruction));
            }
        }

        /// <summary>
        /// Modifies the ApplicationJob based on the control values.
        /// </summary>
        private void WriteApplication()
        {
            m_ApplicationJob.Name = txtApplicationName.Text;
            m_ApplicationJob.FixedDownloadUrl = txtFixedUrl.Text;
            m_ApplicationJob.TargetPath = txtTarget.Text;
            if (rbFolder.Checked)
            {
                m_ApplicationJob.TargetPath = PathEx.QualifyPath(m_ApplicationJob.TargetPath);
            }
            m_ApplicationJob.Enabled = chkEnabled.Checked;
            m_ApplicationJob.FileHippoId = txtFileHippoId.Text;
            m_ApplicationJob.DeletePreviousFile = chkDeletePrevious.Checked;
            m_ApplicationJob.ExecuteCommand = txtExecuteAfter.Text;
            m_ApplicationJob.ExecutePreCommand = txtExecuteBefore.Text;
            m_ApplicationJob.DownloadSourceType = (rbFixedUrl.Checked) ? ApplicationJob.SourceType.FixedUrl : ApplicationJob.SourceType.FileHippo;
            m_ApplicationJob.Category = cboCategory.Text;
            m_ApplicationJob.ExclusiveDownload = chkDownloadExclusively.Checked;
            m_ApplicationJob.ShareApplication = chkShareOnline.Checked;
            m_ApplicationJob.HttpReferer = txtSpoofReferer.Text;
            m_ApplicationJob.UserAgent = txtUserAgent.Text;
            m_ApplicationJob.VariableChangeIndicator = txtUseVariablesForChanges.Text;
            m_ApplicationJob.CheckForUpdatesOnly = chkCheckForUpdatesOnly.Checked;
            m_ApplicationJob.IgnoreFileInformation = chkIgnoreFileInformation.Checked;

            m_ApplicationJob.WebsiteUrl = txtWebsite.Text;
            m_ApplicationJob.UserNotes = txtNotes.Text;

            if (rbAlwaysDownload.Checked)
            {
                m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.AlwaysDownload;
            }
            else if (rbBetaAvoid.Checked)
            {
                m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.Avoid;
            }
            else
            {
                m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.Default;
            }

            // Setup instructions
            m_ApplicationJob.SetupInstructions.Clear();
            foreach (SetupInstructionListBoxPanel panel in instructionsListBox.Panels)
            {
                m_ApplicationJob.SetupInstructions.Add(panel.SetupInstruction);
            }
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
                    string defaultTargetDir = Settings.GetValue("DefaultTargetDir") as string;

                    // Folder browser doesn't like file names
                    if (Directory.Exists(txtTarget.Text))
                    {
                        dialog.SelectedPath = Path.GetDirectoryName(txtTarget.Text);
                    }
                    else if (!string.IsNullOrEmpty(defaultTargetDir))
                    {
                        dialog.SelectedPath = defaultTargetDir;
                    }
                    else
                    {
                        dialog.SelectedPath = txtTarget.Text;
                    }

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtTarget.Text = dialog.SelectedPath;
                        Settings.SetValue("DefaultTargetDir", dialog.SelectedPath);
                    }
                }
            }
        }

        private void rbFileName_CheckedChanged(object sender, EventArgs e)
        {
            SetAutocompleteSource();
        }

        private void rbDirectory_CheckedChanged(object sender, EventArgs e)
        {
            SetAutocompleteSource();
        }

        /// <summary>
        /// Sets the appropriate auto complete source.
        /// </summary>
        private void SetAutocompleteSource()
        {
            // Setting the auto complete value will reset the text.
            // Thus, save and restore it.
            string current = txtTarget.Text;
            txtTarget.AutoCompleteSource = (rbFileName.Checked) ? AutoCompleteSource.FileSystem : AutoCompleteSource.FileSystemDirectories;
            txtTarget.Text = current;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show(this, "You did not enter a download URL. The application will not be downloaded as long as no URL is specified.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Check that a target location is given
                if (string.IsNullOrEmpty(txtTarget.Text))
                {
                    MessageBox.Show(this, "You did not specify a target location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
            }

            if (rbFileHippo.Checked && String.IsNullOrEmpty(txtFileHippoId.Text))
            {
                MessageBox.Show(this, "You did not specify a FileHippo ID.\r\nYou can paste the desired URL from the FileHippo.com website, the ID will be extracted automatically.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            WriteApplication();

            // All good. If necessary, now start a thread
            // which is going to share the application online.
            ApplicationJob job = this.ApplicationJob;
            if (job.ShareApplication)
            {
                Cursor = Cursors.WaitCursor;

                try
                {
                    IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                    proxy.Timeout = 10000;

                    RpcApplication[] existingApps = proxy.GetSimilarApplications(job.Name, job.Guid.ToString());
                    if (existingApps.Length > 0)
                    {
                        // Prevent similar entries by asking the author
                        // to reconsider his choice of name.
                        SimilarApplicationsDialog dialog = new SimilarApplicationsDialog();
                        dialog.ApplicationJob = job;
                        dialog.Applications = existingApps;
                        if (dialog.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    // Everything is fine, upload now.
                    Thread thread = new Thread(new ParameterizedThreadStart(ShareOnline));
                    thread.IsBackground = true;
                    thread.Start(job);
                }
                catch (System.Net.WebException ex)
                {
                    MessageBox.Show(this, "Your application could not be submitted to the online database because of an connection error: " + ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Uploads an application to the online database (as background thread).
        /// </summary>
        /// <param name="argument">The ApplicationJob which is to be uploaded</param>
        private static void ShareOnline(object argument)
        {
            ApplicationJob job = argument as ApplicationJob;
            if (job == null) return;

            try
            {
                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                proxy.Timeout = 10000;

                proxy.SaveApplication(job.GetXmlWithoutGlobalVariables(), Settings.GetValue("AuthorGuid") as string);
            }
            catch (XmlRpcFaultException ex)
            {
                LogDialog.Log("Could not submit '" + job.Name + "' to the online database: " + ex.FaultString);
            }
            catch (Exception)
            {
                // No internet, server down, whatever. We don't have to care.
            }
        }

        private void txtFileHippoId_TextChanged(object sender, EventArgs e)
        {
            rbFileHippo.Checked = true;
            txtFileHippoId.Text = ExternalServices.GetFileHippoIdFromUrl(txtFileHippoId.Text);

            RefreshVariables();
        }

        private void txtFileHippoId_LostFocus(object sender, System.EventArgs e)
        {
            // Determine name in background to prevent annoying users
            Cursor = Cursors.AppStarting;
            Thread thread = new Thread(new ParameterizedThreadStart(AutoFillApplicationName));
            thread.Start(txtFileHippoId.Text);
        }

        /// <summary>
        /// Automatically fills the application name text box based on the 
        /// FileHippo ID (to be used as background procress).
        /// </summary>
        private void AutoFillApplicationName(object fileHippoId)
        {
            string appName = string.Empty;

            try
            {
                appName = ExternalServices.FileHippoAppName(fileHippoId as string);

                if (string.IsNullOrEmpty(appName)) return;
            }
            catch (Exception ex)
            {
                // Ignore any errors, since optional process in background
                LogDialog.Log("FileHippo application name could not be determined", ex);
                return;
            }
            finally
            {
                // Make sure that form does still exist
                if (Visible)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        // Reset cursor
                        Cursor = Cursors.Default;
                        // Only fill the application name if it is still empty
                        if (string.IsNullOrEmpty(txtApplicationName.Text))
                        {
                            txtApplicationName.Text = appName;
                        }
                    });
                }
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
                dialog.ReadOnly = ReadOnly;
                dialog.UserAgent = txtUserAgent.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK) {
                    RefreshVariables();
                }
            }
        }

        private void rbFileHippo_CheckedChanged(object sender, EventArgs e)
        {
            RefreshVariables();
        }

        private void rbFixedUrl_CheckedChanged(object sender, EventArgs e)
        {
            RefreshVariables();
        }

        private void bSaveAsDefault_Click(object sender, EventArgs e)
        {
            // Save entered values as default for next time
            WriteApplication();
            string xml = ApplicationJob.GetXml(new ApplicationJob[] { ApplicationJob }, true, Encoding.UTF8);
            Settings.SetValue("DefaultApplication", xml);
        }

        #region Instructions

        private void mnuStartProcess_Click(object sender, EventArgs e)
        {
            StartProcessInstruction instruction = new StartProcessInstruction();
            instruction.Application = m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, txtExecuteAfter.VariableNames))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction);
                panel.VariableNames = txtExecuteAfter.VariableNames;
                instructionsListBox.Panels.Add(panel);
            }
        }

        private void mnuCopyFile_Click(object sender, EventArgs e)
        {
            CopyFileInstruction instruction = new CopyFileInstruction();
            instruction.Application = m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, txtExecuteAfter.VariableNames))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction);
                panel.VariableNames = txtExecuteAfter.VariableNames;
                instructionsListBox.Panels.Add(panel);
            }
        }

        private void mnuCustomCommand_Click(object sender, EventArgs e)
        {
            CustomSetupInstruction instruction = new CustomSetupInstruction();
            instruction.Application = m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, txtExecuteAfter.VariableNames))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction);
                panel.VariableNames = txtExecuteAfter.VariableNames;
                instructionsListBox.Panels.Add(panel);
            }
        }

        #endregion
    }
}
