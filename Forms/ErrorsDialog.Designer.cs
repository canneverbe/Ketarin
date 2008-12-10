namespace Ketarin.Forms
{
    partial class ErrorsDialog
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
            this.lblDesc = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.olvErrors = new CDBurnerXP.Controls.FastObjectListView();
            this.colAppName = new CDBurnerXP.Controls.OLVColumn();
            this.colError = new CDBurnerXP.Controls.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)(this.olvErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDesc.Location = new System.Drawing.Point(12, 9);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(518, 20);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "The following errors occured while updating the applications:";
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(455, 210);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 1;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            // 
            // olvErrors
            // 
            this.olvErrors.AllColumns.Add(this.colAppName);
            this.olvErrors.AllColumns.Add(this.colError);
            this.olvErrors.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvErrors.AlwaysGroupByColumn = null;
            this.olvErrors.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAppName,
            this.colError});
            this.olvErrors.FullRowSelect = true;
            this.olvErrors.HideSelection = false;
            this.olvErrors.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvErrors.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvErrors.LastSortColumn = null;
            this.olvErrors.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvErrors.Location = new System.Drawing.Point(15, 26);
            this.olvErrors.Name = "olvErrors";
            this.olvErrors.ShowGroups = false;
            this.olvErrors.ShowItemToolTips = true;
            this.olvErrors.Size = new System.Drawing.Size(515, 178);
            this.olvErrors.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvErrors.TabIndex = 2;
            this.olvErrors.UseCompatibleStateImageBehavior = false;
            this.olvErrors.View = System.Windows.Forms.View.Details;
            this.olvErrors.VirtualMode = true;
            // 
            // colAppName
            // 
            this.colAppName.AspectName = "ApplicationJob.Name";
            this.colAppName.Text = "Application";
            this.colAppName.Width = 98;
            // 
            // colError
            // 
            this.colError.AspectName = "Message";
            this.colError.FillsFreeSpace = true;
            this.colError.Text = "Error";
            this.colError.Width = 120;
            // 
            // ErrorsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 245);
            this.Controls.Add(this.olvErrors);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.lblDesc);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 200);
            this.Name = "ErrorsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Errors";
            ((System.ComponentModel.ISupportInitialize)(this.olvErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Button bClose;
        private CDBurnerXP.Controls.FastObjectListView olvErrors;
        private CDBurnerXP.Controls.OLVColumn colAppName;
        private CDBurnerXP.Controls.OLVColumn colError;
    }
}