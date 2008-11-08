namespace Ketarin.Forms
{
    partial class ApplicationDatabaseDialog
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
            this.bSearch = new System.Windows.Forms.Button();
            this.txtSearchSubject = new System.Windows.Forms.TextBox();
            this.lblResults = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.cmnuApplications = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.olvApplications = new CDBurnerXP.Controls.FastObjectListView();
            this.colAppName = new CDBurnerXP.Controls.OLVColumn();
            this.colDate = new CDBurnerXP.Controls.OLVColumn();
            this.cmnuApplications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // bSearch
            // 
            this.bSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSearch.Location = new System.Drawing.Point(227, 12);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(75, 23);
            this.bSearch.TabIndex = 1;
            this.bSearch.Text = "&Search";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // txtSearchSubject
            // 
            this.txtSearchSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchSubject.Location = new System.Drawing.Point(12, 14);
            this.txtSearchSubject.Name = "txtSearchSubject";
            this.txtSearchSubject.Size = new System.Drawing.Size(209, 20);
            this.txtSearchSubject.TabIndex = 0;
            this.txtSearchSubject.TextChanged += new System.EventHandler(this.txtSearchSubject_TextChanged);
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(12, 42);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(112, 13);
            this.lblResults.TabIndex = 2;
            this.lblResults.Text = "&Available applications:";
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(227, 269);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bImport
            // 
            this.bImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bImport.Enabled = false;
            this.bImport.Location = new System.Drawing.Point(146, 269);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(75, 23);
            this.bImport.TabIndex = 5;
            this.bImport.Text = "I&mport";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // cmnuApplications
            // 
            this.cmnuApplications.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuProperties});
            this.cmnuApplications.Name = "cmnuApplications";
            this.cmnuApplications.Size = new System.Drawing.Size(153, 48);
            // 
            // cmnuProperties
            // 
            this.cmnuProperties.Name = "cmnuProperties";
            this.cmnuProperties.Size = new System.Drawing.Size(152, 22);
            this.cmnuProperties.Text = "&Properties";
            this.cmnuProperties.Click += new System.EventHandler(this.cmnuProperties_Click);
            // 
            // olvApplications
            // 
            this.olvApplications.AllColumns.Add(this.colAppName);
            this.olvApplications.AllColumns.Add(this.colDate);
            this.olvApplications.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvApplications.AlwaysGroupByColumn = null;
            this.olvApplications.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApplications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvApplications.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAppName,
            this.colDate});
            this.olvApplications.ContextMenuStrip = this.cmnuApplications;
            this.olvApplications.FullRowSelect = true;
            this.olvApplications.HideSelection = false;
            this.olvApplications.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvApplications.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvApplications.LastSortColumn = null;
            this.olvApplications.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApplications.Location = new System.Drawing.Point(12, 58);
            this.olvApplications.Name = "olvApplications";
            this.olvApplications.ShowGroups = false;
            this.olvApplications.Size = new System.Drawing.Size(290, 205);
            this.olvApplications.TabIndex = 3;
            this.olvApplications.UseCompatibleStateImageBehavior = false;
            this.olvApplications.View = System.Windows.Forms.View.Details;
            this.olvApplications.VirtualMode = true;
            this.olvApplications.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.olvApplications_MouseDoubleClick);
            this.olvApplications.SelectedIndexChanged += new System.EventHandler(this.olvApplications_SelectedIndexChanged);
            // 
            // colAppName
            // 
            this.colAppName.AspectName = "ApplicationName";
            this.colAppName.FillsFreeSpace = true;
            this.colAppName.Text = "Application name";
            this.colAppName.Width = 100;
            // 
            // colDate
            // 
            this.colDate.AspectName = "UpdatedAtDate";
            this.colDate.Text = "Last updated";
            this.colDate.Width = 130;
            // 
            // ApplicationDatabaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 304);
            this.Controls.Add(this.bImport);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.txtSearchSubject);
            this.Controls.Add(this.bSearch);
            this.Controls.Add(this.olvApplications);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(330, 340);
            this.Name = "ApplicationDatabaseDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Online Application Database";
            this.cmnuApplications.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CDBurnerXP.Controls.FastObjectListView olvApplications;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.TextBox txtSearchSubject;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bImport;
        private CDBurnerXP.Controls.OLVColumn colAppName;
        private CDBurnerXP.Controls.OLVColumn colDate;
        private System.Windows.Forms.ContextMenuStrip cmnuApplications;
        private System.Windows.Forms.ToolStripMenuItem cmnuProperties;
    }
}