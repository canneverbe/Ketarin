using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Ketarin.Forms
{
    public partial class EditVariablesDialog : Form
    {
        private ApplicationJob m_Job = null;

        #region Properties

        private UrlVariable CurrentVar
        {
            get
            {
                string name = lbVariables.SelectedItem as string;
                if (m_Job.Variables.ContainsKey(name))
                {
                    return m_Job.Variables[name];
                }
                return null;
            }
        }

        #endregion

        public EditVariablesDialog(ApplicationJob job) : base()
        {
            InitializeComponent();
            AcceptButton = bOK;
            CancelButton = bCancel;
            
            m_Job = job;

            RefreshListBox();
        }

        private void RefreshListBox()
        {
            lbVariables.Items.Clear();
            foreach (KeyValuePair<string, UrlVariable> pair in m_Job.Variables)
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

            if (enable)
            {
                txtUrl.Focus();
                txtUrl.Text = CurrentVar.Url;
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
                    if (m_Job.Variables.ContainsKey(dialog.VariableName))
                    {
                        string msg = string.Format("The variable name '{0}' already exists.", dialog.VariableName);
                        MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    m_Job.Variables.Add(dialog.VariableName, new UrlVariable(dialog.VariableName));
                    RefreshListBox();
                    lbVariables.SelectedItem = dialog.VariableName;
                }
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Uri url = new Uri(txtUrl.Text);
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
                    rtfContent.Text = client.DownloadString(txtUrl.Text);
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

            rtfContent.SelectionStart = 0;
            rtfContent.SelectionLength = rtfContent.Text.Length;
            rtfContent.SelectionColor = SystemColors.WindowText;
            rtfContent.SelectionFont = rtfContent.Font;
            rtfContent.SelectionLength = 0;

            // Highlight StartText if specified
            if (string.IsNullOrEmpty(CurrentVar.StartText)) return;
            int pos = rtfContent.Text.IndexOf(CurrentVar.StartText);
            rtfContent.SelectionStart = pos;
            rtfContent.SelectionLength = CurrentVar.StartText.Length;
            rtfContent.SelectionColor = Color.Blue;

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

        private void rtfContent_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = (rtfContent.SelectionLength > 0);
            bUseAsEnd.Enabled = bUseAsStart.Enabled = enable;
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            m_Job.Variables.Remove(CurrentVar.Name);
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

    }
}
