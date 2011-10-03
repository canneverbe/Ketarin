using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Serialization;
using System.Data.SQLite;
using CDBurnerXP;

namespace Ketarin.Forms
{
    public partial class SettingsDialog : Form
    {
        #region HotkeyTextBox

        /// <summary>
        /// Represents a TextBox that allows a user to press keys for hotkey assignment.
        /// </summary>
        private class HotkeyTextBox : TextBox
        {
            private Keys resultKeys = Keys.None;

            public Keys ResultKeys
            {
                get { return resultKeys; }
            }

            public override string Text
            {
                get
                {
                    return base.Text;
                }
                set
                {
                    base.Text = value;
                    this.resultKeys = Keys.None;
                }
            }

            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                foreach (Keys key in Hotkey.EndKeys)
                {
                    if ((keyData & key) == key)
                    {
                        this.Text = Hotkey.GetShortcutString(keyData);
                        this.resultKeys = keyData;
                        return true;
                    }
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion

        private int currentSelectedCommandEvent = -1;
        private DataTable globalVarsTable = new DataTable();
        private SerializableDictionary<string, string> cachedCustomColumns = new SerializableDictionary<string, string>();
        private bool customColumnsChanged = false;
        private List<Hotkey> hotkeys = new List<Hotkey>();
        private Hotkey currentSelectedHotkey = null;

        #region Properties

        /// <summary>
        /// Gets whether or not the custom columns have been user changed.
        /// </summary>
        public bool CustomColumnsChanged
        {
            get { return customColumnsChanged; }
        }

        /// <summary>
        /// Gets all currently used custom columns.
        /// </summary>
        public static SerializableDictionary<string, string> CustomColumns
        {
            get
            {
                string customColumnsXml = Settings.GetValue("CustomColumns", "") as string;
                if (!string.IsNullOrEmpty(customColumnsXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                    using (StringReader reader = new StringReader(customColumnsXml))
                    {
                        return serializer.Deserialize(reader) as SerializableDictionary<string, string>;
                    }
                }

                SerializableDictionary<string, string> oldColumns = new SerializableDictionary<string, string>();

                // Convert old custom columns if necessary
                string custOld1 = Settings.GetValue("CustomColumn", null) as string;
                if (custOld1 != null)
                {
                    oldColumns.Add("Custom Value", custOld1);
                }
                string custOld2 = Settings.GetValue("CustomColumn2", null) as string;
                if (custOld2 != null)
                {
                    oldColumns.Add("Custom Value 2", custOld2);
                }

                return oldColumns;
            }
            set
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, value);
                    Settings.SetValue("CustomColumns", writer.ToString());
                }
            }
        }

        #endregion

        public SettingsDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            this.globalVarsTable.Columns.Add("Name");
            this.globalVarsTable.Columns.Add("Value");

            gridGlobalVariables.DataSource = this.globalVarsTable;
            gridGlobalVariables.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridGlobalVariables.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.hotkeys = Hotkey.GetHotkeys();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Restore global variables
            UrlVariable.ReloadGlobalVariables();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadSettings();

            cboCommandEvent.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the user controls based on the settings stored in the database.
        /// </summary>
        private void LoadSettings()
        {
            this.cachedCustomColumns = CustomColumns;
            olvCustomColumns.SetObjects(this.cachedCustomColumns);

            chkUpdateAtStartup.Checked = (bool)Settings.GetValue("UpdateAtStartup", false);
            chkBackups.Checked = (bool)Settings.GetValue("CreateDatabaseBackups", true);
            chkAvoidBeta.Checked = (bool)Settings.GetValue("AvoidFileHippoBeta", false);
            chkUpdateOnlineDatabase.Checked = (bool)Settings.GetValue("UpdateOnlineDatabase", true);
            nConnectionTimeout.Value = Convert.ToDecimal(Settings.GetValue("ConnectionTimeout", 10.0));
            nNumThreads.Value = Convert.ToDecimal(Settings.GetValue("ThreadCount", 2));
            nNumRetries.Value = Convert.ToDecimal(Settings.GetValue("RetryCount", 1));
            chkMinToTray.Checked = (bool)Settings.GetValue("MinimizeToTray", false);
            chkOpenWebsite.Checked = (bool)Settings.GetValue("OpenWebsiteOnDoubleClick", false);

            nProxyPort.Value = Convert.ToInt16(Settings.GetValue("ProxyPort", 0));
            txtProxyServer.Text = Settings.GetValue("ProxyServer", "") as string;
            txtProxyUser.Text = Settings.GetValue("ProxyUser", "") as string;
            txtProxyPassword.Text = Settings.GetValue("ProxyPassword", "") as string;
            LoadCommand();
            LoadGlobalVariables();

            lbActions.DataSource = hotkeys;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            SaveCurrentCommand();
            UpdateHotkey();

            CustomColumns = this.cachedCustomColumns;

            // Remove old custom columns
            Settings.SetValue("CustomColumn", null);
            Settings.SetValue("CustomColumn2", null);

            Settings.SetValue("UpdateAtStartup", chkUpdateAtStartup.Checked);
            Settings.SetValue("AvoidFileHippoBeta", chkAvoidBeta.Checked);
            Settings.SetValue("ConnectionTimeout", nConnectionTimeout.Value);
            Settings.SetValue("ThreadCount", Convert.ToInt32(nNumThreads.Value));
            Settings.SetValue("RetryCount", Convert.ToInt32(nNumRetries.Value));
            Settings.SetValue("UpdateOnlineDatabase", chkUpdateOnlineDatabase.Checked);
            Settings.SetValue("MinimizeToTray", chkMinToTray.Checked);
            Settings.SetValue("CreateDatabaseBackups", chkBackups.Checked);
            Settings.SetValue("OpenWebsiteOnDoubleClick", chkOpenWebsite.Checked);

            Settings.SetValue("ProxyPort", nProxyPort.Value);
            Settings.SetValue("ProxyServer", txtProxyServer.Text);
            Settings.SetValue("ProxyUser", txtProxyUser.Text);
            Settings.SetValue("ProxyPassword", txtProxyPassword.Text);

            WebRequest.DefaultWebProxy = DbManager.Proxy;

            SaveGlobalVariables();

            foreach (Hotkey hotkey in this.hotkeys)
            {
                Settings.SetValue("Hotkey: " + hotkey.Name, hotkey.Shortcut);
            }
        }

        private void bExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "XML file|*.xml";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        SettingsExporter.ExportToFile(dialog.FileName);                     
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Failed to export the settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }            
        }

        private void bImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML file|*.xml";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        SettingsExporter.ImportFromFile(dialog.FileName);
                        LoadSettings();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Failed to import the settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }    
        }

        #region Global variables

        private void LoadGlobalVariables()
        {
            this.globalVarsTable.Rows.Clear();

            foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
            {
                this.globalVarsTable.Rows.Add(new string[] { var.Name, var.CachedContent });
            }
        }

        private void SaveGlobalVariables()
        {
            UrlVariable.GlobalVariables.Clear();

            foreach (DataRow row in this.globalVarsTable.Rows)
            {
                string varName = row[0] as string;
                // Skip variables without name
                if (string.IsNullOrEmpty(varName)) continue;

                UrlVariable newVariable = new UrlVariable(varName, null);
                newVariable.CachedContent = row[1] as string;
                UrlVariable.GlobalVariables[varName] = newVariable;
            }

            UrlVariable.GlobalVariables.Save();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Saves the command that is currently being edited to the database.
        /// </summary>
        private void SaveCurrentCommand()
        {
            switch (this.currentSelectedCommandEvent)
            {
                case 0:
                    Settings.SetValue("PreUpdateCommand", commandControl.Text);
                    Settings.SetValue("PreUpdateCommandType", commandControl.CommandType.ToString());
                    break;

                case 1:
                    Settings.SetValue("DefaultCommand", commandControl.Text);
                    Settings.SetValue("DefaultCommandType", commandControl.CommandType.ToString());
                    break;

                case 2:
                    Settings.SetValue("PostUpdateCommand", commandControl.Text);
                    Settings.SetValue("PostUpdateCommandType", commandControl.CommandType.ToString());
                    break;
            }            
        }

        private void cboCommandEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save current command
            SaveCurrentCommand();

            LoadCommand();

            this.currentSelectedCommandEvent = cboCommandEvent.SelectedIndex;
        }

        private void LoadCommand()
        {
            // Load other command
            switch (cboCommandEvent.SelectedIndex)
            {
                case 0:
                    commandControl.Text = Settings.GetValue("PreUpdateCommand", "") as string;
                    commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("PreUpdateCommandType", ScriptType.Batch.ToString()) as string);
                    break;

                case 1:
                    commandControl.Text = Settings.GetValue("DefaultCommand", "") as string;
                    commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("DefaultCommandType", ScriptType.Batch.ToString()) as string);
                    break;

                case 2:
                    commandControl.Text = Settings.GetValue("PostUpdateCommand", "") as string;
                    commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("PostUpdateCommandType", ScriptType.Batch.ToString()) as string);
                    break;
            }
        }

        #endregion

        #region Custom Columns

        private void bAddCustomColumn_Click(object sender, EventArgs e)
        {
            using (AddCustomColumnDialog dialog = new AddCustomColumnDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.cachedCustomColumns[dialog.ColumnName] = dialog.ColumnValue;
                    this.olvCustomColumns.SetObjects(cachedCustomColumns);
                    this.customColumnsChanged = true;
                }
            }
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (olvCustomColumns.SelectedObject != null)
            {
                this.cachedCustomColumns.Remove(((KeyValuePair<string, string>)olvCustomColumns.SelectedObject).Key);
                this.olvCustomColumns.SetObjects(cachedCustomColumns);
                this.customColumnsChanged = true;
            }
        }

        private void olvCustomColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            bRemove.Enabled = (olvCustomColumns.SelectedIndex >= 0);
            bEdit.Enabled = (olvCustomColumns.SelectedIndex >= 0);
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (olvCustomColumns.SelectedObject == null) return;

            int index = olvCustomColumns.SelectedIndex;

            KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>)olvCustomColumns.SelectedObject;
            using (AddCustomColumnDialog dialog = new AddCustomColumnDialog())
            {
                dialog.ColumnName = selectedItem.Key;
                dialog.ColumnValue = selectedItem.Value;
                dialog.Text = "Edit " + selectedItem.Key;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.cachedCustomColumns.Remove(selectedItem.Key);
                    this.cachedCustomColumns[dialog.ColumnName] = dialog.ColumnValue;
                    this.olvCustomColumns.SetObjects(cachedCustomColumns);
                    this.olvCustomColumns.SelectedIndex = index;
                    this.customColumnsChanged = true;
                }
            }
        }

        private void olvCustomColumns_DoubleClick(object sender, EventArgs e)
        {
            bEdit.PerformClick();
        }

        #endregion

        #region Hotkeys

        private void lbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHotkey();

            this.currentSelectedHotkey = lbActions.SelectedItem as Hotkey;

            txtHotkeyKeys.Text = this.currentSelectedHotkey.Shortcut;
        }

        private void bDoubleClick_Click(object sender, EventArgs e)
        {
            if (this.currentSelectedHotkey != null)
            {
                this.currentSelectedHotkey.SetDoubleclickShortcut(Control.ModifierKeys);
                this.txtHotkeyKeys.Text = this.currentSelectedHotkey.Shortcut;
            }
        }

        private void UpdateHotkey()
        {
            if (this.currentSelectedHotkey != null)
            {
                if (string.IsNullOrEmpty(txtHotkeyKeys.Text))
                {
                    this.currentSelectedHotkey.SetKeyShortcut(Keys.None);
                }
                else if (txtHotkeyKeys.ResultKeys != Keys.None)
                {
                    this.currentSelectedHotkey.SetKeyShortcut(txtHotkeyKeys.ResultKeys);
                }
            }
        }

        #endregion
    }
}
