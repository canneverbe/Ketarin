namespace Ketarin.Forms
{
    partial class CustomSetupInstructionDialog
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
            this.txtCode = new ScintillaNet.Scintilla();
            this.bTestScript = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(503, 286);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(422, 286);
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.ConfigurationManager.Language = "cs";
            this.txtCode.IsBraceMatching = true;
            this.txtCode.Location = new System.Drawing.Point(0, 0);
            this.txtCode.Margins.Margin0.Width = 25;
            this.txtCode.Margins.Margin1.Width = 0;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(589, 261);
            this.txtCode.Styles.BraceBad.FontName = "Verdana";
            this.txtCode.Styles.BraceLight.FontName = "Verdana";
            this.txtCode.Styles.ControlChar.FontName = "Verdana";
            this.txtCode.Styles.Default.FontName = "Verdana";
            this.txtCode.Styles.IndentGuide.FontName = "Verdana";
            this.txtCode.Styles.LastPredefined.FontName = "Verdana";
            this.txtCode.Styles.LineNumber.FontName = "Verdana";
            this.txtCode.Styles.Max.FontName = "Verdana";
            this.txtCode.TabIndex = 0;
            // 
            // bTestScript
            // 
            this.bTestScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bTestScript.Location = new System.Drawing.Point(12, 286);
            this.bTestScript.Name = "bTestScript";
            this.bTestScript.Size = new System.Drawing.Size(91, 23);
            this.bTestScript.TabIndex = 1;
            this.bTestScript.Text = "Validate script";
            this.bTestScript.UseVisualStyleBackColor = true;
            this.bTestScript.Click += new System.EventHandler(this.bTestScript_Click);
            // 
            // CustomSetupInstructionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 321);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.bTestScript);
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "CustomSetupInstructionDialog";
            this.Text = "Custom Setup Instruction";
            this.Controls.SetChildIndex(this.bTestScript, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNet.Scintilla txtCode;
        private System.Windows.Forms.Button bTestScript;
    }
}