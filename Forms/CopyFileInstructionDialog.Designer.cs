namespace Ketarin.Forms
{
    partial class CopyFileInstructionDialog
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
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSource = new Ketarin.Forms.VariableTextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.txtTarget = new Ketarin.Forms.VariableTextBox();
            this.bInsertArgument = new wyDay.Controls.SplitButton();
            this.environmentMenu = new System.Windows.Forms.ContextMenu();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(299, 89);
            this.bCancel.TabIndex = 6;
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(218, 89);
            this.bOK.TabIndex = 5;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 15);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(60, 13);
            this.lblSource.TabIndex = 0;
            this.lblSource.Text = "&Source file:";
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(78, 12);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(296, 20);
            this.txtSource.TabIndex = 1;
            this.txtSource.Text = "{file}";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(12, 41);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(57, 13);
            this.lblTarget.TabIndex = 2;
            this.lblTarget.Text = "&Target file:";
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.Location = new System.Drawing.Point(78, 38);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(198, 20);
            this.txtTarget.TabIndex = 3;
            // 
            // bInsertArgument
            // 
            this.bInsertArgument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bInsertArgument.AutoSize = true;
            this.bInsertArgument.Location = new System.Drawing.Point(280, 36);
            this.bInsertArgument.Name = "bInsertArgument";
            this.bInsertArgument.SeparateDropdownButton = false;
            this.bInsertArgument.Size = new System.Drawing.Size(94, 23);
            this.bInsertArgument.SplitMenu = this.environmentMenu;
            this.bInsertArgument.TabIndex = 4;
            this.bInsertArgument.Text = "&Environment";
            this.bInsertArgument.UseVisualStyleBackColor = true;
            // 
            // CopyFileInstructionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 124);
            this.Controls.Add(this.bInsertArgument);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.txtSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CopyFileInstructionDialog";
            this.Text = "Copy File";
            this.Controls.SetChildIndex(this.txtSource, 0);
            this.Controls.SetChildIndex(this.lblSource, 0);
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.txtTarget, 0);
            this.Controls.SetChildIndex(this.lblTarget, 0);
            this.Controls.SetChildIndex(this.bInsertArgument, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private VariableTextBox txtSource;
        private System.Windows.Forms.Label lblTarget;
        private VariableTextBox txtTarget;
        private wyDay.Controls.SplitButton bInsertArgument;
        private System.Windows.Forms.ContextMenu environmentMenu;
    }
}