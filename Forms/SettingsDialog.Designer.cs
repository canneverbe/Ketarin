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
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.chkBackups = new System.Windows.Forms.CheckBox();
            this.chkMinToTray = new System.Windows.Forms.CheckBox();
            this.chkUpdateOnlineDatabase = new System.Windows.Forms.CheckBox();
            this.sepGlobalVariables = new CDBurnerXP.Controls.Separator();
            this.tpConnection = new System.Windows.Forms.TabPage();
            this.sepProxy = new CDBurnerXP.Controls.Separator();
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.txtPreUpdateCommand = new Ketarin.Forms.TextBox();
            this.lblPreUpdateCommand = new System.Windows.Forms.Label();
            this.lblPostUpdateAllCommand = new System.Windows.Forms.Label();
            this.lblPostUpdateCommand = new System.Windows.Forms.Label();
            this.txtPostUpdateAllCommand = new Ketarin.Forms.TextBox();
            this.txtPostUpdateCommand = new Ketarin.Forms.TextBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.txtCustomColumn2 = new System.Windows.Forms.TextBox();
            this.lblCustomColumn2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumRetries)).BeginInit();
            this.tcSettings.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpConnection.SuspendLayout();
            this.tpCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(339, 321);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(258, 321);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 3;
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
            // txtCustomColumn
            // 
            this.txtCustomColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn.Location = new System.Drawing.Point(158, 130);
            this.txtCustomColumn.Name = "txtCustomColumn";
            this.txtCustomColumn.Size = new System.Drawing.Size(230, 20);
            this.txtCustomColumn.TabIndex = 6;
            // 
            // lblCustomColumn
            // 
            this.lblCustomColumn.AutoSize = true;
            this.lblCustomColumn.Location = new System.Drawing.Point(6, 133);
            this.lblCustomColumn.Name = "lblCustomColumn";
            this.lblCustomColumn.Size = new System.Drawing.Size(137, 13);
            this.lblCustomColumn.TabIndex = 5;
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
            this.lblConnectionTimeout.TabIndex = 0;
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
            this.nConnectionTimeout.TabIndex = 1;
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
            this.lblSeconds.TabIndex = 2;
            this.lblSeconds.Text = "seconds";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(15, 124);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 8;
            this.lblServer.Text = "&Server:";
            // 
            // txtProxyServer
            // 
            this.txtProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyServer.Location = new System.Drawing.Point(82, 121);
            this.txtProxyServer.Name = "txtProxyServer";
            this.txtProxyServer.Size = new System.Drawing.Size(242, 20);
            this.txtProxyServer.TabIndex = 9;
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
            this.nProxyPort.TabIndex = 10;
            // 
            // lblProxyUser
            // 
            this.lblProxyUser.AutoSize = true;
            this.lblProxyUser.Location = new System.Drawing.Point(15, 150);
            this.lblProxyUser.Name = "lblProxyUser";
            this.lblProxyUser.Size = new System.Drawing.Size(61, 13);
            this.lblProxyUser.TabIndex = 11;
            this.lblProxyUser.Text = "Us&er name:";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyUser.Location = new System.Drawing.Point(82, 147);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(296, 20);
            this.txtProxyUser.TabIndex = 12;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(15, 176);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 13;
            this.lblProxyPassword.Text = "&Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPassword.Location = new System.Drawing.Point(82, 173);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(296, 20);
            this.txtProxyPassword.TabIndex = 14;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // lblNumThreads
            // 
            this.lblNumThreads.AutoSize = true;
            this.lblNumThreads.Location = new System.Drawing.Point(7, 37);
            this.lblNumThreads.Name = "lblNumThreads";
            this.lblNumThreads.Size = new System.Drawing.Size(97, 13);
            this.lblNumThreads.TabIndex = 3;
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
            this.nNumThreads.TabIndex = 4;
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
            this.cboGlobalVariables.Location = new System.Drawing.Point(54, 210);
            this.cboGlobalVariables.Name = "cboGlobalVariables";
            this.cboGlobalVariables.Size = new System.Drawing.Size(106, 21);
            this.cboGlobalVariables.Sorted = true;
            this.cboGlobalVariables.TabIndex = 11;
            this.cboGlobalVariables.SelectedIndexChanged += new System.EventHandler(this.cboGlobalVariables_SelectedIndexChanged);
            // 
            // txtGlobalVariableValue
            // 
            this.txtGlobalVariableValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobalVariableValue.Location = new System.Drawing.Point(215, 210);
            this.txtGlobalVariableValue.Name = "txtGlobalVariableValue";
            this.txtGlobalVariableValue.Size = new System.Drawing.Size(173, 20);
            this.txtGlobalVariableValue.TabIndex = 13;
            this.txtGlobalVariableValue.TextChanged += new System.EventHandler(this.txtGlobalVariableValue_TextChanged);
            // 
            // lblVarName
            // 
            this.lblVarName.AutoSize = true;
            this.lblVarName.Location = new System.Drawing.Point(10, 213);
            this.lblVarName.Name = "lblVarName";
            this.lblVarName.Size = new System.Drawing.Size(38, 13);
            this.lblVarName.TabIndex = 10;
            this.lblVarName.Text = "&Name:";
            // 
            // lblvalue
            // 
            this.lblvalue.AutoSize = true;
            this.lblvalue.Location = new System.Drawing.Point(172, 213);
            this.lblvalue.Name = "lblvalue";
            this.lblvalue.Size = new System.Drawing.Size(37, 13);
            this.lblvalue.TabIndex = 12;
            this.lblvalue.Text = "&Value:";
            // 
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bAdd.Location = new System.Drawing.Point(232, 236);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 14;
            this.bAdd.Text = "&Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRemove.Enabled = false;
            this.bRemove.Location = new System.Drawing.Point(313, 236);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(75, 23);
            this.bRemove.TabIndex = 15;
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
            this.nNumRetries.TabIndex = 6;
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
            this.label1.TabIndex = 5;
            this.label1.Text = "Number of retrie&s:";
            // 
            // tcSettings
            // 
            this.tcSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSettings.Controls.Add(this.tpGeneral);
            this.tcSettings.Controls.Add(this.tpConnection);
            this.tcSettings.Controls.Add(this.tpCommands);
            this.tcSettings.Location = new System.Drawing.Point(12, 12);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(402, 294);
            this.tcSettings.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.lblCustomColumn2);
            this.tpGeneral.Controls.Add(this.txtCustomColumn2);
            this.tpGeneral.Controls.Add(this.chkBackups);
            this.tpGeneral.Controls.Add(this.chkMinToTray);
            this.tpGeneral.Controls.Add(this.chkUpdateOnlineDatabase);
            this.tpGeneral.Controls.Add(this.bRemove);
            this.tpGeneral.Controls.Add(this.chkUpdateAtStartup);
            this.tpGeneral.Controls.Add(this.bAdd);
            this.tpGeneral.Controls.Add(this.lblvalue);
            this.tpGeneral.Controls.Add(this.txtCustomColumn);
            this.tpGeneral.Controls.Add(this.lblVarName);
            this.tpGeneral.Controls.Add(this.lblCustomColumn);
            this.tpGeneral.Controls.Add(this.txtGlobalVariableValue);
            this.tpGeneral.Controls.Add(this.chkAvoidBeta);
            this.tpGeneral.Controls.Add(this.cboGlobalVariables);
            this.tpGeneral.Controls.Add(this.sepGlobalVariables);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(394, 268);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // chkBackups
            // 
            this.chkBackups.AutoSize = true;
            this.chkBackups.Location = new System.Drawing.Point(6, 103);
            this.chkBackups.Name = "chkBackups";
            this.chkBackups.Size = new System.Drawing.Size(212, 17);
            this.chkBackups.TabIndex = 4;
            this.chkBackups.Text = "Automatically create database &backups";
            this.chkBackups.UseVisualStyleBackColor = true;
            // 
            // chkMinToTray
            // 
            this.chkMinToTray.AutoSize = true;
            this.chkMinToTray.Location = new System.Drawing.Point(6, 80);
            this.chkMinToTray.Name = "chkMinToTray";
            this.chkMinToTray.Size = new System.Drawing.Size(98, 17);
            this.chkMinToTray.TabIndex = 3;
            this.chkMinToTray.Text = "&Minimize to tray";
            this.chkMinToTray.UseVisualStyleBackColor = true;
            // 
            // chkUpdateOnlineDatabase
            // 
            this.chkUpdateOnlineDatabase.AutoSize = true;
            this.chkUpdateOnlineDatabase.Location = new System.Drawing.Point(6, 57);
            this.chkUpdateOnlineDatabase.Name = "chkUpdateOnlineDatabase";
            this.chkUpdateOnlineDatabase.Size = new System.Drawing.Size(220, 17);
            this.chkUpdateOnlineDatabase.TabIndex = 2;
            this.chkUpdateOnlineDatabase.Text = "&Check for updates in the online database";
            this.chkUpdateOnlineDatabase.UseVisualStyleBackColor = true;
            // 
            // sepGlobalVariables
            // 
            this.sepGlobalVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepGlobalVariables.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepGlobalVariables.Location = new System.Drawing.Point(3, 182);
            this.sepGlobalVariables.Name = "sepGlobalVariables";
            this.sepGlobalVariables.Size = new System.Drawing.Size(385, 23);
            this.sepGlobalVariables.TabIndex = 9;
            this.sepGlobalVariables.Text = "Global variables";
            this.sepGlobalVariables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpConnection
            // 
            this.tpConnection.Controls.Add(this.nConnectionTimeout);
            this.tpConnection.Controls.Add(this.txtProxyPassword);
            this.tpConnection.Controls.Add(this.label1);
            this.tpConnection.Controls.Add(this.lblProxyPassword);
            this.tpConnection.Controls.Add(this.lblConnectionTimeout);
            this.tpConnection.Controls.Add(this.txtProxyUser);
            this.tpConnection.Controls.Add(this.nNumRetries);
            this.tpConnection.Controls.Add(this.lblProxyUser);
            this.tpConnection.Controls.Add(this.lblSeconds);
            this.tpConnection.Controls.Add(this.nProxyPort);
            this.tpConnection.Controls.Add(this.nNumThreads);
            this.tpConnection.Controls.Add(this.txtProxyServer);
            this.tpConnection.Controls.Add(this.lblNumThreads);
            this.tpConnection.Controls.Add(this.lblServer);
            this.tpConnection.Controls.Add(this.sepProxy);
            this.tpConnection.Location = new System.Drawing.Point(4, 22);
            this.tpConnection.Name = "tpConnection";
            this.tpConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tpConnection.Size = new System.Drawing.Size(394, 242);
            this.tpConnection.TabIndex = 1;
            this.tpConnection.Text = "Connection";
            this.tpConnection.UseVisualStyleBackColor = true;
            // 
            // sepProxy
            // 
            this.sepProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepProxy.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepProxy.Location = new System.Drawing.Point(3, 93);
            this.sepProxy.Name = "sepProxy";
            this.sepProxy.Size = new System.Drawing.Size(375, 23);
            this.sepProxy.TabIndex = 7;
            this.sepProxy.Text = "HTTP proxy settings";
            this.sepProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.txtPreUpdateCommand);
            this.tpCommands.Controls.Add(this.lblPreUpdateCommand);
            this.tpCommands.Controls.Add(this.lblPostUpdateAllCommand);
            this.tpCommands.Controls.Add(this.lblPostUpdateCommand);
            this.tpCommands.Controls.Add(this.txtPostUpdateAllCommand);
            this.tpCommands.Controls.Add(this.txtPostUpdateCommand);
            this.tpCommands.Location = new System.Drawing.Point(4, 22);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommands.Size = new System.Drawing.Size(394, 242);
            this.tpCommands.TabIndex = 2;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // txtPreUpdateCommand
            // 
            this.txtPreUpdateCommand.AcceptsReturn = true;
            this.txtPreUpdateCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreUpdateCommand.Location = new System.Drawing.Point(6, 25);
            this.txtPreUpdateCommand.Multiline = true;
            this.txtPreUpdateCommand.Name = "txtPreUpdateCommand";
            this.txtPreUpdateCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPreUpdateCommand.Size = new System.Drawing.Size(382, 50);
            this.txtPreUpdateCommand.TabIndex = 1;
            this.txtPreUpdateCommand.WordWrap = false;
            // 
            // lblPreUpdateCommand
            // 
            this.lblPreUpdateCommand.AutoSize = true;
            this.lblPreUpdateCommand.Location = new System.Drawing.Point(6, 9);
            this.lblPreUpdateCommand.Name = "lblPreUpdateCommand";
            this.lblPreUpdateCommand.Size = new System.Drawing.Size(292, 13);
            this.lblPreUpdateCommand.TabIndex = 0;
            this.lblPreUpdateCommand.Text = "Default command to execute &before updating an application:";
            // 
            // lblPostUpdateAllCommand
            // 
            this.lblPostUpdateAllCommand.AutoSize = true;
            this.lblPostUpdateAllCommand.Location = new System.Drawing.Point(6, 170);
            this.lblPostUpdateAllCommand.Name = "lblPostUpdateAllCommand";
            this.lblPostUpdateAllCommand.Size = new System.Drawing.Size(250, 13);
            this.lblPostUpdateAllCommand.TabIndex = 4;
            this.lblPostUpdateAllCommand.Text = "&Command to execute after updating all applications:";
            // 
            // lblPostUpdateCommand
            // 
            this.lblPostUpdateCommand.AutoSize = true;
            this.lblPostUpdateCommand.Location = new System.Drawing.Point(6, 90);
            this.lblPostUpdateCommand.Name = "lblPostUpdateCommand";
            this.lblPostUpdateCommand.Size = new System.Drawing.Size(283, 13);
            this.lblPostUpdateCommand.TabIndex = 2;
            this.lblPostUpdateCommand.Text = "&Default command to execute after updating an application:";
            // 
            // txtPostUpdateAllCommand
            // 
            this.txtPostUpdateAllCommand.AcceptsReturn = true;
            this.txtPostUpdateAllCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostUpdateAllCommand.Location = new System.Drawing.Point(6, 186);
            this.txtPostUpdateAllCommand.Multiline = true;
            this.txtPostUpdateAllCommand.Name = "txtPostUpdateAllCommand";
            this.txtPostUpdateAllCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostUpdateAllCommand.Size = new System.Drawing.Size(382, 50);
            this.txtPostUpdateAllCommand.TabIndex = 5;
            this.txtPostUpdateAllCommand.WordWrap = false;
            // 
            // txtPostUpdateCommand
            // 
            this.txtPostUpdateCommand.AcceptsReturn = true;
            this.txtPostUpdateCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostUpdateCommand.Location = new System.Drawing.Point(6, 106);
            this.txtPostUpdateCommand.Multiline = true;
            this.txtPostUpdateCommand.Name = "txtPostUpdateCommand";
            this.txtPostUpdateCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostUpdateCommand.Size = new System.Drawing.Size(382, 50);
            this.txtPostUpdateCommand.TabIndex = 3;
            this.txtPostUpdateCommand.WordWrap = false;
            // 
            // bExport
            // 
            this.bExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bExport.Location = new System.Drawing.Point(12, 321);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(75, 23);
            this.bExport.TabIndex = 1;
            this.bExport.Text = "Export...";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bImport
            // 
            this.bImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bImport.Location = new System.Drawing.Point(93, 321);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(75, 23);
            this.bImport.TabIndex = 2;
            this.bImport.Text = "Import...";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // txtCustomColumn2
            // 
            this.txtCustomColumn2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn2.Location = new System.Drawing.Point(158, 156);
            this.txtCustomColumn2.Name = "txtCustomColumn2";
            this.txtCustomColumn2.Size = new System.Drawing.Size(230, 20);
            this.txtCustomColumn2.TabIndex = 8;
            // 
            // lblCustomColumn2
            // 
            this.lblCustomColumn2.AutoSize = true;
            this.lblCustomColumn2.Location = new System.Drawing.Point(6, 159);
            this.lblCustomColumn2.Name = "lblCustomColumn2";
            this.lblCustomColumn2.Size = new System.Drawing.Size(146, 13);
            this.lblCustomColumn2.TabIndex = 7;
            this.lblCustomColumn2.Text = "&Variable for custom column 2:";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 356);
            this.Controls.Add(this.bImport);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.tcSettings);
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
            this.tcSettings.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.tpConnection.ResumeLayout(false);
            this.tpConnection.PerformLayout();
            this.tpCommands.ResumeLayout(false);
            this.tpCommands.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox chkUpdateAtStartup;
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
        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpConnection;
        private System.Windows.Forms.CheckBox chkUpdateOnlineDatabase;
        private System.Windows.Forms.CheckBox chkMinToTray;
        private System.Windows.Forms.TabPage tpCommands;
        private TextBox txtPostUpdateAllCommand;
        private System.Windows.Forms.Label lblPostUpdateAllCommand;
        private TextBox txtPostUpdateCommand;
        private System.Windows.Forms.Label lblPostUpdateCommand;
        private TextBox txtPreUpdateCommand;
        private System.Windows.Forms.Label lblPreUpdateCommand;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.CheckBox chkBackups;
        private System.Windows.Forms.Label lblCustomColumn2;
        private System.Windows.Forms.TextBox txtCustomColumn2;
    }
}