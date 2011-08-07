namespace Ketarin
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imlStatus = new System.Windows.Forms.ImageList(this.components);
            this.cmnuJobs = new System.Windows.Forms.ContextMenu();
            this.cmnuUpdate = new System.Windows.Forms.MenuItem();
            this.cmnuCheckForUpdate = new System.Windows.Forms.MenuItem();
            this.cmnuForceDownload = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.cmnuInstall = new System.Windows.Forms.MenuItem();
            this.cmnuUpdateInstall = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.cmnuCommands = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.cmnuOpenFile = new System.Windows.Forms.MenuItem();
            this.cmnuOpenFolder = new System.Windows.Forms.MenuItem();
            this.cmnuProperties = new System.Windows.Forms.MenuItem();
            this.cmnuRename = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.cmnuEdit = new System.Windows.Forms.MenuItem();
            this.cmnuDelete = new System.Windows.Forms.MenuItem();
            this.cmnuCopy = new System.Windows.Forms.MenuItem();
            this.cmnuPaste = new System.Windows.Forms.MenuItem();
            this.mnuSelectAll = new System.Windows.Forms.MenuItem();
            this.mnuInvert = new System.Windows.Forms.MenuItem();
            this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuNew = new System.Windows.Forms.MenuItem();
            this.mnuImport = new System.Windows.Forms.MenuItem();
            this.mnuExportSelected = new System.Windows.Forms.MenuItem();
            this.mnuExportAll = new System.Windows.Forms.MenuItem();
            this.mnusep2 = new System.Windows.Forms.MenuItem();
            this.mnuSettings = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuLog = new System.Windows.Forms.MenuItem();
            this.mnuShowGroups = new System.Windows.Forms.MenuItem();
            this.mnuShowStatusBar = new System.Windows.Forms.MenuItem();
            this.mnuAutoScroll = new System.Windows.Forms.MenuItem();
            this.mnuFind = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuTutorial = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.cmnuUpdateAndInstall = new System.Windows.Forms.MenuItem();
            this.cmuAdd = new System.Windows.Forms.ContextMenu();
            this.cmnuAdd = new System.Windows.Forms.MenuItem();
            this.cmnuImportFile = new System.Windows.Forms.MenuItem();
            this.cmnuImportOnline = new System.Windows.Forms.MenuItem();
            this.olvJobs = new Ketarin.ApplicationJobsListView();
            this.colName = new CDBurnerXP.Controls.OLVColumn();
            this.colLastUpdate = new CDBurnerXP.Controls.OLVColumn();
            this.colProgress = new CDBurnerXP.Controls.OLVColumn();
            this.colTarget = new CDBurnerXP.Controls.OLVColumn();
            this.colCategory = new CDBurnerXP.Controls.OLVColumn();
            this.colStatus = new CDBurnerXP.Controls.OLVColumn();
            this.m_VistaMenu = new CDBurnerXP.Controls.VistaMenu(this.components);
            this.cmuRun = new System.Windows.Forms.ContextMenu();
            this.cmnuCheckAndDownload = new System.Windows.Forms.MenuItem();
            this.cmnuOnlyCheck = new System.Windows.Forms.MenuItem();
            this.ntiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmnuTrayIconMenu = new System.Windows.Forms.ContextMenu();
            this.cmnuShow = new System.Windows.Forms.MenuItem();
            this.cmnuExit = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tbSelectedApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbNumByStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbTotalApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.bInstall = new wyDay.Controls.SplitButton();
            this.bRun = new wyDay.Controls.SplitButton();
            this.bAddApplication = new wyDay.Controls.SplitButton();
            this.cmnuRunPostDownload = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // imlStatus
            // 
            this.imlStatus.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlStatus.ImageSize = new System.Drawing.Size(16, 16);
            this.imlStatus.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmnuJobs
            // 
            this.cmnuJobs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuUpdate,
            this.cmnuCheckForUpdate,
            this.cmnuForceDownload,
            this.menuItem2,
            this.cmnuInstall,
            this.cmnuUpdateInstall,
            this.menuItem1,
            this.cmnuCommands,
            this.menuItem5,
            this.cmnuOpenFile,
            this.cmnuOpenFolder,
            this.cmnuProperties,
            this.cmnuRename,
            this.menuItem4,
            this.cmnuEdit,
            this.cmnuDelete,
            this.cmnuCopy,
            this.cmnuPaste,
            this.mnuSelectAll,
            this.mnuInvert});
            this.cmnuJobs.Popup += new System.EventHandler(this.cmnuJobs_Popup);
            // 
            // cmnuUpdate
            // 
            this.m_VistaMenu.SetImage(this.cmnuUpdate, global::Ketarin.Properties.Resources.Restart);
            this.cmnuUpdate.Index = 0;
            this.cmnuUpdate.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            this.cmnuUpdate.Text = "&Update";
            this.cmnuUpdate.Click += new System.EventHandler(this.cmuUpdate_Click);
            // 
            // cmnuCheckForUpdate
            // 
            this.cmnuCheckForUpdate.Index = 1;
            this.cmnuCheckForUpdate.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftU;
            this.cmnuCheckForUpdate.Text = "C&heck for update";
            this.cmnuCheckForUpdate.Click += new System.EventHandler(this.cmnuCheckForUpdate_Click);
            // 
            // cmnuForceDownload
            // 
            this.cmnuForceDownload.Index = 2;
            this.cmnuForceDownload.Shortcut = System.Windows.Forms.Shortcut.CtrlF5;
            this.cmnuForceDownload.Text = "&Force download";
            this.cmnuForceDownload.Click += new System.EventHandler(this.cmnuForceDownload_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // cmnuInstall
            // 
            this.cmnuInstall.Index = 4;
            this.cmnuInstall.Text = "&Install";
            this.cmnuInstall.Click += new System.EventHandler(this.cmnuInstall_Click);
            // 
            // cmnuUpdateInstall
            // 
            this.cmnuUpdateInstall.Index = 5;
            this.cmnuUpdateInstall.Text = "Upda&te and install";
            this.cmnuUpdateInstall.Click += new System.EventHandler(this.cmnuUpdateInstall_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 6;
            this.menuItem1.Text = "-";
            // 
            // cmnuCommands
            // 
            this.cmnuCommands.Index = 7;
            this.cmnuCommands.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuRunPostDownload});
            this.cmnuCommands.Text = "Com&mands";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 8;
            this.menuItem5.Text = "-";
            // 
            // cmnuOpenFile
            // 
            this.cmnuOpenFile.Enabled = false;
            this.cmnuOpenFile.Index = 9;
            this.cmnuOpenFile.Text = "&Open file";
            this.cmnuOpenFile.Click += new System.EventHandler(this.cmnuOpenFile_Click);
            // 
            // cmnuOpenFolder
            // 
            this.cmnuOpenFolder.Index = 10;
            this.cmnuOpenFolder.Text = "Ope&n folder";
            this.cmnuOpenFolder.Click += new System.EventHandler(this.cmnuOpenFolder_Click);
            // 
            // cmnuProperties
            // 
            this.cmnuProperties.Enabled = false;
            this.cmnuProperties.Index = 11;
            this.cmnuProperties.Shortcut = System.Windows.Forms.Shortcut.F9;
            this.cmnuProperties.Text = "File propertie&s";
            this.cmnuProperties.Click += new System.EventHandler(this.cmnuProperties_Click);
            // 
            // cmnuRename
            // 
            this.cmnuRename.Enabled = false;
            this.cmnuRename.Index = 12;
            this.cmnuRename.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.cmnuRename.Text = "&Rename file";
            this.cmnuRename.Click += new System.EventHandler(this.cmnuRename_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 13;
            this.menuItem4.Text = "-";
            // 
            // cmnuEdit
            // 
            this.cmnuEdit.DefaultItem = true;
            this.cmnuEdit.Enabled = false;
            this.cmnuEdit.Index = 14;
            this.cmnuEdit.Text = "&Edit";
            this.cmnuEdit.Click += new System.EventHandler(this.cmnuEdit_Click);
            // 
            // cmnuDelete
            // 
            this.cmnuDelete.Enabled = false;
            this.cmnuDelete.Index = 15;
            this.cmnuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.cmnuDelete.Text = "&Delete";
            this.cmnuDelete.Click += new System.EventHandler(this.cmnuDelete_Click);
            // 
            // cmnuCopy
            // 
            this.cmnuCopy.Index = 16;
            this.cmnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.cmnuCopy.Text = "&Copy";
            this.cmnuCopy.Click += new System.EventHandler(this.cmnuCopy_Click);
            // 
            // cmnuPaste
            // 
            this.cmnuPaste.Index = 17;
            this.cmnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.cmnuPaste.Text = "&Paste";
            this.cmnuPaste.Click += new System.EventHandler(this.cmnuPaste_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Index = 18;
            this.mnuSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuSelectAll.Text = "Select &all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuInvert
            // 
            this.mnuInvert.Index = 19;
            this.mnuInvert.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.mnuInvert.Text = "In&vert selection";
            this.mnuInvert.Click += new System.EventHandler(this.mnuInvert_Click);
            // 
            // mnuMain
            // 
            this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNew,
            this.mnuImport,
            this.mnuExportSelected,
            this.mnuExportAll,
            this.mnusep2,
            this.mnuSettings,
            this.menuItem7,
            this.mnuExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.m_VistaMenu.SetImage(this.mnuNew, global::Ketarin.Properties.Resources.AddSmall);
            this.mnuNew.Index = 0;
            this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNew.Text = "&New application...";
            this.mnuNew.Click += new System.EventHandler(this.mnuAddNew_Click);
            // 
            // mnuImport
            // 
            this.mnuImport.Index = 1;
            this.mnuImport.Text = "&Import...";
            this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
            // 
            // mnuExportSelected
            // 
            this.mnuExportSelected.Index = 2;
            this.mnuExportSelected.Text = "E&xport selected...";
            this.mnuExportSelected.Click += new System.EventHandler(this.mnuExportSelected_Click);
            // 
            // mnuExportAll
            // 
            this.mnuExportAll.Index = 3;
            this.mnuExportAll.Text = "Export &all...";
            this.mnuExportAll.Click += new System.EventHandler(this.mnuExportAll_Click);
            // 
            // mnusep2
            // 
            this.mnusep2.Index = 4;
            this.mnusep2.Text = "-";
            // 
            // mnuSettings
            // 
            this.mnuSettings.Index = 5;
            this.mnuSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            this.mnuSettings.Text = "&Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 6;
            this.menuItem7.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 7;
            this.mnuExit.Text = "&Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 1;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuLog,
            this.mnuShowGroups,
            this.mnuShowStatusBar,
            this.mnuAutoScroll,
            this.mnuFind});
            this.mnuView.Text = "&View";
            // 
            // mnuLog
            // 
            this.mnuLog.Index = 0;
            this.mnuLog.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.mnuLog.Text = "&Show log";
            this.mnuLog.Click += new System.EventHandler(this.mnuLog_Click);
            // 
            // mnuShowGroups
            // 
            this.mnuShowGroups.Checked = true;
            this.mnuShowGroups.Index = 1;
            this.mnuShowGroups.Text = "Show gr&oups";
            this.mnuShowGroups.Click += new System.EventHandler(this.mnuShowGroups_Click);
            // 
            // mnuShowStatusBar
            // 
            this.mnuShowStatusBar.Index = 2;
            this.mnuShowStatusBar.Text = "Show status &bar";
            this.mnuShowStatusBar.Click += new System.EventHandler(this.mnuShowStatusBar_Click);
            // 
            // mnuAutoScroll
            // 
            this.mnuAutoScroll.Checked = true;
            this.mnuAutoScroll.Index = 3;
            this.mnuAutoScroll.Text = "&Auto scroll";
            this.mnuAutoScroll.Click += new System.EventHandler(this.mnuAutoScroll_Click);
            // 
            // mnuFind
            // 
            this.mnuFind.Index = 4;
            this.mnuFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.mnuFind.Text = "&Find";
            this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 2;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTutorial,
            this.mnuAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuTutorial
            // 
            this.mnuTutorial.Index = 0;
            this.mnuTutorial.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.mnuTutorial.Text = "&Tutorial";
            this.mnuTutorial.Click += new System.EventHandler(this.mnuTutorial_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 1;
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // cmnuUpdateAndInstall
            // 
            this.cmnuUpdateAndInstall.Index = 2;
            this.cmnuUpdateAndInstall.Text = "Update &all and install updates";
            this.cmnuUpdateAndInstall.Click += new System.EventHandler(this.cmnuUpdateAndInstall_Click);
            // 
            // cmuAdd
            // 
            this.cmuAdd.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuAdd,
            this.cmnuImportFile,
            this.cmnuImportOnline});
            // 
            // cmnuAdd
            // 
            this.cmnuAdd.Index = 0;
            this.cmnuAdd.Text = "&New...";
            this.cmnuAdd.Click += new System.EventHandler(this.cmnuAdd_Click);
            // 
            // cmnuImportFile
            // 
            this.cmnuImportFile.Index = 1;
            this.cmnuImportFile.Text = "Imp&ort from file...";
            this.cmnuImportFile.Click += new System.EventHandler(this.cmnuImport_Click);
            // 
            // cmnuImportOnline
            // 
            this.cmnuImportOnline.Index = 2;
            this.cmnuImportOnline.Text = "I&mport from online database...";
            this.cmnuImportOnline.Click += new System.EventHandler(this.cmnuImportOnline_Click);
            // 
            // olvJobs
            // 
            this.olvJobs.AllColumns.Add(this.colName);
            this.olvJobs.AllColumns.Add(this.colLastUpdate);
            this.olvJobs.AllColumns.Add(this.colProgress);
            this.olvJobs.AllColumns.Add(this.colTarget);
            this.olvJobs.AllColumns.Add(this.colCategory);
            this.olvJobs.AllColumns.Add(this.colStatus);
            this.olvJobs.AllowColumnReorder = true;
            this.olvJobs.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvJobs.AlwaysGroupByColumn = null;
            this.olvJobs.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLastUpdate,
            this.colProgress,
            this.colTarget,
            this.colCategory});
            this.olvJobs.FullRowSelect = true;
            this.olvJobs.HideSelection = false;
            this.olvJobs.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvJobs.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvJobs.LastSortColumn = null;
            this.olvJobs.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvJobs.Location = new System.Drawing.Point(12, 12);
            this.olvJobs.Name = "olvJobs";
            this.olvJobs.OwnerDraw = true;
            this.olvJobs.Size = new System.Drawing.Size(658, 262);
            this.olvJobs.SmallImageList = this.imlStatus;
            this.olvJobs.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvJobs.TabIndex = 0;
            this.olvJobs.UseCompatibleStateImageBehavior = false;
            this.olvJobs.View = System.Windows.Forms.View.Details;
            this.olvJobs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvJobs_KeyDown);
            this.olvJobs.SelectionChanged += new System.EventHandler(this.olvJobs_SelectionChanged);
            this.olvJobs.SelectedIndexChanged += new System.EventHandler(this.olvJobs_SelectedIndexChanged);
            this.olvJobs.DoubleClick += new System.EventHandler(this.olvJobs_DoubleClick);
            // 
            // colName
            // 
            this.colName.AspectName = null;
            this.colName.Text = "Application";
            this.colName.Width = 183;
            // 
            // colLastUpdate
            // 
            this.colLastUpdate.AspectName = null;
            this.colLastUpdate.Text = "Last updated";
            this.colLastUpdate.Width = 110;
            // 
            // colProgress
            // 
            this.colProgress.AspectName = null;
            this.colProgress.MaximumWidth = 100;
            this.colProgress.MinimumWidth = 100;
            this.colProgress.Text = "Progress";
            this.colProgress.Width = 100;
            // 
            // colTarget
            // 
            this.colTarget.AspectName = null;
            this.colTarget.FillsFreeSpace = true;
            this.colTarget.Text = "Target";
            // 
            // colCategory
            // 
            this.colCategory.AspectName = "Category";
            this.colCategory.Text = "Category";
            this.colCategory.Width = 80;
            // 
            // colStatus
            // 
            this.colStatus.AspectName = null;
            this.colStatus.IsVisible = false;
            this.colStatus.Text = "Status";
            this.colStatus.Width = 80;
            // 
            // m_VistaMenu
            // 
            this.m_VistaMenu.ContainerControl = this;
            // 
            // cmuRun
            // 
            this.cmuRun.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuCheckAndDownload,
            this.cmnuOnlyCheck,
            this.cmnuUpdateAndInstall});
            // 
            // cmnuCheckAndDownload
            // 
            this.cmnuCheckAndDownload.Index = 0;
            this.cmnuCheckAndDownload.Text = "&Update all";
            this.cmnuCheckAndDownload.Click += new System.EventHandler(this.cmnuCheckAndDownload_Click);
            // 
            // cmnuOnlyCheck
            // 
            this.cmnuOnlyCheck.Index = 1;
            this.cmnuOnlyCheck.Text = "&Check all for updates only, do not download";
            this.cmnuOnlyCheck.Click += new System.EventHandler(this.cmnuOnlyCheck_Click);
            // 
            // ntiTrayIcon
            // 
            this.ntiTrayIcon.ContextMenu = this.cmnuTrayIconMenu;
            this.ntiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiTrayIcon.Icon")));
            this.ntiTrayIcon.Text = "Ketarin (Idle)";
            this.ntiTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntiTrayIcon_MouseDoubleClick);
            // 
            // cmnuTrayIconMenu
            // 
            this.cmnuTrayIconMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmnuShow,
            this.cmnuExit});
            // 
            // cmnuShow
            // 
            this.cmnuShow.Index = 0;
            this.cmnuShow.Text = "&Show";
            this.cmnuShow.Click += new System.EventHandler(this.cmnuShow_Click);
            // 
            // cmnuExit
            // 
            this.cmnuExit.Index = 1;
            this.cmnuExit.Text = "E&xit";
            this.cmnuExit.Click += new System.EventHandler(this.cmnuExit_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSelectedApplications,
            this.tbNumByStatus,
            this.tbTotalApplications});
            this.statusBar.Location = new System.Drawing.Point(0, 240);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(682, 24);
            this.statusBar.TabIndex = 6;
            this.statusBar.Text = "statusBar";
            this.statusBar.Visible = false;
            // 
            // tbSelectedApplications
            // 
            this.tbSelectedApplications.Name = "tbSelectedApplications";
            this.tbSelectedApplications.Size = new System.Drawing.Size(130, 19);
            this.tbSelectedApplications.Text = "Selected applications: 0";
            // 
            // tbNumByStatus
            // 
            this.tbNumByStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tbNumByStatus.Name = "tbNumByStatus";
            this.tbNumByStatus.Size = new System.Drawing.Size(197, 19);
            this.tbNumByStatus.Text = "By status: 0 Idle, 0 Finished, 0 Failed";
            // 
            // tbTotalApplications
            // 
            this.tbTotalApplications.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tbTotalApplications.Name = "tbTotalApplications";
            this.tbTotalApplications.Size = new System.Drawing.Size(340, 19);
            this.tbTotalApplications.Spring = true;
            this.tbTotalApplications.Text = "Number of applications: 0";
            this.tbTotalApplications.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bInstall
            // 
            this.bInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bInstall.AutoSize = true;
            this.bInstall.Image = global::Ketarin.Properties.Resources.Setup;
            this.bInstall.Location = new System.Drawing.Point(290, 280);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(85, 24);
            this.bInstall.TabIndex = 5;
            this.bInstall.Text = "I&nstall...";
            this.bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // bRun
            // 
            this.bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRun.AutoSize = true;
            this.bRun.Image = global::Ketarin.Properties.Resources.Restart;
            this.bRun.Location = new System.Drawing.Point(168, 280);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(116, 24);
            this.bRun.SplitMenu = this.cmuRun;
            this.bRun.TabIndex = 4;
            this.bRun.Text = "&Update all";
            this.bRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // bAddApplication
            // 
            this.bAddApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddApplication.AutoSize = true;
            this.bAddApplication.Image = global::Ketarin.Properties.Resources.AddSmall;
            this.bAddApplication.Location = new System.Drawing.Point(12, 280);
            this.bAddApplication.Name = "bAddApplication";
            this.bAddApplication.Size = new System.Drawing.Size(150, 24);
            this.bAddApplication.SplitMenu = this.cmuAdd;
            this.bAddApplication.TabIndex = 3;
            this.bAddApplication.Text = "&Add new application";
            this.bAddApplication.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bAddApplication.UseVisualStyleBackColor = true;
            this.bAddApplication.Click += new System.EventHandler(this.sbAddApplication_Click);
            // 
            // cmnuRunPostDownload
            // 
            this.cmnuRunPostDownload.Index = 0;
            this.cmnuRunPostDownload.Text = "&Run post-download command";
            this.cmnuRunPostDownload.Click += new System.EventHandler(this.cmnuRunPostDownload_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 316);
            this.Controls.Add(this.bInstall);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.bAddApplication);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.olvJobs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mnuMain;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.SavePosition = true;
            this.Text = "Ketarin";
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ApplicationJobsListView olvJobs;
        private System.Windows.Forms.ImageList imlStatus;
        private CDBurnerXP.Controls.VistaMenu m_VistaMenu;
        private System.Windows.Forms.ContextMenu cmnuJobs;
        private System.Windows.Forms.MenuItem cmnuUpdate;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem cmnuEdit;
        private System.Windows.Forms.MenuItem cmnuDelete;
        private CDBurnerXP.Controls.OLVColumn colName;
        private CDBurnerXP.Controls.OLVColumn colLastUpdate;
        private CDBurnerXP.Controls.OLVColumn colProgress;
        private CDBurnerXP.Controls.OLVColumn colTarget;
        private System.Windows.Forms.MenuItem cmnuOpenFile;
        private System.Windows.Forms.MenuItem cmnuRename;
        private CDBurnerXP.Controls.OLVColumn colCategory;
        private System.Windows.Forms.MainMenu mnuMain;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.MenuItem mnuNew;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem mnuExportSelected;
        private System.Windows.Forms.MenuItem mnuImport;
        private wyDay.Controls.SplitButton bAddApplication;
        private System.Windows.Forms.ContextMenu cmuAdd;
        private System.Windows.Forms.MenuItem cmnuAdd;
        private System.Windows.Forms.MenuItem cmnuImportFile;
        private System.Windows.Forms.MenuItem cmnuImportOnline;
        private System.Windows.Forms.MenuItem mnusep2;
        private System.Windows.Forms.MenuItem mnuSettings;
        private System.Windows.Forms.MenuItem cmnuCopy;
        private System.Windows.Forms.MenuItem cmnuPaste;
        private System.Windows.Forms.MenuItem mnuSelectAll;
        private System.Windows.Forms.MenuItem cmnuOpenFolder;
        private System.Windows.Forms.MenuItem mnuView;
        private wyDay.Controls.SplitButton bRun;
        private System.Windows.Forms.ContextMenu cmuRun;
        private System.Windows.Forms.MenuItem cmnuCheckAndDownload;
        private System.Windows.Forms.MenuItem cmnuOnlyCheck;
        private System.Windows.Forms.MenuItem mnuTutorial;
        private System.Windows.Forms.NotifyIcon ntiTrayIcon;
        private System.Windows.Forms.ContextMenu cmnuTrayIconMenu;
        private System.Windows.Forms.MenuItem cmnuShow;
        private System.Windows.Forms.MenuItem cmnuExit;
        private System.Windows.Forms.MenuItem mnuExportAll;
        private System.Windows.Forms.MenuItem cmnuCheckForUpdate;
        private System.Windows.Forms.MenuItem mnuLog;
        private System.Windows.Forms.MenuItem mnuShowGroups;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tbSelectedApplications;
        private System.Windows.Forms.ToolStripStatusLabel tbTotalApplications;
        private System.Windows.Forms.MenuItem mnuShowStatusBar;
        private System.Windows.Forms.MenuItem mnuInvert;
        private System.Windows.Forms.MenuItem cmnuForceDownload;
        private System.Windows.Forms.MenuItem mnuFind;
        private System.Windows.Forms.MenuItem mnuAutoScroll;
        private System.Windows.Forms.MenuItem cmnuUpdateAndInstall;
        private CDBurnerXP.Controls.OLVColumn colStatus;
        private System.Windows.Forms.ToolStripStatusLabel tbNumByStatus;
        private System.Windows.Forms.MenuItem cmnuProperties;
        private wyDay.Controls.SplitButton bInstall;
        private System.Windows.Forms.MenuItem cmnuInstall;
        private System.Windows.Forms.MenuItem cmnuUpdateInstall;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem cmnuCommands;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem cmnuRunPostDownload;
    }
}

