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
            this.cmnuOpenFile = new System.Windows.Forms.MenuItem();
            this.cmnuRename = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.cmnuEdit = new System.Windows.Forms.MenuItem();
            this.cmnuDelete = new System.Windows.Forms.MenuItem();
            this.sepView = new System.Windows.Forms.MenuItem();
            this.cmnuShowGroups = new System.Windows.Forms.MenuItem();
            this.bAbout = new System.Windows.Forms.Button();
            this.bRun = new System.Windows.Forms.Button();
            this.bAddNew = new System.Windows.Forms.Button();
            this.olvJobs = new CDBurnerXP.Controls.ObjectListView();
            this.colName = new CDBurnerXP.Controls.OLVColumn();
            this.colLastUpdate = new CDBurnerXP.Controls.OLVColumn();
            this.colProgress = new CDBurnerXP.Controls.OLVColumn();
            this.colTarget = new CDBurnerXP.Controls.OLVColumn();
            this.m_VistaMenu = new CDBurnerXP.Controls.VistaMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).BeginInit();
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
            this.cmnuOpenFile,
            this.cmnuRename,
            this.menuItem4,
            this.cmnuEdit,
            this.cmnuDelete,
            this.sepView,
            this.cmnuShowGroups});
            this.cmnuJobs.Popup += new System.EventHandler(this.cmnuJobs_Popup);
            // 
            // cmnuUpdate
            // 
            this.m_VistaMenu.SetImage(this.cmnuUpdate, global::Ketarin.Properties.Resources.Restart);
            this.cmnuUpdate.Index = 0;
            this.cmnuUpdate.Text = "&Update";
            this.cmnuUpdate.Click += new System.EventHandler(this.cmuUpdate_Click);
            // 
            // cmnuOpenFile
            // 
            this.cmnuOpenFile.Enabled = false;
            this.cmnuOpenFile.Index = 1;
            this.cmnuOpenFile.Text = "&Open file";
            this.cmnuOpenFile.Click += new System.EventHandler(this.cmnuOpenFile_Click);
            // 
            // cmnuRename
            // 
            this.cmnuRename.Enabled = false;
            this.cmnuRename.Index = 2;
            this.cmnuRename.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.cmnuRename.Text = "&Rename file";
            this.cmnuRename.Click += new System.EventHandler(this.cmnuRename_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            // 
            // cmnuEdit
            // 
            this.cmnuEdit.Enabled = false;
            this.cmnuEdit.Index = 4;
            this.cmnuEdit.Text = "&Edit";
            this.cmnuEdit.Click += new System.EventHandler(this.cmnuEdit_Click);
            // 
            // cmnuDelete
            // 
            this.cmnuDelete.Enabled = false;
            this.cmnuDelete.Index = 5;
            this.cmnuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.cmnuDelete.Text = "&Delete";
            this.cmnuDelete.Click += new System.EventHandler(this.cmnuDelete_Click);
            // 
            // sepView
            // 
            this.sepView.Index = 6;
            this.sepView.Text = "-";
            // 
            // cmnuShowGroups
            // 
            this.cmnuShowGroups.Checked = true;
            this.cmnuShowGroups.Index = 7;
            this.cmnuShowGroups.Text = "&Show groups";
            this.cmnuShowGroups.Click += new System.EventHandler(this.cmnuShowGroups_Click);
            // 
            // bAbout
            // 
            this.bAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAbout.Image = global::Ketarin.Properties.Resources.Symbol_Information;
            this.bAbout.Location = new System.Drawing.Point(640, 302);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(30, 24);
            this.bAbout.TabIndex = 3;
            this.bAbout.UseVisualStyleBackColor = true;
            this.bAbout.Click += new System.EventHandler(this.bAbout_Click);
            // 
            // bRun
            // 
            this.bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRun.Image = global::Ketarin.Properties.Resources.Restart;
            this.bRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bRun.Location = new System.Drawing.Point(168, 302);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(107, 24);
            this.bRun.TabIndex = 2;
            this.bRun.Text = "&Update now";
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // bAddNew
            // 
            this.bAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddNew.Image = global::Ketarin.Properties.Resources.AddSmall;
            this.bAddNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bAddNew.Location = new System.Drawing.Point(12, 302);
            this.bAddNew.Name = "bAddNew";
            this.bAddNew.Size = new System.Drawing.Size(150, 24);
            this.bAddNew.TabIndex = 1;
            this.bAddNew.Text = "&Add new application";
            this.bAddNew.UseVisualStyleBackColor = true;
            this.bAddNew.Click += new System.EventHandler(this.bAddNew_Click);
            // 
            // olvJobs
            // 
            this.olvJobs.AllColumns.Add(this.colName);
            this.olvJobs.AllColumns.Add(this.colLastUpdate);
            this.olvJobs.AllColumns.Add(this.colProgress);
            this.olvJobs.AllColumns.Add(this.colTarget);
            this.olvJobs.AllColumns.Add(this.colName);
            this.olvJobs.AllColumns.Add(this.colLastUpdate);
            this.olvJobs.AllColumns.Add(this.colProgress);
            this.olvJobs.AllColumns.Add(this.colTarget);
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
            this.colTarget});
            this.olvJobs.EmptyListMsg = "No applications have been added yet.";
            this.olvJobs.FullRowSelect = true;
            this.olvJobs.HideSelection = false;
            this.olvJobs.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvJobs.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvJobs.LastSortColumn = null;
            this.olvJobs.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvJobs.Location = new System.Drawing.Point(12, 12);
            this.olvJobs.Name = "olvJobs";
            this.olvJobs.OwnerDraw = true;
            this.olvJobs.Size = new System.Drawing.Size(658, 284);
            this.olvJobs.SmallImageList = this.imlStatus;
            this.olvJobs.TabIndex = 0;
            this.olvJobs.UseCompatibleStateImageBehavior = false;
            this.olvJobs.View = System.Windows.Forms.View.Details;
            this.olvJobs.DoubleClick += new System.EventHandler(this.olvJobs_DoubleClick);
            this.olvJobs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvJobs_KeyDown);
            this.olvJobs.SelectedIndexChanged += new System.EventHandler(this.olvJobs_SelectedIndexChanged);
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
            // m_VistaMenu
            // 
            this.m_VistaMenu.ContainerControl = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 337);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.bAddNew);
            this.Controls.Add(this.olvJobs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.Text = "Ketarin";
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CDBurnerXP.Controls.ObjectListView olvJobs;
        private System.Windows.Forms.Button bAddNew;
        private System.Windows.Forms.Button bRun;
        private System.Windows.Forms.ImageList imlStatus;
        private CDBurnerXP.Controls.VistaMenu m_VistaMenu;
        private System.Windows.Forms.ContextMenu cmnuJobs;
        private System.Windows.Forms.MenuItem cmnuUpdate;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem cmnuEdit;
        private System.Windows.Forms.MenuItem cmnuDelete;
        private System.Windows.Forms.MenuItem sepView;
        private System.Windows.Forms.MenuItem cmnuShowGroups;
        private CDBurnerXP.Controls.OLVColumn colName;
        private CDBurnerXP.Controls.OLVColumn colLastUpdate;
        private CDBurnerXP.Controls.OLVColumn colProgress;
        private CDBurnerXP.Controls.OLVColumn colTarget;
        private System.Windows.Forms.Button bAbout;
        private System.Windows.Forms.MenuItem cmnuOpenFile;
        private System.Windows.Forms.MenuItem cmnuRename;
    }
}

