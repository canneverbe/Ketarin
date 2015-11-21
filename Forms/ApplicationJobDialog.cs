using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CDBurnerXP;
using CDBurnerXP.Forms;
using CDBurnerXP.IO;
using CookComputing.XmlRpc;

namespace Ketarin.Forms
{
    public partial class ApplicationJobDialog : PersistentForm
    {
        #region NonValidatingComboBox

        /// <summary>
        /// Solved a problem renaming categories: http://ketarin.canneverbe.com/forum/viewtopic.php?id=789
        /// By not validating, NotifyAutoComplete() is not called in the ComboBox base class.
        /// </summary>
        public class NonValidatingComboBox : ComboBox
        {
            protected override void OnValidating(CancelEventArgs e)
            {
            }
        }

        #endregion

        private ApplicationJob m_ApplicationJob = new ApplicationJob();

        #region Properties

        /// <summary>
        /// Gets or sets the ApplicationJob object shown in the dialog.
        /// </summary>
        internal ApplicationJob ApplicationJob
        {
            get {
                return this.m_ApplicationJob;
            }
            set {
                if (this.m_ApplicationJob == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.m_ApplicationJob = value;
                this.bSaveAsDefault.Visible = false;
                this.ReadApplication();
                this.Text = "Edit " + this.m_ApplicationJob.Name;
            }
        }

        /// <summary>
        /// Gets or sets whether or not the dialog is in read-only mode.
        /// </summary>
        public bool ReadOnly 
        {
            get
            {
                return this.txtApplicationName.ReadOnly;
            }
            set
            {
                bool enable = !value;
                this.txtApplicationName.ReadOnly = value;
                this.txtExecuteAfter.ReadOnly = value;
                this.txtExecuteBefore.ReadOnly = value;
                this.txtNotes.ReadOnly = value;
                this.txtWebsite.ReadOnly = value;
                this.txtFixedUrl.ReadOnly = value;
                this.txtTarget.ReadOnly = value;
                this.txtSpoofReferer.ReadOnly = value;
                this.txtUserAgent.ReadOnly = value;
                this.txtFileHippoId.ReadOnly = value;
                this.txtUseVariablesForChanges.Enabled = enable;
                this.cboCategory.Enabled = enable;
                this.chkDownloadExclusively.Enabled = enable;
                this.chkDeletePrevious.Enabled = enable;
                this.chkEnabled.Enabled = enable;
                this.chkShareOnline.Enabled = enable;
                this.chkCheckForUpdatesOnly.Enabled = enable;
                this.chkIgnoreFileInformation.Enabled = enable;
                this.rbAlwaysDownload.Enabled = enable;
                this.rbBetaAvoid.Enabled = enable;
                this.rbBetaDefault.Enabled = enable;
                this.bBrowseFile.Enabled = enable;
                this.bAddInstruction.Enabled = enable;
                this.instructionsListBox.Enabled = enable;
                this.cboHashVariable.Enabled = enable;
                this.cboHashType.Enabled = enable;
                this.bOK.Enabled = enable;
                this.bOK.Visible = enable;
                this.bCancel.Text = value ? "Close" : "Cancel";
            }
        }

        #endregion

        public ApplicationJobDialog()
        {
            this.InitializeComponent();
            this.AcceptButton = this.bOK;
            this.CancelButton = this.bCancel;
            this.cboHashType.SelectedIndex = 0;

            string defaultXml = Settings.GetValue("DefaultApplication", "") as string;
            if (!string.IsNullOrEmpty(defaultXml))
            {
                this.m_ApplicationJob = ApplicationJob.LoadOneFromXml(defaultXml);
                this.ReadApplication();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.cboCategory.DataSource = DbManager.GetCategories();
            this.RefreshVariables();
            this.SetAutocompleteSource();

            if (this.m_ApplicationJob != null)
            {
                this.cboCategory.Text = string.IsNullOrEmpty(this.m_ApplicationJob.Category) ? null : this.m_ApplicationJob.Category;
            }

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
            this.txtTarget.Dispose();
        }

        /// <summary>
        /// Refreshes the variable menu items in the context menus.
        /// </summary>
        private void RefreshVariables()
        {
            if (this.m_ApplicationJob == null) return;

            this.txtExecuteAfter.Application = this.m_ApplicationJob;
            this.txtExecuteBefore.Application = this.m_ApplicationJob;

            // Adjust context menus
            List<string> appVarNames = this.m_ApplicationJob.Variables.Values.Select(var => var.Name).ToList();
            appVarNames.AddRange(UrlVariable.GlobalVariables.Values.Select(gVar => gVar.Name));

            // Add global  variables

            // Add "version" variable to context menu if filehippo ID is present
            if (this.rbFileHippo.Checked && !string.IsNullOrEmpty(this.txtFileHippoId.Text) && !appVarNames.Contains("version"))
            {
                appVarNames.Add("version");
            }

            this.txtExecuteAfter.SetVariableNames(new[] { "file", "root", "category", "appname" }, appVarNames.ToArray());
            this.txtExecuteBefore.SetVariableNames(new[] { "file", "root", "category", "appname" }, appVarNames.ToArray());
            this.txtFixedUrl.SetVariableNames(new[] { "category", "appname" }, appVarNames.ToArray());
            this.txtTarget.SetVariableNames(new[] { "category", "appname" }, appVarNames.ToArray());
            this.txtSpoofReferer.SetVariableNames(new[] { "category", "appname" }, appVarNames.ToArray());
            this.txtUseVariablesForChanges.Items.Clear();
            this.txtUseVariablesForChanges.Items.AddRange(appVarNames.ToArray());
            this.cboHashVariable.Items.Clear();
            this.cboHashVariable.Items.AddRange(appVarNames.ToArray());

            foreach (SetupInstructionListBoxPanel panel in this.instructionsListBox.Panels)
            {
                panel.VariableNames = this.txtExecuteAfter.VariableNames;
            }
        }

        /// <summary>
        /// Reads the ApplicationJob and fills the controls accordingly.
        /// </summary>
        private void ReadApplication()
        {
            this.txtApplicationName.Text = this.m_ApplicationJob.Name;
            this.txtFixedUrl.Text = this.m_ApplicationJob.FixedDownloadUrl;
            this.txtTarget.Text = this.m_ApplicationJob.TargetPath;
            this.txtUserAgent.Text = this.m_ApplicationJob.UserAgent;
            this.txtFileHippoId.Text = this.m_ApplicationJob.FileHippoId;
            this.rbFileHippo.Checked = (this.m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FileHippo);
            this.rbFixedUrl.Checked = (this.m_ApplicationJob.DownloadSourceType == ApplicationJob.SourceType.FixedUrl);
            this.chkEnabled.Checked = this.m_ApplicationJob.Enabled;

            this.rbFolder.Checked = this.m_ApplicationJob.TargetIsFolder;
            // One of the two must be checked (always)
            if (!this.rbFolder.Checked) this.rbFileName.Checked = true;

            this.chkDeletePrevious.Checked = this.m_ApplicationJob.DeletePreviousFile;
            this.txtExecuteAfter.Text = this.m_ApplicationJob.ExecuteCommand;
            this.txtExecuteBefore.Text = this.m_ApplicationJob.ExecutePreCommand;
            this.cboCategory.SelectedIndex = -1;
            this.cboCategory.Text = string.IsNullOrEmpty(this.m_ApplicationJob.Category) ? null : this.m_ApplicationJob.Category;
            this.chkShareOnline.Checked = this.m_ApplicationJob.ShareApplication;
            this.chkShareOnline.Enabled = this.m_ApplicationJob.CanBeShared;
            this.chkDownloadExclusively.Checked = this.m_ApplicationJob.ExclusiveDownload;
            this.chkCheckForUpdatesOnly.Checked = this.m_ApplicationJob.CheckForUpdatesOnly;
            this.txtSpoofReferer.Text = this.m_ApplicationJob.HttpReferer;
            this.rbBetaAvoid.Checked = (this.ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.Avoid);
            this.rbBetaDefault.Checked = (this.ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.Default);
            this.rbAlwaysDownload.Checked = (this.ApplicationJob.DownloadBeta == ApplicationJob.DownloadBetaType.AlwaysDownload);
            this.txtUseVariablesForChanges.Text = this.m_ApplicationJob.VariableChangeIndicator;
            this.chkIgnoreFileInformation.Checked = this.m_ApplicationJob.IgnoreFileInformation;
            this.cboHashType.SelectedIndex = (int) this.m_ApplicationJob.HashType;
            this.cboHashVariable.Text = this.m_ApplicationJob.HashVariable;

            this.txtWebsite.Text = this.m_ApplicationJob.WebsiteUrl;
            this.txtNotes.Text = this.m_ApplicationJob.UserNotes;

            this.txtExecuteAfter.CommandType = this.m_ApplicationJob.ExecuteCommandType;
            this.txtExecuteBefore.CommandType = this.m_ApplicationJob.ExecutePreCommandType;

            // Setup instructions
            this.instructionsListBox.Panels.Clear();
            foreach (SetupInstruction instruction in this.m_ApplicationJob.SetupInstructions)
            {
                this.instructionsListBox.Panels.Add(new SetupInstructionListBoxPanel(instruction.Clone() as SetupInstruction));
            }
        }

        /// <summary>
        /// Modifies the ApplicationJob based on the control values.
        /// </summary>
        private void WriteApplication()
        {
            this.m_ApplicationJob.Name = this.txtApplicationName.Text;
            this.m_ApplicationJob.FixedDownloadUrl = this.txtFixedUrl.Text;
            this.m_ApplicationJob.TargetPath = this.txtTarget.Text;
            if (this.rbFolder.Checked)
            {
                this.m_ApplicationJob.TargetPath = PathEx.QualifyPath(this.m_ApplicationJob.TargetPath);
            }
            this.m_ApplicationJob.Enabled = this.chkEnabled.Checked;
            this.m_ApplicationJob.FileHippoId = this.txtFileHippoId.Text;
            this.m_ApplicationJob.DeletePreviousFile = this.chkDeletePrevious.Checked;
            this.m_ApplicationJob.ExecuteCommand = this.txtExecuteAfter.Text;
            this.m_ApplicationJob.ExecutePreCommand = this.txtExecuteBefore.Text;
            this.m_ApplicationJob.DownloadSourceType = (this.rbFixedUrl.Checked) ? ApplicationJob.SourceType.FixedUrl : ApplicationJob.SourceType.FileHippo;
            this.m_ApplicationJob.Category = this.cboCategory.Text;
            this.m_ApplicationJob.ExclusiveDownload = this.chkDownloadExclusively.Checked;
            this.m_ApplicationJob.ShareApplication = this.chkShareOnline.Checked;
            this.m_ApplicationJob.HttpReferer = this.txtSpoofReferer.Text;
            this.m_ApplicationJob.UserAgent = this.txtUserAgent.Text;
            this.m_ApplicationJob.VariableChangeIndicator = this.txtUseVariablesForChanges.Text;
            this.m_ApplicationJob.CheckForUpdatesOnly = this.chkCheckForUpdatesOnly.Checked;
            this.m_ApplicationJob.IgnoreFileInformation = this.chkIgnoreFileInformation.Checked;
            this.m_ApplicationJob.HashType = (HashType)this.cboHashType.SelectedIndex;
            this.m_ApplicationJob.HashVariable = this.cboHashVariable.Text;

            this.m_ApplicationJob.WebsiteUrl = this.txtWebsite.Text;
            this.m_ApplicationJob.UserNotes = this.txtNotes.Text;
            this.m_ApplicationJob.ExecuteCommandType = this.txtExecuteAfter.CommandType;
            this.m_ApplicationJob.ExecutePreCommandType = this.txtExecuteBefore.CommandType;

            if (this.rbAlwaysDownload.Checked)
            {
                this.m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.AlwaysDownload;
            }
            else if (this.rbBetaAvoid.Checked)
            {
                this.m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.Avoid;
            }
            else
            {
                this.m_ApplicationJob.DownloadBeta = ApplicationJob.DownloadBetaType.Default;
            }

            // Setup instructions
            this.m_ApplicationJob.SetupInstructions.Clear();
            foreach (SetupInstructionListBoxPanel panel in this.instructionsListBox.GetPanels())
            {
                this.m_ApplicationJob.SetupInstructions.Add(panel.SetupInstruction);
            }
        }

        private void bBrowseFile_Click(object sender, EventArgs e)
        {
            // Depending on the save type, either open a file or directory.

            if (this.rbFileName.Checked)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.InitialDirectory = this.txtTarget.Text;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.txtTarget.Text = dialog.FileName;
                    }
                }
            }
            else
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    string defaultTargetDir = Settings.GetValue("DefaultTargetDir") as string;

                    // Folder browser doesn't like file names
                    if (Directory.Exists(this.txtTarget.Text))
                    {
                        dialog.SelectedPath = Path.GetDirectoryName(this.txtTarget.Text);
                    }
                    else if (!string.IsNullOrEmpty(defaultTargetDir))
                    {
                        dialog.SelectedPath = defaultTargetDir;
                    }
                    else
                    {
                        dialog.SelectedPath = this.txtTarget.Text;
                    }

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.txtTarget.Text = dialog.SelectedPath;
                        Settings.SetValue("DefaultTargetDir", dialog.SelectedPath);
                    }
                }
            }
        }

        private void rbFileName_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutocompleteSource();
        }

        private void rbDirectory_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAutocompleteSource();
        }

        /// <summary>
        /// Sets the appropriate auto complete source.
        /// </summary>
        private void SetAutocompleteSource()
        {
            // Setting the auto complete value will reset the text.
            // Thus, save and restore it.
            string current = this.txtTarget.Text;
            this.txtTarget.AutoCompleteSource = (this.rbFileName.Checked) ? AutoCompleteSource.FileSystem : AutoCompleteSource.FileSystemDirectories;
            this.txtTarget.Text = current;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Check that name is not empty
            if (string.IsNullOrEmpty(this.txtApplicationName.Text))
            {
                MessageBox.Show(this, "The application name must not be empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            // Check for valid URL
            if (this.rbFixedUrl.Checked && string.IsNullOrEmpty(this.txtFixedUrl.Text))
            {
                MessageBox.Show(this, "You did not enter a download URL. The application will not be downloaded as long as no URL is specified.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Check that a target location is given
                if (string.IsNullOrEmpty(this.txtTarget.Text))
                {
                    MessageBox.Show(this, "You did not specify a target location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            if (this.rbFileHippo.Checked && String.IsNullOrEmpty(this.txtFileHippoId.Text))
            {
                MessageBox.Show(this, "You did not specify a FileHippo ID.\r\nYou can paste the desired URL from the FileHippo.com website, the ID will be extracted automatically.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            this.WriteApplication();

            // All good. If necessary, now start a thread
            // which is going to share the application online.
            ApplicationJob job = this.ApplicationJob;
            if (job.ShareApplication)
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                    proxy.Timeout = 10000;

                    RpcApplication[] existingApps = proxy.GetSimilarApplications(job.Name, job.Guid.ToString());
                    if (existingApps.Length > 0)
                    {
                        // Prevent similar entries by asking the author
                        // to reconsider his choice of name.
                        SimilarApplicationsDialog dialog = new SimilarApplicationsDialog
                        {
                            ApplicationJob = job,
                            Applications = existingApps
                        };
                        if (dialog.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    // Everything is fine, upload now.
                    Thread thread = new Thread(ShareOnline) {IsBackground = true};
                    thread.Start(job);
                }
                catch (WebException ex)
                {
                    MessageBox.Show(this, "Your application could not be submitted to the online database because of an connection error: " + ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

            // Required for non modal call
            this.Close();
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
            this.rbFileHippo.Checked = true;
            this.txtFileHippoId.Text = ExternalServices.GetFileHippoIdFromUrl(this.txtFileHippoId.Text);

            this.RefreshVariables();
        }

        private void txtFileHippoId_LostFocus(object sender, EventArgs e)
        {
            // Determine name in background to prevent annoying users
            this.Cursor = Cursors.AppStarting;
            Thread thread = new Thread(this.AutoFillApplicationName);
            thread.Start(this.txtFileHippoId.Text);
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
                if (this.Visible)
                {
                    this.BeginInvoke((MethodInvoker)delegate
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
            this.rbFixedUrl.Checked = true;
        }

        private void bVariables_Click(object sender, EventArgs e)
        {
            using (EditVariablesDialog dialog = new EditVariablesDialog(this.m_ApplicationJob))
            {
                dialog.ReadOnly = this.ReadOnly;
                dialog.UserAgent = this.txtUserAgent.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK) {
                    this.RefreshVariables();
                }
            }
        }

        private void rbFileHippo_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshVariables();
        }

        private void rbFixedUrl_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshVariables();
        }

        private void bSaveAsDefault_Click(object sender, EventArgs e)
        {
            // Save entered values as default for next time
            this.WriteApplication();
            string xml = ApplicationJob.GetXml(new[] {this.ApplicationJob }, true, Encoding.UTF8);
            Settings.SetValue("DefaultApplication", xml);
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            // Required for non modal call
            this.Close();
        }

        #region Instructions

        private void mnuStartProcess_Click(object sender, EventArgs e)
        {
            StartProcessInstruction instruction = new StartProcessInstruction();
            instruction.Application = this.m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, this.txtExecuteAfter.VariableNames, this.m_ApplicationJob))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction)
                {
                    VariableNames = this.txtExecuteAfter.VariableNames
                };
                this.instructionsListBox.Panels.Add(panel);
            }
        }

        private void mnuCopyFile_Click(object sender, EventArgs e)
        {
            CopyFileInstruction instruction = new CopyFileInstruction();
            instruction.Application = this.m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, this.txtExecuteAfter.VariableNames, this.m_ApplicationJob))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction)
                {
                    VariableNames = this.txtExecuteAfter.VariableNames
                };
                this.instructionsListBox.Panels.Add(panel);
            }
        }

        private void mnuCustomCommand_Click(object sender, EventArgs e)
        {
            CustomSetupInstruction instruction = new CustomSetupInstruction();
            instruction.Application = this.m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, this.txtExecuteAfter.VariableNames, this.m_ApplicationJob))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction)
                {
                    VariableNames = this.txtExecuteAfter.VariableNames
                };
                this.instructionsListBox.Panels.Add(panel);
            }
        }

        private void mnuCloseProcess_Click(object sender, EventArgs e)
        {
            CloseProcessInstruction instruction = new CloseProcessInstruction();
            instruction.Application = this.m_ApplicationJob;
            if (InstructionBaseDialog.ShowDialog(this, instruction, this.txtExecuteAfter.VariableNames, this.m_ApplicationJob))
            {
                SetupInstructionListBoxPanel panel = new SetupInstructionListBoxPanel(instruction)
                {
                    VariableNames = this.txtExecuteAfter.VariableNames
                };
                this.instructionsListBox.Panels.Add(panel);
            }
        }

        #endregion
    }
}
