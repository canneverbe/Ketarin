namespace Ketarin.Forms
{
    partial class ImportFromDatabaseDialog
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
            this.txtSearchSubject = new System.Windows.Forms.TextBox();
            this.bSearch = new System.Windows.Forms.Button();
            this.bTop50 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.Text = "I&mport";
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // olvApplications
            // 
            this.olvApplications.Location = new System.Drawing.Point(12, 59);
            this.olvApplications.Size = new System.Drawing.Size(290, 204);
            // 
            // lblResults
            // 
            this.lblResults.Location = new System.Drawing.Point(12, 43);
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
            // bTop50
            // 
            this.bTop50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bTop50.Location = new System.Drawing.Point(12, 269);
            this.bTop50.Name = "bTop50";
            this.bTop50.Size = new System.Drawing.Size(75, 23);
            this.bTop50.TabIndex = 7;
            this.bTop50.Text = "&Top 50";
            this.bTop50.UseVisualStyleBackColor = true;
            this.bTop50.Click += new System.EventHandler(this.bTop50_Click);
            // 
            // ImportFromDatabaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 304);
            this.Controls.Add(this.bTop50);
            this.Controls.Add(this.txtSearchSubject);
            this.Controls.Add(this.bSearch);
            this.Name = "ImportFromDatabaseDialog";
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.lblResults, 0);
            this.Controls.SetChildIndex(this.olvApplications, 0);
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bSearch, 0);
            this.Controls.SetChildIndex(this.txtSearchSubject, 0);
            this.Controls.SetChildIndex(this.bTop50, 0);
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchSubject;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.Button bTop50;
    }
}