namespace Ketarin.Forms
{
    partial class ChooseAppsToInstallDialog
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
            this.components = new System.ComponentModel.Container();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.olvLists = new CDBurnerXP.Controls.ObjectListView();
            this.colListName = new CDBurnerXP.Controls.OLVColumn();
            this.colListAppNames = new CDBurnerXP.Controls.OLVColumn();
            this.imlLists = new System.Windows.Forms.ImageList(this.components);
            this.olvApps = new CDBurnerXP.Controls.FastObjectListView();
            this.colAppsName = new CDBurnerXP.Controls.OLVColumn();
            this.lblAppLists = new System.Windows.Forms.Label();
            this.lblAppsInCurrentList = new System.Windows.Forms.Label();
            this.bNewList = new System.Windows.Forms.Button();
            this.bRemoveList = new System.Windows.Forms.Button();
            this.bSelectApp = new wyDay.Controls.SplitButton();
            this.selectionMenu = new System.Windows.Forms.ContextMenu();
            this.mnuSelectAll = new System.Windows.Forms.MenuItem();
            this.mnuSelectNone = new System.Windows.Forms.MenuItem();
            this.mnuInvertSelection = new System.Windows.Forms.MenuItem();
            this.sepNewList = new System.Windows.Forms.MenuItem();
            this.mnuSaveAsNewList = new System.Windows.Forms.MenuItem();
            this.bAddApp = new System.Windows.Forms.Button();
            this.bRemoveApp = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlApps = new System.Windows.Forms.Panel();
            this.pnlAppLists = new System.Windows.Forms.Panel();
            this.lblUndoDelete = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.olvLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvApps)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlApps.SuspendLayout();
            this.pnlAppLists.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(515, 359);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 99;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(434, 359);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 98;
            this.bOK.Text = "I&nstall";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // olvLists
            // 
            this.olvLists.AllColumns.Add(this.colListName);
            this.olvLists.AllColumns.Add(this.colListAppNames);
            this.olvLists.AllowDrop = true;
            this.olvLists.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvLists.AlwaysGroupByColumn = null;
            this.olvLists.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvLists.CellEditActivation = CDBurnerXP.Controls.ObjectListView.CellEditActivateMode.F2Only;
            this.olvLists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colListName,
            this.colListAppNames});
            this.olvLists.FullRowSelect = true;
            this.olvLists.HideSelection = false;
            this.olvLists.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvLists.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvLists.LargeImageList = this.imlLists;
            this.olvLists.LastSortColumn = null;
            this.olvLists.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvLists.Location = new System.Drawing.Point(0, 16);
            this.olvLists.Name = "olvLists";
            this.olvLists.ShowGroups = false;
            this.olvLists.Size = new System.Drawing.Size(286, 280);
            this.olvLists.TabIndex = 1;
            this.olvLists.UseCompatibleStateImageBehavior = false;
            this.olvLists.View = System.Windows.Forms.View.Tile;
            this.olvLists.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvLists_KeyDown);
            this.olvLists.CellEditStarting += new CDBurnerXP.Controls.ObjectListView.CellEditEventHandler(this.olvLists_CellEditStarting);
            this.olvLists.CellEditFinished += new CDBurnerXP.Controls.ObjectListView.CellEditEventHandler(this.olvLists_CellEditFinished);
            this.olvLists.DragDrop += new System.Windows.Forms.DragEventHandler(this.olvLists_DragDrop);
            this.olvLists.SelectedIndexChanged += new System.EventHandler(this.olvLists_SelectedIndexChanged);
            this.olvLists.DragOver += new System.Windows.Forms.DragEventHandler(this.olvLists_DragOver);
            this.olvLists.DragEnter += new System.Windows.Forms.DragEventHandler(this.olvLists_DragEnter);
            // 
            // colListName
            // 
            this.colListName.AspectName = "Name";
            this.colListName.IsTileViewColumn = true;
            this.colListName.Text = "Name";
            // 
            // colListAppNames
            // 
            this.colListAppNames.AspectName = "ApplicationNames";
            this.colListAppNames.IsTileViewColumn = true;
            this.colListAppNames.Text = "Applications";
            // 
            // imlLists
            // 
            this.imlLists.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlLists.ImageSize = new System.Drawing.Size(32, 32);
            this.imlLists.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // olvApps
            // 
            this.olvApps.AllColumns.Add(this.colAppsName);
            this.olvApps.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvApps.AlwaysGroupByColumn = null;
            this.olvApps.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvApps.CheckBoxes = true;
            this.olvApps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAppsName});
            this.olvApps.EmptyListMsg = "No applications in list \"{0}\"";
            this.olvApps.FullRowSelect = true;
            this.olvApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvApps.HideSelection = false;
            this.olvApps.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvApps.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvApps.LastSortColumn = null;
            this.olvApps.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApps.Location = new System.Drawing.Point(0, 16);
            this.olvApps.MultiSelect = false;
            this.olvApps.Name = "olvApps";
            this.olvApps.ShowGroups = false;
            this.olvApps.Size = new System.Drawing.Size(286, 280);
            this.olvApps.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvApps.TabIndex = 1;
            this.olvApps.UseCompatibleStateImageBehavior = false;
            this.olvApps.View = System.Windows.Forms.View.Details;
            this.olvApps.VirtualMode = true;
            this.olvApps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvApps_KeyDown);
            this.olvApps.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.olvApps_ItemDrag);
            this.olvApps.SelectedIndexChanged += new System.EventHandler(this.olvApps_SelectedIndexChanged);
            // 
            // colAppsName
            // 
            this.colAppsName.AspectName = "Name";
            this.colAppsName.FillsFreeSpace = true;
            this.colAppsName.Text = "Name";
            this.colAppsName.Width = 120;
            // 
            // lblAppLists
            // 
            this.lblAppLists.AutoSize = true;
            this.lblAppLists.Location = new System.Drawing.Point(-3, 0);
            this.lblAppLists.Name = "lblAppLists";
            this.lblAppLists.Size = new System.Drawing.Size(185, 13);
            this.lblAppLists.TabIndex = 0;
            this.lblAppLists.Text = "In&stall the following list of applications:";
            // 
            // lblAppsInCurrentList
            // 
            this.lblAppsInCurrentList.AutoSize = true;
            this.lblAppsInCurrentList.Location = new System.Drawing.Point(-3, 0);
            this.lblAppsInCurrentList.Name = "lblAppsInCurrentList";
            this.lblAppsInCurrentList.Size = new System.Drawing.Size(181, 13);
            this.lblAppsInCurrentList.TabIndex = 0;
            this.lblAppsInCurrentList.Text = "Inst&all these applications from the list:";
            // 
            // bNewList
            // 
            this.bNewList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bNewList.Location = new System.Drawing.Point(0, 302);
            this.bNewList.Name = "bNewList";
            this.bNewList.Size = new System.Drawing.Size(63, 23);
            this.bNewList.TabIndex = 2;
            this.bNewList.Text = "Ne&w";
            this.bNewList.UseVisualStyleBackColor = true;
            this.bNewList.Click += new System.EventHandler(this.bNewList_Click);
            // 
            // bRemoveList
            // 
            this.bRemoveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRemoveList.Enabled = false;
            this.bRemoveList.Location = new System.Drawing.Point(69, 302);
            this.bRemoveList.Name = "bRemoveList";
            this.bRemoveList.Size = new System.Drawing.Size(63, 23);
            this.bRemoveList.TabIndex = 3;
            this.bRemoveList.Text = "&Remove";
            this.bRemoveList.UseVisualStyleBackColor = true;
            this.bRemoveList.Click += new System.EventHandler(this.bRemoveList_Click);
            // 
            // bSelectApp
            // 
            this.bSelectApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSelectApp.AutoSize = true;
            this.bSelectApp.Location = new System.Drawing.Point(138, 302);
            this.bSelectApp.Name = "bSelectApp";
            this.bSelectApp.SeparateDropdownButton = false;
            this.bSelectApp.Size = new System.Drawing.Size(80, 23);
            this.bSelectApp.SplitMenu = this.selectionMenu;
            this.bSelectApp.TabIndex = 4;
            this.bSelectApp.Text = "Sele&ction";
            this.bSelectApp.UseVisualStyleBackColor = true;
            // 
            // selectionMenu
            // 
            this.selectionMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSelectAll,
            this.mnuSelectNone,
            this.mnuInvertSelection,
            this.sepNewList,
            this.mnuSaveAsNewList});
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Index = 0;
            this.mnuSelectAll.Text = "Select &all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuSelectNone
            // 
            this.mnuSelectNone.Index = 1;
            this.mnuSelectNone.Text = "Select &none";
            this.mnuSelectNone.Click += new System.EventHandler(this.mnuSelectNone_Click);
            // 
            // mnuInvertSelection
            // 
            this.mnuInvertSelection.Index = 2;
            this.mnuInvertSelection.Text = "In&vert selection";
            this.mnuInvertSelection.Click += new System.EventHandler(this.mnuInvertSelection_Click);
            // 
            // sepNewList
            // 
            this.sepNewList.Index = 3;
            this.sepNewList.Text = "-";
            // 
            // mnuSaveAsNewList
            // 
            this.mnuSaveAsNewList.Index = 4;
            this.mnuSaveAsNewList.Text = "&Save as new list";
            this.mnuSaveAsNewList.Click += new System.EventHandler(this.mnuSaveAsNewList_Click);
            // 
            // bAddApp
            // 
            this.bAddApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddApp.Location = new System.Drawing.Point(0, 302);
            this.bAddApp.Name = "bAddApp";
            this.bAddApp.Size = new System.Drawing.Size(63, 23);
            this.bAddApp.TabIndex = 2;
            this.bAddApp.Text = "A&dd...";
            this.bAddApp.UseVisualStyleBackColor = true;
            this.bAddApp.Click += new System.EventHandler(this.bAddApp_Click);
            // 
            // bRemoveApp
            // 
            this.bRemoveApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRemoveApp.Enabled = false;
            this.bRemoveApp.Location = new System.Drawing.Point(69, 302);
            this.bRemoveApp.Name = "bRemoveApp";
            this.bRemoveApp.Size = new System.Drawing.Size(63, 23);
            this.bRemoveApp.TabIndex = 3;
            this.bRemoveApp.Text = "Re&move";
            this.bRemoveApp.UseVisualStyleBackColor = true;
            this.bRemoveApp.Click += new System.EventHandler(this.bRemoveApp_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.pnlApps, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlAppLists, 0, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(578, 325);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pnlApps
            // 
            this.pnlApps.Controls.Add(this.lblAppsInCurrentList);
            this.pnlApps.Controls.Add(this.olvApps);
            this.pnlApps.Controls.Add(this.bSelectApp);
            this.pnlApps.Controls.Add(this.bRemoveApp);
            this.pnlApps.Controls.Add(this.bAddApp);
            this.pnlApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlApps.Location = new System.Drawing.Point(292, 0);
            this.pnlApps.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pnlApps.Name = "pnlApps";
            this.pnlApps.Size = new System.Drawing.Size(286, 325);
            this.pnlApps.TabIndex = 0;
            // 
            // pnlAppLists
            // 
            this.pnlAppLists.Controls.Add(this.lblUndoDelete);
            this.pnlAppLists.Controls.Add(this.lblAppLists);
            this.pnlAppLists.Controls.Add(this.olvLists);
            this.pnlAppLists.Controls.Add(this.bNewList);
            this.pnlAppLists.Controls.Add(this.bRemoveList);
            this.pnlAppLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAppLists.Location = new System.Drawing.Point(0, 0);
            this.pnlAppLists.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlAppLists.Name = "pnlAppLists";
            this.pnlAppLists.Size = new System.Drawing.Size(286, 325);
            this.pnlAppLists.TabIndex = 0;
            // 
            // lblUndoDelete
            // 
            this.lblUndoDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUndoDelete.BackColor = System.Drawing.SystemColors.Info;
            this.lblUndoDelete.Location = new System.Drawing.Point(1, 277);
            this.lblUndoDelete.Name = "lblUndoDelete";
            this.lblUndoDelete.Size = new System.Drawing.Size(284, 18);
            this.lblUndoDelete.TabIndex = 4;
            this.lblUndoDelete.TabStop = true;
            this.lblUndoDelete.Text = "List deleted. Undo";
            this.lblUndoDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUndoDelete.Visible = false;
            this.lblUndoDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblUndoDelete_LinkClicked);
            // 
            // ChooseAppsToInstallDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 394);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 300);
            this.Name = "ChooseAppsToInstallDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Applications to Install";
            ((System.ComponentModel.ISupportInitialize)(this.olvLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvApps)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlApps.ResumeLayout(false);
            this.pnlApps.PerformLayout();
            this.pnlAppLists.ResumeLayout(false);
            this.pnlAppLists.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button bCancel;
        protected System.Windows.Forms.Button bOK;
        private CDBurnerXP.Controls.ObjectListView olvLists;
        private CDBurnerXP.Controls.FastObjectListView olvApps;
        private System.Windows.Forms.Label lblAppLists;
        private System.Windows.Forms.Label lblAppsInCurrentList;
        private System.Windows.Forms.Button bNewList;
        private System.Windows.Forms.Button bRemoveList;
        private CDBurnerXP.Controls.OLVColumn colListName;
        private CDBurnerXP.Controls.OLVColumn colListAppNames;
        private System.Windows.Forms.ImageList imlLists;
        private wyDay.Controls.SplitButton bSelectApp;
        private System.Windows.Forms.Button bAddApp;
        private System.Windows.Forms.Button bRemoveApp;
        private System.Windows.Forms.ContextMenu selectionMenu;
        private CDBurnerXP.Controls.OLVColumn colAppsName;
        private System.Windows.Forms.MenuItem mnuSelectAll;
        private System.Windows.Forms.MenuItem mnuSelectNone;
        private System.Windows.Forms.MenuItem mnuInvertSelection;
        private System.Windows.Forms.MenuItem sepNewList;
        private System.Windows.Forms.MenuItem mnuSaveAsNewList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel pnlApps;
        private System.Windows.Forms.Panel pnlAppLists;
        private System.Windows.Forms.LinkLabel lblUndoDelete;
    }
}