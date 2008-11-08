namespace Ketarin.Forms
{
    partial class SettingsDialog
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.chkUpdateAtStartup = new System.Windows.Forms.CheckBox();
            this.lblDefaultCommand = new System.Windows.Forms.Label();
            this.txtDefaultCommand = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(283, 95);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 11;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(202, 95);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 10;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // chkUpdateAtStartup
            // 
            this.chkUpdateAtStartup.AutoSize = true;
            this.chkUpdateAtStartup.Location = new System.Drawing.Point(12, 12);
            this.chkUpdateAtStartup.Name = "chkUpdateAtStartup";
            this.chkUpdateAtStartup.Size = new System.Drawing.Size(172, 17);
            this.chkUpdateAtStartup.TabIndex = 0;
            this.chkUpdateAtStartup.Text = "&Update automatically at startup";
            this.chkUpdateAtStartup.UseVisualStyleBackColor = true;
            // 
            // lblDefaultCommand
            // 
            this.lblDefaultCommand.AutoSize = true;
            this.lblDefaultCommand.Location = new System.Drawing.Point(9, 37);
            this.lblDefaultCommand.Name = "lblDefaultCommand";
            this.lblDefaultCommand.Size = new System.Drawing.Size(283, 13);
            this.lblDefaultCommand.TabIndex = 1;
            this.lblDefaultCommand.Text = "Default &command to execute after updating an application:";
            // 
            // txtDefaultCommand
            // 
            this.txtDefaultCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultCommand.Location = new System.Drawing.Point(12, 53);
            this.txtDefaultCommand.Name = "txtDefaultCommand";
            this.txtDefaultCommand.Size = new System.Drawing.Size(346, 20);
            this.txtDefaultCommand.TabIndex = 2;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 130);
            this.Controls.Add(this.txtDefaultCommand);
            this.Controls.Add(this.lblDefaultCommand);
            this.Controls.Add(this.chkUpdateAtStartup);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox chkUpdateAtStartup;
        private System.Windows.Forms.Label lblDefaultCommand;
        private System.Windows.Forms.TextBox txtDefaultCommand;
    }
}