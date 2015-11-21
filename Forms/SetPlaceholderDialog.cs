using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which allows the user to enter 
    /// a value for a placeholder within a template
    /// </summary>
    public partial class SetPlaceholderDialog : PersistentForm
    {
        private readonly Dictionary<string, Placeholder> placeholders = new Dictionary<string, Placeholder>();
        private readonly string templateXml = string.Empty;
        private readonly Dictionary<string, string> result = new Dictionary<string, string>();
        private bool automaticallyDetermine = true;

        #region Placeholder

        /// <summary>
        /// Represents a placeholder. Any number of them can be requested in the dialog.
        /// </summary>
        private class Placeholder
        {
            public string Name { get; set; }
            public string[] Options { get; set; }
            public string Value { get; set; }
            public string Variable { get; set; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of all entered placeholder values.
        /// </summary>
        public Dictionary<string, string> Placeholders
        {
            get
            {
                foreach (Control control in tblMain.Controls)
                {
                    Placeholder placeholder = control.Tag as Placeholder;
                    // Only copy the values from controls that have not been determined automatically
                    if (placeholder != null && (!result.ContainsKey(placeholder.Name) || string.IsNullOrEmpty(result[placeholder.Name])))
                    {
                        result[placeholder.Name] = control.Text;
                    }
                }
                return result;
            }
        }

        #endregion

        public SetPlaceholderDialog(string templateXml)
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            this.templateXml = templateXml;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.SuspendLayout();

            // Add a TextBox or a ComboBox for each placeholder
            foreach (Placeholder placeholder in this.placeholders.Values)
            {
                Label placeholderLabel = new Label
                {
                    Text = placeholder.Name + ":",
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                tblMain.Controls.Add(placeholderLabel);

                Control placeholderEditControl;

                if (placeholder.Options.Length == 0)
                {
                    TextBox placeholderTextBox = new TextBox();
                    placeholderEditControl = placeholderTextBox;
                    placeholderTextBox.Tag = placeholder;
                    placeholderTextBox.Dock = DockStyle.Fill;
                    placeholderTextBox.Text = placeholder.Value ?? string.Empty;
                    tblMain.Controls.Add(placeholderTextBox);
                }
                else
                {
                    ComboBox placeholderTextBox = new ComboBox();
                    placeholderEditControl = placeholderTextBox;
                    placeholderTextBox.Tag = placeholder;
                    placeholderTextBox.Dock = DockStyle.Fill;
                    tblMain.Controls.Add(placeholderTextBox);
                    if (string.Compare(placeholder.Options[0], "{categories}", true) == 0)
                    {
                        placeholderTextBox.Items.AddRange(DbManager.GetCategories());
                    }
                    else
                    {
                        placeholderTextBox.Items.AddRange(placeholder.Options);
                        placeholderTextBox.Text = placeholder.Value ?? string.Empty;
                        if (placeholderTextBox.Items.Count > 0)
                        {
                            placeholderTextBox.SelectedIndex = 0;
                        }
                    }
                }

                tblMain.RowStyles[tblMain.RowCount - 1] = new RowStyle(SizeType.AutoSize);

                // Hide for now, try to determine automatically
                if (!string.IsNullOrEmpty(placeholder.Variable))
                {
                    placeholderLabel.Visible = false;
                    placeholderEditControl.Visible = false;
                }
            }

            this.ResumeLayout(true);
        }

        /// <summary>
        /// Shows the dialog and requests placeholders if necessary.
        /// </summary>
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            // Do not show if no placeholders exist
            if (this.placeholders.Count == 0)
            {
                return DialogResult.OK;
            }

            // Do not show dialog if all placeholders are determined automatically
            // Though that should not usually happen.
            bool allVariables = true;
            foreach (Placeholder placeholder in this.placeholders.Values)
            {
                if (string.IsNullOrEmpty(placeholder.Variable))
                {
                    allVariables = false;
                }
                else
                {
                    // Initial value for XML parsing required
                    this.result[placeholder.Name] = string.Empty;
                }
            }

            if (allVariables && DeterminePlaceholders(owner))
            {
                return DialogResult.OK;
            }
            else
            {
                return base.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Adds a placeholder to request a value for to the dialog.
        /// </summary>
        /// <param name="name">Name of placeholder (description)</param>
        /// <param name="options">Options to offer for the placeholder (pipe separated), ComboBox</param>
        /// <param name="value">Default value for the placeholder</param>
        internal void AddPlaceHolder(string name, string[] options, string value, string variable)
        {
            this.placeholders[name] = new Placeholder() { Name = name, Options = options, Value = value, Variable = variable };
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DeterminePlaceholders(this);
        }

        /// <summary>
        /// Tries to determine the variable based placeholders.
        /// </summary>
        /// <returns>true if the placeholders could be determined automatically, false otherwise</returns>
        private bool DeterminePlaceholders(IWin32Window owner)
        {
            if (!this.automaticallyDetermine)
            {
                return false;
            }

            // Try to determine variable-based placeholders
            UseWaitCursor = true;
            Application.DoEvents();

            try
            {
                this.automaticallyDetermine = false;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.templateXml);
                ApplicationJob.SetPlaceholders(doc, this.Placeholders);

                ApplicationJob[] jobs = ApplicationJob.ImportFromXmlString(doc.OuterXml, false);

                foreach (Placeholder placeholder in this.placeholders.Values)
                {
                    if (!string.IsNullOrEmpty(placeholder.Variable))
                    {
                        string newValue = jobs[0].Variables.ReplaceAllInString("{" + placeholder.Variable + "}");
                        if (string.IsNullOrEmpty(newValue))
                        {
                            throw new ApplicationException("\"" + placeholder.Name + "\" is undefined.");
                        }

                        this.result[placeholder.Name] = newValue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, "Some information cannot not automatically be determined: " + ex.Message + "\r\n\r\nPlease enter the required information manually.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                foreach (Control control in tblMain.Controls)
                {
                    control.Visible = !control.Visible;
                }

                DialogResult = DialogResult.None;
                return false;
            }
            finally
            {
                UseWaitCursor = false;
            }

            return true;
        }
    }
}
