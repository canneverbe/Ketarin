namespace Ketarin.Forms
{
    partial class AddCustomColumnDialog
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
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lblColName = new System.Windows.Forms.Label();
            this.txtColumnName = new Ketarin.Forms.TextBox();
            this.lblColValue = new System.Windows.Forms.Label();
            this.txtColumnValue = new System.Windows.Forms.TextBox();
            this.lblExample = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(154, 102);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(235, 102);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // lblColName
            // 
            this.lblColName.AutoSize = true;
            this.lblColName.Location = new System.Drawing.Point(12, 15);
            this.lblColName.Name = "lblColName";
            this.lblColName.Size = new System.Drawing.Size(74, 13);
            this.lblColName.TabIndex = 0;
            this.lblColName.Text = "Column &name:";
            // 
            // txtColumnName
            // 
            this.txtColumnName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtColumnName.Location = new System.Drawing.Point(92, 12);
            this.txtColumnName.Name = "txtColumnName";
            this.txtColumnName.Size = new System.Drawing.Size(218, 20);
            this.txtColumnName.TabIndex = 1;
            // 
            // lblColValue
            // 
            this.lblColValue.AutoSize = true;
            this.lblColValue.Location = new System.Drawing.Point(12, 41);
            this.lblColValue.Name = "lblColValue";
            this.lblColValue.Size = new System.Drawing.Size(74, 13);
            this.lblColValue.TabIndex = 2;
            this.lblColValue.Text = "Column &value:";
            // 
            // txtColumnValue
            // 
            this.txtColumnValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtColumnValue.Location = new System.Drawing.Point(92, 38);
            this.txtColumnValue.Name = "txtColumnValue";
            this.txtColumnValue.Size = new System.Drawing.Size(218, 20);
            this.txtColumnValue.TabIndex = 3;
            this.txtColumnValue.TextChanged += new System.EventHandler(this.txtColumnValue_TextChanged);
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(89, 61);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(155, 13);
            this.lblExample.TabIndex = 6;
            this.lblExample.Text = "Example: {filesize:formatfilesize}";
            // 
            // AddCustomColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 137);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.txtColumnValue);
            this.Controls.Add(this.lblColValue);
            this.Controls.Add(this.txtColumnName);
            this.Controls.Add(this.lblColName);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCustomColumn";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Custom Column";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label lblColName;
        private TextBox txtColumnName;
        private System.Windows.Forms.Label lblColValue;
        private System.Windows.Forms.TextBox txtColumnValue;
        private System.Windows.Forms.Label lblExample;
    }
}