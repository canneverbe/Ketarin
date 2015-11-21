using System.ComponentModel;

namespace Ketarin.Forms
{
    partial class CustomSetupInstructionDialog
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
            this.commandControl = new Ketarin.Forms.CommandControl();
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
            // commandControl
            // 
            this.commandControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commandControl.Application = null;
            this.commandControl.CommandType = Ketarin.ScriptType.Batch;
            this.commandControl.IndentButton = 12;
            this.commandControl.Location = new System.Drawing.Point(0, 0);
            this.commandControl.Margin = new System.Windows.Forms.Padding(0);
            this.commandControl.Name = "commandControl";
            this.commandControl.ReadOnly = false;
            this.commandControl.ShowBorder = false;
            this.commandControl.Size = new System.Drawing.Size(590, 283);
            this.commandControl.TabIndex = 4;
            this.commandControl.VariableNames = new string[0];
            // 
            // CustomSetupInstructionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 321);
            this.Controls.Add(this.commandControl);
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "CustomSetupInstructionDialog";
            this.Text = "Custom Setup Instruction";
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.commandControl, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private CommandControl commandControl;

    }
}