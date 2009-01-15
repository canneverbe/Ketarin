using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// This dialog offers a GUI for editing an application
    /// job's variables.
    /// </summary>
    public partial class EditVariablesDialog : PersistentForm
    {
        private ApplicationJob.UrlVariableCollection m_Variables = null;
        private ApplicationJob m_Job = null;
        private bool m_Updating = false;

        private delegate UrlVariable VariableResultDelegate();

        #region Properties

        /// <summary>
        /// The variable which is currently being edited.
        /// </summary>
        protected UrlVariable CurrentVariable
        {
            get
            {
                if (InvokeRequired)
                {
                    return this.Invoke(new VariableResultDelegate(delegate() { return CurrentVariable; })) as UrlVariable;
                }

                string name = lbVariables.SelectedItem as string;
                if (name == null) return null;

                if (m_Variables.ContainsKey(name))
                {
                    return m_Variables[name];
                }
                return null;
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
                return !bAdd.Enabled;
            }
            set
            {
                bool enable = !value;
                bAdd.Enabled = enable;
                bRemove.Enabled = enable;
                bOK.Enabled = enable;
                bOK.Visible = enable;
            }
        }

        #endregion

        public EditVariablesDialog(ApplicationJob job) : base()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;

            m_Job = job;
            m_Variables = new ApplicationJob.UrlVariableCollection(job);
            // Get a copy of all variables
            foreach (KeyValuePair<string, UrlVariable> pair in job.Variables)
            {
                m_Variables.Add(pair.Key, pair.Value.Clone() as UrlVariable);
            }
        }

        /// <summary>
        /// Prepare context menu as soon as handles have been created.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RefreshListBox();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Prevent an invalid operation exception because of automcomplete
            txtUrl.Dispose();
        }

        /// <summary>
        /// Re-populates the ListBox with the available variables.
        /// </summary>
        private void RefreshListBox()
        {
            List<string> appVarNames = new List<string>();
            lbVariables.Items.Clear();
            foreach (KeyValuePair<string, UrlVariable> pair in m_Variables)
            {
                lbVariables.Items.Add(pair.Key);
                appVarNames.Add(pair.Value.Name);
            }

            // Adjust context menus
            txtUrl.SetVariableNames(new string[] { "category", "appname" }, appVarNames.ToArray());
        }

        /// <summary>
        /// Updates the interface according to the currently selected variable.
        /// </summary>
        private void UpdateInterface()
        {
            // Enable or disable controls if variables exist
            bool enable = (lbVariables.SelectedIndex >= 0);
            bRemove.Enabled = enable;
            lblUrl.Enabled = enable;
            txtUrl.Enabled = enable;
            bLoad.Enabled = enable;
            lblFind.Enabled = enable;
            txtFind.Enabled = enable;
            bFind.Enabled = enable;
            rtfContent.Enabled = enable;
            bUseAsStart.Enabled = enable;
            bUseAsEnd.Enabled = enable;
            lblRegex.Enabled = enable;
            txtRegularExpression.Enabled = enable;
            rbContentUrlStartEnd.Enabled = enable;
            rbContentText.Enabled = enable;
            rbContentUrlRegex.Enabled = enable;

            if (!enable) return;

            m_Updating = true;

            try
            {
                // Uodate controls which belong to the variable
                using (new ControlRedrawLock(this))
                {
                    // Set the auto complete of the URL text box
                    txtUrl.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtUrl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection urls = new AutoCompleteStringCollection();
                    urls.AddRange(DbManager.GetVariableUrls());
                    txtUrl.AutoCompleteCustomSource = urls;

                    // Set remaining controls
                    txtUrl.Text = CurrentVariable.Url;
                    txtRegularExpression.Text = CurrentVariable.Regex;

                    switch (CurrentVariable.VariableType)
                    {
                        case UrlVariable.Type.Textual:
                            rbContentText.Checked = true;
                            SetLayout(false, false, false);

                            rtfContent.Top = rbContentText.Bottom + rbContentText.Margin.Bottom + rtfContent.Margin.Top;
                            rtfContent.Height = lbVariables.Bottom - rtfContent.Top;
                            break;

                        case UrlVariable.Type.StartEnd:
                            rbContentUrlStartEnd.Checked = true;
                            SetLayout(true, false, true);

                            rtfContent.Top = txtFind.Bottom + txtFind.Margin.Bottom + rtfContent.Margin.Top;
                            rtfContent.Height = lbVariables.Bottom - rtfContent.Top;
                            break;

                        case UrlVariable.Type.RegularExpression:
                            rbContentUrlRegex.Checked = true;
                            SetLayout(false, true, true);

                            rtfContent.Top = txtRegularExpression.Bottom + txtRegularExpression.Margin.Bottom + rtfContent.Margin.Top;
                            rtfContent.Height = lbVariables.Bottom - rtfContent.Top;
                            break;
                    }
                }

                SetRtfContent();
            }
            finally
            {
                m_Updating = false;
            }
        }

        private void lbVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();

            txtUrl.Focus();
        }

        /// <summary>
        /// Adjusts the RTF content based on the currently
        /// selected variable.
        /// </summary>
        private void SetRtfContent()
        {
            if (CurrentVariable.VariableType == UrlVariable.Type.Textual)
            {
                rtfContent.Text = CurrentVariable.TextualContent;
            }
            else
            {
                if (string.IsNullOrEmpty(CurrentVariable.TempContent) && !string.IsNullOrEmpty(txtUrl.Text))
                {
                    rtfContent.Text = string.Empty;
                    bLoad.PerformClick();
                }
                else
                {
                    rtfContent.Text = CurrentVariable.TempContent;
                }
                RefreshRtfFormatting();
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
            txtRegularExpression.Visible = regex;
            lblRegex.Visible = regex;
            bUseAsEnd.Visible = startEnd;
            bUseAsStart.Visible = startEnd;
            txtFind.Visible = findAndUrl;
            lblFind.Visible = findAndUrl;
            bFind.Visible = findAndUrl;
            lblUrl.Visible = findAndUrl;
            txtUrl.Visible = findAndUrl;
            bLoad.Visible = findAndUrl;
            rtfContent.ReadOnly = findAndUrl || ReadOnly;
        }

        private void rbContentUrlStartEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Updating || !rbContentUrlStartEnd.Checked) return;

            CurrentVariable.VariableType = UrlVariable.Type.StartEnd;
            UpdateInterface();
        }

        private void rbContentUrlRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Updating || !rbContentUrlRegex.Checked) return;

            CurrentVariable.VariableType = UrlVariable.Type.RegularExpression;
            UpdateInterface();
        }

        private void rbContentText_CheckedChanged(object sender, EventArgs e)
        {
            if (m_Updating || !rbContentText.Checked) return;

            CurrentVariable.VariableType = UrlVariable.Type.Textual;
            UpdateInterface();
        }

        #endregion

        private void bAdd_Click(object sender, EventArgs e)
        {
            using (NewVariableDialog dialog = new NewVariableDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (m_Variables.ContainsKey(dialog.VariableName))
                    {
                        string msg = string.Format("The variable name '{0}' already exists.", dialog.VariableName);
                        MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_Variables.Add(dialog.VariableName, new UrlVariable(dialog.VariableName, m_Variables));
                    RefreshListBox();
                    lbVariables.SelectedItem = dialog.VariableName;
                }
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            Uri url;

            // Check whether or not the URL is valid and show an error message if necessary
            try
            {
                url = new Uri(CurrentVariable.ExpandedUrl);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show(this, "The URL you entered is empty and cannot be loaded.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);    
                return;
            }
            catch (UriFormatException)
            {
                MessageBox.Show(this, "The specified URL is not valid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load URL contents and show a wait cursor in the meantime
            Cursor = Cursors.WaitCursor;
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Note: The Text property might modify the text value
                    using (ProgressDialog dialog = new ProgressDialog("Loading URL", "Please wait while the content is being downloaded..."))
                    {
                        dialog.OnDoWork = delegate()
                        {
                            CurrentVariable.TempContent = client.DownloadString(url);
                            return true;
                        };
                        dialog.OnCancel = delegate()
                        {
                            dialog.Cancel();
                        };
                        dialog.ShowDialog(this);
                        
                        // Did an error occur?
                        if (!dialog.Cancelled && dialog.Error != null)
                        {
                            LogDialog.Log("Failed loading URL", dialog.Error);
                            MessageBox.Show(this, "The contents of the URL could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    rtfContent.Text = CurrentVariable.TempContent;
                    RefreshRtfFormatting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occured when loading the URL: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void bFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text)) return;

            // Do not find the same string again...
            int searchPos = rtfContent.SelectionStart + 1;
            // ... and start from the beginning if necessary
            if (searchPos >= rtfContent.Text.Length)
            {
                searchPos = 0;
            }
            int pos = rtfContent.Find(txtFind.Text, searchPos , RichTextBoxFinds.None);

            if (pos < 0) return;
            // If a match has been found, highlight it
            rtfContent.SelectionStart = pos;
            rtfContent.SelectionLength = txtFind.Text.Length;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    // Do not push the OK button in certain occasions
                    if (lbVariables.Items.Count == 0)
                    {
                        bAdd.PerformClick();
                        return true;
                    }
                    else if (txtUrl.Focused)
                    {
                        bLoad.PerformClick();
                        return true;
                    }
                    else if (txtFind.Focus())
                    {
                        bFind.PerformClick();
                        return true;
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            CurrentVariable.Url = txtUrl.Text;
        }

        private void bUseAsStart_Click(object sender, EventArgs e)
        {
            CurrentVariable.StartText = rtfContent.SelectedText;
            RefreshRtfFormatting();
        }

        private void bUseAsEnd_Click(object sender, EventArgs e)
        {
            CurrentVariable.EndText = rtfContent.SelectedText;
            RefreshRtfFormatting();
        }

        private void RefreshRtfFormatting()
        {
            if (string.IsNullOrEmpty(rtfContent.Text) || CurrentVariable.VariableType == UrlVariable.Type.Textual) return;

            using (new ControlRedrawLock(this))
            {
                rtfContent.SelectionStart = 0;
                rtfContent.SelectionLength = rtfContent.Text.Length;
                rtfContent.SelectionColor = SystemColors.WindowText;
                rtfContent.SelectionFont = rtfContent.Font;
                rtfContent.SelectionLength = 0;

                if (!string.IsNullOrEmpty(CurrentVariable.Regex))
                {
                    Regex regex = new Regex(CurrentVariable.Regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match match = regex.Match(rtfContent.Text);

                    rtfContent.SelectionStart = match.Index;
                    rtfContent.SelectionLength = match.Length;
                    rtfContent.SelectionColor = Color.Red;

                    if (match.Groups.Count > 1)
                    {
                        rtfContent.SelectionStart = match.Groups[1].Index;
                        rtfContent.SelectionLength = match.Groups[1].Length;
                        rtfContent.SelectionColor = Color.Blue;
                    }

                    rtfContent.SelectionLength = 0;
                }
                else
                {
                    // Highlight StartText if specified
                    if (string.IsNullOrEmpty(CurrentVariable.StartText)) return;
                    int pos = rtfContent.Text.IndexOf(CurrentVariable.StartText);
                    if (pos == -1)
                    {
                        pos = 0;
                    }
                    else
                    {
                        rtfContent.SelectionStart = pos;
                        rtfContent.SelectionLength = CurrentVariable.StartText.Length;
                        rtfContent.SelectionColor = Color.Blue;
                    }

                    int boldStart = pos + CurrentVariable.StartText.Length;

                    // Highlight EndText if specified
                    if (string.IsNullOrEmpty(CurrentVariable.EndText)) return;
                    pos = rtfContent.Text.IndexOf(CurrentVariable.EndText, boldStart);
                    if (pos < 0) return;
                    rtfContent.SelectionStart = pos;
                    rtfContent.SelectionLength = CurrentVariable.EndText.Length;

                    rtfContent.SelectionColor = Color.Red;

                    rtfContent.SelectionStart = boldStart;
                    rtfContent.SelectionLength = pos - boldStart;
                    rtfContent.SelectionFont = new Font(rtfContent.SelectionFont, FontStyle.Bold);
                    rtfContent.SelectionLength = 0;
                }
            }
        }

        private void rtfContent_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = (rtfContent.SelectionLength > 0 && string.IsNullOrEmpty(CurrentVariable.Regex));
            bUseAsEnd.Enabled = bUseAsStart.Enabled = enable;
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (CurrentVariable == null) return;

            m_Variables.Remove(CurrentVariable.Name);
            RefreshListBox();

            if (lbVariables.Items.Count > 0)
            {
                lbVariables.SelectedIndex = 0;
            }
            else
            {
                lbVariables.SelectedIndex = -1;
            }
            lbVariables_SelectedIndexChanged(this, null);
        }

        private void rtfContent_TextChanged(object sender, EventArgs e)
        {
            if (CurrentVariable != null && CurrentVariable.VariableType == UrlVariable.Type.Textual)
            {
                CurrentVariable.TextualContent = rtfContent.Text;
            }
        }

        private void txtRegularExpression_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegularExpression.Text))
            {
                bUseAsStart.Enabled = true;
                bUseAsEnd.Enabled = true;
                CurrentVariable.Regex = string.Empty;
                RefreshRtfFormatting();
                return;
            }

            // We cannot (or rather don't want to) combine those to for now
            bUseAsEnd.Enabled = false;
            bUseAsStart.Enabled = false;

            try
            {
                Regex regex = new Regex(txtRegularExpression.Text, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                CurrentVariable.Regex = regex.ToString();
            }
            catch (ArgumentException)
            {
                txtRegularExpression.BackColor = Color.Tomato;
                return;
            }

            txtRegularExpression.BackColor = Color.White;
            
            RefreshRtfFormatting();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Delete empty variables
            List<string> toDelete = new List<string>();
            foreach (KeyValuePair<string, UrlVariable> pair in m_Variables) 
            {
                if (pair.Value.IsEmpty) toDelete.Add(pair.Key);
            }
            while (toDelete.Count > 0) { m_Variables.Remove(toDelete[0]); toDelete.RemoveAt(0); }

            // Save results
            m_Job.Variables = m_Variables;
        }
    }
}
