using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using CDBurnerXP;

namespace Ketarin.Forms
{
    public partial class SettingsDialog : Form
    {
        #region Properties

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtDefaultCommand.Text = Settings.GetValue("DefaultCommand", "") as string;
            chkUpdateAtStartup.Checked = (bool)Settings.GetValue("UpdateAtStartup", false);
            txtCustomColumn.Text = Settings.GetValue("CustomColumn", "") as string;
            chkAvoidBeta.Checked = (bool)Settings.GetValue("AvoidFileHippoBeta", false);
            nConnectionTimeout.Value = Convert.ToDecimal(Settings.GetValue("ConnectionTimeout", 10.0));

            nProxyPort.Value = Convert.ToInt16(Settings.GetValue("ProxyPort", 0));
            txtProxyServer.Text = Settings.GetValue("ProxyServer", "") as string;
            txtProxyUser.Text = Settings.GetValue("ProxyUser", "") as string;
            txtProxyPassword.Text = Settings.GetValue("ProxyPassword", "") as string;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Settings.SetValue("DefaultCommand", txtDefaultCommand.Text);
            Settings.SetValue("UpdateAtStartup", chkUpdateAtStartup.Checked);
            Settings.SetValue("CustomColumn", txtCustomColumn.Text);
            Settings.SetValue("AvoidFileHippoBeta", chkAvoidBeta.Checked);
            Settings.SetValue("ConnectionTimeout", nConnectionTimeout.Value);

            Settings.SetValue("ProxyPort", nProxyPort.Value);
            Settings.SetValue("ProxyServer", txtProxyServer.Text);
            Settings.SetValue("ProxyUser", txtProxyUser.Text);
            Settings.SetValue("ProxyPassword", txtProxyPassword.Text);

            WebRequest.DefaultWebProxy = DbManager.Proxy;
        }
    }
}
