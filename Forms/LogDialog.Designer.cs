using System.ComponentModel;

namespace Ketarin.Forms
{
    partial class LogDialog
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
            this.txtLog = new Ketarin.Forms.TextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuClearLog = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.White;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(464, 264);
            this.txtLog.TabIndex = 1;
            this.txtLog.WordWrap = false;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuClearLog});
            // 
            // mnuClearLog
            // 
            this.mnuClearLog.Index = 0;
            this.mnuClearLog.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
            this.mnuClearLog.Text = "&Clear log";
            this.mnuClearLog.Click += new System.EventHandler(this.mnuClearLog_Click);
            // 
            // LogDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 264);
            this.Controls.Add(this.txtLog);
            this.MinimizeBox = false;
            this.Name = "LogDialog";
            this.SavePosition = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtLog;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem mnuClearLog;
    }
}