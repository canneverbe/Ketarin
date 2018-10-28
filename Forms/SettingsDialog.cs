using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
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
                get { return this.resultKeys; }
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
                if (Hotkey.EndKeys.Any(key => (keyData & key) == key))
                {
                    this.Text = Hotkey.GetShortcutString(keyData);
                    this.resultKeys = keyData;
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion

        private int currentSelectedCommandEvent = -1;
        private readonly DataTable globalVarsTable = new DataTable();
        private SerializableDictionary<string, string> cachedCustomColumns = new SerializableDictionary<string, string>();
        private readonly List<Hotkey> hotkeys = new List<Hotkey>();
        private Hotkey currentSelectedHotkey;

        #region Properties

        /// <summary>
        /// Gets whether or not the custom columns have been user changed.
        /// </summary>
        public bool CustomColumnsChanged { get; private set; }

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
            this.InitializeComponent();

            this.AcceptButton = this.bOK;
            this.CancelButton = this.bCancel;

            this.globalVarsTable.Columns.Add("Name");
            this.globalVarsTable.Columns.Add("Value");

            this.gridGlobalVariables.DataSource = this.globalVarsTable;
            this.gridGlobalVariables.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.gridGlobalVariables.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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

            this.LoadSettings();

            this.cboCommandEvent.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the user controls based on the settings stored in the database.
        /// </summary>
        private void LoadSettings()
        {
            this.cachedCustomColumns = CustomColumns;
            this.olvCustomColumns.SetObjects(this.cachedCustomColumns);

            this.chkUpdateAtStartup.Checked = (bool)Settings.GetValue("UpdateAtStartup", false);
            this.chkBackups.Checked = (bool)Settings.GetValue("CreateDatabaseBackups", true);
            this.chkAvoidBeta.Checked = (bool)Settings.GetValue("AvoidFileHippoBeta", false);
            this.chkUpdateOnlineDatabase.Checked = (bool)Settings.GetValue("UpdateOnlineDatabase", true);
            this.nConnectionTimeout.Value = Convert.ToDecimal(Settings.GetValue("ConnectionTimeout", 10.0));
            this.nNumThreads.Value = Convert.ToDecimal(Settings.GetValue("ThreadCount", 2));
            this.nNumRetries.Value = Convert.ToDecimal(Settings.GetValue("RetryCount", 1));
            this.nNumSegments.Value = Convert.ToDecimal(Settings.GetValue("SegmentCount", 1));
            this.chkMinToTray.Checked = (bool)Settings.GetValue("MinimizeToTray", false);
            this.chkOpenWebsite.Checked = (bool)Settings.GetValue("OpenWebsiteOnDoubleClick", false);
            this.chkAvoidNonBinary.Checked = (bool)Settings.GetValue("AvoidDownloadingNonBinaryFiles", true);

            this.nProxyPort.Value = Convert.ToInt16(Settings.GetValue("ProxyPort", 0));
            this.txtProxyServer.Text = Settings.GetValue("ProxyServer", "") as string;
            this.txtProxyUser.Text = Settings.GetValue("ProxyUser", "") as string;
            this.txtProxyPassword.Text = Settings.GetValue("ProxyPassword", "") as string;
            this.txtUserAgent.Text = Settings.GetValue("DefaultUserAgent", WebClient.DefaultUserAgent) as string;

            this.LoadCommand();
            this.LoadGlobalVariables();

            this.lbActions.DataSource = this.hotkeys;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.SaveCurrentCommand();
            this.UpdateHotkey();

            CustomColumns = this.cachedCustomColumns;

            // Remove old custom columns
            Settings.SetValue("CustomColumn", null);
            Settings.SetValue("CustomColumn2", null);

            Settings.SetValue("UpdateAtStartup", this.chkUpdateAtStartup.Checked);
            Settings.SetValue("AvoidFileHippoBeta", this.chkAvoidBeta.Checked);
            Settings.SetValue("ConnectionTimeout", this.nConnectionTimeout.Value);
            Settings.SetValue("ThreadCount", Convert.ToInt32(this.nNumThreads.Value));
            Settings.SetValue("RetryCount", Convert.ToInt32(this.nNumRetries.Value));
            Settings.SetValue("SegmentCount", Convert.ToInt32(this.nNumSegments.Value));
            Settings.SetValue("UpdateOnlineDatabase", this.chkUpdateOnlineDatabase.Checked);
            Settings.SetValue("MinimizeToTray", this.chkMinToTray.Checked);
            Settings.SetValue("CreateDatabaseBackups", this.chkBackups.Checked);
            Settings.SetValue("OpenWebsiteOnDoubleClick", this.chkOpenWebsite.Checked);
            Settings.SetValue("AvoidDownloadingNonBinaryFiles", this.chkAvoidNonBinary.Checked);

            Settings.SetValue("ProxyPort", this.nProxyPort.Value);
            Settings.SetValue("ProxyServer", this.txtProxyServer.Text);
            Settings.SetValue("ProxyUser", this.txtProxyUser.Text);
            Settings.SetValue("ProxyPassword", this.txtProxyPassword.Text);
            Settings.SetValue("DefaultUserAgent", this.txtUserAgent.Text);

            WebRequest.DefaultWebProxy = DbManager.Proxy;
            WebClient.DefaultUserAgent = this.txtUserAgent.Text;

            this.SaveGlobalVariables();

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
                        this.LoadSettings();
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
                this.globalVarsTable.Rows.Add(var.Name, var.CachedContent);
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

                UrlVariable newVariable = new UrlVariable(varName, null) {CachedContent = row[1] as string};
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
                    Settings.SetValue("PreUpdateCommand", this.commandControl.Text);
                    Settings.SetValue("PreUpdateCommandType", this.commandControl.CommandType.ToString());
                    break;

                case 1:
                    Settings.SetValue("DefaultCommand", this.commandControl.Text);
                    Settings.SetValue("DefaultCommandType", this.commandControl.CommandType.ToString());
                    break;

                case 2:
                    Settings.SetValue("PostUpdateCommand", this.commandControl.Text);
                    Settings.SetValue("PostUpdateCommandType", this.commandControl.CommandType.ToString());
                    break;
            }            
        }

        private void cboCommandEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save current command
            this.SaveCurrentCommand();

            this.LoadCommand();

            this.currentSelectedCommandEvent = this.cboCommandEvent.SelectedIndex;
        }

        private void LoadCommand()
        {
            // Load other command
            switch (this.cboCommandEvent.SelectedIndex)
            {
                case 0:
                    this.commandControl.Text = Settings.GetValue("PreUpdateCommand", "") as string;
                    this.commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("PreUpdateCommandType", ScriptType.Batch.ToString()) as string);
                    break;

                case 1:
                    this.commandControl.Text = Settings.GetValue("DefaultCommand", "") as string;
                    this.commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("DefaultCommandType", ScriptType.Batch.ToString()) as string);
                    break;

                case 2:
                    this.commandControl.Text = Settings.GetValue("PostUpdateCommand", "") as string;
                    this.commandControl.CommandType = Command.ConvertToScriptType(Settings.GetValue("PostUpdateCommandType", ScriptType.Batch.ToString()) as string);
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
                    this.olvCustomColumns.SetObjects(this.cachedCustomColumns);
                    this.CustomColumnsChanged = true;
                }
            }
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            if (this.olvCustomColumns.SelectedObject != null)
            {
                this.cachedCustomColumns.Remove(((KeyValuePair<string, string>) this.olvCustomColumns.SelectedObject).Key);
                this.olvCustomColumns.SetObjects(this.cachedCustomColumns);
                this.CustomColumnsChanged = true;
            }
        }

        private void olvCustomColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bRemove.Enabled = (this.olvCustomColumns.SelectedIndex >= 0);
            this.bEdit.Enabled = (this.olvCustomColumns.SelectedIndex >= 0);
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (this.olvCustomColumns.SelectedObject == null) return;

            int index = this.olvCustomColumns.SelectedIndex;

            KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>) this.olvCustomColumns.SelectedObject;
            using (AddCustomColumnDialog dialog = new AddCustomColumnDialog())
            {
                dialog.ColumnName = selectedItem.Key;
                dialog.ColumnValue = selectedItem.Value;
                dialog.Text = "Edit " + selectedItem.Key;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.cachedCustomColumns.Remove(selectedItem.Key);
                    this.cachedCustomColumns[dialog.ColumnName] = dialog.ColumnValue;
                    this.olvCustomColumns.SetObjects(this.cachedCustomColumns);
                    this.olvCustomColumns.SelectedIndex = index;
                    this.CustomColumnsChanged = true;
                }
            }
        }

        private void olvCustomColumns_DoubleClick(object sender, EventArgs e)
        {
            this.bEdit.PerformClick();
        }

        #endregion

        #region Hotkeys

        private void lbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateHotkey();

            this.currentSelectedHotkey = this.lbActions.SelectedItem as Hotkey;

            this.txtHotkeyKeys.Text = this.currentSelectedHotkey.Shortcut;
        }

        private void bDoubleClick_Click(object sender, EventArgs e)
        {
            if (this.currentSelectedHotkey != null)
            {
                this.currentSelectedHotkey.SetDoubleclickShortcut(ModifierKeys);
                this.txtHotkeyKeys.Text = this.currentSelectedHotkey.Shortcut;
            }
        }

        private void UpdateHotkey()
        {
            if (this.currentSelectedHotkey != null)
            {
                if (string.IsNullOrEmpty(this.txtHotkeyKeys.Text))
                {
                    this.currentSelectedHotkey.SetKeyShortcut(Keys.None);
                }
                else if (this.txtHotkeyKeys.ResultKeys != Keys.None)
                {
                    this.currentSelectedHotkey.SetKeyShortcut(this.txtHotkeyKeys.ResultKeys);
                }
            }
        }

        #endregion
    }
}
