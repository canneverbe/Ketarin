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
            this.nNumRetries = new System.Windows.Forms.NumericUpDown();
            this.lblNumRetries = new System.Windows.Forms.Label();
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.chkOpenWebsite = new System.Windows.Forms.CheckBox();
            this.lblCustomColumn2 = new System.Windows.Forms.Label();
            this.txtCustomColumn2 = new System.Windows.Forms.TextBox();
            this.chkBackups = new System.Windows.Forms.CheckBox();
            this.chkMinToTray = new System.Windows.Forms.CheckBox();
            this.chkUpdateOnlineDatabase = new System.Windows.Forms.CheckBox();
            this.tpConnection = new System.Windows.Forms.TabPage();
            this.sepProxy = new CDBurnerXP.Controls.Separator();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridGlobalVariables = new System.Windows.Forms.DataGridView();
            this.lblGlobalVariables = new System.Windows.Forms.Label();
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.commandControl = new Ketarin.Forms.CommandControl();
            this.lblCommandEvent = new System.Windows.Forms.Label();
            this.cboCommandEvent = new System.Windows.Forms.ComboBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumRetries)).BeginInit();
            this.tcSettings.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpConnection.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGlobalVariables)).BeginInit();
            this.tpCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(339, 337);
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
            this.bOK.Location = new System.Drawing.Point(258, 337);
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
            this.chkUpdateAtStartup.Text = "Upd&ate automatically at startup";
            this.chkUpdateAtStartup.UseVisualStyleBackColor = true;
            // 
            // txtCustomColumn
            // 
            this.txtCustomColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn.Location = new System.Drawing.Point(158, 155);
            this.txtCustomColumn.Name = "txtCustomColumn";
            this.txtCustomColumn.Size = new System.Drawing.Size(230, 20);
            this.txtCustomColumn.TabIndex = 7;
            // 
            // lblCustomColumn
            // 
            this.lblCustomColumn.AutoSize = true;
            this.lblCustomColumn.Location = new System.Drawing.Point(6, 158);
            this.lblCustomColumn.Name = "lblCustomColumn";
            this.lblCustomColumn.Size = new System.Drawing.Size(137, 13);
            this.lblCustomColumn.TabIndex = 6;
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
            this.nConnectionTimeout.Size = new System.Drawing.Size(46, 20);
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
            this.lblSeconds.Location = new System.Drawing.Point(166, 11);
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
            this.nNumThreads.Size = new System.Drawing.Size(46, 20);
            this.nNumThreads.TabIndex = 4;
            this.nNumThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
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
            this.nNumRetries.Size = new System.Drawing.Size(46, 20);
            this.nNumRetries.TabIndex = 6;
            this.nNumRetries.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblNumRetries
            // 
            this.lblNumRetries.AutoSize = true;
            this.lblNumRetries.Location = new System.Drawing.Point(7, 63);
            this.lblNumRetries.Name = "lblNumRetries";
            this.lblNumRetries.Size = new System.Drawing.Size(90, 13);
            this.lblNumRetries.TabIndex = 5;
            this.lblNumRetries.Text = "Number of retrie&s:";
            // 
            // tcSettings
            // 
            this.tcSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSettings.Controls.Add(this.tpGeneral);
            this.tcSettings.Controls.Add(this.tpConnection);
            this.tcSettings.Controls.Add(this.tabPage1);
            this.tcSettings.Controls.Add(this.tpCommands);
            this.tcSettings.Location = new System.Drawing.Point(12, 12);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(402, 310);
            this.tcSettings.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.chkOpenWebsite);
            this.tpGeneral.Controls.Add(this.lblCustomColumn2);
            this.tpGeneral.Controls.Add(this.txtCustomColumn2);
            this.tpGeneral.Controls.Add(this.chkBackups);
            this.tpGeneral.Controls.Add(this.chkMinToTray);
            this.tpGeneral.Controls.Add(this.chkUpdateOnlineDatabase);
            this.tpGeneral.Controls.Add(this.chkUpdateAtStartup);
            this.tpGeneral.Controls.Add(this.txtCustomColumn);
            this.tpGeneral.Controls.Add(this.lblCustomColumn);
            this.tpGeneral.Controls.Add(this.chkAvoidBeta);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(394, 284);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // chkOpenWebsite
            // 
            this.chkOpenWebsite.AutoSize = true;
            this.chkOpenWebsite.Location = new System.Drawing.Point(6, 126);
            this.chkOpenWebsite.Name = "chkOpenWebsite";
            this.chkOpenWebsite.Size = new System.Drawing.Size(337, 17);
            this.chkOpenWebsite.TabIndex = 5;
            this.chkOpenWebsite.Text = "Ope&n website when double-clicking on an application (if specified)";
            this.chkOpenWebsite.UseVisualStyleBackColor = true;
            // 
            // lblCustomColumn2
            // 
            this.lblCustomColumn2.AutoSize = true;
            this.lblCustomColumn2.Location = new System.Drawing.Point(6, 184);
            this.lblCustomColumn2.Name = "lblCustomColumn2";
            this.lblCustomColumn2.Size = new System.Drawing.Size(146, 13);
            this.lblCustomColumn2.TabIndex = 8;
            this.lblCustomColumn2.Text = "&Variable for custom column 2:";
            // 
            // txtCustomColumn2
            // 
            this.txtCustomColumn2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn2.Location = new System.Drawing.Point(158, 181);
            this.txtCustomColumn2.Name = "txtCustomColumn2";
            this.txtCustomColumn2.Size = new System.Drawing.Size(230, 20);
            this.txtCustomColumn2.TabIndex = 9;
            // 
            // chkBackups
            // 
            this.chkBackups.AutoSize = true;
            this.chkBackups.Location = new System.Drawing.Point(6, 103);
            this.chkBackups.Name = "chkBackups";
            this.chkBackups.Size = new System.Drawing.Size(212, 17);
            this.chkBackups.TabIndex = 4;
            this.chkBackups.Text = "A&utomatically create database backups";
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
            // tpConnection
            // 
            this.tpConnection.Controls.Add(this.nConnectionTimeout);
            this.tpConnection.Controls.Add(this.txtProxyPassword);
            this.tpConnection.Controls.Add(this.lblNumRetries);
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
            this.tpConnection.Size = new System.Drawing.Size(394, 284);
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridGlobalVariables);
            this.tabPage1.Controls.Add(this.lblGlobalVariables);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 284);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Global variables";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridGlobalVariables
            // 
            this.gridGlobalVariables.AllowUserToResizeRows = false;
            this.gridGlobalVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridGlobalVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridGlobalVariables.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridGlobalVariables.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridGlobalVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGlobalVariables.Location = new System.Drawing.Point(7, 27);
            this.gridGlobalVariables.Name = "gridGlobalVariables";
            this.gridGlobalVariables.Size = new System.Drawing.Size(379, 251);
            this.gridGlobalVariables.TabIndex = 2;
            // 
            // lblGlobalVariables
            // 
            this.lblGlobalVariables.AutoSize = true;
            this.lblGlobalVariables.Location = new System.Drawing.Point(4, 11);
            this.lblGlobalVariables.Name = "lblGlobalVariables";
            this.lblGlobalVariables.Size = new System.Drawing.Size(85, 13);
            this.lblGlobalVariables.TabIndex = 0;
            this.lblGlobalVariables.Text = "&Global variables:";
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.commandControl);
            this.tpCommands.Controls.Add(this.lblCommandEvent);
            this.tpCommands.Controls.Add(this.cboCommandEvent);
            this.tpCommands.Location = new System.Drawing.Point(4, 22);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommands.Size = new System.Drawing.Size(394, 284);
            this.tpCommands.TabIndex = 2;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // commandControl
            // 
            this.commandControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commandControl.Application = null;
            this.commandControl.Location = new System.Drawing.Point(6, 36);
            this.commandControl.Margin = new System.Windows.Forms.Padding(0);
            this.commandControl.Name = "commandControl";
            this.commandControl.ReadOnly = false;
            this.commandControl.Size = new System.Drawing.Size(382, 245);
            this.commandControl.TabIndex = 2;
            this.commandControl.VariableNames = new string[0];
            // 
            // lblCommandEvent
            // 
            this.lblCommandEvent.AutoSize = true;
            this.lblCommandEvent.Location = new System.Drawing.Point(3, 12);
            this.lblCommandEvent.Name = "lblCommandEvent";
            this.lblCommandEvent.Size = new System.Drawing.Size(122, 13);
            this.lblCommandEvent.TabIndex = 0;
            this.lblCommandEvent.Text = "Edit &command for event:";
            // 
            // cboCommandEvent
            // 
            this.cboCommandEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCommandEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommandEvent.FormattingEnabled = true;
            this.cboCommandEvent.Items.AddRange(new object[] {
            "Before updating an application",
            "After updating an application",
            "After updating all applications"});
            this.cboCommandEvent.Location = new System.Drawing.Point(131, 9);
            this.cboCommandEvent.Name = "cboCommandEvent";
            this.cboCommandEvent.Size = new System.Drawing.Size(257, 21);
            this.cboCommandEvent.TabIndex = 1;
            this.cboCommandEvent.SelectedIndexChanged += new System.EventHandler(this.cboCommandEvent_SelectedIndexChanged);
            // 
            // bExport
            // 
            this.bExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bExport.Location = new System.Drawing.Point(12, 337);
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
            this.bImport.Location = new System.Drawing.Point(93, 337);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(75, 23);
            this.bImport.TabIndex = 2;
            this.bImport.Text = "Import...";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 372);
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGlobalVariables)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nNumRetries;
        private System.Windows.Forms.Label lblNumRetries;
        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpConnection;
        private System.Windows.Forms.CheckBox chkUpdateOnlineDatabase;
        private System.Windows.Forms.CheckBox chkMinToTray;
        private System.Windows.Forms.TabPage tpCommands;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.CheckBox chkBackups;
        private System.Windows.Forms.Label lblCustomColumn2;
        private System.Windows.Forms.TextBox txtCustomColumn2;
        private System.Windows.Forms.CheckBox chkOpenWebsite;
        private System.Windows.Forms.Label lblCommandEvent;
        private System.Windows.Forms.ComboBox cboCommandEvent;
        private CommandControl commandControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblGlobalVariables;
        private System.Windows.Forms.DataGridView gridGlobalVariables;
    }
}