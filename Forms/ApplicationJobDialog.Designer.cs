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
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.txtCommand = new Ketarin.Forms.VariableTextBox();
            this.lblExecuteCommand = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.sepTarget = new CDBurnerXP.Controls.Separator();
            this.sepDownload = new CDBurnerXP.Controls.Separator();
            this.chkShareOnline = new System.Windows.Forms.CheckBox();
            this.lblSpoofReferer = new System.Windows.Forms.Label();
            this.txtSpoofReferer = new System.Windows.Forms.TextBox();
            this.rbBetaDefault = new System.Windows.Forms.RadioButton();
            this.lblBetaVersions = new System.Windows.Forms.Label();
            this.rbAlwaysDownload = new System.Windows.Forms.RadioButton();
            this.rbBetaAvoid = new System.Windows.Forms.RadioButton();
            this.tcApplication = new System.Windows.Forms.TabControl();
            this.tpApplication = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.txtUseVariablesForChanges = new Ketarin.Forms.VariableTextBox();
            this.lblUseVariableForChanges = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnlTarget.SuspendLayout();
            this.tcApplication.SuspendLayout();
            this.tpApplication.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(320, 292);
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
            this.bOK.Location = new System.Drawing.Point(239, 292);
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.bVariables);
            this.panel1.Controls.Add(this.txtFileHippoId);
            this.panel1.Controls.Add(this.rbFileHippo);
            this.panel1.Controls.Add(this.txtFixedUrl);
            this.panel1.Controls.Add(this.rbFixedUrl);
            this.panel1.Location = new System.Drawing.Point(4, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 54);
            this.panel1.TabIndex = 3;
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
            this.txtFileHippoId.Location = new System.Drawing.Point(95, 28);
            this.txtFileHippoId.Name = "txtFileHippoId";
            this.txtFileHippoId.Size = new System.Drawing.Size(270, 20);
            this.txtFileHippoId.TabIndex = 15;
            this.txtFileHippoId.TextChanged += new System.EventHandler(this.txtFileHippoId_TextChanged);
            // 
            // rbFileHippo
            // 
            this.rbFileHippo.AutoSize = true;
            this.rbFileHippo.Location = new System.Drawing.Point(3, 29);
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
            this.txtFixedUrl.VariableNames = new string[0];
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
            this.pnlTarget.Location = new System.Drawing.Point(3, 164);
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
            this.txtTarget.VariableNames = new string[0];
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
            this.chkEnabled.Location = new System.Drawing.Point(6, 52);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 33;
            this.chkEnabled.Text = "&Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // chkDeletePrevious
            // 
            this.chkDeletePrevious.AutoSize = true;
            this.chkDeletePrevious.Checked = true;
            this.chkDeletePrevious.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeletePrevious.Location = new System.Drawing.Point(6, 6);
            this.chkDeletePrevious.Name = "chkDeletePrevious";
            this.chkDeletePrevious.Size = new System.Drawing.Size(218, 17);
            this.chkDeletePrevious.TabIndex = 30;
            this.chkDeletePrevious.Text = "Always &delete previously downloaded file";
            this.chkDeletePrevious.UseVisualStyleBackColor = true;
            // 
            // txtCommand
            // 
            this.txtCommand.AcceptsReturn = true;
            this.txtCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommand.Location = new System.Drawing.Point(6, 120);
            this.txtCommand.Multiline = true;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCommand.Size = new System.Drawing.Size(363, 49);
            this.txtCommand.TabIndex = 40;
            this.txtCommand.VariableNames = new string[0];
            this.txtCommand.WordWrap = false;
            // 
            // lblExecuteCommand
            // 
            this.lblExecuteCommand.AutoSize = true;
            this.lblExecuteCommand.Location = new System.Drawing.Point(3, 104);
            this.lblExecuteCommand.Name = "lblExecuteCommand";
            this.lblExecuteCommand.Size = new System.Drawing.Size(247, 13);
            this.lblExecuteCommand.TabIndex = 39;
            this.lblExecuteCommand.Text = "E&xecute the following command after downloading:";
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
            this.sepTarget.Location = new System.Drawing.Point(3, 141);
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
            this.chkShareOnline.Location = new System.Drawing.Point(6, 29);
            this.chkShareOnline.Name = "chkShareOnline";
            this.chkShareOnline.Size = new System.Drawing.Size(212, 17);
            this.chkShareOnline.TabIndex = 31;
            this.chkShareOnline.Text = "S&hare this application online with others";
            this.chkShareOnline.UseVisualStyleBackColor = true;
            // 
            // lblSpoofReferer
            // 
            this.lblSpoofReferer.AutoSize = true;
            this.lblSpoofReferer.Location = new System.Drawing.Point(3, 178);
            this.lblSpoofReferer.Name = "lblSpoofReferer";
            this.lblSpoofReferer.Size = new System.Drawing.Size(103, 13);
            this.lblSpoofReferer.TabIndex = 41;
            this.lblSpoofReferer.Text = "Sp&oof HTTP referer:";
            // 
            // txtSpoofReferer
            // 
            this.txtSpoofReferer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpoofReferer.Location = new System.Drawing.Point(112, 175);
            this.txtSpoofReferer.Name = "txtSpoofReferer";
            this.txtSpoofReferer.Size = new System.Drawing.Size(257, 20);
            this.txtSpoofReferer.TabIndex = 42;
            // 
            // rbBetaDefault
            // 
            this.rbBetaDefault.AutoSize = true;
            this.rbBetaDefault.Checked = true;
            this.rbBetaDefault.Location = new System.Drawing.Point(83, 79);
            this.rbBetaDefault.Name = "rbBetaDefault";
            this.rbBetaDefault.Size = new System.Drawing.Size(79, 17);
            this.rbBetaDefault.TabIndex = 36;
            this.rbBetaDefault.TabStop = true;
            this.rbBetaDefault.Text = "Use &default";
            this.rbBetaDefault.UseVisualStyleBackColor = true;
            // 
            // lblBetaVersions
            // 
            this.lblBetaVersions.AutoSize = true;
            this.lblBetaVersions.Location = new System.Drawing.Point(3, 81);
            this.lblBetaVersions.Name = "lblBetaVersions";
            this.lblBetaVersions.Size = new System.Drawing.Size(74, 13);
            this.lblBetaVersions.TabIndex = 34;
            this.lblBetaVersions.Text = "&Beta versions:";
            // 
            // rbAlwaysDownload
            // 
            this.rbAlwaysDownload.AutoSize = true;
            this.rbAlwaysDownload.Location = new System.Drawing.Point(226, 79);
            this.rbAlwaysDownload.Name = "rbAlwaysDownload";
            this.rbAlwaysDownload.Size = new System.Drawing.Size(108, 17);
            this.rbAlwaysDownload.TabIndex = 38;
            this.rbAlwaysDownload.Text = "Download al&ways";
            this.rbAlwaysDownload.UseVisualStyleBackColor = true;
            // 
            // rbBetaAvoid
            // 
            this.rbBetaAvoid.AutoSize = true;
            this.rbBetaAvoid.Location = new System.Drawing.Point(168, 79);
            this.rbBetaAvoid.Name = "rbBetaAvoid";
            this.rbBetaAvoid.Size = new System.Drawing.Size(52, 17);
            this.rbBetaAvoid.TabIndex = 37;
            this.rbBetaAvoid.Text = "A&void";
            this.rbBetaAvoid.UseVisualStyleBackColor = true;
            // 
            // tcApplication
            // 
            this.tcApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcApplication.Controls.Add(this.tpApplication);
            this.tcApplication.Controls.Add(this.tpSettings);
            this.tcApplication.Location = new System.Drawing.Point(12, 12);
            this.tcApplication.Name = "tcApplication";
            this.tcApplication.SelectedIndex = 0;
            this.tcApplication.Size = new System.Drawing.Size(383, 265);
            this.tcApplication.TabIndex = 0;
            // 
            // tpApplication
            // 
            this.tpApplication.Controls.Add(this.lblApplicationName);
            this.tpApplication.Controls.Add(this.txtApplicationName);
            this.tpApplication.Controls.Add(this.lblCategory);
            this.tpApplication.Controls.Add(this.cboCategory);
            this.tpApplication.Controls.Add(this.sepDownload);
            this.tpApplication.Controls.Add(this.panel1);
            this.tpApplication.Controls.Add(this.pnlTarget);
            this.tpApplication.Controls.Add(this.sepTarget);
            this.tpApplication.Location = new System.Drawing.Point(4, 22);
            this.tpApplication.Name = "tpApplication";
            this.tpApplication.Padding = new System.Windows.Forms.Padding(3);
            this.tpApplication.Size = new System.Drawing.Size(375, 239);
            this.tpApplication.TabIndex = 0;
            this.tpApplication.Text = "Application";
            this.tpApplication.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.txtUseVariablesForChanges);
            this.tpSettings.Controls.Add(this.lblUseVariableForChanges);
            this.tpSettings.Controls.Add(this.rbBetaAvoid);
            this.tpSettings.Controls.Add(this.chkEnabled);
            this.tpSettings.Controls.Add(this.rbAlwaysDownload);
            this.tpSettings.Controls.Add(this.chkDeletePrevious);
            this.tpSettings.Controls.Add(this.lblBetaVersions);
            this.tpSettings.Controls.Add(this.txtCommand);
            this.tpSettings.Controls.Add(this.rbBetaDefault);
            this.tpSettings.Controls.Add(this.lblExecuteCommand);
            this.tpSettings.Controls.Add(this.txtSpoofReferer);
            this.tpSettings.Controls.Add(this.chkShareOnline);
            this.tpSettings.Controls.Add(this.lblSpoofReferer);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(375, 239);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Advanced settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // txtUseVariablesForChanges
            // 
            this.txtUseVariablesForChanges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUseVariablesForChanges.Location = new System.Drawing.Point(256, 206);
            this.txtUseVariablesForChanges.Name = "txtUseVariablesForChanges";
            this.txtUseVariablesForChanges.Size = new System.Drawing.Size(113, 20);
            this.txtUseVariablesForChanges.TabIndex = 44;
            this.txtUseVariablesForChanges.VariableNames = new string[0];
            // 
            // lblUseVariableForChanges
            // 
            this.lblUseVariableForChanges.AutoSize = true;
            this.lblUseVariableForChanges.Location = new System.Drawing.Point(3, 209);
            this.lblUseVariableForChanges.Name = "lblUseVariableForChanges";
            this.lblUseVariableForChanges.Size = new System.Drawing.Size(247, 13);
            this.lblUseVariableForChanges.TabIndex = 43;
            this.lblUseVariableForChanges.Text = "&Use the following variable as indicator for changes:";
            // 
            // ApplicationJobDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 327);
            this.Controls.Add(this.tcApplication);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplicationJobDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Application";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlTarget.ResumeLayout(false);
            this.pnlTarget.PerformLayout();
            this.tcApplication.ResumeLayout(false);
            this.tpApplication.ResumeLayout(false);
            this.tpApplication.PerformLayout();
            this.tpSettings.ResumeLayout(false);
            this.tpSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lblApplicationName;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.Panel panel1;
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
        private VariableTextBox txtCommand;
        private System.Windows.Forms.Label lblExecuteCommand;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.CheckBox chkShareOnline;
        private System.Windows.Forms.Label lblSpoofReferer;
        private System.Windows.Forms.TextBox txtSpoofReferer;
        private System.Windows.Forms.RadioButton rbBetaDefault;
        private System.Windows.Forms.Label lblBetaVersions;
        private System.Windows.Forms.RadioButton rbAlwaysDownload;
        private System.Windows.Forms.RadioButton rbBetaAvoid;
        private System.Windows.Forms.TabControl tcApplication;
        private System.Windows.Forms.TabPage tpApplication;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.Label lblUseVariableForChanges;
        private VariableTextBox txtUseVariablesForChanges;
    }
}