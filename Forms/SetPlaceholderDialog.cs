using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which allows the user to enter 
    /// a value for a placeholder within a template
    /// </summary>
    public partial class SetPlaceholderDialog : Form
    {
        private Dictionary<string, Placeholder> placeholders = new Dictionary<string, Placeholder>();

        #region Placeholder

        /// <summary>
        /// Represents a placeholder. Any number of them can be requested in the dialog.
        /// </summary>
        private class Placeholder
        {
            public string Name { get; set; }
            public string Options { get; set; }
            public string Value { get; set; }
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
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (Control control in tblMain.Controls)
                {
                    Placeholder placeholder = control.Tag as Placeholder;
                    if (placeholder != null)
                    {
                        result[placeholder.Name] = control.Text;
                    }
                }
                return result;
            }
        }

        #endregion

        public SetPlaceholderDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Add a TextBox or a ComboBox for each placeholder
            foreach (Placeholder placeholder in this.placeholders.Values)
            {
                Label placeholderLabel = new Label();
                placeholderLabel.Text = placeholder.Name + ":";
                placeholderLabel.Dock = DockStyle.Fill;
                placeholderLabel.AutoSize = true;
                placeholderLabel.TextAlign = ContentAlignment.MiddleLeft;
                tblMain.Controls.Add(placeholderLabel);

                if (string.IsNullOrEmpty(placeholder.Options))
                {
                    TextBox placeholderTextBox = new TextBox();
                    placeholderTextBox.Tag = placeholder;
                    placeholderTextBox.Dock = DockStyle.Fill;
                    placeholderTextBox.Text = placeholder.Value ?? string.Empty;
                    tblMain.Controls.Add(placeholderTextBox);
                }
                else
                {
                    ComboBox placeholderTextBox = new ComboBox();
                    placeholderTextBox.Tag = placeholder;
                    placeholderTextBox.Dock = DockStyle.Fill;
                    tblMain.Controls.Add(placeholderTextBox);
                    placeholderTextBox.Items.AddRange(placeholder.Options.Split('|'));
                    placeholderTextBox.Text = placeholder.Value ?? string.Empty;
                    if (placeholderTextBox.Items.Count > 0)
                    {
                        placeholderTextBox.SelectedIndex = 0;
                    }
                }
            }
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

            return base.ShowDialog(owner);
        }

        /// <summary>
        /// Adds a placeholder to request a value for to the dialog.
        /// </summary>
        /// <param name="name">Name of placeholder (description)</param>
        /// <param name="options">Options to offer for the placeholder (pipe separated), ComboBox</param>
        /// <param name="value">Default value for the placeholder</param>
        internal void AddPlaceHolder(string name, string options, string value)
        {
            this.placeholders[name] = new Placeholder() { Name = name, Options = options, Value = value };
        }
    }
}
