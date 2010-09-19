using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace Ketarin.Forms
{
    /// <summary>
    /// Allows editing of either batch or C# commands.
    /// </summary>
    public partial class CommandControl : UserControl
    {
        private const string csSample = @"/*
  Enter a custom C# script here. C# is case sensitive.
  ""app"" references the current application.
  Example:
  MessageBox.Show(app.Name);
  
  = Notable methods =
  app.PreviousLocation
    Corresponds to the variable {file}

  app.Variables.ReplaceAllInString(""Any {text} with variables."")
    Replaces all known variables in a given string.
    Example: string new = app.Variables.ReplaceAllInString(""{file}"")

  return;
    Exits the script.

  Abort(""Error text"");
    Exits the script with a given error.
*/";

        private string[] variableNames = new string[0];

        #region Properties

        /// <summary>
        /// Gets or sets if a border should be displayed around the text control.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowBorder
        {
            get
            {
                return txtBorder.Visible;
            }
            set
            {
                if (value != txtCode.Visible)
                {
                    if (value)
                    {
                        txtCode.Bounds = new Rectangle(txtCode.Left + 1, txtCode.Top + 1, txtCode.Width - 2, txtCode.Height - 2);
                    }
                    else
                    {
                        txtCode.Bounds = new Rectangle(txtCode.Left - 1, txtCode.Top - 1, txtCode.Width + 2, txtCode.Height + 2);
                    }
                }
                txtBorder.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the control is in read only mode.
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                txtCode.IsReadOnly = value;
                bCommand.Enabled = !value;
            }
            get
            {
                return txtCode.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the currently exiting variables for the current application.
        /// </summary>
        public string[] VariableNames
        {
            get { return this.variableNames; }
            set
            {
                this.variableNames = value;

                // Remove old menu items
                for (int i = cmnuCommand.MenuItems.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(cmnuCommand.MenuItems[i].Tag as string))
                    {
                        cmnuCommand.MenuItems.RemoveAt(i);
                    }
                }

                // Add necessary menu items
                if (this.VariableNames != null && this.VariableNames.Length > 0)
                {
                    MenuItem varSeparator = new MenuItem("-");
                    varSeparator.Tag = "VarSeparator";
                    cmnuCommand.MenuItems.Add(0,varSeparator);

                    for (int i = this.VariableNames.Length - 1; i >= 0; i--)
                    {
                        MenuItem varItem = new MenuItem("{" + VariableNames[i] + "}", delegate(object sender, EventArgs ev)
                        {
                            txtCode.InsertText(((MenuItem)sender).Text);
                        });
                        varItem.Tag = VariableNames[i];
                        cmnuCommand.MenuItems.Add(0, varItem);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently edited application.
        /// </summary>
        public ApplicationJob Application
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to indent the button.
        /// </summary>
        [DefaultValue(0)]
        public int IndentButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of command.
        /// </summary>
        public ScriptType CommandType
        {
            get
            {
                if (mnuCSScript.Checked) return ScriptType.CS;
                return ScriptType.Batch;
            }
            set
            {
                switch (value)
                {
                    case ScriptType.CS:
                        mnuBatchScript.Checked = false;
                        mnuCSScript.Checked = true;
                        mnuValidate.Enabled = true;
                        txtCode.ConfigurationManager.Language = "cs";
                        if (string.IsNullOrEmpty(txtCode.Text))
                        {
                            txtCode.Text = csSample;
                        }
                        break;

                    default:
                        mnuBatchScript.Checked = true;
                        mnuCSScript.Checked = false;
                        mnuValidate.Enabled = false;
                        txtCode.ConfigurationManager.Language = "batch";

                        if (txtCode.Text == csSample)
                        {
                            txtCode.Text = string.Empty;
                        }
                        break;
                }
                LoadSnippets();
            }
        }

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        public string CommandText
        {
            get
            {
                return txtCode.Text;
            }
            set
            {
                txtCode.Text = value;
            }
        }

        #endregion

        public CommandControl()
        {
            InitializeComponent();
            CommandType = ScriptType.Batch;
            this.txtCode.ContextMenu = cmnuCommand;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) return;

            base.OnLoad(e);
            
            bCommand.Left += IndentButton;
            LoadSnippets();
        }

        /// <summary>
        /// Shortcut function to add multiple collection of variable
        /// names at once.
        /// </summary>
        public void SetVariableNames(params string[][] variableNames)
        {
            List<string> varNames = new List<string>();

            foreach (string[] names in variableNames)
            {
                varNames.AddRange(names);
            }

            VariableNames = varNames.ToArray();
        }

        private void LoadSnippets()
        {
            mnuInsertSnippet.MenuItems.Clear();
            mnuDeleteSnippet.MenuItems.Clear();
            txtCode.Snippets.List.Clear();

            while (mnuSaveAs.MenuItems.Count > 2)
            {
                mnuSaveAs.MenuItems.RemoveAt(2);
            }

            Snippet[] snippets = DbManager.GetSnippets();
            foreach (Snippet snippet in snippets)
            {
                ScintillaNet.Snippet sciteSnippet = new ScintillaNet.Snippet(snippet.Name.Replace(" ", "-"), snippet.Text);
                sciteSnippet.Languages.Add(snippet.Type == ScriptType.Batch ? "batch" : "cs");
                txtCode.Snippets.List.Add(sciteSnippet);
                
                MenuItem newItem = new MenuItem(snippet.Name);
                newItem.Tag = snippet;
                newItem.Click += new EventHandler(OnInsertSnippetClick);
                mnuInsertSnippet.MenuItems.Add(newItem);

                newItem = new MenuItem(snippet.Name);
                newItem.Tag = snippet;
                newItem.Click += new EventHandler(OnDeleteSnippetClick);
                mnuDeleteSnippet.MenuItems.Add(newItem);

                newItem = new MenuItem(snippet.Name);
                newItem.Tag = snippet;
                newItem.Click += new EventHandler(OnSaveSnippetAs);
                mnuSaveAs.MenuItems.Add(newItem);
            }

            mnuDeleteSnippet.Enabled = (snippets.Length > 0);
            mnuInsertSnippet.Enabled = (snippets.Length > 0);
            sepSaveAs.Visible = (snippets.Length > 0);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.B:
                case Keys.Control | Keys.Shift | Keys.B:
                    ValidateScript(true);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Verifies the syntactic validity of the user script.
        /// </summary>
        private bool ValidateScript(bool confirmOK)
        {
            try
            {
                UserCSScript testInstruction = new UserCSScript(txtCode.Text);

                CompilerErrorCollection errors;
                testInstruction.Compile(out errors);

                txtCode.ClearAllAnnotations();

                if (errors.HasErrors)
                {
                    bool hasScrolled = false;

                    foreach (CompilerError error in errors)
                    {
                        int lineNum = error.Line - testInstruction.LineAtCodeStart;
                        if (!hasScrolled)
                        {
                            hasScrolled = true;
                            txtCode.ScrollToLine(lineNum);
                        }

                        txtCode.SetAnnotation(lineNum, error.ErrorText, error.IsWarning);
                    }
                }
                else
                {
                    if (confirmOK)
                    {
                        MessageBox.Show(this, "No errors could be found in the script.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "The code cannot be compiled: " + ex.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        #region Command menu

        private void mnuBatchScript_Click(object sender, EventArgs e)
        {
            CommandType = ScriptType.Batch;
        }

        private void mnuCSScript_Click(object sender, EventArgs e)
        {
            CommandType = ScriptType.CS;
        }

        private void mnuValidate_Click(object sender, EventArgs e)
        {
            ValidateScript(true);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            txtCode.SelectAll();
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            txtCode.Clear();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtCode.Clipboard.Paste();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            txtCode.Clipboard.Copy();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            txtCode.Clipboard.Cut();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            txtCode.UndoRedo.Redo();
        }

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            txtCode.UndoRedo.Undo();
        }

        private void mnuRun_Click(object sender, EventArgs e)
        {
            try
            {
                switch (CommandType)
                {
                    case ScriptType.CS:
                        UserCSScript script = new UserCSScript(this.CommandText);
                        script.Execute(this.Application);
                        break;

                    default:
                        Updater.ExecuteCommand(this.Application, this.CommandText);
                        break;

                }

                MessageBox.Show(this, "Script executed successfully.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Script can not be executed.\r\n\r\n" + ex.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuNewScript_Click(object sender, EventArgs e)
        {
            using (NewSnippetDialog dialog = new NewSnippetDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string text = string.IsNullOrEmpty(txtCode.Selection.Text) ? txtCode.Text : txtCode.Selection.Text;
                    Snippet script = new Snippet()
                    {
                        Name = dialog.ScriptName,
                        Text = text,
                        Type = CommandType
                    };
                    script.Save();
                    LoadSnippets();
                }
            }
        }

        private void OnInsertSnippetClick(object sender, EventArgs e)
        {
            Snippet snippet = ((MenuItem)sender).Tag as Snippet;
            if (snippet != null)
            {
                txtCode.InsertText(snippet.Text);
            }
        }

        private void OnDeleteSnippetClick(object sender, EventArgs e)
        {
            Snippet snippet = ((MenuItem)sender).Tag as Snippet;
            if (snippet != null)
            {
                snippet.Delete();
                LoadSnippets();
            }
        }

        private void OnSaveSnippetAs(object sender, EventArgs e)
        {
            Snippet snippet = ((MenuItem)sender).Tag as Snippet;
            if (snippet != null)
            {
                string text = string.IsNullOrEmpty(txtCode.Selection.Text) ? txtCode.Text : txtCode.Selection.Text;
                snippet.Text = text;
                snippet.Type = CommandType;
                snippet.Save();
            }
        }

        private void cmnuCommand_Popup(object sender, EventArgs e)
        {
            bool isEditControl = (((ContextMenu)sender).SourceControl == txtCode);
            sepDefaultCommands.Visible = isEditControl;
            mnuCut.Visible = isEditControl;
            mnuCopy.Visible = isEditControl;
            mnuPaste.Visible = isEditControl;
            mnuPaste.Enabled = txtCode.Clipboard.CanPaste;
            mnuRedo.Visible = isEditControl;
            mnuRedo.Enabled = txtCode.UndoRedo.CanRedo;
            mnuUndo.Visible = isEditControl;
            mnuUndo.Enabled = txtCode.UndoRedo.CanUndo;
            mnuClear.Visible = isEditControl;
            mnuSelectAll.Visible = isEditControl;
            sepClipboard.Visible = isEditControl;
            sepSelection.Visible = isEditControl;
            
            // Variable menu items should only be visible in edit control too
            foreach (MenuItem item in cmnuCommand.MenuItems)
            {
                if (!string.IsNullOrEmpty(item.Tag as string))
                {
                    item.Visible = isEditControl;
                }
            }
        }

        #endregion
    }
}
