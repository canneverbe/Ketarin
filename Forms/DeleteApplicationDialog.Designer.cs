namespace Ketarin.Forms
{
    partial class DeleteApplicationDialog
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
            this.lblQuestion = new System.Windows.Forms.Label();
            this.bDeleteApplication = new System.Windows.Forms.Button();
            this.bDeleteApplicationAndFile = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.picIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblQuestion
            // 
            this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuestion.Location = new System.Drawing.Point(50, 12);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(328, 32);
            this.lblQuestion.TabIndex = 0;
            this.lblQuestion.Text = "Do you really want to delete the application \'{0}\'?";
            this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bDeleteApplication
            // 
            this.bDeleteApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bDeleteApplication.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bDeleteApplication.Location = new System.Drawing.Point(40, 60);
            this.bDeleteApplication.Name = "bDeleteApplication";
            this.bDeleteApplication.Size = new System.Drawing.Size(104, 23);
            this.bDeleteApplication.TabIndex = 1;
            this.bDeleteApplication.Text = "Delete application";
            this.bDeleteApplication.UseVisualStyleBackColor = true;
            // 
            // bDeleteApplicationAndFile
            // 
            this.bDeleteApplicationAndFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bDeleteApplicationAndFile.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bDeleteApplicationAndFile.Location = new System.Drawing.Point(150, 60);
            this.bDeleteApplicationAndFile.Name = "bDeleteApplicationAndFile";
            this.bDeleteApplicationAndFile.Size = new System.Drawing.Size(147, 23);
            this.bDeleteApplicationAndFile.TabIndex = 2;
            this.bDeleteApplicationAndFile.Text = "&Delete application and file";
            this.bDeleteApplicationAndFile.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(303, 60);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // picIcon
            // 
            this.picIcon.Location = new System.Drawing.Point(12, 12);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(32, 32);
            this.picIcon.TabIndex = 4;
            this.picIcon.TabStop = false;
            // 
            // DeleteApplicationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 95);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bDeleteApplicationAndFile);
            this.Controls.Add(this.bDeleteApplication);
            this.Controls.Add(this.lblQuestion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteApplicationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete application";
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Button bDeleteApplication;
        private System.Windows.Forms.Button bDeleteApplicationAndFile;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.PictureBox picIcon;
    }
}