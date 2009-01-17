using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Ketarin.Forms
{
    /// <summary>
    /// A usual textbox plus variable selection tool.
    /// </summary>
    class VariableTextBox : TextBox
    {
        private string[] m_VariableNames = new string[0];
        private ContextMenuCustomiser m_Customiser = null;
        private bool m_EnableEditor = true;

        #region Properties

        /// <summary>
        /// Gets or sets the variable names to show
        /// within the context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string[] VariableNames
        {
            get { return m_VariableNames; }
            set {
                if (m_VariableNames != value)
                {
                    m_VariableNames = value;
                    RebuildContextMenu();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not an editor for the 
        /// text box can be opened from the context menu.
        /// </summary>
        [DefaultValue(true)]
        public bool EnableEditor
        {
            get { return m_EnableEditor; }
            set { m_EnableEditor = value; }
        }

        #endregion

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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            m_Customiser = new ContextMenuCustomiser(this);
            RebuildContextMenu();
        }

        /// <summary>
        /// Adds all context menu items based on the given variable names.
        /// </summary>
        private void RebuildContextMenu()
        {
            if (m_Customiser == null) return;

            m_Customiser.MenuItems.Clear();

            // No options for read only text boxes
            if (ReadOnly) return;

            if (m_VariableNames.Length > 0)
            {

                List<string> vars = new List<string>(m_VariableNames);
                vars.Sort();
                // We insert in reverse order
                vars.Reverse();

                // Add final separator
                m_Customiser.MenuItems.Add(new ContextMenuItem(string.Empty, 0));

                foreach (string var in vars)
                {
                    ContextMenuItem newItem = new ContextMenuItem("{" + var + "}", 0);
                    newItem.EventHandler = OnVariableSelected;
                    m_Customiser.MenuItems.Add(newItem);
                }
            }

            // Add context menu item for multiline editor
            if (Multiline && m_EnableEditor)
            {
                // Add final separator
                m_Customiser.MenuItems.Add(new ContextMenuItem(string.Empty, 0));

                ContextMenuItem newItem = new ContextMenuItem("Editor...", 0);
                newItem.EventHandler = delegate(ContextMenuItem item)
                {
                    using (MultilineEditorDialog dialog = new MultilineEditorDialog())
                    {
                        dialog.Value = this.Text;
                        dialog.SetVariableNames(m_VariableNames);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            this.Text = dialog.Value;
                        }
                    }
                };
                m_Customiser.MenuItems.Add(newItem);
            }
        }

        /// <summary>
        /// Inserts the selected variable at the current cursor position.
        /// </summary>
        private void OnVariableSelected(ContextMenuItem menuItem)
        {
            int selStart = SelectionStart;
            Text = Text.Insert(selStart, menuItem.Text);
            SelectionStart = selStart + menuItem.Text.Length;
        }
    }
}
