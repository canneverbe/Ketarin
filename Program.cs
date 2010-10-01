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
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CDBurnerXP;
using Ketarin.Forms;
using Microsoft.Win32;

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

            // Set an error handler (just a message box) for unexpected exceptions in threads
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // Parse command line arguments
            CommandlineArguments arguments = new CommandlineArguments(args);

            // Is a database path set per command line?
            if (!string.IsNullOrEmpty(arguments["database"]))
            {
                DbManager.DatabasePath = arguments["database"];
            }

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

            // Either run silently on command line...
            if (arguments["silent"] != null)
            {
                Kernel32.ManagedAttachConsole(Kernel32.ATTACH_PARENT_PROCESS);

                ApplicationJob[] jobs = DbManager.GetJobs();
                Updater updater = new Updater();
                updater.StatusChanged += new EventHandler<Updater.JobStatusChangedEventArgs>(updater_StatusChanged);
                updater.ProgressChanged += new EventHandler<Updater.JobProgressChangedEventArgs>(updater_ProgressChanged);
                updater.BeginUpdate(jobs, false, false, false);

                if (arguments["notify"] != null)
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
            // ...perform database operations...
            else if (arguments["update"] != null && arguments["appguid"] != null)
            {
                // Update properties of an application in the database
                ApplicationJob job = DbManager.GetJob(new Guid(arguments["appguid"]));
                if (job == null) return;

                if (arguments["PreviousLocation"] != null)
                {
                    job.PreviousLocation = arguments["PreviousLocation"];
                }

                job.Save();
            }
            else if (arguments["export"] != null)
            {
                ApplicationJob[] jobs = DbManager.GetJobs();
                string exportedXml = ApplicationJob.GetXml(jobs, false, System.Text.Encoding.UTF8);
                try
                {
                    File.WriteAllText(arguments["export"] as string, exportedXml, System.Text.Encoding.UTF8); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could export to the specified location: " + ex.Message);
                }
            }
            else if (arguments["install"] != null)
            {
                try
                {
                    // Install all applications in the given XML
                    string path = arguments["install"] as string;
                    ApplicationJob[] appsToInstall = ApplicationJob.ImportFromXml(path);

                    InstallingApplicationsDialog dialog = new InstallingApplicationsDialog();
                    dialog.Applications = appsToInstall;
                    dialog.ShowIcon = true;
                    dialog.ShowInTaskbar = true;
                    Application.Run(dialog);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Setup cannot be started: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // ...or launch the GUI.
            else
            {
                Application.Run(new MainForm());
            }

            string logFile = arguments["log"];
            if (!string.IsNullOrEmpty(logFile))
            {
                try
                {
                    logFile = UrlVariable.GlobalVariables.ReplaceAllInString(logFile);
                    LogDialog.SaveLogToFile(logFile);
                }
                catch (Exception)
                {
                    // ignore errors
                }
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show("An unhandled exception occured and Ketarin needs to be closed.\n\n" + (ex == null ? "" : ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Command line updater

        static void updater_ProgressChanged(object sender, Updater.JobProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ApplicationJob.Name + ": " + e.ProgressPercentage + "%");
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

            Console.WriteLine(status);

            if (m_Icon != null)
            {
                m_Icon.ShowBalloonTip(2000, "Ketarin", status, (e.NewStatus == Updater.Status.Failure ? ToolTipIcon.Error : ToolTipIcon.Info));
            }
        }

        #endregion
    }
}
