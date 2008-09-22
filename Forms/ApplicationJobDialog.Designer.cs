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
            this.txtFixedUrl = new System.Windows.Forms.TextBox();
            this.rbFixedUrl = new System.Windows.Forms.RadioButton();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.bBrowseFile = new System.Windows.Forms.Button();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.rbFileName = new System.Windows.Forms.RadioButton();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.chkDeletePrevious = new System.Windows.Forms.CheckBox();
            this.sepOptions = new CDBurnerXP.Controls.Separator();
            this.sepTarget = new CDBurnerXP.Controls.Separator();
            this.sepDownload = new CDBurnerXP.Controls.Separator();
            this.panel1.SuspendLayout();
            this.pnlTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(294, 242);
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
            this.bOK.Location = new System.Drawing.Point(213, 242);
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
            this.lblApplicationName.Location = new System.Drawing.Point(12, 15);
            this.lblApplicationName.Name = "lblApplicationName";
            this.lblApplicationName.Size = new System.Drawing.Size(91, 13);
            this.lblApplicationName.TabIndex = 0;
            this.lblApplicationName.Text = "&Application name:";
            // 
            // txtApplicationName
            // 
            this.txtApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApplicationName.Location = new System.Drawing.Point(109, 12);
            this.txtApplicationName.Name = "txtApplicationName";
            this.txtApplicationName.Size = new System.Drawing.Size(260, 20);
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
            this.panel1.Location = new System.Drawing.Point(12, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 54);
            this.panel1.TabIndex = 3;
            // 
            // bVariables
            // 
            this.bVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bVariables.Location = new System.Drawing.Point(282, 0);
            this.bVariables.Name = "bVariables";
            this.bVariables.Size = new System.Drawing.Size(75, 23);
            this.bVariables.TabIndex = 5;
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
            this.txtFileHippoId.Size = new System.Drawing.Size(262, 20);
            this.txtFileHippoId.TabIndex = 7;
            this.txtFileHippoId.TextChanged += new System.EventHandler(this.txtFileHippoId_TextChanged);
            // 
            // rbFileHippo
            // 
            this.rbFileHippo.AutoSize = true;
            this.rbFileHippo.Location = new System.Drawing.Point(3, 29);
            this.rbFileHippo.Name = "rbFileHippo";
            this.rbFileHippo.Size = new System.Drawing.Size(86, 17);
            this.rbFileHippo.TabIndex = 6;
            this.rbFileHippo.Text = "File&Hippo ID:";
            this.rbFileHippo.UseVisualStyleBackColor = true;
            // 
            // txtFixedUrl
            // 
            this.txtFixedUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFixedUrl.Location = new System.Drawing.Point(95, 2);
            this.txtFixedUrl.Name = "txtFixedUrl";
            this.txtFixedUrl.Size = new System.Drawing.Size(181, 20);
            this.txtFixedUrl.TabIndex = 4;
            this.txtFixedUrl.TextChanged += new System.EventHandler(this.txtFixedUrl_TextChanged);
            // 
            // rbFixedUrl
            // 
            this.rbFixedUrl.AutoSize = true;
            this.rbFixedUrl.Checked = true;
            this.rbFixedUrl.Location = new System.Drawing.Point(3, 3);
            this.rbFixedUrl.Name = "rbFixedUrl";
            this.rbFixedUrl.Size = new System.Drawing.Size(50, 17);
            this.rbFixedUrl.TabIndex = 3;
            this.rbFixedUrl.TabStop = true;
            this.rbFixedUrl.Text = "&URL:";
            this.rbFixedUrl.UseVisualStyleBackColor = true;
            // 
            // pnlTarget
            // 
            this.pnlTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTarget.Controls.Add(this.bBrowseFile);
            this.pnlTarget.Controls.Add(this.txtTarget);
            this.pnlTarget.Controls.Add(this.rbFolder);
            this.pnlTarget.Controls.Add(this.rbFileName);
            this.pnlTarget.Location = new System.Drawing.Point(8, 141);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(369, 53);
            this.pnlTarget.TabIndex = 9;
            // 
            // bBrowseFile
            // 
            this.bBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBrowseFile.Location = new System.Drawing.Point(329, 24);
            this.bBrowseFile.Name = "bBrowseFile";
            this.bBrowseFile.Size = new System.Drawing.Size(32, 23);
            this.bBrowseFile.TabIndex = 12;
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
            this.txtTarget.Location = new System.Drawing.Point(3, 26);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(320, 20);
            this.txtTarget.TabIndex = 11;
            // 
            // rbFolder
            // 
            this.rbFolder.AutoSize = true;
            this.rbFolder.Location = new System.Drawing.Point(91, 3);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(90, 17);
            this.rbFolder.TabIndex = 10;
            this.rbFolder.Text = "Save in f&older";
            this.rbFolder.UseVisualStyleBackColor = true;
            this.rbFolder.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbFileName
            // 
            this.rbFileName.AutoSize = true;
            this.rbFileName.Checked = true;
            this.rbFileName.Location = new System.Drawing.Point(7, 3);
            this.rbFileName.Name = "rbFileName";
            this.rbFileName.Size = new System.Drawing.Size(78, 17);
            this.rbFileName.TabIndex = 9;
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
            this.chkEnabled.Location = new System.Drawing.Point(15, 215);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 14;
            this.chkEnabled.Text = "&Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // chkDeletePrevious
            // 
            this.chkDeletePrevious.AutoSize = true;
            this.chkDeletePrevious.Location = new System.Drawing.Point(86, 215);
            this.chkDeletePrevious.Name = "chkDeletePrevious";
            this.chkDeletePrevious.Size = new System.Drawing.Size(218, 17);
            this.chkDeletePrevious.TabIndex = 15;
            this.chkDeletePrevious.Text = "Always &delete previously downloaded file";
            this.chkDeletePrevious.UseVisualStyleBackColor = true;
            // 
            // sepOptions
            // 
            this.sepOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepOptions.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepOptions.Location = new System.Drawing.Point(5, 189);
            this.sepOptions.Name = "sepOptions";
            this.sepOptions.Size = new System.Drawing.Size(371, 23);
            this.sepOptions.TabIndex = 13;
            this.sepOptions.Text = "Options";
            this.sepOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepTarget
            // 
            this.sepTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepTarget.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepTarget.Location = new System.Drawing.Point(5, 117);
            this.sepTarget.Name = "sepTarget";
            this.sepTarget.Size = new System.Drawing.Size(371, 23);
            this.sepTarget.TabIndex = 8;
            this.sepTarget.Text = "Download location";
            this.sepTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepDownload
            // 
            this.sepDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepDownload.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepDownload.Location = new System.Drawing.Point(5, 38);
            this.sepDownload.Name = "sepDownload";
            this.sepDownload.Size = new System.Drawing.Size(371, 23);
            this.sepDownload.TabIndex = 2;
            this.sepDownload.Text = "Download source";
            this.sepDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ApplicationJobDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 277);
            this.Controls.Add(this.chkDeletePrevious);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.sepOptions);
            this.Controls.Add(this.sepTarget);
            this.Controls.Add(this.pnlTarget);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sepDownload);
            this.Controls.Add(this.txtApplicationName);
            this.Controls.Add(this.lblApplicationName);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label lblApplicationName;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtFixedUrl;
        private System.Windows.Forms.RadioButton rbFixedUrl;
        private CDBurnerXP.Controls.Separator sepDownload;
        private System.Windows.Forms.Panel pnlTarget;
        private System.Windows.Forms.RadioButton rbFolder;
        private System.Windows.Forms.RadioButton rbFileName;
        private CDBurnerXP.Controls.Separator sepTarget;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button bBrowseFile;
        private CDBurnerXP.Controls.Separator sepOptions;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.TextBox txtFileHippoId;
        private System.Windows.Forms.RadioButton rbFileHippo;
        private System.Windows.Forms.CheckBox chkDeletePrevious;
        private System.Windows.Forms.Button bVariables;
    }
}