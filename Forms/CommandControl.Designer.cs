using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;
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
            this.bCommand = new wyDay.Controls.SplitButton();
            this.txtCode = new Scintilla();
            this.mnuPowerShell = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // cmnuCommand
            // 
            this.cmnuCommand.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuBatchScript,
            this.mnuCSScript,
            this.mnuPowerShell,
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
            this.sepRun.Index = 3;
            this.sepRun.Text = "-";
            // 
            // mnuValidate
            // 
            this.mnuValidate.Index = 4;
            this.mnuValidate.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
            this.mnuValidate.Text = "&Validate";
            this.mnuValidate.Click += new System.EventHandler(this.mnuValidate_Click);
            // 
            // mnuRun
            // 
            this.mnuRun.Index = 5;
            this.mnuRun.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.mnuRun.Text = "&Run";
            this.mnuRun.Click += new System.EventHandler(this.mnuRun_Click);
            // 
            // sepSnippets
            // 
            this.sepSnippets.Index = 6;
            this.sepSnippets.Text = "-";
            // 
            // mnuInsertSnippet
            // 
            this.mnuInsertSnippet.Index = 7;
            this.mnuInsertSnippet.Text = "I&nsert snippet";
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Index = 8;
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
            this.mnuDeleteSnippet.Index = 9;
            this.mnuDeleteSnippet.Text = "&Delete snippet";
            // 
            // sepDefaultCommands
            // 
            this.sepDefaultCommands.Index = 10;
            this.sepDefaultCommands.Text = "-";
            // 
            // mnuUndo
            // 
            this.mnuUndo.Index = 11;
            this.mnuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndo.Text = "&Undo";
            this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
            // 
            // mnuRedo
            // 
            this.mnuRedo.Index = 12;
            this.mnuRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedo.Text = "R&edo";
            this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
            // 
            // sepClipboard
            // 
            this.sepClipboard.Index = 13;
            this.sepClipboard.Text = "-";
            // 
            // mnuCut
            // 
            this.mnuCut.Index = 14;
            this.mnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuCut.Text = "Cu&t";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Index = 15;
            this.mnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Index = 16;
            this.mnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Index = 17;
            this.mnuClear.Text = "Cle&ar";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // sepSelection
            // 
            this.sepSelection.Index = 18;
            this.sepSelection.Text = "-";
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Index = 19;
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
            this.txtCode.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.txtCode.Location = new Point(1, 1);
            this.txtCode.Name = "txtCode";
            this.txtCode.Margins[0].Width = 17;
            this.txtCode.Size = new Size(514, 192);
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
            // mnuPowerShell
            // 
            this.mnuPowerShell.Index = 2;
            this.mnuPowerShell.Text = "&PowerShell script";
            this.mnuPowerShell.Click += new System.EventHandler(this.mnuPowerShell_Click);
            // 
            // CommandControl
            // 
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.bCommand);
            this.Controls.Add(this.txtBorder);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CommandControl";
            this.Size = new System.Drawing.Size(517, 223);
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
        private MenuItem mnuPowerShell;
    }
}
