using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Data.SQLite;
using CDBurnerXP;

namespace Ketarin.Forms
{
    public partial class SettingsDialog : Form
    {
        #region Properties

        /// <summary>
        /// Returns the name of the custom column variable, without { and }.
        /// </summary>
        public static string CustomColumnVariableName
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

        #endregion

        public SettingsDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
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
            txtDefaultCommand.Text = Settings.GetValue("DefaultCommand", "") as string;
            chkUpdateAtStartup.Checked = (bool)Settings.GetValue("UpdateAtStartup", false);
            txtCustomColumn.Text = Settings.GetValue("CustomColumn", "") as string;
            chkAvoidBeta.Checked = (bool)Settings.GetValue("AvoidFileHippoBeta", false);
            chkUpdateOnlineDatabase.Checked = (bool)Settings.GetValue("UpdateOnlineDatabase", true);
            nConnectionTimeout.Value = Convert.ToDecimal(Settings.GetValue("ConnectionTimeout", 10.0));
            nNumThreads.Value = Convert.ToDecimal(Settings.GetValue("ThreadCount", 2));
            nNumRetries.Value = Convert.ToDecimal(Settings.GetValue("RetryCount", 1));
            chkMinToTray.Checked = (bool)Settings.GetValue("MinimizeToTray", false);
            
            nProxyPort.Value = Convert.ToInt16(Settings.GetValue("ProxyPort", 0));
            txtProxyServer.Text = Settings.GetValue("ProxyServer", "") as string;
            txtProxyUser.Text = Settings.GetValue("ProxyUser", "") as string;
            txtProxyPassword.Text = Settings.GetValue("ProxyPassword", "") as string;

            LoadGlobalVariables();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Settings.SetValue("DefaultCommand", txtDefaultCommand.Text);
            Settings.SetValue("UpdateAtStartup", chkUpdateAtStartup.Checked);
            Settings.SetValue("CustomColumn", txtCustomColumn.Text);
            Settings.SetValue("AvoidFileHippoBeta", chkAvoidBeta.Checked);
            Settings.SetValue("ConnectionTimeout", nConnectionTimeout.Value);
            Settings.SetValue("ThreadCount", Convert.ToInt32(nNumThreads.Value));
            Settings.SetValue("RetryCount", Convert.ToInt32(nNumRetries.Value));
            Settings.SetValue("UpdateOnlineDatabase", chkUpdateOnlineDatabase.Checked);
            Settings.SetValue("MinimizeToTray", chkMinToTray.Checked);

            Settings.SetValue("ProxyPort", nProxyPort.Value);
            Settings.SetValue("ProxyServer", txtProxyServer.Text);
            Settings.SetValue("ProxyUser", txtProxyUser.Text);
            Settings.SetValue("ProxyPassword", txtProxyPassword.Text);

            WebRequest.DefaultWebProxy = DbManager.Proxy;

            SaveGlobalVariables();
        }

        #region Global variables

        private void LoadGlobalVariables()
        {
            cboGlobalVariables.Items.Clear();

            foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
            {
                cboGlobalVariables.Items.Add(var);
            }

            if (cboGlobalVariables.Items.Count > 0)
            {
                cboGlobalVariables.SelectedIndex = 0;
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

                foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
                {
                    var.Save(transaction, Guid.Empty);
                }

                transaction.Commit();
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            using (NewVariableDialog dialog = new NewVariableDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Check for duplicate variables
                    if (UrlVariable.GlobalVariables.ContainsKey(dialog.VariableName))
                    {
                        string msg = string.Format("The variable name '{0}' already exists.", dialog.VariableName);
                        MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    UrlVariable newVariable = new UrlVariable(dialog.VariableName, null);
                    UrlVariable.GlobalVariables.Add(dialog.VariableName, newVariable);

                    cboGlobalVariables.Items.Add(newVariable);
                    cboGlobalVariables.SelectedItem = newVariable;
                }
            }
        }

        private void cboGlobalVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            UrlVariable current = cboGlobalVariables.SelectedItem as UrlVariable;
            if (current != null)
            {
                txtGlobalVariableValue.Text = current.CachedContent;
            }
            bRemove.Enabled = (current != null);
        }

        private void txtGlobalVariableValue_TextChanged(object sender, EventArgs e)
        {
            UrlVariable current = cboGlobalVariables.SelectedItem as UrlVariable;
            if (current != null)
            {
                current.CachedContent = txtGlobalVariableValue.Text;
            }
        }

        private void bRemove_Click(object sender, EventArgs e)
        {
            UrlVariable current = cboGlobalVariables.SelectedItem as UrlVariable;
            if (current != null)
            {
                cboGlobalVariables.Items.Remove(current);
                if (cboGlobalVariables.Items.Count > 0)
                {
                    cboGlobalVariables.SelectedIndex = 0;
                }
                else
                {
                    bRemove.Enabled = false;
                    txtGlobalVariableValue.Text = string.Empty;
                }
                
                UrlVariable.GlobalVariables.Remove(current.Name);
            }
        }

        #endregion
    }
}
