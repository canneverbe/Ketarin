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
    public partial class EditVariablesDialog : PersistentForm
    {
        private ApplicationJob.UrlVariableCollection m_Variables = new ApplicationJob.UrlVariableCollection();
        private ApplicationJob m_Job = null;

        #region Properties

        private UrlVariable CurrentVar
        {
            get
            {
                string name = lbVariables.SelectedItem as string;
                if (m_Variables.ContainsKey(name))
                {
                    return m_Variables[name];
                }
                return null;
            }
        }

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
            // Get a copy of all variables
            foreach (KeyValuePair<string, UrlVariable> pair in job.Variables)
            {
                m_Variables.Add(pair.Key, pair.Value.Clone() as UrlVariable);
            }

            RefreshListBox();
        }

        private void RefreshListBox()
        {
            lbVariables.Items.Clear();
            foreach (KeyValuePair<string, UrlVariable> pair in m_Variables)
            {
                lbVariables.Items.Add(pair.Key);
            }
        }

        private void lbVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            if (enable)
            {
                txtUrl.Focus();
                txtUrl.Text = CurrentVar.Url;
                txtRegularExpression.Text = CurrentVar.Regex;
                if (string.IsNullOrEmpty(CurrentVar.TempContent) && !string.IsNullOrEmpty(txtUrl.Text))
                {
                    bLoad.PerformClick();
                }
                else
                {
                    rtfContent.Text = CurrentVar.TempContent;
                }
                RefreshRtfFormatting();
            }
        }

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
                    m_Variables.Add(dialog.VariableName, new UrlVariable(dialog.VariableName, m_Job));
                    RefreshListBox();
                    lbVariables.SelectedItem = dialog.VariableName;
                }
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            Uri url;

            try
            {
                url = new Uri(CurrentVar.ExpandedUrl);
            }
            catch (UriFormatException)
            {
                MessageBox.Show(this, "The specified URL is not valid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                using (WebClient client = new WebClient())
                {
                    rtfContent.Text = client.DownloadString(url);
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

            int searchPos = rtfContent.SelectionStart + 1;
            if (searchPos >= rtfContent.Text.Length)
            {
                searchPos = 0;
            }
            int pos = rtfContent.Find(txtFind.Text, searchPos , RichTextBoxFinds.None);

            if (pos < 0) return;
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
            CurrentVar.Url = txtUrl.Text;
        }

        private void rtfContent_TextChanged(object sender, EventArgs e)
        {
            CurrentVar.TempContent = rtfContent.Text;
        }

        private void bUseAsStart_Click(object sender, EventArgs e)
        {
            CurrentVar.StartText = rtfContent.SelectedText;
            RefreshRtfFormatting();
        }

        private void bUseAsEnd_Click(object sender, EventArgs e)
        {
            CurrentVar.EndText = rtfContent.SelectedText;
            RefreshRtfFormatting();
        }

        private void RefreshRtfFormatting()
        {
            if (string.IsNullOrEmpty(rtfContent.Text)) return;

            using (new ControlRedrawLock(this))
            {
                rtfContent.SelectionStart = 0;
                rtfContent.SelectionLength = rtfContent.Text.Length;
                rtfContent.SelectionColor = SystemColors.WindowText;
                rtfContent.SelectionFont = rtfContent.Font;
                rtfContent.SelectionLength = 0;

                if (!string.IsNullOrEmpty(CurrentVar.Regex))
                {
                    Regex regex = new Regex(CurrentVar.Regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
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
                    if (string.IsNullOrEmpty(CurrentVar.StartText)) return;
                    int pos = rtfContent.Text.IndexOf(CurrentVar.StartText);
                    if (pos == -1)
                    {
                        pos = 0;
                    }
                    else
                    {
                        rtfContent.SelectionStart = pos;
                        rtfContent.SelectionLength = CurrentVar.StartText.Length;
                        rtfContent.SelectionColor = Color.Blue;
                    }

                    int boldStart = pos + CurrentVar.StartText.Length;

                    // Highlight EndText if specified
                    if (string.IsNullOrEmpty(CurrentVar.EndText)) return;
                    pos = rtfContent.Text.IndexOf(CurrentVar.EndText, boldStart);
                    if (pos < 0) return;
                    rtfContent.SelectionStart = pos;
                    rtfContent.SelectionLength = CurrentVar.EndText.Length;

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
            bool enable = (rtfContent.SelectionLength > 0 && string.IsNullOrEmpty(CurrentVar.Regex));
            bUseAsEnd.Enabled = bUseAsStart.Enabled = enable;
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            m_Variables.Remove(CurrentVar.Name);
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

        private void txtRegularExpression_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegularExpression.Text))
            {
                bUseAsStart.Enabled = true;
                bUseAsEnd.Enabled = true;
                CurrentVar.Regex = string.Empty;
                RefreshRtfFormatting();
                return;
            }

            // We cannot (or rather don't want to) combine those to for now
            bUseAsEnd.Enabled = false;
            bUseAsStart.Enabled = false;

            try
            {
                Regex regex = new Regex(txtRegularExpression.Text, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                CurrentVar.Regex = regex.ToString();
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
                if (string.IsNullOrEmpty(pair.Value.Url))
                {
                    toDelete.Add(pair.Key);
                }
            }
            while (toDelete.Count > 0) { m_Variables.Remove(toDelete[0]); toDelete.RemoveAt(0); }

            // Save results
            m_Job.Variables = m_Variables;
        }

    }
}
