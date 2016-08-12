using System.ComponentModel;
using System.Windows.Forms;
using CDBurnerXP.Controls;

namespace Ketarin.Forms
{
    partial class SettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.bEdit = new System.Windows.Forms.Button();
            this.separator1 = new CDBurnerXP.Controls.Separator();
            this.bRemove = new System.Windows.Forms.Button();
            this.bAddCustomColumn = new System.Windows.Forms.Button();
            this.olvCustomColumns = new CDBurnerXP.Controls.ObjectListView();
            this.colName = ((CDBurnerXP.Controls.OLVColumn)(new CDBurnerXP.Controls.OLVColumn()));
            this.colValue = ((CDBurnerXP.Controls.OLVColumn)(new CDBurnerXP.Controls.OLVColumn()));
            this.chkOpenWebsite = new System.Windows.Forms.CheckBox();
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
            this.tpHotkeys = new System.Windows.Forms.TabPage();
            this.bDoubleClick = new System.Windows.Forms.Button();
            this.txtHotkeyKeys = new Ketarin.Forms.SettingsDialog.HotkeyTextBox();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.lblActions = new System.Windows.Forms.Label();
            this.lbActions = new System.Windows.Forms.ListBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.nNumSegments = new System.Windows.Forms.NumericUpDown();
            this.lblSegments = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nNumRetries)).BeginInit();
            this.tcSettings.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvCustomColumns)).BeginInit();
            this.tpConnection.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGlobalVariables)).BeginInit();
            this.tpCommands.SuspendLayout();
            this.tpHotkeys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nNumSegments)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(339, 402);
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
            this.bOK.Location = new System.Drawing.Point(258, 402);
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
            this.chkUpdateAtStartup.Location = new System.Drawing.Point(8, 11);
            this.chkUpdateAtStartup.Name = "chkUpdateAtStartup";
            this.chkUpdateAtStartup.Size = new System.Drawing.Size(172, 17);
            this.chkUpdateAtStartup.TabIndex = 0;
            this.chkUpdateAtStartup.Text = "Upd&ate automatically at startup";
            this.chkUpdateAtStartup.UseVisualStyleBackColor = true;
            // 
            // chkAvoidBeta
            // 
            this.chkAvoidBeta.AutoSize = true;
            this.chkAvoidBeta.Location = new System.Drawing.Point(8, 34);
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
            this.nConnectionTimeout.Location = new System.Drawing.Point(187, 9);
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
            this.lblSeconds.Location = new System.Drawing.Point(239, 11);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 2;
            this.lblSeconds.Text = "seconds";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(15, 148);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 10;
            this.lblServer.Text = "&Server:";
            // 
            // txtProxyServer
            // 
            this.txtProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyServer.Location = new System.Drawing.Point(82, 145);
            this.txtProxyServer.Name = "txtProxyServer";
            this.txtProxyServer.Size = new System.Drawing.Size(242, 20);
            this.txtProxyServer.TabIndex = 11;
            // 
            // nProxyPort
            // 
            this.nProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nProxyPort.Location = new System.Drawing.Point(330, 145);
            this.nProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nProxyPort.Name = "nProxyPort";
            this.nProxyPort.Size = new System.Drawing.Size(48, 20);
            this.nProxyPort.TabIndex = 12;
            // 
            // lblProxyUser
            // 
            this.lblProxyUser.AutoSize = true;
            this.lblProxyUser.Location = new System.Drawing.Point(15, 174);
            this.lblProxyUser.Name = "lblProxyUser";
            this.lblProxyUser.Size = new System.Drawing.Size(61, 13);
            this.lblProxyUser.TabIndex = 13;
            this.lblProxyUser.Text = "Us&er name:";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyUser.Location = new System.Drawing.Point(82, 171);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(296, 20);
            this.txtProxyUser.TabIndex = 14;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(15, 200);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 15;
            this.lblProxyPassword.Text = "&Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPassword.Location = new System.Drawing.Point(82, 197);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(296, 20);
            this.txtProxyPassword.TabIndex = 16;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // lblNumThreads
            // 
            this.lblNumThreads.AutoSize = true;
            this.lblNumThreads.Location = new System.Drawing.Point(7, 37);
            this.lblNumThreads.Name = "lblNumThreads";
            this.lblNumThreads.Size = new System.Drawing.Size(149, 13);
            this.lblNumThreads.TabIndex = 3;
            this.lblNumThreads.Text = "&Number of parallel downloads:";
            // 
            // nNumThreads
            // 
            this.nNumThreads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nNumThreads.Location = new System.Drawing.Point(187, 35);
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
            this.nNumRetries.Location = new System.Drawing.Point(187, 61);
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
            this.lblNumRetries.Size = new System.Drawing.Size(157, 13);
            this.lblNumRetries.TabIndex = 5;
            this.lblNumRetries.Text = "Number of retrie&s per download:";
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
            this.tcSettings.Controls.Add(this.tpHotkeys);
            this.tcSettings.Location = new System.Drawing.Point(12, 12);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(402, 375);
            this.tcSettings.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.bEdit);
            this.tpGeneral.Controls.Add(this.separator1);
            this.tpGeneral.Controls.Add(this.bRemove);
            this.tpGeneral.Controls.Add(this.bAddCustomColumn);
            this.tpGeneral.Controls.Add(this.olvCustomColumns);
            this.tpGeneral.Controls.Add(this.chkOpenWebsite);
            this.tpGeneral.Controls.Add(this.chkBackups);
            this.tpGeneral.Controls.Add(this.chkMinToTray);
            this.tpGeneral.Controls.Add(this.chkUpdateOnlineDatabase);
            this.tpGeneral.Controls.Add(this.chkUpdateAtStartup);
            this.tpGeneral.Controls.Add(this.chkAvoidBeta);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(394, 349);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // bEdit
            // 
            this.bEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bEdit.Enabled = false;
            this.bEdit.Location = new System.Drawing.Point(89, 317);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(75, 23);
            this.bEdit.TabIndex = 9;
            this.bEdit.Text = "&Edit";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // separator1
            // 
            this.separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separator1.Location = new System.Drawing.Point(6, 153);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(382, 23);
            this.separator1.TabIndex = 6;
            this.separator1.Text = "Custom columns";
            // 
            // bRemove
            // 
            this.bRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRemove.Enabled = false;
            this.bRemove.Location = new System.Drawing.Point(170, 317);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(75, 23);
            this.bRemove.TabIndex = 10;
            this.bRemove.Text = "&Remove";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bAddCustomColumn
            // 
            this.bAddCustomColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddCustomColumn.Location = new System.Drawing.Point(8, 317);
            this.bAddCustomColumn.Name = "bAddCustomColumn";
            this.bAddCustomColumn.Size = new System.Drawing.Size(75, 23);
            this.bAddCustomColumn.TabIndex = 8;
            this.bAddCustomColumn.Text = "A&dd...";
            this.bAddCustomColumn.UseVisualStyleBackColor = true;
            this.bAddCustomColumn.Click += new System.EventHandler(this.bAddCustomColumn_Click);
            // 
            // olvCustomColumns
            // 
            this.olvCustomColumns.AllColumns.Add(this.colName);
            this.olvCustomColumns.AllColumns.Add(this.colValue);
            this.olvCustomColumns.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvCustomColumns.AlwaysGroupByColumn = null;
            this.olvCustomColumns.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvCustomColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvCustomColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.olvCustomColumns.FullRowSelect = true;
            this.olvCustomColumns.HideSelection = false;
            this.olvCustomColumns.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvCustomColumns.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvCustomColumns.LastSortColumn = null;
            this.olvCustomColumns.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvCustomColumns.Location = new System.Drawing.Point(9, 179);
            this.olvCustomColumns.Name = "olvCustomColumns";
            this.olvCustomColumns.ShowGroups = false;
            this.olvCustomColumns.Size = new System.Drawing.Size(379, 132);
            this.olvCustomColumns.TabIndex = 7;
            this.olvCustomColumns.UseCompatibleStateImageBehavior = false;
            this.olvCustomColumns.View = System.Windows.Forms.View.Details;
            this.olvCustomColumns.SelectedIndexChanged += new System.EventHandler(this.olvCustomColumns_SelectedIndexChanged);
            this.olvCustomColumns.DoubleClick += new System.EventHandler(this.olvCustomColumns_DoubleClick);
            // 
            // colName
            // 
            this.colName.AspectName = "Key";
            this.colName.Text = "Name";
            this.colName.Width = 125;
            // 
            // colValue
            // 
            this.colValue.AspectName = "Value";
            this.colValue.Text = "Value";
            this.colValue.Width = 216;
            // 
            // chkOpenWebsite
            // 
            this.chkOpenWebsite.AutoSize = true;
            this.chkOpenWebsite.Location = new System.Drawing.Point(8, 126);
            this.chkOpenWebsite.Name = "chkOpenWebsite";
            this.chkOpenWebsite.Size = new System.Drawing.Size(337, 17);
            this.chkOpenWebsite.TabIndex = 5;
            this.chkOpenWebsite.Text = "Ope&n website when double-clicking on an application (if specified)";
            this.chkOpenWebsite.UseVisualStyleBackColor = true;
            // 
            // chkBackups
            // 
            this.chkBackups.AutoSize = true;
            this.chkBackups.Location = new System.Drawing.Point(8, 103);
            this.chkBackups.Name = "chkBackups";
            this.chkBackups.Size = new System.Drawing.Size(212, 17);
            this.chkBackups.TabIndex = 4;
            this.chkBackups.Text = "A&utomatically create database backups";
            this.chkBackups.UseVisualStyleBackColor = true;
            // 
            // chkMinToTray
            // 
            this.chkMinToTray.AutoSize = true;
            this.chkMinToTray.Location = new System.Drawing.Point(8, 80);
            this.chkMinToTray.Name = "chkMinToTray";
            this.chkMinToTray.Size = new System.Drawing.Size(98, 17);
            this.chkMinToTray.TabIndex = 3;
            this.chkMinToTray.Text = "&Minimize to tray";
            this.chkMinToTray.UseVisualStyleBackColor = true;
            // 
            // chkUpdateOnlineDatabase
            // 
            this.chkUpdateOnlineDatabase.AutoSize = true;
            this.chkUpdateOnlineDatabase.Location = new System.Drawing.Point(8, 57);
            this.chkUpdateOnlineDatabase.Name = "chkUpdateOnlineDatabase";
            this.chkUpdateOnlineDatabase.Size = new System.Drawing.Size(220, 17);
            this.chkUpdateOnlineDatabase.TabIndex = 2;
            this.chkUpdateOnlineDatabase.Text = "&Check for updates in the online database";
            this.chkUpdateOnlineDatabase.UseVisualStyleBackColor = true;
            // 
            // tpConnection
            // 
            this.tpConnection.Controls.Add(this.nNumSegments);
            this.tpConnection.Controls.Add(this.lblSegments);
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
            this.tpConnection.Size = new System.Drawing.Size(394, 349);
            this.tpConnection.TabIndex = 1;
            this.tpConnection.Text = "Connection";
            this.tpConnection.UseVisualStyleBackColor = true;
            // 
            // sepProxy
            // 
            this.sepProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sepProxy.Location = new System.Drawing.Point(3, 117);
            this.sepProxy.Name = "sepProxy";
            this.sepProxy.Size = new System.Drawing.Size(375, 23);
            this.sepProxy.TabIndex = 9;
            this.sepProxy.Text = "HTTP proxy settings";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridGlobalVariables);
            this.tabPage1.Controls.Add(this.lblGlobalVariables);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 349);
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
            this.gridGlobalVariables.Size = new System.Drawing.Size(379, 316);
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
            this.tpCommands.Size = new System.Drawing.Size(394, 349);
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
            this.commandControl.ShowBorder = false;
            this.commandControl.Size = new System.Drawing.Size(382, 310);
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
            // tpHotkeys
            // 
            this.tpHotkeys.Controls.Add(this.bDoubleClick);
            this.tpHotkeys.Controls.Add(this.txtHotkeyKeys);
            this.tpHotkeys.Controls.Add(this.lblHotkey);
            this.tpHotkeys.Controls.Add(this.lblActions);
            this.tpHotkeys.Controls.Add(this.lbActions);
            this.tpHotkeys.Location = new System.Drawing.Point(4, 22);
            this.tpHotkeys.Name = "tpHotkeys";
            this.tpHotkeys.Padding = new System.Windows.Forms.Padding(3);
            this.tpHotkeys.Size = new System.Drawing.Size(394, 349);
            this.tpHotkeys.TabIndex = 4;
            this.tpHotkeys.Text = "Hotkeys";
            this.tpHotkeys.UseVisualStyleBackColor = true;
            // 
            // bDoubleClick
            // 
            this.bDoubleClick.Location = new System.Drawing.Point(237, 160);
            this.bDoubleClick.Name = "bDoubleClick";
            this.bDoubleClick.Size = new System.Drawing.Size(75, 23);
            this.bDoubleClick.TabIndex = 4;
            this.bDoubleClick.Text = "Doubleclick";
            this.bDoubleClick.UseVisualStyleBackColor = true;
            this.bDoubleClick.Click += new System.EventHandler(this.bDoubleClick_Click);
            // 
            // txtHotkeyKeys
            // 
            this.txtHotkeyKeys.Location = new System.Drawing.Point(73, 162);
            this.txtHotkeyKeys.Name = "txtHotkeyKeys";
            this.txtHotkeyKeys.Size = new System.Drawing.Size(158, 20);
            this.txtHotkeyKeys.TabIndex = 3;
            // 
            // lblHotkey
            // 
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new System.Drawing.Point(6, 165);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(61, 13);
            this.lblHotkey.TabIndex = 2;
            this.lblHotkey.Text = "&Press keys:";
            // 
            // lblActions
            // 
            this.lblActions.AutoSize = true;
            this.lblActions.Location = new System.Drawing.Point(4, 11);
            this.lblActions.Name = "lblActions";
            this.lblActions.Size = new System.Drawing.Size(45, 13);
            this.lblActions.TabIndex = 0;
            this.lblActions.Text = "&Actions:";
            // 
            // lbActions
            // 
            this.lbActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbActions.DisplayMember = "DisplayName";
            this.lbActions.FormattingEnabled = true;
            this.lbActions.Location = new System.Drawing.Point(7, 27);
            this.lbActions.Name = "lbActions";
            this.lbActions.Size = new System.Drawing.Size(381, 121);
            this.lbActions.Sorted = true;
            this.lbActions.TabIndex = 1;
            this.lbActions.SelectedIndexChanged += new System.EventHandler(this.lbActions_SelectedIndexChanged);
            // 
            // bExport
            // 
            this.bExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bExport.Location = new System.Drawing.Point(12, 402);
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
            this.bImport.Location = new System.Drawing.Point(93, 402);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(75, 23);
            this.bImport.TabIndex = 2;
            this.bImport.Text = "Import...";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // nNumSegments
            // 
            this.nNumSegments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nNumSegments.Location = new System.Drawing.Point(187, 87);
            this.nNumSegments.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nNumSegments.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nNumSegments.Name = "nNumSegments";
            this.nNumSegments.Size = new System.Drawing.Size(46, 20);
            this.nNumSegments.TabIndex = 8;
            this.nNumSegments.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSegments
            // 
            this.lblSegments.AutoSize = true;
            this.lblSegments.Location = new System.Drawing.Point(7, 89);
            this.lblSegments.Name = "lblSegments";
            this.lblSegments.Size = new System.Drawing.Size(174, 13);
            this.lblSegments.TabIndex = 7;
            this.lblSegments.Text = "Number &of segments per download:";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 437);
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
            ((System.ComponentModel.ISupportInitialize)(this.olvCustomColumns)).EndInit();
            this.tpConnection.ResumeLayout(false);
            this.tpConnection.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGlobalVariables)).EndInit();
            this.tpCommands.ResumeLayout(false);
            this.tpCommands.PerformLayout();
            this.tpHotkeys.ResumeLayout(false);
            this.tpHotkeys.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nNumSegments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button bCancel;
        private Button bOK;
        private CheckBox chkUpdateAtStartup;
        private CheckBox chkAvoidBeta;
        private Label lblConnectionTimeout;
        private NumericUpDown nConnectionTimeout;
        private Label lblSeconds;
        private Separator sepProxy;
        private Label lblServer;
        private System.Windows.Forms.TextBox txtProxyServer;
        private NumericUpDown nProxyPort;
        private Label lblProxyUser;
        private System.Windows.Forms.TextBox txtProxyUser;
        private Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private Label lblNumThreads;
        private NumericUpDown nNumThreads;
        private NumericUpDown nNumRetries;
        private Label lblNumRetries;
        private TabControl tcSettings;
        private TabPage tpGeneral;
        private TabPage tpConnection;
        private CheckBox chkUpdateOnlineDatabase;
        private CheckBox chkMinToTray;
        private TabPage tpCommands;
        private Button bExport;
        private Button bImport;
        private CheckBox chkBackups;
        private CheckBox chkOpenWebsite;
        private Label lblCommandEvent;
        private ComboBox cboCommandEvent;
        private CommandControl commandControl;
        private TabPage tabPage1;
        private Label lblGlobalVariables;
        private DataGridView gridGlobalVariables;
        private ObjectListView olvCustomColumns;
        private OLVColumn colName;
        private OLVColumn colValue;
        private Button bAddCustomColumn;
        private Button bRemove;
        private Separator separator1;
        private Button bEdit;
        private TabPage tpHotkeys;
        private Label lblActions;
        private ListBox lbActions;
        private Button bDoubleClick;
        private HotkeyTextBox txtHotkeyKeys;
        private Label lblHotkey;
        private NumericUpDown nNumSegments;
        private Label lblSegments;
    }
}