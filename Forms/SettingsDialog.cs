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
        private int currentSelectedCommandEvent = -1;
        private DataTable globalVarsTable = new DataTable();

        #region Properties

        /// <summary>
        /// Returns the name of the custom column variable, without { and }.
        /// </summary>
        public static string CustomColumnVariableName1
        {
            get
            {
                string var = Settings.GetValue("CustomColumn") as string;
                if (!string.IsNullOrEmpty(var))
                {
                    return var.Trim('{', '}');
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the name of the second custom column variable, without { and }.
        /// </summary>
        public static string CustomColumnVariableName2
        {
            get
            {
                string var = Settings.GetValue("CustomColumn2") as string;
                if (!string.IsNullOrEmpty(var))
                {
                    return var.Trim('{', '}');
                }
                return string.Empty;
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

            LoadGlobalVariables();
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
            chkUpdateAtStartup.Checked = (bool)Settings.GetValue("UpdateAtStartup", false);
            chkBackups.Checked = (bool)Settings.GetValue("CreateDatabaseBackups", true);
            txtCustomColumn.Text = Settings.GetValue("CustomColumn", "") as string;
            txtCustomColumn2.Text = Settings.GetValue("CustomColumn2", "") as string;
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
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            SaveCurrentCommand();

            Settings.SetValue("UpdateAtStartup", chkUpdateAtStartup.Checked);
            Settings.SetValue("CustomColumn", txtCustomColumn.Text);
            Settings.SetValue("CustomColumn2", txtCustomColumn2.Text);
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
                        using (FileStream stream = File.Open(dialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            // Export settings to XML file
                            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                            serializer.Serialize(stream, DbManager.GetSettings());
                        }
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
                        using (FileStream stream = File.OpenRead(dialog.FileName))
                        {
                            // Import settings from file as dictionary
                            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                            DbManager.SetSettings(serializer.Deserialize(stream) as Dictionary<string, string>);
                            LoadSettings();
                        }
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
            using (IDbTransaction transaction = DbManager.Connection.BeginTransaction())
            {
                using (IDbCommand comm = DbManager.Connection.CreateCommand())
                {
                    comm.Transaction = transaction;
                    comm.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                    comm.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(Guid.Empty)));
                    comm.ExecuteNonQuery();
                }

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

                foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
                {
                    var.Save(transaction, Guid.Empty);
                }

                transaction.Commit();
            }
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

            this.currentSelectedCommandEvent = cboCommandEvent.SelectedIndex;
        }

        #endregion
    }
}
