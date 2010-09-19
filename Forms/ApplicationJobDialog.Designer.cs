namespace Ketarin.Forms
{
    partial class ApplicationJobDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.lblApplicationName = new System.Windows.Forms.Label();
            this.txtApplicationName = new System.Windows.Forms.TextBox();
            this.pnlDownloadSource = new System.Windows.Forms.Panel();
            this.pnlBeta = new System.Windows.Forms.Panel();
            this.rbBetaAvoid = new System.Windows.Forms.RadioButton();
            this.rbAlwaysDownload = new System.Windows.Forms.RadioButton();
            this.lblBetaVersions = new System.Windows.Forms.Label();
            this.rbBetaDefault = new System.Windows.Forms.RadioButton();
            this.bVariables = new System.Windows.Forms.Button();
            this.txtFileHippoId = new System.Windows.Forms.TextBox();
            this.rbFileHippo = new System.Windows.Forms.RadioButton();
            this.txtFixedUrl = new Ketarin.Forms.VariableTextBox();
            this.rbFixedUrl = new System.Windows.Forms.RadioButton();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.bBrowseFile = new System.Windows.Forms.Button();
            this.txtTarget = new Ketarin.Forms.VariableTextBox();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.rbFileName = new System.Windows.Forms.RadioButton();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.chkDeletePrevious = new System.Windows.Forms.CheckBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.sepTarget = new CDBurnerXP.Controls.Separator();
            this.sepDownload = new CDBurnerXP.Controls.Separator();
            this.chkShareOnline = new System.Windows.Forms.CheckBox();
            this.lblSpoofReferer = new System.Windows.Forms.Label();
            this.txtSpoofReferer = new Ketarin.Forms.VariableTextBox();
            this.tcApplication = new System.Windows.Forms.TabControl();
            this.tpApplication = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.txtUserAgent = new Ketarin.Forms.VariableTextBox();
            this.lblUserAgent = new System.Windows.Forms.Label();
            this.sepMiscellaneous = new CDBurnerXP.Controls.Separator();
            this.sepUpdateDetection = new CDBurnerXP.Controls.Separator();
            this.sepDownloads = new CDBurnerXP.Controls.Separator();
            this.chkIgnoreFileInformation = new System.Windows.Forms.CheckBox();
            this.chkCheckForUpdatesOnly = new System.Windows.Forms.CheckBox();
            this.chkDownloadExclusively = new System.Windows.Forms.CheckBox();
            this.txtUseVariablesForChanges = new System.Windows.Forms.ComboBox();
            this.lblUseVariableForChanges = new System.Windows.Forms.Label();
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.tableLayoutCommands = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCommandBefore = new System.Windows.Forms.Panel();
            this.txtExecuteBefore = new Ketarin.Forms.CommandControl();
            this.lblCommandBefore = new System.Windows.Forms.Label();
            this.pnlCommandAfter = new System.Windows.Forms.Panel();
            this.txtExecuteAfter = new Ketarin.Forms.CommandControl();
            this.lblExecuteCommand = new System.Windows.Forms.Label();
            this.tpInformation = new System.Windows.Forms.TabPage();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.lblWebsite = new System.Windows.Forms.Label();
            this.tpSetup = new System.Windows.Forms.TabPage();
            this.bAddInstruction = new wyDay.Controls.SplitButton();
            this.cmnuAddInstruction = new System.Windows.Forms.ContextMenu();
            this.mnuStartProcess = new System.Windows.Forms.MenuItem();
            this.mnuCopyFile = new System.Windows.Forms.MenuItem();
            this.mnuCustomCommand = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.instructionsListBox = new CDBurnerXP.Controls.AdvancedListBox();
            this.bSaveAsDefault = new System.Windows.Forms.Button();
            this.pnlDownloadSource.SuspendLayout();
            this.pnlBeta.SuspendLayout();
            this.pnlTarget.SuspendLayout();
            this.tcApplication.SuspendLayout();
            this.tpApplication.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tpCommands.SuspendLayout();
            this.tableLayoutCommands.SuspendLayout();
            this.pnlCommandBefore.SuspendLayout();
            this.pnlCommandAfter.SuspendLayout();
            this.tpInformation.SuspendLayout();
            this.tpSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(320, 356);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 100;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(239, 356);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 99;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lblApplicationName
            // 
            this.lblApplicationName.AutoSize = true;
            this.lblApplicationName.Location = new System.Drawing.Point(4, 10);
            this.lblApplicationName.Name = "lblApplicationName";
            this.lblApplicationName.Size = new System.Drawing.Size(91, 13);
            this.lblApplicationName.TabIndex = 0;
            this.lblApplicationName.Text = "&Application name:";
            // 
            // txtApplicationName
            // 
            this.txtApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicationName.Location = new System.Drawing.Point(101, 7);
            this.txtApplicationName.MaxLength = 255;
            this.txtApplicationName.Name = "txtApplicationName";
            this.txtApplicationName.Size = new System.Drawing.Size(268, 20);
            this.txtApplicationName.TabIndex = 1;
            // 
            // pnlDownloadSource
            // 
            this.pnlDownloadSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDownloadSource.Controls.Add(this.pnlBeta);
            this.pnlDownloadSource.Controls.Add(this.bVariables);
            this.pnlDownloadSource.Controls.Add(this.txtFileHippoId);
            this.pnlDownloadSource.Controls.Add(this.rbFileHippo);
            this.pnlDownloadSource.Controls.Add(this.txtFixedUrl);
            this.pnlDownloadSource.Controls.Add(this.rbFixedUrl);
            this.pnlDownloadSource.Location = new System.Drawing.Point(4, 84);
            this.pnlDownloadSource.Name = "pnlDownloadSource";
            this.pnlDownloadSource.Size = new System.Drawing.Size(365, 80);
            this.pnlDownloadSource.TabIndex = 3;
            // 
            // pnlBeta
            // 
            this.pnlBeta.Controls.Add(this.rbBetaAvoid);
            this.pnlBeta.Controls.Add(this.rbAlwaysDownload);
            this.pnlBeta.Controls.Add(this.lblBetaVersions);
            this.pnlBeta.Controls.Add(this.rbBetaDefault);
            this.pnlBeta.Location = new System.Drawing.Point(92, 55);
            this.pnlBeta.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBeta.Name = "pnlBeta";
            this.pnlBeta.Size = new System.Drawing.Size(281, 18);
            this.pnlBeta.TabIndex = 16;
            // 
            // rbBetaAvoid
            // 
            this.rbBetaAvoid.AutoSize = true;
            this.rbBetaAvoid.Location = new System.Drawing.Point(145, 0);
            this.rbBetaAvoid.Name = "rbBetaAvoid";
            this.rbBetaAvoid.Size = new System.Drawing.Size(52, 17);
            this.rbBetaAvoid.TabIndex = 27;
            this.rbBetaAvoid.Text = "A&void";
            this.rbBetaAvoid.UseVisualStyleBackColor = true;
            // 
            // rbAlwaysDownload
            // 
            this.rbAlwaysDownload.AutoSize = true;
            this.rbAlwaysDownload.Location = new System.Drawing.Point(203, 0);
            this.rbAlwaysDownload.Name = "rbAlwaysDownload";
            this.rbAlwaysDownload.Size = new System.Drawing.Size(73, 17);
            this.rbAlwaysDownload.TabIndex = 28;
            this.rbAlwaysDownload.Text = "Do&wnload";
            this.rbAlwaysDownload.UseVisualStyleBackColor = true;
            // 
            // lblBetaVersions
            // 
            this.lblBetaVersions.AutoSize = true;
            this.lblBetaVersions.Location = new System.Drawing.Point(0, 2);
            this.lblBetaVersions.Name = "lblBetaVersions";
            this.lblBetaVersions.Size = new System.Drawing.Size(74, 13);
            this.lblBetaVersions.TabIndex = 25;
            this.lblBetaVersions.Text = "&Beta versions:";
            // 
            // rbBetaDefault
            // 
            this.rbBetaDefault.AutoSize = true;
            this.rbBetaDefault.Checked = true;
            this.rbBetaDefault.Location = new System.Drawing.Point(77, 0);
            this.rbBetaDefault.Name = "rbBetaDefault";
            this.rbBetaDefault.Size = new System.Drawing.Size(65, 17);
            this.rbBetaDefault.TabIndex = 26;
            this.rbBetaDefault.TabStop = true;
            this.rbBetaDefault.Text = "(&Default)";
            this.rbBetaDefault.UseVisualStyleBackColor = true;
            // 
            // bVariables
            // 
            this.bVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bVariables.Location = new System.Drawing.Point(290, 0);
            this.bVariables.Name = "bVariables";
            this.bVariables.Size = new System.Drawing.Size(75, 23);
            this.bVariables.TabIndex = 13;
            this.bVariables.Text = "&Variables";
            this.bVariables.UseVisualStyleBackColor = true;
            this.bVariables.Click += new System.EventHandler(this.bVariables_Click);
            // 
            // txtFileHippoId
            // 
            this.txtFileHippoId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileHippoId.Location = new System.Drawing.Point(95, 31);
            this.txtFileHippoId.Name = "txtFileHippoId";
            this.txtFileHippoId.Size = new System.Drawing.Size(270, 20);
            this.txtFileHippoId.TabIndex = 15;
            this.txtFileHippoId.TextChanged += new System.EventHandler(this.txtFileHippoId_TextChanged);
            this.txtFileHippoId.LostFocus += new System.EventHandler(this.txtFileHippoId_LostFocus);
            // 
            // rbFileHippo
            // 
            this.rbFileHippo.AutoSize = true;
            this.rbFileHippo.Location = new System.Drawing.Point(3, 32);
            this.rbFileHippo.Name = "rbFileHippo";
            this.rbFileHippo.Size = new System.Drawing.Size(86, 17);
            this.rbFileHippo.TabIndex = 14;
            this.rbFileHippo.Text = "File&Hippo ID:";
            this.rbFileHippo.UseVisualStyleBackColor = true;
            this.rbFileHippo.CheckedChanged += new System.EventHandler(this.rbFileHippo_CheckedChanged);
            // 
            // txtFixedUrl
            // 
            this.txtFixedUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFixedUrl.Location = new System.Drawing.Point(95, 2);
            this.txtFixedUrl.Name = "txtFixedUrl";
            this.txtFixedUrl.Size = new System.Drawing.Size(186, 20);
            this.txtFixedUrl.TabIndex = 12;
            this.txtFixedUrl.TextChanged += new System.EventHandler(this.txtFixedUrl_TextChanged);
            // 
            // rbFixedUrl
            // 
            this.rbFixedUrl.AutoSize = true;
            this.rbFixedUrl.Checked = true;
            this.rbFixedUrl.Location = new System.Drawing.Point(3, 3);
            this.rbFixedUrl.Name = "rbFixedUrl";
            this.rbFixedUrl.Size = new System.Drawing.Size(50, 17);
            this.rbFixedUrl.TabIndex = 11;
            this.rbFixedUrl.TabStop = true;
            this.rbFixedUrl.Text = "&URL:";
            this.rbFixedUrl.UseVisualStyleBackColor = true;
            this.rbFixedUrl.CheckedChanged += new System.EventHandler(this.rbFixedUrl_CheckedChanged);
            // 
            // pnlTarget
            // 
            this.pnlTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTarget.Controls.Add(this.bBrowseFile);
            this.pnlTarget.Controls.Add(this.txtTarget);
            this.pnlTarget.Controls.Add(this.rbFolder);
            this.pnlTarget.Controls.Add(this.rbFileName);
            this.pnlTarget.Location = new System.Drawing.Point(3, 186);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(365, 53);
            this.pnlTarget.TabIndex = 9;
            // 
            // bBrowseFile
            // 
            this.bBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowseFile.Location = new System.Drawing.Point(333, 24);
            this.bBrowseFile.Name = "bBrowseFile";
            this.bBrowseFile.Size = new System.Drawing.Size(32, 23);
            this.bBrowseFile.TabIndex = 24;
            this.bBrowseFile.Text = "...";
            this.bBrowseFile.UseVisualStyleBackColor = true;
            this.bBrowseFile.Click += new System.EventHandler(this.bBrowseFile_Click);
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtTarget.Location = new System.Drawing.Point(4, 26);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(323, 20);
            this.txtTarget.TabIndex = 23;
            // 
            // rbFolder
            // 
            this.rbFolder.AutoSize = true;
            this.rbFolder.Location = new System.Drawing.Point(87, 3);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(90, 17);
            this.rbFolder.TabIndex = 22;
            this.rbFolder.Text = "Save in f&older";
            this.rbFolder.UseVisualStyleBackColor = true;
            this.rbFolder.CheckedChanged += new System.EventHandler(this.rbDirectory_CheckedChanged);
            // 
            // rbFileName
            // 
            this.rbFileName.AutoSize = true;
            this.rbFileName.Checked = true;
            this.rbFileName.Location = new System.Drawing.Point(3, 3);
            this.rbFileName.Name = "rbFileName";
            this.rbFileName.Size = new System.Drawing.Size(78, 17);
            this.rbFileName.TabIndex = 21;
            this.rbFileName.TabStop = true;
            this.rbFileName.Text = "&Save to file";
            this.rbFileName.UseVisualStyleBackColor = true;
            this.rbFileName.CheckedChanged += new System.EventHandler(this.rbFileName_CheckedChanged);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Location = new System.Drawing.Point(9, 229);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 12;
            this.chkEnabled.Text = "&Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // chkDeletePrevious
            // 
            this.chkDeletePrevious.AutoSize = true;
            this.chkDeletePrevious.Checked = true;
            this.chkDeletePrevious.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeletePrevious.Location = new System.Drawing.Point(9, 275);
            this.chkDeletePrevious.Name = "chkDeletePrevious";
            this.chkDeletePrevious.Size = new System.Drawing.Size(218, 17);
            this.chkDeletePrevious.TabIndex = 14;
            this.chkDeletePrevious.Text = "Always &delete previously downloaded file";
            this.chkDeletePrevious.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(5, 36);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "&Category:";
            // 
            // cboCategory
            // 
            this.cboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(101, 33);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(268, 21);
            this.cboCategory.TabIndex = 3;
            // 
            // sepTarget
            // 
            this.sepTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepTarget.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepTarget.Location = new System.Drawing.Point(3, 163);
            this.sepTarget.Name = "sepTarget";
            this.sepTarget.Size = new System.Drawing.Size(366, 23);
            this.sepTarget.TabIndex = 20;
            this.sepTarget.Text = "Download location";
            this.sepTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepDownload
            // 
            this.sepDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepDownload.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepDownload.Location = new System.Drawing.Point(3, 61);
            this.sepDownload.Name = "sepDownload";
            this.sepDownload.Size = new System.Drawing.Size(366, 23);
            this.sepDownload.TabIndex = 10;
            this.sepDownload.Text = "Download source";
            this.sepDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkShareOnline
            // 
            this.chkShareOnline.AutoSize = true;
            this.chkShareOnline.Location = new System.Drawing.Point(9, 252);
            this.chkShareOnline.Name = "chkShareOnline";
            this.chkShareOnline.Size = new System.Drawing.Size(212, 17);
            this.chkShareOnline.TabIndex = 13;
            this.chkShareOnline.Text = "S&hare this application online with others";
            this.chkShareOnline.UseVisualStyleBackColor = true;
            // 
            // lblSpoofReferer
            // 
            this.lblSpoofReferer.AutoSize = true;
            this.lblSpoofReferer.Location = new System.Drawing.Point(6, 78);
            this.lblSpoofReferer.Name = "lblSpoofReferer";
            this.lblSpoofReferer.Size = new System.Drawing.Size(103, 13);
            this.lblSpoofReferer.TabIndex = 3;
            this.lblSpoofReferer.Text = "Sp&oof HTTP referer:";
            // 
            // txtSpoofReferer
            // 
            this.txtSpoofReferer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpoofReferer.Location = new System.Drawing.Point(115, 75);
            this.txtSpoofReferer.Name = "txtSpoofReferer";
            this.txtSpoofReferer.Size = new System.Drawing.Size(253, 20);
            this.txtSpoofReferer.TabIndex = 4;
            // 
            // tcApplication
            // 
            this.tcApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcApplication.Controls.Add(this.tpApplication);
            this.tcApplication.Controls.Add(this.tpSettings);
            this.tcApplication.Controls.Add(this.tpCommands);
            this.tcApplication.Controls.Add(this.tpInformation);
            this.tcApplication.Controls.Add(this.tpSetup);
            this.tcApplication.Location = new System.Drawing.Point(12, 12);
            this.tcApplication.Name = "tcApplication";
            this.tcApplication.SelectedIndex = 0;
            this.tcApplication.Size = new System.Drawing.Size(383, 329);
            this.tcApplication.TabIndex = 0;
            // 
            // tpApplication
            // 
            this.tpApplication.Controls.Add(this.lblApplicationName);
            this.tpApplication.Controls.Add(this.txtApplicationName);
            this.tpApplication.Controls.Add(this.lblCategory);
            this.tpApplication.Controls.Add(this.cboCategory);
            this.tpApplication.Controls.Add(this.sepDownload);
            this.tpApplication.Controls.Add(this.pnlDownloadSource);
            this.tpApplication.Controls.Add(this.pnlTarget);
            this.tpApplication.Controls.Add(this.sepTarget);
            this.tpApplication.Location = new System.Drawing.Point(4, 22);
            this.tpApplication.Name = "tpApplication";
            this.tpApplication.Padding = new System.Windows.Forms.Padding(3);
            this.tpApplication.Size = new System.Drawing.Size(375, 303);
            this.tpApplication.TabIndex = 0;
            this.tpApplication.Text = "Application";
            this.tpApplication.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.txtUserAgent);
            this.tpSettings.Controls.Add(this.lblUserAgent);
            this.tpSettings.Controls.Add(this.sepMiscellaneous);
            this.tpSettings.Controls.Add(this.sepUpdateDetection);
            this.tpSettings.Controls.Add(this.sepDownloads);
            this.tpSettings.Controls.Add(this.chkIgnoreFileInformation);
            this.tpSettings.Controls.Add(this.chkCheckForUpdatesOnly);
            this.tpSettings.Controls.Add(this.chkDownloadExclusively);
            this.tpSettings.Controls.Add(this.txtUseVariablesForChanges);
            this.tpSettings.Controls.Add(this.lblUseVariableForChanges);
            this.tpSettings.Controls.Add(this.chkEnabled);
            this.tpSettings.Controls.Add(this.chkDeletePrevious);
            this.tpSettings.Controls.Add(this.txtSpoofReferer);
            this.tpSettings.Controls.Add(this.chkShareOnline);
            this.tpSettings.Controls.Add(this.lblSpoofReferer);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(375, 303);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Advanced settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserAgent.Location = new System.Drawing.Point(115, 101);
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.Size = new System.Drawing.Size(253, 20);
            this.txtUserAgent.TabIndex = 6;
            // 
            // lblUserAgent
            // 
            this.lblUserAgent.AutoSize = true;
            this.lblUserAgent.Location = new System.Drawing.Point(6, 104);
            this.lblUserAgent.Name = "lblUserAgent";
            this.lblUserAgent.Size = new System.Drawing.Size(62, 13);
            this.lblUserAgent.TabIndex = 5;
            this.lblUserAgent.Text = "User age&nt:";
            // 
            // sepMiscellaneous
            // 
            this.sepMiscellaneous.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepMiscellaneous.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepMiscellaneous.Location = new System.Drawing.Point(6, 203);
            this.sepMiscellaneous.Name = "sepMiscellaneous";
            this.sepMiscellaneous.Size = new System.Drawing.Size(363, 23);
            this.sepMiscellaneous.TabIndex = 11;
            this.sepMiscellaneous.Text = "Miscellaneous";
            this.sepMiscellaneous.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepUpdateDetection
            // 
            this.sepUpdateDetection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepUpdateDetection.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepUpdateDetection.Location = new System.Drawing.Point(6, 124);
            this.sepUpdateDetection.Name = "sepUpdateDetection";
            this.sepUpdateDetection.Size = new System.Drawing.Size(363, 23);
            this.sepUpdateDetection.TabIndex = 7;
            this.sepUpdateDetection.Text = "Update detection";
            this.sepUpdateDetection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepDownloads
            // 
            this.sepDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepDownloads.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepDownloads.Location = new System.Drawing.Point(6, 3);
            this.sepDownloads.Name = "sepDownloads";
            this.sepDownloads.Size = new System.Drawing.Size(363, 23);
            this.sepDownloads.TabIndex = 0;
            this.sepDownloads.Text = "&Downloading";
            this.sepDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkIgnoreFileInformation
            // 
            this.chkIgnoreFileInformation.AutoSize = true;
            this.chkIgnoreFileInformation.Location = new System.Drawing.Point(9, 150);
            this.chkIgnoreFileInformation.Name = "chkIgnoreFileInformation";
            this.chkIgnoreFileInformation.Size = new System.Drawing.Size(297, 17);
            this.chkIgnoreFileInformation.TabIndex = 8;
            this.chkIgnoreFileInformation.Text = "Ignore file infor&mation (do not re-download if file is missing)";
            this.chkIgnoreFileInformation.UseVisualStyleBackColor = true;
            // 
            // chkCheckForUpdatesOnly
            // 
            this.chkCheckForUpdatesOnly.AutoSize = true;
            this.chkCheckForUpdatesOnly.Location = new System.Drawing.Point(9, 52);
            this.chkCheckForUpdatesOnly.Name = "chkCheckForUpdatesOnly";
            this.chkCheckForUpdatesOnly.Size = new System.Drawing.Size(221, 17);
            this.chkCheckForUpdatesOnly.TabIndex = 2;
            this.chkCheckForUpdatesOnly.Text = "Do not download, &check for updates only";
            this.chkCheckForUpdatesOnly.UseVisualStyleBackColor = true;
            // 
            // chkDownloadExclusively
            // 
            this.chkDownloadExclusively.AutoSize = true;
            this.chkDownloadExclusively.Location = new System.Drawing.Point(9, 29);
            this.chkDownloadExclusively.Name = "chkDownloadExclusively";
            this.chkDownloadExclusively.Size = new System.Drawing.Size(359, 17);
            this.chkDownloadExclusively.TabIndex = 1;
            this.chkDownloadExclusively.Text = "Do not download this application &simultaneously with other applications";
            this.chkDownloadExclusively.UseVisualStyleBackColor = true;
            // 
            // txtUseVariablesForChanges
            // 
            this.txtUseVariablesForChanges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUseVariablesForChanges.Location = new System.Drawing.Point(259, 173);
            this.txtUseVariablesForChanges.Name = "txtUseVariablesForChanges";
            this.txtUseVariablesForChanges.Size = new System.Drawing.Size(109, 21);
            this.txtUseVariablesForChanges.TabIndex = 10;
            // 
            // lblUseVariableForChanges
            // 
            this.lblUseVariableForChanges.AutoSize = true;
            this.lblUseVariableForChanges.Location = new System.Drawing.Point(6, 176);
            this.lblUseVariableForChanges.Name = "lblUseVariableForChanges";
            this.lblUseVariableForChanges.Size = new System.Drawing.Size(247, 13);
            this.lblUseVariableForChanges.TabIndex = 9;
            this.lblUseVariableForChanges.Text = "&Use the following variable as indicator for changes:";
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.tableLayoutCommands);
            this.tpCommands.Location = new System.Drawing.Point(4, 22);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Size = new System.Drawing.Size(375, 303);
            this.tpCommands.TabIndex = 2;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // tableLayoutCommands
            // 
            this.tableLayoutCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutCommands.ColumnCount = 1;
            this.tableLayoutCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutCommands.Controls.Add(this.pnlCommandBefore, 0, 0);
            this.tableLayoutCommands.Controls.Add(this.pnlCommandAfter, 0, 1);
            this.tableLayoutCommands.Location = new System.Drawing.Point(6, 3);
            this.tableLayoutCommands.Name = "tableLayoutCommands";
            this.tableLayoutCommands.RowCount = 2;
            this.tableLayoutCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutCommands.Size = new System.Drawing.Size(366, 294);
            this.tableLayoutCommands.TabIndex = 4;
            // 
            // pnlCommandBefore
            // 
            this.pnlCommandBefore.Controls.Add(this.txtExecuteBefore);
            this.pnlCommandBefore.Controls.Add(this.lblCommandBefore);
            this.pnlCommandBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandBefore.Location = new System.Drawing.Point(0, 3);
            this.pnlCommandBefore.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pnlCommandBefore.Name = "pnlCommandBefore";
            this.pnlCommandBefore.Size = new System.Drawing.Size(363, 141);
            this.pnlCommandBefore.TabIndex = 0;
            // 
            // txtExecuteBefore
            // 
            this.txtExecuteBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecuteBefore.Application = null;
            this.txtExecuteBefore.CommandText = "";
            this.txtExecuteBefore.CommandType = Ketarin.ScriptType.Batch;
            this.txtExecuteBefore.Location = new System.Drawing.Point(0, 16);
            this.txtExecuteBefore.Margin = new System.Windows.Forms.Padding(0);
            this.txtExecuteBefore.Name = "txtExecuteBefore";
            this.txtExecuteBefore.ReadOnly = false;
            this.txtExecuteBefore.Size = new System.Drawing.Size(363, 122);
            this.txtExecuteBefore.TabIndex = 1;
            this.txtExecuteBefore.VariableNames = new string[0];
            // 
            // lblCommandBefore
            // 
            this.lblCommandBefore.AutoSize = true;
            this.lblCommandBefore.Location = new System.Drawing.Point(-3, 0);
            this.lblCommandBefore.Name = "lblCommandBefore";
            this.lblCommandBefore.Size = new System.Drawing.Size(256, 13);
            this.lblCommandBefore.TabIndex = 0;
            this.lblCommandBefore.Text = "Execute the following command &before downloading:";
            // 
            // pnlCommandAfter
            // 
            this.pnlCommandAfter.Controls.Add(this.txtExecuteAfter);
            this.pnlCommandAfter.Controls.Add(this.lblExecuteCommand);
            this.pnlCommandAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandAfter.Location = new System.Drawing.Point(0, 147);
            this.pnlCommandAfter.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlCommandAfter.Name = "pnlCommandAfter";
            this.pnlCommandAfter.Size = new System.Drawing.Size(363, 147);
            this.pnlCommandAfter.TabIndex = 1;
            // 
            // txtExecuteAfter
            // 
            this.txtExecuteAfter.Application = null;
            this.txtExecuteAfter.CommandText = "";
            this.txtExecuteAfter.CommandType = Ketarin.ScriptType.Batch;
            this.txtExecuteAfter.Location = new System.Drawing.Point(0, 21);
            this.txtExecuteAfter.Margin = new System.Windows.Forms.Padding(0);
            this.txtExecuteAfter.Name = "txtExecuteAfter";
            this.txtExecuteAfter.ReadOnly = false;
            this.txtExecuteAfter.Size = new System.Drawing.Size(363, 126);
            this.txtExecuteAfter.TabIndex = 3;
            this.txtExecuteAfter.VariableNames = new string[0];
            // 
            // lblExecuteCommand
            // 
            this.lblExecuteCommand.AutoSize = true;
            this.lblExecuteCommand.Location = new System.Drawing.Point(-3, 5);
            this.lblExecuteCommand.Name = "lblExecuteCommand";
            this.lblExecuteCommand.Size = new System.Drawing.Size(247, 13);
            this.lblExecuteCommand.TabIndex = 2;
            this.lblExecuteCommand.Text = "Execute the following command &after downloading:";
            // 
            // tpInformation
            // 
            this.tpInformation.Controls.Add(this.txtNotes);
            this.tpInformation.Controls.Add(this.lblNotes);
            this.tpInformation.Controls.Add(this.txtWebsite);
            this.tpInformation.Controls.Add(this.lblWebsite);
            this.tpInformation.Location = new System.Drawing.Point(4, 22);
            this.tpInformation.Name = "tpInformation";
            this.tpInformation.Size = new System.Drawing.Size(375, 303);
            this.tpInformation.TabIndex = 3;
            this.tpInformation.Text = "Information";
            this.tpInformation.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.AcceptsReturn = true;
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(6, 55);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(363, 242);
            this.txtNotes.TabIndex = 4;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(3, 39);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.TabIndex = 3;
            this.lblNotes.Text = "N&otes:";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWebsite.Location = new System.Drawing.Point(58, 7);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(311, 20);
            this.txtWebsite.TabIndex = 1;
            // 
            // lblWebsite
            // 
            this.lblWebsite.AutoSize = true;
            this.lblWebsite.Location = new System.Drawing.Point(3, 10);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.Size = new System.Drawing.Size(49, 13);
            this.lblWebsite.TabIndex = 0;
            this.lblWebsite.Text = "&Website:";
            // 
            // tpSetup
            // 
            this.tpSetup.Controls.Add(this.bAddInstruction);
            this.tpSetup.Controls.Add(this.label1);
            this.tpSetup.Controls.Add(this.instructionsListBox);
            this.tpSetup.Location = new System.Drawing.Point(4, 22);
            this.tpSetup.Name = "tpSetup";
            this.tpSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tpSetup.Size = new System.Drawing.Size(375, 303);
            this.tpSetup.TabIndex = 4;
            this.tpSetup.Text = "Setup";
            this.tpSetup.UseVisualStyleBackColor = true;
            // 
            // bAddInstruction
            // 
            this.bAddInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddInstruction.AutoSize = true;
            this.bAddInstruction.Location = new System.Drawing.Point(6, 274);
            this.bAddInstruction.Name = "bAddInstruction";
            this.bAddInstruction.SeparateDropdownButton = false;
            this.bAddInstruction.Size = new System.Drawing.Size(105, 23);
            this.bAddInstruction.SplitMenu = this.cmnuAddInstruction;
            this.bAddInstruction.TabIndex = 3;
            this.bAddInstruction.Text = "Add instruction";
            this.bAddInstruction.UseVisualStyleBackColor = true;
            // 
            // cmnuAddInstruction
            // 
            this.cmnuAddInstruction.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuStartProcess,
            this.mnuCopyFile,
            this.mnuCustomCommand});
            // 
            // mnuStartProcess
            // 
            this.mnuStartProcess.Index = 0;
            this.mnuStartProcess.Text = "&Start process...";
            this.mnuStartProcess.Click += new System.EventHandler(this.mnuStartProcess_Click);
            // 
            // mnuCopyFile
            // 
            this.mnuCopyFile.Index = 1;
            this.mnuCopyFile.Text = "&Copy file...";
            this.mnuCopyFile.Click += new System.EventHandler(this.mnuCopyFile_Click);
            // 
            // mnuCustomCommand
            // 
            this.mnuCustomCommand.Index = 2;
            this.mnuCustomCommand.Text = "C&ustom command...";
            this.mnuCustomCommand.Click += new System.EventHandler(this.mnuCustomCommand_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "I&nstructions to follow for setup:";
            // 
            // instructionsListBox
            // 
            this.instructionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsListBox.AutoScroll = true;
            this.instructionsListBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.instructionsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.instructionsListBox.Location = new System.Drawing.Point(6, 25);
            this.instructionsListBox.Name = "instructionsListBox";
            this.instructionsListBox.SelectedPanel = null;
            this.instructionsListBox.Size = new System.Drawing.Size(363, 243);
            this.instructionsListBox.TabIndex = 0;
            // 
            // bSaveAsDefault
            // 
            this.bSaveAsDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSaveAsDefault.Location = new System.Drawing.Point(12, 356);
            this.bSaveAsDefault.Name = "bSaveAsDefault";
            this.bSaveAsDefault.Size = new System.Drawing.Size(99, 23);
            this.bSaveAsDefault.TabIndex = 98;
            this.bSaveAsDefault.Text = "Save as &default";
            this.bSaveAsDefault.UseVisualStyleBackColor = true;
            this.bSaveAsDefault.Click += new System.EventHandler(this.bSaveAsDefault_Click);
            // 
            // ApplicationJobDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 391);
            this.Controls.Add(this.bSaveAsDefault);
            this.Controls.Add(this.tcApplication);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(423, 429);
            this.Name = "ApplicationJobDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Application";
            this.pnlDownloadSource.ResumeLayout(false);
            this.pnlDownloadSource.PerformLayout();
            this.pnlBeta.ResumeLayout(false);
            this.pnlBeta.PerformLayout();
            this.pnlTarget.ResumeLayout(false);
            this.pnlTarget.PerformLayout();
            this.tcApplication.ResumeLayout(false);
            this.tpApplication.ResumeLayout(false);
            this.tpApplication.PerformLayout();
            this.tpSettings.ResumeLayout(false);
            this.tpSettings.PerformLayout();
            this.tpCommands.ResumeLayout(false);
            this.tableLayoutCommands.ResumeLayout(false);
            this.pnlCommandBefore.ResumeLayout(false);
            this.pnlCommandBefore.PerformLayout();
            this.pnlCommandAfter.ResumeLayout(false);
            this.pnlCommandAfter.PerformLayout();
            this.tpInformation.ResumeLayout(false);
            this.tpInformation.PerformLayout();
            this.tpSetup.ResumeLayout(false);
            this.tpSetup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lblApplicationName;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.Panel pnlDownloadSource;
        private VariableTextBox txtFixedUrl;
        private System.Windows.Forms.RadioButton rbFixedUrl;
        private CDBurnerXP.Controls.Separator sepDownload;
        private System.Windows.Forms.Panel pnlTarget;
        private System.Windows.Forms.RadioButton rbFolder;
        private System.Windows.Forms.RadioButton rbFileName;
        private CDBurnerXP.Controls.Separator sepTarget;
        private VariableTextBox txtTarget;
        private System.Windows.Forms.Button bBrowseFile;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.TextBox txtFileHippoId;
        private System.Windows.Forms.RadioButton rbFileHippo;
        private System.Windows.Forms.CheckBox chkDeletePrevious;
        private System.Windows.Forms.Button bVariables;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.CheckBox chkShareOnline;
        private System.Windows.Forms.Label lblSpoofReferer;
        private VariableTextBox txtSpoofReferer;
        private System.Windows.Forms.TabControl tcApplication;
        private System.Windows.Forms.TabPage tpApplication;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.Label lblUseVariableForChanges;
        private System.Windows.Forms.ComboBox txtUseVariablesForChanges;
        private System.Windows.Forms.TabPage tpCommands;
        private System.Windows.Forms.Label lblCommandBefore;
        private System.Windows.Forms.Label lblExecuteCommand;
        private System.Windows.Forms.CheckBox chkDownloadExclusively;
        private System.Windows.Forms.Button bSaveAsDefault;
        private System.Windows.Forms.CheckBox chkCheckForUpdatesOnly;
        private System.Windows.Forms.CheckBox chkIgnoreFileInformation;
        private System.Windows.Forms.TabPage tpInformation;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtWebsite;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TabPage tpSetup;
        private wyDay.Controls.SplitButton bAddInstruction;
        private System.Windows.Forms.Label label1;
        private CDBurnerXP.Controls.AdvancedListBox instructionsListBox;
        private System.Windows.Forms.ContextMenu cmnuAddInstruction;
        private System.Windows.Forms.MenuItem mnuCopyFile;
        private System.Windows.Forms.MenuItem mnuCustomCommand;
        private System.Windows.Forms.MenuItem mnuStartProcess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutCommands;
        private System.Windows.Forms.Panel pnlCommandBefore;
        private System.Windows.Forms.Panel pnlCommandAfter;
        private CDBurnerXP.Controls.Separator sepMiscellaneous;
        private CDBurnerXP.Controls.Separator sepUpdateDetection;
        private CDBurnerXP.Controls.Separator sepDownloads;
        private System.Windows.Forms.Panel pnlBeta;
        private System.Windows.Forms.RadioButton rbBetaAvoid;
        private System.Windows.Forms.RadioButton rbAlwaysDownload;
        private System.Windows.Forms.Label lblBetaVersions;
        private System.Windows.Forms.RadioButton rbBetaDefault;
        private VariableTextBox txtUserAgent;
        private System.Windows.Forms.Label lblUserAgent;
        private CommandControl txtExecuteBefore;
        private CommandControl txtExecuteAfter;
    }
}