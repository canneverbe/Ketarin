namespace Ketarin.Forms
{
    partial class SettingsDialog
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
            this.chkUpdateAtStartup = new System.Windows.Forms.CheckBox();
            this.lblDefaultCommand = new System.Windows.Forms.Label();
            this.txtDefaultCommand = new System.Windows.Forms.TextBox();
            this.txtCustomColumn = new System.Windows.Forms.TextBox();
            this.lblCustomColumn = new System.Windows.Forms.Label();
            this.chkAvoidBeta = new System.Windows.Forms.CheckBox();
            this.lblConnectionTimeout = new System.Windows.Forms.Label();
            this.nConnectionTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtProxyServer = new System.Windows.Forms.TextBox();
            this.nProxyPort = new System.Windows.Forms.NumericUpDown();
            this.lblProxyUser = new System.Windows.Forms.Label();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.lblNumThreads = new System.Windows.Forms.Label();
            this.nNumThreads = new System.Windows.Forms.NumericUpDown();
            this.cboGlobalVariables = new System.Windows.Forms.ComboBox();
            this.txtGlobalVariableValue = new System.Windows.Forms.TextBox();
            this.lblVarName = new System.Windows.Forms.Label();
            this.lblvalue = new System.Windows.Forms.Label();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.nNumRetries = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkUpdateOnlineDatabase = new System.Windows.Forms.CheckBox();
            this.sepGlobalVariables = new CDBurnerXP.Controls.Separator();
            this.sepProxy = new CDBurnerXP.Controls.Separator();
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumRetries)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(339, 341);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 27;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(258, 341);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 26;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // chkUpdateAtStartup
            // 
            this.chkUpdateAtStartup.AutoSize = true;
            this.chkUpdateAtStartup.Location = new System.Drawing.Point(6, 11);
            this.chkUpdateAtStartup.Name = "chkUpdateAtStartup";
            this.chkUpdateAtStartup.Size = new System.Drawing.Size(172, 17);
            this.chkUpdateAtStartup.TabIndex = 0;
            this.chkUpdateAtStartup.Text = "&Update automatically at startup";
            this.chkUpdateAtStartup.UseVisualStyleBackColor = true;
            // 
            // lblDefaultCommand
            // 
            this.lblDefaultCommand.AutoSize = true;
            this.lblDefaultCommand.Location = new System.Drawing.Point(3, 77);
            this.lblDefaultCommand.Name = "lblDefaultCommand";
            this.lblDefaultCommand.Size = new System.Drawing.Size(283, 13);
            this.lblDefaultCommand.TabIndex = 2;
            this.lblDefaultCommand.Text = "Default &command to execute after updating an application:";
            // 
            // txtDefaultCommand
            // 
            this.txtDefaultCommand.AcceptsReturn = true;
            this.txtDefaultCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultCommand.Location = new System.Drawing.Point(6, 93);
            this.txtDefaultCommand.Multiline = true;
            this.txtDefaultCommand.Name = "txtDefaultCommand";
            this.txtDefaultCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDefaultCommand.Size = new System.Drawing.Size(382, 73);
            this.txtDefaultCommand.TabIndex = 3;
            this.txtDefaultCommand.WordWrap = false;
            // 
            // txtCustomColumn
            // 
            this.txtCustomColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn.Location = new System.Drawing.Point(149, 172);
            this.txtCustomColumn.Name = "txtCustomColumn";
            this.txtCustomColumn.Size = new System.Drawing.Size(239, 20);
            this.txtCustomColumn.TabIndex = 5;
            // 
            // lblCustomColumn
            // 
            this.lblCustomColumn.AutoSize = true;
            this.lblCustomColumn.Location = new System.Drawing.Point(6, 175);
            this.lblCustomColumn.Name = "lblCustomColumn";
            this.lblCustomColumn.Size = new System.Drawing.Size(137, 13);
            this.lblCustomColumn.TabIndex = 4;
            this.lblCustomColumn.Text = "&Variable for custom column:";
            // 
            // chkAvoidBeta
            // 
            this.chkAvoidBeta.AutoSize = true;
            this.chkAvoidBeta.Location = new System.Drawing.Point(6, 34);
            this.chkAvoidBeta.Name = "chkAvoidBeta";
            this.chkAvoidBeta.Size = new System.Drawing.Size(181, 17);
            this.chkAvoidBeta.TabIndex = 1;
            this.chkAvoidBeta.Text = "Avoid &beta versions on FileHippo";
            this.chkAvoidBeta.UseVisualStyleBackColor = true;
            // 
            // lblConnectionTimeout
            // 
            this.lblConnectionTimeout.AutoSize = true;
            this.lblConnectionTimeout.Location = new System.Drawing.Point(7, 11);
            this.lblConnectionTimeout.Name = "lblConnectionTimeout";
            this.lblConnectionTimeout.Size = new System.Drawing.Size(101, 13);
            this.lblConnectionTimeout.TabIndex = 6;
            this.lblConnectionTimeout.Text = "&Connection timeout:";
            // 
            // nConnectionTimeout
            // 
            this.nConnectionTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nConnectionTimeout.Location = new System.Drawing.Point(114, 9);
            this.nConnectionTimeout.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nConnectionTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nConnectionTimeout.Name = "nConnectionTimeout";
            this.nConnectionTimeout.Size = new System.Drawing.Size(210, 20);
            this.nConnectionTimeout.TabIndex = 7;
            this.nConnectionTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSeconds
            // 
            this.lblSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(331, 11);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 8;
            this.lblSeconds.Text = "seconds";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(15, 124);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 19;
            this.lblServer.Text = "&Server:";
            // 
            // txtProxyServer
            // 
            this.txtProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyServer.Location = new System.Drawing.Point(82, 121);
            this.txtProxyServer.Name = "txtProxyServer";
            this.txtProxyServer.Size = new System.Drawing.Size(242, 20);
            this.txtProxyServer.TabIndex = 20;
            // 
            // nProxyPort
            // 
            this.nProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nProxyPort.Location = new System.Drawing.Point(330, 121);
            this.nProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nProxyPort.Name = "nProxyPort";
            this.nProxyPort.Size = new System.Drawing.Size(48, 20);
            this.nProxyPort.TabIndex = 21;
            // 
            // lblProxyUser
            // 
            this.lblProxyUser.AutoSize = true;
            this.lblProxyUser.Location = new System.Drawing.Point(15, 150);
            this.lblProxyUser.Name = "lblProxyUser";
            this.lblProxyUser.Size = new System.Drawing.Size(61, 13);
            this.lblProxyUser.TabIndex = 22;
            this.lblProxyUser.Text = "Us&er name:";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyUser.Location = new System.Drawing.Point(82, 147);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(296, 20);
            this.txtProxyUser.TabIndex = 23;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(15, 176);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 24;
            this.lblProxyPassword.Text = "&Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPassword.Location = new System.Drawing.Point(82, 173);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(296, 20);
            this.txtProxyPassword.TabIndex = 25;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // lblNumThreads
            // 
            this.lblNumThreads.AutoSize = true;
            this.lblNumThreads.Location = new System.Drawing.Point(7, 37);
            this.lblNumThreads.Name = "lblNumThreads";
            this.lblNumThreads.Size = new System.Drawing.Size(97, 13);
            this.lblNumThreads.TabIndex = 9;
            this.lblNumThreads.Text = "&Number of threads:";
            // 
            // nNumThreads
            // 
            this.nNumThreads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nNumThreads.Location = new System.Drawing.Point(114, 35);
            this.nNumThreads.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nNumThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nNumThreads.Name = "nNumThreads";
            this.nNumThreads.Size = new System.Drawing.Size(264, 20);
            this.nNumThreads.TabIndex = 10;
            this.nNumThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cboGlobalVariables
            // 
            this.cboGlobalVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGlobalVariables.FormattingEnabled = true;
            this.cboGlobalVariables.Location = new System.Drawing.Point(54, 233);
            this.cboGlobalVariables.Name = "cboGlobalVariables";
            this.cboGlobalVariables.Size = new System.Drawing.Size(106, 21);
            this.cboGlobalVariables.Sorted = true;
            this.cboGlobalVariables.TabIndex = 13;
            this.cboGlobalVariables.SelectedIndexChanged += new System.EventHandler(this.cboGlobalVariables_SelectedIndexChanged);
            // 
            // txtGlobalVariableValue
            // 
            this.txtGlobalVariableValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobalVariableValue.Location = new System.Drawing.Point(215, 233);
            this.txtGlobalVariableValue.Name = "txtGlobalVariableValue";
            this.txtGlobalVariableValue.Size = new System.Drawing.Size(173, 20);
            this.txtGlobalVariableValue.TabIndex = 15;
            this.txtGlobalVariableValue.TextChanged += new System.EventHandler(this.txtGlobalVariableValue_TextChanged);
            // 
            // lblVarName
            // 
            this.lblVarName.AutoSize = true;
            this.lblVarName.Location = new System.Drawing.Point(10, 236);
            this.lblVarName.Name = "lblVarName";
            this.lblVarName.Size = new System.Drawing.Size(38, 13);
            this.lblVarName.TabIndex = 12;
            this.lblVarName.Text = "&Name:";
            // 
            // lblvalue
            // 
            this.lblvalue.AutoSize = true;
            this.lblvalue.Location = new System.Drawing.Point(172, 236);
            this.lblvalue.Name = "lblvalue";
            this.lblvalue.Size = new System.Drawing.Size(37, 13);
            this.lblvalue.TabIndex = 14;
            this.lblvalue.Text = "&Value:";
            // 
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bAdd.Location = new System.Drawing.Point(232, 259);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 16;
            this.bAdd.Text = "&Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRemove.Enabled = false;
            this.bRemove.Location = new System.Drawing.Point(313, 259);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(75, 23);
            this.bRemove.TabIndex = 17;
            this.bRemove.Text = "&Remove";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // nNumRetries
            // 
            this.nNumRetries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nNumRetries.Location = new System.Drawing.Point(114, 61);
            this.nNumRetries.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nNumRetries.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nNumRetries.Name = "nNumRetries";
            this.nNumRetries.Size = new System.Drawing.Size(264, 20);
            this.nNumRetries.TabIndex = 28;
            this.nNumRetries.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Number of retrie&s:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 314);
            this.tabControl1.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkUpdateOnlineDatabase);
            this.tabPage1.Controls.Add(this.txtDefaultCommand);
            this.tabPage1.Controls.Add(this.bRemove);
            this.tabPage1.Controls.Add(this.chkUpdateAtStartup);
            this.tabPage1.Controls.Add(this.bAdd);
            this.tabPage1.Controls.Add(this.lblDefaultCommand);
            this.tabPage1.Controls.Add(this.lblvalue);
            this.tabPage1.Controls.Add(this.txtCustomColumn);
            this.tabPage1.Controls.Add(this.lblVarName);
            this.tabPage1.Controls.Add(this.lblCustomColumn);
            this.tabPage1.Controls.Add(this.txtGlobalVariableValue);
            this.tabPage1.Controls.Add(this.chkAvoidBeta);
            this.tabPage1.Controls.Add(this.cboGlobalVariables);
            this.tabPage1.Controls.Add(this.sepGlobalVariables);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 288);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.nConnectionTimeout);
            this.tabPage2.Controls.Add(this.txtProxyPassword);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lblProxyPassword);
            this.tabPage2.Controls.Add(this.lblConnectionTimeout);
            this.tabPage2.Controls.Add(this.txtProxyUser);
            this.tabPage2.Controls.Add(this.nNumRetries);
            this.tabPage2.Controls.Add(this.lblProxyUser);
            this.tabPage2.Controls.Add(this.lblSeconds);
            this.tabPage2.Controls.Add(this.nProxyPort);
            this.tabPage2.Controls.Add(this.nNumThreads);
            this.tabPage2.Controls.Add(this.txtProxyServer);
            this.tabPage2.Controls.Add(this.lblNumThreads);
            this.tabPage2.Controls.Add(this.lblServer);
            this.tabPage2.Controls.Add(this.sepProxy);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(394, 288);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Connection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkUpdateOnlineDatabase
            // 
            this.chkUpdateOnlineDatabase.AutoSize = true;
            this.chkUpdateOnlineDatabase.Location = new System.Drawing.Point(6, 57);
            this.chkUpdateOnlineDatabase.Name = "chkUpdateOnlineDatabase";
            this.chkUpdateOnlineDatabase.Size = new System.Drawing.Size(220, 17);
            this.chkUpdateOnlineDatabase.TabIndex = 18;
            this.chkUpdateOnlineDatabase.Text = "&Check for updates in the online database";
            this.chkUpdateOnlineDatabase.UseVisualStyleBackColor = true;
            // 
            // sepGlobalVariables
            // 
            this.sepGlobalVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepGlobalVariables.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepGlobalVariables.Location = new System.Drawing.Point(3, 205);
            this.sepGlobalVariables.Name = "sepGlobalVariables";
            this.sepGlobalVariables.Size = new System.Drawing.Size(385, 23);
            this.sepGlobalVariables.TabIndex = 11;
            this.sepGlobalVariables.Text = "Global variables";
            this.sepGlobalVariables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sepProxy
            // 
            this.sepProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepProxy.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepProxy.Location = new System.Drawing.Point(3, 93);
            this.sepProxy.Name = "sepProxy";
            this.sepProxy.Size = new System.Drawing.Size(375, 23);
            this.sepProxy.TabIndex = 18;
            this.sepProxy.Text = "HTTP proxy settings";
            this.sepProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 376);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumRetries)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox chkUpdateAtStartup;
        private System.Windows.Forms.Label lblDefaultCommand;
        private System.Windows.Forms.TextBox txtDefaultCommand;
        private System.Windows.Forms.TextBox txtCustomColumn;
        private System.Windows.Forms.Label lblCustomColumn;
        private System.Windows.Forms.CheckBox chkAvoidBeta;
        private System.Windows.Forms.Label lblConnectionTimeout;
        private System.Windows.Forms.NumericUpDown nConnectionTimeout;
        private System.Windows.Forms.Label lblSeconds;
        private CDBurnerXP.Controls.Separator sepProxy;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtProxyServer;
        private System.Windows.Forms.NumericUpDown nProxyPort;
        private System.Windows.Forms.Label lblProxyUser;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label lblNumThreads;
        private System.Windows.Forms.NumericUpDown nNumThreads;
        private CDBurnerXP.Controls.Separator sepGlobalVariables;
        private System.Windows.Forms.ComboBox cboGlobalVariables;
        private System.Windows.Forms.TextBox txtGlobalVariableValue;
        private System.Windows.Forms.Label lblVarName;
        private System.Windows.Forms.Label lblvalue;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.NumericUpDown nNumRetries;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkUpdateOnlineDatabase;
    }
}