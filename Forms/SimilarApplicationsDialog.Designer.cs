using System.ComponentModel;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    partial class SimilarApplicationsDialog
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
            this.lblIntro = new System.Windows.Forms.Label();
            this.lblNewName = new System.Windows.Forms.Label();
            this.txtNewName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).BeginInit();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // olvApplications
            // 
            this.olvApplications.Location = new System.Drawing.Point(12, 65);
            this.olvApplications.Size = new System.Drawing.Size(290, 170);
            // 
            // lblResults
            // 
            this.lblResults.Location = new System.Drawing.Point(12, 49);
            this.lblResults.Size = new System.Drawing.Size(105, 13);
            this.lblResults.Text = "&Existing applications:";
            // 
            // lblIntro
            // 
            this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIntro.Location = new System.Drawing.Point(12, 9);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Size = new System.Drawing.Size(290, 27);
            this.lblIntro.TabIndex = 0;
            this.lblIntro.Text = "The application name you chose does already exist in the online database. Please " +
                "try to use a more specific name.";
            // 
            // lblNewName
            // 
            this.lblNewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(9, 245);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(61, 13);
            this.lblNewName.TabIndex = 4;
            this.lblNewName.Text = "&New name:";
            // 
            // txtNewName
            // 
            this.txtNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewName.Location = new System.Drawing.Point(79, 242);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.Size = new System.Drawing.Size(223, 20);
            this.txtNewName.TabIndex = 5;
            this.txtNewName.TextChanged += new System.EventHandler(this.txtNewName_TextChanged);
            // 
            // SimilarApplicationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 304);
            this.Controls.Add(this.lblIntro);
            this.Controls.Add(this.txtNewName);
            this.Controls.Add(this.lblNewName);
            this.Name = "SimilarApplicationsDialog";
            this.Text = "Similar Applications";
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.lblNewName, 0);
            this.Controls.SetChildIndex(this.txtNewName, 0);
            this.Controls.SetChildIndex(this.lblIntro, 0);
            this.Controls.SetChildIndex(this.lblResults, 0);
            this.Controls.SetChildIndex(this.olvApplications, 0);
            ((System.ComponentModel.ISupportInitialize)(this.olvApplications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblIntro;
        private Label lblNewName;
        private System.Windows.Forms.TextBox txtNewName;
    }
}