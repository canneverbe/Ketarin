using System.ComponentModel;
using System.Windows.Forms;
using CDBurnerXP.Controls;

namespace Ketarin.Forms
{
    partial class SelectApplicationDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.olvApplications = new Ketarin.ApplicationJobsListView();
            this.colAppName = new CDBurnerXP.Controls.OLVColumn();
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(275, 334);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(194, 334);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Applications to add:";
            // 
            // olvApplications
            // 
            this.olvApplications.AllColumns.Add(this.colAppName);
            this.olvApplications.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvApplications.AlwaysGroupByColumn = null;
            this.olvApplications.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApplications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvApplications.CheckBoxes = true;
            this.olvApplications.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAppName});
            this.olvApplications.FullRowSelect = true;
            this.olvApplications.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvApplications.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvApplications.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvApplications.LastSortColumn = null;
            this.olvApplications.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvApplications.Location = new System.Drawing.Point(15, 25);
            this.olvApplications.Name = "olvApplications";
            this.olvApplications.ShowGroups = false;
            this.olvApplications.Size = new System.Drawing.Size(335, 292);
            this.olvApplications.TabIndex = 1;
            this.olvApplications.UseCompatibleStateImageBehavior = false;
            this.olvApplications.View = System.Windows.Forms.View.Details;
            this.olvApplications.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.olvApplications_ItemChecked);
            // 
            // colAppName
            // 
            this.colAppName.AspectName = "Name";
            this.colAppName.FillsFreeSpace = true;
            this.colAppName.Text = "Application name";
            this.colAppName.Width = 100;
            // 
            // SelectApplicationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 369);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.olvApplications);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 400);
            this.Name = "SelectApplicationDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Applications";
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected Button bCancel;
        protected Button bOK;
        private ApplicationJobsListView olvApplications;
        private Label label1;
        private OLVColumn colAppName;
    }
}