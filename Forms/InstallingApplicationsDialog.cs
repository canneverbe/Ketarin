using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using System.Threading;
using CDBurnerXP.IO;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog that shows the progress of installing applications.
    /// </summary>
    public partial class InstallingApplicationsDialog : PersistentForm
    {
        private bool expanded = false;
        private List<LogItem> logItems = new List<LogItem>();
        private int installCounter = 0;
        private bool updateApplications = false;

        #region LogItem

        private enum LogItemType
        {
            Info,
            Warning, 
            Error
        }

        private class LogItem
        {
            public string Message { get; set; }
            public LogItemType Type { get; set; }
            public DateTime Time { get; set; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether or not the applications should be updated before installing.
        /// </summary>
        public bool UpdateApplications
        {
            get { return this.updateApplications; }
            set { this.updateApplications = value; }
        }

        /// <summary>
        /// Gets or sets the applications that are to be installed.
        /// </summary>
        public ApplicationJob[] Applications
        {
            get; set;
        }

        #endregion

        public InstallingApplicationsDialog()
        {
            InitializeComponent();

            CancelButton = bCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) return;

            // Collapse dialog initially
            pnlExpanded.Visible = this.expanded;
            this.Height -= pnlExpanded.Height;

            colTime.ImageGetter = delegate(object x)
            {
                return Convert.ToInt32(((LogItem)x).Type);
            };

            base.OnLoad(e);

            // Since progress hardly be determined, show animated progress bar
            progressBar.Style = ProgressBarStyle.Marquee;
            bgwSetup.RunWorkerAsync();
        }

        /// <summary>
        /// Updates the status label, thread safe.
        /// </summary>
        private void UpdateStatus(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate() { UpdateStatus(text); });
            }

            this.lblSetupStatus.Text = text;
        }

        /// <summary>
        /// Inserts an item into the log, thread safe.
        /// </summary>
        private void LogInfo(string text, LogItemType type)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate() { LogInfo(text, type); });
                return;
            }

            logItems.Add(new LogItem() { Message = text, Type = type, Time = DateTime.Now });
            olvLog.SetObjects(logItems);
        }

        private void bgwSetup_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 1;

            foreach (ApplicationJob job in this.Applications)
            {
                try
                {
                    UpdateAndInstallApp(e, job, ref count);
                }
                catch (Exception ex)
                {
                    LogInfo(job.Name + ": Setup failed (" + ex.Message + ")", LogItemType.Error);
                }
            }
        }

        private void UpdateAndInstallApp(DoWorkEventArgs e, ApplicationJob job, ref int count)
        {
            // Check: Are actually some instructions defined?
            if (job.SetupInstructions.Count == 0)
            {
                LogInfo(job.Name + ": Skipped since no setup instructions exist", LogItemType.Warning);
                return;
            }

            if (bgwSetup.CancellationPending) return;

            // Force update if no file exists
            if (this.updateApplications || !job.FileExists)
            {
                UpdateStatus(string.Format("Updating application {0} of {1}: {2}", count, this.Applications.Length, job.Name));

                Updater updater = new Updater();
                updater.BeginUpdate(new ApplicationJob[] { job }, false, false);

                // Wait until finished
                while (updater.IsBusy)
                {
                    if (bgwSetup.CancellationPending)
                    {
                        updater.Cancel();
                        return;
                    }
                    Thread.Sleep(500);
                }

                // Did update fail? Install if {file} is present.
                if (updater.Errors.Length > 0)
                {
                    if (PathEx.TryGetFileSize(job.PreviousLocation) > 0)
                    {
                        LogInfo(job.Name + ": Update failed, installing previously available version", LogItemType.Warning);
                    }
                    else
                    {
                        LogInfo(job.Name + ": Update failed", LogItemType.Error);
                        return;
                    }
                }
            }

            UpdateStatus(string.Format("Installing application {0} of {1}: {2}", count, this.Applications.Length, job.Name));

            foreach (SetupInstruction instruction in job.SetupInstructions)
            {
                if (bgwSetup.CancellationPending) return;

                instruction.Execute();
            }

            LogInfo(job.Name + ": Installed successfully", LogItemType.Info);

            this.installCounter++;
            count++;
        }

        private void bgwSetup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateStatus(string.Format("{0} of {1} applications installed successfully.", this.installCounter, this.Applications.Length));
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 100;
            bCancel.Enabled = true;
            bCancel.Text = "Close";
        }


        private void bCancel_Click(object sender, EventArgs e)
        {
            if (bgwSetup.IsBusy)
            {
                bgwSetup.CancelAsync();
                bCancel.Enabled = false;
                // Do not close yet!
                DialogResult = DialogResult.None;
            }
            else
            {
                this.Close();
            }
        }

        #region Expansion 

        private void lbDetails_MouseEnter(object sender, EventArgs e)
        {
            lbShowHideDetails.ImageIndex = (expanded ? 1 : 4);
        }

        private void lbDetails_MouseLeave(object sender, EventArgs e)
        {
            lbShowHideDetails.ImageIndex = (expanded ? 0 : 3);
        }

        private void lbDetails_MouseUp(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = (expanded ? 1 : 4);
        }

        private void lbDetails_MouseDown(object sender, MouseEventArgs e)
        {
            lbShowHideDetails.ImageIndex = (expanded ? 2 : 5);
        }

        private void lbDetails_Click(object sender, EventArgs e)
        {
            this.expanded = !this.expanded;
            pnlExpanded.Visible = this.expanded;
            lbShowHideDetails.Text = (this.expanded ? "        " + "&Hide details" : "        " + "&Show details");
            if (this.expanded)
                this.Height += pnlExpanded.Height;
            else
                this.Height -= pnlExpanded.Height;
        }

        #endregion 

    }
}
