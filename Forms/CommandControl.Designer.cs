using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNet;
using wyDay.Controls;

namespace Ketarin.Forms
{
    partial class CommandControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;

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
            this.cmnuCommand = new ContextMenu();
            this.mnuBatchScript = new MenuItem();
            this.mnuCSScript = new MenuItem();
            this.sepRun = new MenuItem();
            this.mnuValidate = new MenuItem();
            this.mnuRun = new MenuItem();
            this.sepSnippets = new MenuItem();
            this.mnuInsertSnippet = new MenuItem();
            this.mnuSaveAs = new MenuItem();
            this.mnuNewScript = new MenuItem();
            this.sepSaveAs = new MenuItem();
            this.mnuDeleteSnippet = new MenuItem();
            this.sepDefaultCommands = new MenuItem();
            this.mnuUndo = new MenuItem();
            this.mnuRedo = new MenuItem();
            this.sepClipboard = new MenuItem();
            this.mnuCut = new MenuItem();
            this.mnuCopy = new MenuItem();
            this.mnuPaste = new MenuItem();
            this.mnuClear = new MenuItem();
            this.sepSelection = new MenuItem();
            this.mnuSelectAll = new MenuItem();
            this.txtBorder = new System.Windows.Forms.TextBox();
            this.txtCode = new Scintilla();
            this.bCommand = new SplitButton();
            ((ISupportInitialize)(this.txtCode)).BeginInit();
            this.SuspendLayout();
            // 
            // cmnuCommand
            // 
            this.cmnuCommand.MenuItems.AddRange(new MenuItem[] {
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
            this.cmnuCommand.Popup += new EventHandler(this.cmnuCommand_Popup);
            // 
            // mnuBatchScript
            // 
            this.mnuBatchScript.Checked = true;
            this.mnuBatchScript.Index = 0;
            this.mnuBatchScript.RadioCheck = true;
            this.mnuBatchScript.Text = "&Batch script";
            this.mnuBatchScript.Click += new EventHandler(this.mnuBatchScript_Click);
            // 
            // mnuCSScript
            // 
            this.mnuCSScript.Index = 1;
            this.mnuCSScript.RadioCheck = true;
            this.mnuCSScript.Text = "C&# script";
            this.mnuCSScript.Click += new EventHandler(this.mnuCSScript_Click);
            // 
            // sepRun
            // 
            this.sepRun.Index = 2;
            this.sepRun.Text = "-";
            // 
            // mnuValidate
            // 
            this.mnuValidate.Index = 3;
            this.mnuValidate.Shortcut = Shortcut.CtrlB;
            this.mnuValidate.Text = "&Validate";
            this.mnuValidate.Click += new EventHandler(this.mnuValidate_Click);
            // 
            // mnuRun
            // 
            this.mnuRun.Index = 4;
            this.mnuRun.Shortcut = Shortcut.CtrlR;
            this.mnuRun.Text = "&Run";
            this.mnuRun.Click += new EventHandler(this.mnuRun_Click);
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
            this.mnuSaveAs.MenuItems.AddRange(new MenuItem[] {
            this.mnuNewScript,
            this.sepSaveAs});
            this.mnuSaveAs.Text = "&Save as";
            // 
            // mnuNewScript
            // 
            this.mnuNewScript.Index = 0;
            this.mnuNewScript.Text = "&New...";
            this.mnuNewScript.Click += new EventHandler(this.mnuNewScript_Click);
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
            this.mnuUndo.Shortcut = Shortcut.CtrlZ;
            this.mnuUndo.Text = "&Undo";
            this.mnuUndo.Click += new EventHandler(this.mnuUndo_Click);
            // 
            // mnuRedo
            // 
            this.mnuRedo.Index = 11;
            this.mnuRedo.Shortcut = Shortcut.CtrlY;
            this.mnuRedo.Text = "R&edo";
            this.mnuRedo.Click += new EventHandler(this.mnuRedo_Click);
            // 
            // sepClipboard
            // 
            this.sepClipboard.Index = 12;
            this.sepClipboard.Text = "-";
            // 
            // mnuCut
            // 
            this.mnuCut.Index = 13;
            this.mnuCut.Shortcut = Shortcut.CtrlX;
            this.mnuCut.Text = "Cu&t";
            this.mnuCut.Click += new EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Index = 14;
            this.mnuCopy.Shortcut = Shortcut.CtrlC;
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Index = 15;
            this.mnuPaste.Shortcut = Shortcut.CtrlV;
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new EventHandler(this.mnuPaste_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Index = 16;
            this.mnuClear.Text = "Cle&ar";
            this.mnuClear.Click += new EventHandler(this.mnuClear_Click);
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
            this.mnuSelectAll.Click += new EventHandler(this.mnuSelectAll_Click);
            // 
            // txtBorder
            // 
            this.txtBorder.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.txtBorder.Location = new Point(0, 0);
            this.txtBorder.Multiline = true;
            this.txtBorder.Name = "txtBorder";
            this.txtBorder.ReadOnly = true;
            this.txtBorder.Size = new Size(517, 194);
            this.txtBorder.TabIndex = 2;
            this.txtBorder.TabStop = false;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.txtCode.Location = new Point(1, 1);
            this.txtCode.Margins.Margin0.Width = 25;
            this.txtCode.Margins.Margin1.Width = 0;
            this.txtCode.Margins.Right = 0;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new Size(514, 192);
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
            this.bCommand.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.bCommand.AutoSize = true;
            this.bCommand.Location = new Point(0, 200);
            this.bCommand.Name = "bCommand";
            this.bCommand.SeparateDropdownButton = false;
            this.bCommand.Size = new Size(82, 23);
            this.bCommand.SplitMenu = this.cmnuCommand;
            this.bCommand.TabIndex = 1;
            this.bCommand.Text = "&Command";
            this.bCommand.UseVisualStyleBackColor = true;
            // 
            // CommandControl
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.bCommand);
            this.Controls.Add(this.txtBorder);
            this.Margin = new Padding(0);
            this.Name = "CommandControl";
            this.Size = new Size(517, 223);
            ((ISupportInitialize)(this.txtCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitButton bCommand;
        private ContextMenu cmnuCommand;
        private MenuItem mnuBatchScript;
        private MenuItem mnuCSScript;
        private MenuItem sepRun;
        private MenuItem mnuValidate;
        private MenuItem mnuRun;
        private MenuItem sepSnippets;
        private MenuItem mnuInsertSnippet;
        private MenuItem mnuSaveAs;
        private MenuItem mnuNewScript;
        private MenuItem sepSaveAs;
        private MenuItem mnuDeleteSnippet;
        private Scintilla txtCode;
        private MenuItem sepDefaultCommands;
        private MenuItem mnuUndo;
        private MenuItem mnuRedo;
        private MenuItem sepClipboard;
        private MenuItem mnuCut;
        private MenuItem mnuCopy;
        private MenuItem mnuPaste;
        private MenuItem mnuClear;
        private MenuItem sepSelection;
        private MenuItem mnuSelectAll;
        private System.Windows.Forms.TextBox txtBorder;
    }
}
