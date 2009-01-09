// Ketarin - Copyright (C) 2008  Canneverbe Limited
// The icons used within the application are *NOT* licensed under the GNU General Public License.

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CDBurnerXP;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net;

namespace Ketarin
{
    static class Program
    {
        private static NotifyIcon m_Icon = null;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialise database
            try
            {
                DbManager.CreateOrUpgradeDatabase();

                WebRequest.DefaultWebProxy = DbManager.Proxy;
                if (Settings.GetValue("AuthorGuid") == null)
                {
                    Settings.SetValue("AuthorGuid", Guid.NewGuid().ToString("B"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create or load the database file: " + ex.Message);
                return;
            }

            // Either run silently on command line or launch GUI
            if (args.Length > 0)
            {
                List<string> arguments = new List<string>(args);
                if (arguments.Contains("/SILENT"))
                {
                    Kernel32.AttachConsole(Kernel32.ATTACH_PARENT_PROCESS);

                    ApplicationJob[] jobs = DbManager.GetJobs();
                    Updater updater = new Updater();
                    updater.StatusChanged += new EventHandler<Updater.JobStatusChangedEventArgs>(updater_StatusChanged);
                    updater.ProgressChanged += new EventHandler<Updater.JobProgressChangedEventArgs>(updater_ProgressChanged);
                    updater.BeginUpdate(jobs, false);

                    if (arguments.Contains("/NOTIFY"))
                    {
                        m_Icon = new NotifyIcon();
                        m_Icon.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.Restart.GetHicon());
                        m_Icon.Text = "Ketarin is working...";
                        m_Icon.Visible = true;
                    }

                    while (updater.IsBusy)
                    {
                        Thread.Sleep(1000);
                    }

                    if (m_Icon != null)
                    {
                        m_Icon.Dispose();
                    }

                    Kernel32.FreeConsole();
                }
            }
            else
            {
                Application.Run(new MainForm());
            }
        }

        #region Command line updater

        static void updater_ProgressChanged(object sender, Updater.JobProgressChangedEventArgs e)
        {
            Console.Write(" " + e.ProgressPercentage + "%");
        }

        static void updater_StatusChanged(object sender, Updater.JobStatusChangedEventArgs e)
        {
            if (e.NewStatus == Updater.Status.Downloading)
            {
                // No status of interest
                return;
            }

            string status = e.ApplicationJob.Name + ": ";

            switch (e.NewStatus)
            {
                case Updater.Status.Failure:
                    status += "Failed.";
                    break;

                case Updater.Status.NoUpdate:
                    status += "No update available.";
                    break;

                case Updater.Status.UpdateSuccessful:
                    status += "Update successful.";
                    break;
            }

            if (m_Icon != null)
            {
                m_Icon.ShowBalloonTip(2000, "Ketarin", status, (e.NewStatus == Updater.Status.Failure ? ToolTipIcon.Error : ToolTipIcon.Info));
            }
        }

        #endregion
    }
}
