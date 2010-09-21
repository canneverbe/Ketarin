namespace Ketarin.Forms
{
    partial class CommandControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmnuCommand = new System.Windows.Forms.ContextMenu();
            this.mnuBatchScript = new System.Windows.Forms.MenuItem();
            this.mnuCSScript = new System.Windows.Forms.MenuItem();
            this.sepRun = new System.Windows.Forms.MenuItem();
            this.mnuValidate = new System.Windows.Forms.MenuItem();
            this.mnuRun = new System.Windows.Forms.MenuItem();
            this.sepSnippets = new System.Windows.Forms.MenuItem();
            this.mnuInsertSnippet = new System.Windows.Forms.MenuItem();
            this.mnuSaveAs = new System.Windows.Forms.MenuItem();
            this.mnuNewScript = new System.Windows.Forms.MenuItem();
            this.sepSaveAs = new System.Windows.Forms.MenuItem();
            this.mnuDeleteSnippet = new System.Windows.Forms.MenuItem();
            this.sepDefaultCommands = new System.Windows.Forms.MenuItem();
            this.mnuUndo = new System.Windows.Forms.MenuItem();
            this.mnuRedo = new System.Windows.Forms.MenuItem();
            this.sepClipboard = new System.Windows.Forms.MenuItem();
            this.mnuCut = new System.Windows.Forms.MenuItem();
            this.mnuCopy = new System.Windows.Forms.MenuItem();
            this.mnuPaste = new System.Windows.Forms.MenuItem();
            this.mnuClear = new System.Windows.Forms.MenuItem();
            this.sepSelection = new System.Windows.Forms.MenuItem();
            this.mnuSelectAll = new System.Windows.Forms.MenuItem();
            this.txtBorder = new System.Windows.Forms.TextBox();
            this.txtCode = new ScintillaNet.Scintilla();
            this.bCommand = new wyDay.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            this.SuspendLayout();
            // 
            // cmnuCommand
            // 
            this.cmnuCommand.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuBatchScript,
            this.mnuCSScript,
            this.sepRun,
            this.mnuValidate,
            this.mnuRun,
            this.sepSnippets,
            this.mnuInsertSnippet,
            this.mnuSaveAs,
            this.mnuDeleteSnippet,
            this.sepDefaultCommands,
            this.mnuUndo,
            this.mnuRedo,
            this.sepClipboard,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuClear,
            this.sepSelection,
            this.mnuSelectAll});
            this.cmnuCommand.Popup += new System.EventHandler(this.cmnuCommand_Popup);
            // 
            // mnuBatchScript
            // 
            this.mnuBatchScript.Checked = true;
            this.mnuBatchScript.Index = 0;
            this.mnuBatchScript.RadioCheck = true;
            this.mnuBatchScript.Text = "&Batch script";
            this.mnuBatchScript.Click += new System.EventHandler(this.mnuBatchScript_Click);
            // 
            // mnuCSScript
            // 
            this.mnuCSScript.Index = 1;
            this.mnuCSScript.RadioCheck = true;
            this.mnuCSScript.Text = "C&# script";
            this.mnuCSScript.Click += new System.EventHandler(this.mnuCSScript_Click);
            // 
            // sepRun
            // 
            this.sepRun.Index = 2;
            this.sepRun.Text = "-";
            // 
            // mnuValidate
            // 
            this.mnuValidate.Index = 3;
            this.mnuValidate.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
            this.mnuValidate.Text = "&Validate";
            this.mnuValidate.Click += new System.EventHandler(this.mnuValidate_Click);
            // 
            // mnuRun
            // 
            this.mnuRun.Index = 4;
            this.mnuRun.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.mnuRun.Text = "&Run";
            this.mnuRun.Click += new System.EventHandler(this.mnuRun_Click);
            // 
            // sepSnippets
            // 
            this.sepSnippets.Index = 5;
            this.sepSnippets.Text = "-";
            // 
            // mnuInsertSnippet
            // 
            this.mnuInsertSnippet.Index = 6;
            this.mnuInsertSnippet.Text = "I&nsert snippet";
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Index = 7;
            this.mnuSaveAs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNewScript,
            this.sepSaveAs});
            this.mnuSaveAs.Text = "&Save as";
            // 
            // mnuNewScript
            // 
            this.mnuNewScript.Index = 0;
            this.mnuNewScript.Text = "&New...";
            this.mnuNewScript.Click += new System.EventHandler(this.mnuNewScript_Click);
            // 
            // sepSaveAs
            // 
            this.sepSaveAs.Index = 1;
            this.sepSaveAs.Text = "-";
            // 
            // mnuDeleteSnippet
            // 
            this.mnuDeleteSnippet.Index = 8;
            this.mnuDeleteSnippet.Text = "&Delete snippet";
            // 
            // sepDefaultCommands
            // 
            this.sepDefaultCommands.Index = 9;
            this.sepDefaultCommands.Text = "-";
            // 
            // mnuUndo
            // 
            this.mnuUndo.Index = 10;
            this.mnuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndo.Text = "&Undo";
            this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
            // 
            // mnuRedo
            // 
            this.mnuRedo.Index = 11;
            this.mnuRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedo.Text = "R&edo";
            this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
            // 
            // sepClipboard
            // 
            this.sepClipboard.Index = 12;
            this.sepClipboard.Text = "-";
            // 
            // mnuCut
            // 
            this.mnuCut.Index = 13;
            this.mnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuCut.Text = "Cu&t";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Index = 14;
            this.mnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Index = 15;
            this.mnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Index = 16;
            this.mnuClear.Text = "Cle&ar";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // sepSelection
            // 
            this.sepSelection.Index = 17;
            this.sepSelection.Text = "-";
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Index = 18;
            this.mnuSelectAll.Text = "Se&lect all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // txtBorder
            // 
            this.txtBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBorder.Location = new System.Drawing.Point(0, 0);
            this.txtBorder.Multiline = true;
            this.txtBorder.Name = "txtBorder";
            this.txtBorder.ReadOnly = true;
            this.txtBorder.Size = new System.Drawing.Size(517, 194);
            this.txtBorder.TabIndex = 2;
            this.txtBorder.TabStop = false;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.Location = new System.Drawing.Point(1, 1);
            this.txtCode.Margins.Margin0.Width = 25;
            this.txtCode.Margins.Margin1.Width = 0;
            this.txtCode.Margins.Right = 0;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(514, 192);
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
            // bCommand
            // 
            this.bCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCommand.AutoSize = true;
            this.bCommand.Location = new System.Drawing.Point(0, 200);
            this.bCommand.Name = "bCommand";
            this.bCommand.SeparateDropdownButton = false;
            this.bCommand.Size = new System.Drawing.Size(82, 23);
            this.bCommand.SplitMenu = this.cmnuCommand;
            this.bCommand.TabIndex = 1;
            this.bCommand.Text = "&Command";
            this.bCommand.UseVisualStyleBackColor = true;
            // 
            // CommandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.bCommand);
            this.Controls.Add(this.txtBorder);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CommandControl";
            this.Size = new System.Drawing.Size(517, 223);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private wyDay.Controls.SplitButton bCommand;
        private System.Windows.Forms.ContextMenu cmnuCommand;
        private System.Windows.Forms.MenuItem mnuBatchScript;
        private System.Windows.Forms.MenuItem mnuCSScript;
        private System.Windows.Forms.MenuItem sepRun;
        private System.Windows.Forms.MenuItem mnuValidate;
        private System.Windows.Forms.MenuItem mnuRun;
        private System.Windows.Forms.MenuItem sepSnippets;
        private System.Windows.Forms.MenuItem mnuInsertSnippet;
        private System.Windows.Forms.MenuItem mnuSaveAs;
        private System.Windows.Forms.MenuItem mnuNewScript;
        private System.Windows.Forms.MenuItem sepSaveAs;
        private System.Windows.Forms.MenuItem mnuDeleteSnippet;
        private ScintillaNet.Scintilla txtCode;
        private System.Windows.Forms.MenuItem sepDefaultCommands;
        private System.Windows.Forms.MenuItem mnuUndo;
        private System.Windows.Forms.MenuItem mnuRedo;
        private System.Windows.Forms.MenuItem sepClipboard;
        private System.Windows.Forms.MenuItem mnuCut;
        private System.Windows.Forms.MenuItem mnuCopy;
        private System.Windows.Forms.MenuItem mnuPaste;
        private System.Windows.Forms.MenuItem mnuClear;
        private System.Windows.Forms.MenuItem sepSelection;
        private System.Windows.Forms.MenuItem mnuSelectAll;
        private System.Windows.Forms.TextBox txtBorder;
    }
}
