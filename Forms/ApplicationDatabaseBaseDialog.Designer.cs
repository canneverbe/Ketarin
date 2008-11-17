namespace Ketarin.Forms
{
    partial class ApplicationDatabaseBaseDialog
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
            this.lblResults = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.cmnuApplications = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.olvApplications = new CDBurnerXP.Controls.FastObjectListView();
            this.colAppName = new CDBurnerXP.Controls.OLVColumn();
            this.colDate = new CDBurnerXP.Controls.OLVColumn();
            this.cmnuApplications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(12, 9);
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
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(146, 269);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 5;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // cmnuApplications
            // 
            this.cmnuApplications.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuProperties});
            this.cmnuApplications.Name = "cmnuApplications";
            this.cmnuApplications.Size = new System.Drawing.Size(128, 26);
            // 
            // cmnuProperties
            // 
            this.cmnuProperties.Name = "cmnuProperties";
            this.cmnuProperties.Size = new System.Drawing.Size(127, 22);
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
            this.olvApplications.Location = new System.Drawing.Point(12, 25);
            this.olvApplications.Name = "olvApplications";
            this.olvApplications.ShowGroups = false;
            this.olvApplications.Size = new System.Drawing.Size(290, 238);
            this.olvApplications.TabIndex = 3;
            this.olvApplications.UseCompatibleStateImageBehavior = false;
            this.olvApplications.View = System.Windows.Forms.View.Details;
            this.olvApplications.VirtualMode = true;
            this.olvApplications.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.olvApplications_MouseDoubleClick);
            this.olvApplications.SelectedIndexChanged += new System.EventHandler(this.OnSelectedApplicationChanged);
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
            // ApplicationDatabaseBaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 304);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.olvApplications);
            this.Controls.Add(this.lblResults);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(330, 340);
            this.Name = "ApplicationDatabaseBaseDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Available Applications";
            this.cmnuApplications.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CDBurnerXP.Controls.OLVColumn colAppName;
        private CDBurnerXP.Controls.OLVColumn colDate;
        private System.Windows.Forms.ContextMenuStrip cmnuApplications;
        private System.Windows.Forms.ToolStripMenuItem cmnuProperties;
        protected System.Windows.Forms.Button bOK;
        protected CDBurnerXP.Controls.FastObjectListView olvApplications;
        protected System.Windows.Forms.Label lblResults;
        protected System.Windows.Forms.Button bCancel;
    }
}