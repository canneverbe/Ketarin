using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP;

namespace Ketarin.Forms
{
    public partial class SettingsDialog : Form
    {
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
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Settings.SetValue("DefaultCommand", txtDefaultCommand.Text);
            Settings.SetValue("UpdateAtStartup", chkUpdateAtStartup.Checked);
            Settings.SetValue("CustomColumn", txtCustomColumn.Text);
            Settings.SetValue("AvoidFileHippoBeta", chkAvoidBeta.Checked);
        }
    }
}
