using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CDBurnerXP.IO;
using Microsoft.Win32;

namespace Ketarin.Forms
{
    /// <summary>
    /// This dialog offers a GUI for editing an application
    /// job's variables.
    /// </summary>
    public partial class EditVariablesDialog : PersistentForm
    {
        /// <summary>
        /// Allows to refresh the contents of the ListBox.
        /// </summary>
        private class VariableListBox : ListBox
        {
            public new void RefreshItems()
            {
                base.RefreshItems();
            }
        }

        private readonly ApplicationJob.UrlVariableCollection m_Variables;
        private readonly ApplicationJob m_Job;
        private bool m_Updating;
        private string m_MatchSelection;
        private int m_MatchPosition = -1;
        private BrowserPreviewDialog m_Preview;
        private Thread regexThread;
        private bool gotoMatch;

        private delegate UrlVariable VariableResultDelegate();

        #region Properties

        /// <summary>
        /// Gets or sets the user agent to use for requests.
        /// </summary>
        public string UserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the currently used match for a given
        /// start/end or regex.
        /// </summary>
        public string MatchSelection
        {
            get { return this.m_MatchSelection; }
            set
            {
                this.m_MatchSelection = value;
                this.cmnuCopyMatch.Enabled = !string.IsNullOrEmpty(this.m_MatchSelection);
                this.cmnuGoToMatch.Enabled = this.cmnuCopyMatch.Enabled;
            }
        }

        /// <summary>
        /// Gets an instance of the preview dialog
        /// </summary>
        protected BrowserPreviewDialog PreviewDialog
        {
            get { return this.m_Preview ?? (this.m_Preview = new BrowserPreviewDialog()); }
        }

        /// <summary>
        /// The variable which is currently being edited.
        /// </summary>
        protected UrlVariable CurrentVariable
        {
            get
            {
                if (this.InvokeRequired)
                {
                    return this.Invoke(new VariableResultDelegate(delegate() { return this.CurrentVariable; })) as UrlVariable;
                }

                return this.lbVariables.SelectedItem as UrlVariable;
            }
        }

        /// <summary>
        /// Gets or sets whether the dialog should be
        /// opened in read-only mode.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return this.txtUrl.ReadOnly;
            }
            set
            {
                bool enable = !value;
                this.txtUrl.ReadOnly = value;
                this.txtFind.ReadOnly = value;
                this.txtRegularExpression.ReadOnly = value;
                this.chkRightToLeft.Enabled = enable;
                this.bAdd.Enabled = enable;
                this.bRemove.Enabled = enable;
                this.bOK.Enabled = enable;
                this.bOK.Visible = enable;
            }
        }

        #endregion

        public EditVariablesDialog(ApplicationJob job) : base()
        {
            this.InitializeComponent();
            this.AcceptButton = this.bOK;
            this.CancelButton = this.bCancel;

            this.m_Job = job;
            this.m_Variables = new ApplicationJob.UrlVariableCollection(job);
            // Get a copy of all variables
            foreach (KeyValuePair<string, UrlVariable> pair in job.Variables)
            {
                UrlVariable clonedVariable = pair.Value.Clone() as UrlVariable;
                this.m_Variables.Add(pair.Key, clonedVariable);
                clonedVariable.Parent = this.m_Variables;
            }
        }

        /// <summary>
        /// Prepare context menu as soon as handles have been created.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Turn off auto word selection (workaround .NET bug).
            this.rtfContent.AutoWordSelection = true;
            this.rtfContent.AutoWordSelection = false;

            this.ReloadVariables(true);

            if (this.m_Job != null && !string.IsNullOrEmpty(this.m_Job.Name))
            {
                this.Text = "Edit Variables - " + this.m_Job.Name;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Prevent an invalid operation exception because of automcomplete
            this.txtUrl.Dispose();
        }

        /// <summary>
        /// Re-populates the ListBox with the available variables.
        /// </summary>
        private void ReloadVariables(bool updateList)
        {
            List<string> appVarNames = new List<string>();
            if (updateList) this.lbVariables.Items.Clear();

            foreach (KeyValuePair<string, UrlVariable> pair in this.m_Variables)
            {
                if (updateList) this.lbVariables.Items.Add(pair.Value);
                appVarNames.Add(pair.Value.Name);
            }

            // Adjust context menus
            this.txtUrl.SetVariableNames(new[] { "category", "appname" }, appVarNames.ToArray());
        }

        /// <summary>
        /// Updates the interface according to the currently selected variable.
        /// </summary>
        private void UpdateInterface()
        {
            // Enable or disable controls if variables exist
            bool enable = (this.lbVariables.SelectedIndex >= 0);
            this.bRemove.Enabled = enable;
            this.lblUrl.Enabled = enable;
            this.txtUrl.Enabled = enable;
            this.bLoad.Enabled = enable;
            this.bPostData.Enabled = enable;
            this.lblFind.Enabled = enable;
            this.txtFind.Enabled = enable;
            this.bFind.Enabled = enable;
            this.rtfContent.Enabled = enable;
            this.bUseAsStart.Enabled = enable;
            this.bUseAsEnd.Enabled = enable;
            this.lblRegex.Enabled = enable;
            this.txtRegularExpression.Enabled = enable;
            this.chkRightToLeft.Enabled = enable;
            this.rbContentUrlStartEnd.Enabled = enable;
            this.rbContentText.Enabled = enable;
            this.rbContentUrlRegex.Enabled = enable;

            if (!enable) return;

            this.m_Updating = true;

            try
            {
                // Uodate controls which belong to the variable
                using (new ControlRedrawLock(this))
                {
                    // Set the auto complete of the URL text box
                    this.txtUrl.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.txtUrl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection urls = new AutoCompleteStringCollection();
                    urls.AddRange(DbManager.GetVariableUrls());
                    this.txtUrl.AutoCompleteCustomSource = urls;

                    // Set remaining controls
                    this.txtUrl.Text = this.CurrentVariable.Url;
                    this.chkRightToLeft.Checked = this.CurrentVariable.RegexRightToLeft;
                    this.txtRegularExpression.Text = this.CurrentVariable.Regex;

                    switch (this.CurrentVariable.VariableType)
                    {
                        case UrlVariable.Type.Textual:
                            this.rbContentText.Checked = true;
                            this.SetLayout(false, false, false);

                            this.rtfContent.Top = this.rbContentText.Bottom + this.rbContentText.Margin.Bottom + this.rtfContent.Margin.Top;
                            this.rtfContent.Height = this.lbVariables.Bottom - this.rtfContent.Top;
                            break;

                        case UrlVariable.Type.StartEnd:
                            this.rbContentUrlStartEnd.Checked = true;
                            this.SetLayout(true, false, true);

                            this.rtfContent.Top = this.txtFind.Bottom + this.txtFind.Margin.Bottom + this.rtfContent.Margin.Top;
                            this.rtfContent.Height = this.lbVariables.Bottom - this.rtfContent.Top;
                            break;

                        case UrlVariable.Type.RegularExpression:
                            this.rbContentUrlRegex.Checked = true;
                            this.SetLayout(false, true, true);

                            this.rtfContent.Top = this.txtRegularExpression.Bottom + this.txtRegularExpression.Margin.Bottom + this.rtfContent.Margin.Top;
                            this.rtfContent.Height = this.lbVariables.Bottom - this.rtfContent.Top;
                            break;
                    }
                }

                this.SetRtfContent();
                this.UpdateRegexMatches();
            }
            finally
            {
                this.m_Updating = false;
            }
        }

        private void lbVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gotoMatch = true;
            this.UpdateInterface();
            this.txtUrl.Focus();
        }

        /// <summary>
        /// Makes sure, that the matched content is visible
        /// in the richtextbox.
        /// </summary>
        private void GoToMatch()
        {
            if (!string.IsNullOrEmpty(this.m_MatchSelection))
            {
                if (this.m_MatchPosition >= 0)
                {
                    this.rtfContent.SelectionStart = this.m_MatchPosition;
                    this.rtfContent.SelectionLength = this.m_MatchSelection.Length;
                    this.rtfContent.SelectionLength = 0;
                }
            }
        }

        /// <summary>
        /// Adjusts the RTF content based on the currently
        /// selected variable.
        /// </summary>
        private void SetRtfContent()
        {
            if (this.CurrentVariable.VariableType == UrlVariable.Type.Textual)
            {
                this.rtfContent.Text = this.CurrentVariable.TextualContent;
            }
            else
            {
                if (string.IsNullOrEmpty(this.CurrentVariable.TempContent) && !string.IsNullOrEmpty(this.txtUrl.Text))
                {
                    this.rtfContent.Text = string.Empty;
                    this.bLoad.PerformClick();
                }
                else
                {
                    this.rtfContent.Text = this.CurrentVariable.TempContent;
                }
                this.RefreshRtfFormatting();
            }
        }

        #region Layout options

        /// <summary>
        /// Helper function to hide certain parts of the layout
        /// </summary>
        /// <param name="startEnd">Hide or show the start/end buttons</param>
        /// <param name="regex">Hide or show the regular expression field</param>
        /// <param name="find">Hide or show the search box and URL field</param>
        private void SetLayout(bool startEnd, bool regex, bool findAndUrl)
        {
            this.txtRegularExpression.Visible = regex;
            this.chkRightToLeft.Visible = regex;
            this.lblRegex.Visible = regex;
            this.bUseAsEnd.Visible = startEnd;
            this.bUseAsStart.Visible = startEnd;
            this.txtFind.Visible = findAndUrl;
            this.lblFind.Visible = findAndUrl;
            this.bFind.Visible = findAndUrl;
            this.lblUrl.Visible = findAndUrl;
            this.txtUrl.Visible = findAndUrl;
            this.bLoad.Visible = findAndUrl;
            this.bPostData.Visible = findAndUrl;
            this.rtfContent.ReadOnly = findAndUrl || this.ReadOnly;
        }

        private void rbContentUrlStartEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_Updating || !this.rbContentUrlStartEnd.Checked) return;

            this.CurrentVariable.VariableType = UrlVariable.Type.StartEnd;
            this.UpdateInterface();
        }

        private void rbContentUrlRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_Updating || !this.rbContentUrlRegex.Checked) return;

            this.CurrentVariable.VariableType = UrlVariable.Type.RegularExpression;
            this.UpdateInterface();
        }

        private void rbContentText_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_Updating || !this.rbContentText.Checked) return;

            this.CurrentVariable.VariableType = UrlVariable.Type.Textual;
            this.MatchSelection = null;
            this.m_MatchPosition = -1;
            this.UpdateInterface();
        }

        #endregion

        private void bAdd_Click(object sender, EventArgs e)
        {
            using (NewVariableDialog dialog = new NewVariableDialog(this.m_Variables))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.m_Variables.Add(dialog.VariableName, new UrlVariable(dialog.VariableName, this.m_Variables));
                    this.ReloadVariables(true);
                    this.lbVariables.SelectedItem = this.m_Variables[dialog.VariableName];
                }
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            // Load URL contents and show a wait cursor in the meantime
            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (WebClient client = new WebClient(this.UserAgent))
                {
                    string expandedUrl = null;
                    string postData = null;

                    // Note: The Text property might modify the text value
                    using (ProgressDialog dialog = new ProgressDialog("Loading URL", "Please wait while the content is being downloaded..."))
                    {
                        dialog.OnDoWork = delegate
                        {
                            expandedUrl = this.CurrentVariable.ExpandedUrl;
                            if (dialog.Cancelled) return false;
                            client.SetPostData(this.CurrentVariable);
                            postData = client.PostData;
                            this.CurrentVariable.TempContent = client.DownloadString(new Uri(expandedUrl));
                            return true;
                        };
                        dialog.OnCancel = delegate {
                            dialog.Cancel();
                        };
                        dialog.ShowDialog(this);
                        
                        // Did an error occur?
                        if (!dialog.Cancelled && dialog.Error != null)
                        {
                            LogDialog.Log("Failed loading URL", dialog.Error);

                            // Check whether or not the URL is valid and show an error message if necessary
                            if (dialog.Error is ArgumentNullException || string.IsNullOrEmpty(expandedUrl))
                            {
                                MessageBox.Show(this, "The URL you entered is empty and cannot be loaded.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (dialog.Error is UriFormatException)
                            {
                                MessageBox.Show(this, "The specified URL '" + expandedUrl + "' is not valid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show(this, "The contents of the URL can not be loaded: " + dialog.Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    this.rtfContent.Text = this.CurrentVariable.TempContent;
                    // For Regex: Go to match after thread finish
                    this.gotoMatch = true;
                    this.UpdateRegexMatches();
                    // For Start/End: Go to match now
                    this.RefreshRtfFormatting();

                    this.GoToMatch();

                    // Show page preview if desired
                    if (this.cmnuBrowser.Checked)
                    {
                        this.PreviewDialog.ShowPreview(this, expandedUrl, postData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occured when loading the URL: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void bFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFind.Text)) return;

            // Do not find the same string again...
            int searchPos = this.rtfContent.SelectionStart + 1;
            // ... and start from the beginning if necessary
            if (searchPos >= this.rtfContent.Text.Length)
            {
                searchPos = 0;
            }
            int pos = this.rtfContent.Find(this.txtFind.Text, searchPos , RichTextBoxFinds.None);

            if (pos < 0) return;
            // If a match has been found, highlight it
            this.rtfContent.SelectionStart = pos;
            this.rtfContent.SelectionLength = this.txtFind.Text.Length;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    if (this.CurrentVariable != null)
                    {
                        NewVariableDialog dialog = new NewVariableDialog(this.m_Variables)
                        {
                            VariableName = this.CurrentVariable.Name,
                            Text = "Rename variable"
                        };
                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            this.m_Variables.Remove(this.CurrentVariable.Name);
                            this.CurrentVariable.Name = dialog.VariableName;
                            this.m_Variables.Add(this.CurrentVariable.Name, this.CurrentVariable);
                            this.lbVariables.RefreshItems();
                            this.ReloadVariables(false);
                        }
                    }
                    break;

                case Keys.Enter:
                    // Do not push the OK button in certain occasions
                    if (this.lbVariables.Items.Count == 0)
                    {
                        this.bAdd.PerformClick();
                        return true;
                    }
                    else if (this.txtUrl.Focused)
                    {
                        this.bLoad.PerformClick();
                        return true;
                    }
                    else if (this.txtFind.Focus())
                    {
                        this.bFind.PerformClick();
                        return true;
                    }
                    break;

                case Keys.Control | Keys.G:
                    this.GoToMatch();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            this.CurrentVariable.Url = this.txtUrl.Text;
        }

        private void bUseAsStart_Click(object sender, EventArgs e)
        {
            this.CurrentVariable.StartText = this.rtfContent.SelectedText;
            this.RefreshRtfFormatting();
        }

        private void bUseAsEnd_Click(object sender, EventArgs e)
        {
            this.CurrentVariable.EndText = this.rtfContent.SelectedText;
            this.RefreshRtfFormatting();
        }

        /// <summary>
        /// Highlights the currently matched content (based 
        /// on regex or start/end) within the richtextbox.
        /// </summary>
        private void RefreshRtfFormatting(Match match = null)
        {
            if (string.IsNullOrEmpty(this.rtfContent.Text) || this.CurrentVariable.VariableType == UrlVariable.Type.Textual) return;

            using (new ControlRedrawLock(this))
            {
                // Determine scroll position
                User32.POINT scrollPos = new User32.POINT();
                User32.SendMessage(this.rtfContent.Handle, User32.EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPos);

                // Reset text area
                this.MatchSelection = string.Empty;
                this.m_MatchPosition = -1;
                this.rtfContent.SelectionStart = 0;
                this.rtfContent.SelectionLength = this.rtfContent.Text.Length;
                this.rtfContent.SelectionColor = SystemColors.WindowText;
                this.rtfContent.SelectionFont = this.rtfContent.Font;
                this.rtfContent.SelectionBackColor = SystemColors.Window;
                this.rtfContent.SelectionLength = 0;

                if (match != null)
                {
                    this.m_MatchPosition = match.Index;
                    this.rtfContent.SelectionStart = match.Index;
                    this.rtfContent.SelectionLength = match.Length;
                    this.rtfContent.SelectionColor = Color.White;
                    this.rtfContent.SelectionBackColor = Color.Blue;

                    if (match.Groups.Count > 1)
                    {
                        this.m_MatchPosition = match.Groups[1].Index;
                        this.rtfContent.SelectionStart = match.Groups[1].Index;
                        this.rtfContent.SelectionLength = match.Groups[1].Length;
                        this.rtfContent.SelectionColor = Color.White;
                        this.rtfContent.SelectionBackColor = Color.Red;
                    }

                    this.MatchSelection = this.rtfContent.SelectedText;
                    this.rtfContent.SelectionLength = 0;
                }
                else if (this.CurrentVariable.VariableType == UrlVariable.Type.StartEnd && !string.IsNullOrEmpty(this.CurrentVariable.StartText))
                {
                    // Highlight StartText with blue background
                    int pos = this.rtfContent.Text.IndexOf(this.CurrentVariable.StartText);
                    if (pos == -1)
                    {
                        pos = 0;
                    }
                    else
                    {
                        this.rtfContent.SelectionStart = pos;
                        this.rtfContent.SelectionLength = this.CurrentVariable.StartText.Length;
                        this.rtfContent.SelectionColor = Color.White;
                        this.rtfContent.SelectionBackColor = Color.Blue;
                    }

                    int matchStart = pos + this.CurrentVariable.StartText.Length;

                    if (!string.IsNullOrEmpty(this.CurrentVariable.EndText) && matchStart <= this.rtfContent.Text.Length)
                    {
                        pos = this.rtfContent.Text.IndexOf(this.CurrentVariable.EndText, matchStart);
                        if (pos >= 0)
                        {
                            // Highlight EndText with blue background if specified
                            this.m_MatchPosition = pos;
                            this.rtfContent.SelectionStart = pos;
                            this.rtfContent.SelectionLength = this.CurrentVariable.EndText.Length;
                            this.rtfContent.SelectionColor = Color.White;
                            this.rtfContent.SelectionBackColor = Color.Blue;

                            // Highlight match with red background
                            this.rtfContent.SelectionStart = matchStart;
                            this.rtfContent.SelectionLength = pos - matchStart;
                            this.MatchSelection = this.rtfContent.SelectedText;
                            this.rtfContent.SelectionColor = Color.White;
                            this.rtfContent.SelectionBackColor = Color.Red;
                            this.rtfContent.SelectionLength = 0;
                        }
                    }
                }

                // Restore scroll position
                User32.SendMessage(this.rtfContent.Handle, User32.EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPos);
            }
        }

        private void rtfContent_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = (this.rtfContent.SelectionLength > 0);
            this.cmnuCopy.Enabled = enable;
            this.bUseAsEnd.Enabled = this.bUseAsStart.Enabled = enable;
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (this.CurrentVariable == null) return;

            this.m_Variables.Remove(this.CurrentVariable.Name);
            this.ReloadVariables(true);

            if (this.lbVariables.Items.Count > 0)
            {
                this.lbVariables.SelectedIndex = 0;
            }
            else
            {
                this.lbVariables.SelectedIndex = -1;
            }
            this.lbVariables_SelectedIndexChanged(this, null);
        }

        private void rtfContent_TextChanged(object sender, EventArgs e)
        {
            if (this.CurrentVariable != null && this.CurrentVariable.VariableType == UrlVariable.Type.Textual)
            {
                this.CurrentVariable.TextualContent = this.rtfContent.Text;
            }
        }

        private void txtRegularExpression_TextChanged(object sender, EventArgs e)
        {
            this.UpdateRegexMatches();
        }

        private void UpdateRegexMatches()
        {
            // "Disable" regex if empty
            if (string.IsNullOrEmpty(this.txtRegularExpression.Text))
            {
                this.CurrentVariable.Regex = string.Empty;
                this.RefreshRtfFormatting();
                return;
            }

            try
            {
                // Check if Regex is valid
                new Regex(this.txtRegularExpression.Text);

                // Set value
                this.CurrentVariable.Regex = this.txtRegularExpression.Text;
            }
            catch (ArgumentException)
            {
                // Make sure that user notices an error. Just showing error 
                // messages will be annoying.
                this.txtRegularExpression.BackColor = Color.Tomato;
                return;
            }

            this.txtRegularExpression.BackColor = SystemColors.Window;

            this.txtRegularExpression.HintText = "Evaluating...";

            // Cancel current evaluation and start new
            if (this.regexThread != null)
            {
                this.regexThread.Abort();
                this.regexThread = null;
            }
            this.regexThread = new Thread(this.EvaluateRegex);
            this.regexThread.Start(this.rtfContent.Text);
        }

        private void EvaluateRegex(object text)
        {
            try
            {
                try
                {
                    UrlVariable currenVar = this.CurrentVariable;

                    string compareRegex = currenVar.Regex;
                    Regex regex = currenVar.CreateRegex();

                    if (regex == null) return;

                    Match match = regex.Match(text as string);

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        this.SetRegexResult(compareRegex, match);
                    });
                }
                catch (UriFormatException ex)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        MessageBox.Show(this, "The regular expression cannot be evaluated: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                catch (ThreadAbortException)
                {
                    /* Thread aborted, no error */ 
                }
                finally
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        this.txtRegularExpression.HintText = string.Empty;
                    });
                }
            }
            catch (InvalidOperationException)
            {
                /* Ignore error if form is closed */ 
            }
        }

        private void SetRegexResult(string regex, Match match)
        {
            // Called from thread. Only execute if current regex matches
            // the just returned result.
            if (this.CurrentVariable.Regex != regex)
            {
                return;
            }

            this.txtRegularExpression.HintText = string.Empty;
            this.RefreshRtfFormatting(match);

            if (match.Success && this.gotoMatch)
            {
                this.gotoMatch = false;
                this.GoToMatch();
            }
        }

        private void chkRightToLeft_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentVariable.RegexRightToLeft = this.chkRightToLeft.Checked;
            this.RefreshRtfFormatting();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Delete empty variables
            List<string> toDelete = this.m_Variables.Where(pair => pair.Value.IsEmpty).Select(pair => pair.Key).ToList();
            while (toDelete.Count > 0)
            {
                this.m_Variables.Remove(toDelete[0]);
                toDelete.RemoveAt(0);
            }

            // Save results
            this.m_Job.Variables = this.m_Variables;
        }

        private void bPostData_Click(object sender, EventArgs e)
        {
            using (PostDataEditor editor = new PostDataEditor())
            {
                editor.PostData = this.CurrentVariable.PostData;
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    this.CurrentVariable.PostData = editor.PostData;
                }
            }
        }

        #region Context menu

        private void cmnuCopy_Click(object sender, EventArgs e)
        {
            SafeClipboard.SetData(this.rtfContent.SelectedText, true);
        }

        private void cmnuPaste_Click(object sender, EventArgs e)
        {
            this.rtfContent.Paste();
        }

        private void cmnuCopyMatch_Click(object sender, EventArgs e)
        {
            SafeClipboard.SetData(this.MatchSelection, true);
        }

        private void cmnuGoToMatch_Click(object sender, EventArgs e)
        {
            this.GoToMatch();
        }

        private void cmnuWrap_Click(object sender, EventArgs e)
        {
            this.rtfContent.WordWrap = !this.cmnuWrap.Checked;
            this.cmnuWrap.Checked = this.rtfContent.WordWrap;
        }

        private void cmnuBrowser_Click(object sender, EventArgs e)
        {
            this.cmnuBrowser.Checked = !this.cmnuBrowser.Checked;
            if (this.cmnuBrowser.Checked)
            {
                if (this.m_Preview == null)
                {
                    this.PreviewDialog.VisibleChanged += this.PreviewDialog_VisibleChanged;
                }
                this.bLoad.PerformClick(); // Reload browser contents
            }
            else
            {
                this.PreviewDialog.Hide();
            }
        }

        private void PreviewDialog_VisibleChanged(object sender, EventArgs e)
        {
            this.cmnuBrowser.Checked = this.PreviewDialog.Visible;
        }

        #endregion
    }
}
