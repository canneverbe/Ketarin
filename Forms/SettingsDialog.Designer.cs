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
            this.txtCustomColumn = new System.Windows.Forms.TextBox();
            this.lblCustomColumn = new System.Windows.Forms.Label();
            this.chkAvoidBeta = new System.Windows.Forms.CheckBox();
            this.lblConnectionTimeout = new System.Windows.Forms.Label();
            this.nConnectionTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.sepProxy = new CDBurnerXP.Controls.Separator();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtProxyServer = new System.Windows.Forms.TextBox();
            this.nProxyPort = new System.Windows.Forms.NumericUpDown();
            this.lblProxyUser = new System.Windows.Forms.Label();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(283, 334);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 22;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(202, 334);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 21;
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
            this.lblDefaultCommand.Location = new System.Drawing.Point(9, 60);
            this.lblDefaultCommand.Name = "lblDefaultCommand";
            this.lblDefaultCommand.Size = new System.Drawing.Size(283, 13);
            this.lblDefaultCommand.TabIndex = 2;
            this.lblDefaultCommand.Text = "Default &command to execute after updating an application:";
            // 
            // txtDefaultCommand
            // 
            this.txtDefaultCommand.AcceptsReturn = true;
            this.txtDefaultCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultCommand.Location = new System.Drawing.Point(12, 76);
            this.txtDefaultCommand.Multiline = true;
            this.txtDefaultCommand.Name = "txtDefaultCommand";
            this.txtDefaultCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDefaultCommand.Size = new System.Drawing.Size(346, 73);
            this.txtDefaultCommand.TabIndex = 3;
            this.txtDefaultCommand.WordWrap = false;
            // 
            // txtCustomColumn
            // 
            this.txtCustomColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomColumn.Location = new System.Drawing.Point(155, 155);
            this.txtCustomColumn.Name = "txtCustomColumn";
            this.txtCustomColumn.Size = new System.Drawing.Size(203, 20);
            this.txtCustomColumn.TabIndex = 5;
            // 
            // lblCustomColumn
            // 
            this.lblCustomColumn.AutoSize = true;
            this.lblCustomColumn.Location = new System.Drawing.Point(12, 158);
            this.lblCustomColumn.Name = "lblCustomColumn";
            this.lblCustomColumn.Size = new System.Drawing.Size(137, 13);
            this.lblCustomColumn.TabIndex = 4;
            this.lblCustomColumn.Text = "&Variable for custom column:";
            // 
            // chkAvoidBeta
            // 
            this.chkAvoidBeta.AutoSize = true;
            this.chkAvoidBeta.Location = new System.Drawing.Point(12, 35);
            this.chkAvoidBeta.Name = "chkAvoidBeta";
            this.chkAvoidBeta.Size = new System.Drawing.Size(181, 17);
            this.chkAvoidBeta.TabIndex = 1;
            this.chkAvoidBeta.Text = "Avoid &beta versions on FileHippo";
            this.chkAvoidBeta.UseVisualStyleBackColor = true;
            // 
            // lblConnectionTimeout
            // 
            this.lblConnectionTimeout.AutoSize = true;
            this.lblConnectionTimeout.Location = new System.Drawing.Point(12, 183);
            this.lblConnectionTimeout.Name = "lblConnectionTimeout";
            this.lblConnectionTimeout.Size = new System.Drawing.Size(101, 13);
            this.lblConnectionTimeout.TabIndex = 6;
            this.lblConnectionTimeout.Text = "&Connection timeout:";
            // 
            // nConnectionTimeout
            // 
            this.nConnectionTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nConnectionTimeout.Location = new System.Drawing.Point(155, 181);
            this.nConnectionTimeout.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nConnectionTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nConnectionTimeout.Name = "nConnectionTimeout";
            this.nConnectionTimeout.Size = new System.Drawing.Size(152, 20);
            this.nConnectionTimeout.TabIndex = 7;
            this.nConnectionTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(313, 183);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 12;
            this.lblSeconds.Text = "seconds";
            // 
            // sepProxy
            // 
            this.sepProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sepProxy.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.sepProxy.Location = new System.Drawing.Point(9, 215);
            this.sepProxy.Name = "sepProxy";
            this.sepProxy.Size = new System.Drawing.Size(351, 23);
            this.sepProxy.TabIndex = 13;
            this.sepProxy.Text = "HTTP proxy settings";
            this.sepProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(21, 246);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 14;
            this.lblServer.Text = "&Server:";
            // 
            // txtProxyServer
            // 
            this.txtProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyServer.Location = new System.Drawing.Point(88, 243);
            this.txtProxyServer.Name = "txtProxyServer";
            this.txtProxyServer.Size = new System.Drawing.Size(189, 20);
            this.txtProxyServer.TabIndex = 15;
            // 
            // nProxyPort
            // 
            this.nProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nProxyPort.Location = new System.Drawing.Point(283, 243);
            this.nProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nProxyPort.Name = "nProxyPort";
            this.nProxyPort.Size = new System.Drawing.Size(77, 20);
            this.nProxyPort.TabIndex = 16;
            // 
            // lblProxyUser
            // 
            this.lblProxyUser.AutoSize = true;
            this.lblProxyUser.Location = new System.Drawing.Point(21, 272);
            this.lblProxyUser.Name = "lblProxyUser";
            this.lblProxyUser.Size = new System.Drawing.Size(61, 13);
            this.lblProxyUser.TabIndex = 17;
            this.lblProxyUser.Text = "Us&er name:";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyUser.Location = new System.Drawing.Point(88, 269);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(272, 20);
            this.txtProxyUser.TabIndex = 18;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.AutoSize = true;
            this.lblProxyPassword.Location = new System.Drawing.Point(21, 298);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.lblProxyPassword.TabIndex = 19;
            this.lblProxyPassword.Text = "&Password:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPassword.Location = new System.Drawing.Point(88, 295);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(272, 20);
            this.txtProxyPassword.TabIndex = 20;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 369);
            this.Controls.Add(this.txtProxyPassword);
            this.Controls.Add(this.lblProxyPassword);
            this.Controls.Add(this.txtProxyUser);
            this.Controls.Add(this.lblProxyUser);
            this.Controls.Add(this.nProxyPort);
            this.Controls.Add(this.txtProxyServer);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.sepProxy);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.nConnectionTimeout);
            this.Controls.Add(this.lblConnectionTimeout);
            this.Controls.Add(this.chkAvoidBeta);
            this.Controls.Add(this.lblCustomColumn);
            this.Controls.Add(this.txtCustomColumn);
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
            ((System.ComponentModel.ISupportInitialize)(this.nConnectionTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nProxyPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox chkUpdateAtStartup;
        private System.Windows.Forms.Label lblDefaultCommand;
        private System.Windows.Forms.TextBox txtDefaultCommand;
        private System.Windows.Forms.TextBox txtCustomColumn;
        private System.Windows.Forms.Label lblCustomColumn;
        private System.Windows.Forms.CheckBox chkAvoidBeta;
        private System.Windows.Forms.Label lblConnectionTimeout;
        private System.Windows.Forms.NumericUpDown nConnectionTimeout;
        private System.Windows.Forms.Label lblSeconds;
        private CDBurnerXP.Controls.Separator sepProxy;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtProxyServer;
        private System.Windows.Forms.NumericUpDown nProxyPort;
        private System.Windows.Forms.Label lblProxyUser;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label lblProxyPassword;
        private System.Windows.Forms.TextBox txtProxyPassword;
    }
}