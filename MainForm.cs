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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ketarin.Forms;
using CDBurnerXP.Controls;
using Microsoft.Win32;
using System.IO;
using CDBurnerXP.IO;
using CDBurnerXP;

namespace Ketarin
{
    public partial class MainForm : Form
    {
        private ApplicationJob[] m_Jobs = null;
        private Updater m_Updater = new Updater();

        private class ProgressRenderer : BarRenderer
        {
            private Updater m_Updater = null;

            public ProgressRenderer(Updater updater, int min, int max)
                : base(min, max)
            {
                m_Updater = updater;
            }

            public override void Render(Graphics g, Rectangle r)
            {
                ApplicationJob job = RowObject as ApplicationJob;
                // Do not draw anything if the updater is not currently working
                if (m_Updater.GetProgress(job) == -1)
                {
                    base.DrawBackground(g, r);
                    return;
                }

                base.Render(g, r);

                long fileSize = m_Updater.GetDownloadSize(job);
                if (fileSize < 0) return;

                using (Brush fontBrush = new SolidBrush(SystemColors.WindowText))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    string text = FormatFileSize.Format(fileSize);
                    g.DrawString(text, new Font(this.Font, FontStyle.Bold), fontBrush, r, format);
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            olvJobs.ContextMenu = cmnuJobs;

            colName.AspectGetter = delegate(object x) { return ((ApplicationJob)x).Name; };
            colName.GroupKeyGetter = delegate(object x) {
                ApplicationJob job = (ApplicationJob)x;
                if (job.Name.Length == 0) return string.Empty;
                return job.Name[0].ToString().ToUpper();
            };
            colName.ImageGetter = delegate(object x) {
                ApplicationJob job = (ApplicationJob)x;
                // "No" icon if disabled
                if (!job.Enabled) return 4;

                // If available and idle, use the program icon
                if (m_Updater.GetStatus(job) == Updater.Status.Idle && !string.IsNullOrEmpty(job.PreviousLocation))
                {
                    if (!imlStatus.Images.ContainsKey(job.PreviousLocation) && File.Exists(job.PreviousLocation))
                    {
                        Icon programIcon = IconReader.GetFileIcon(job.PreviousLocation, IconReader.IconSize.Small, false);
                        imlStatus.Images.Add(job.PreviousLocation, programIcon);
                    }
                    return job.PreviousLocation;
                }
                
                return (int)m_Updater.GetStatus(job);
            };

            colTarget.AspectGetter = delegate(object x) { return ((ApplicationJob)x).TargetPath; };
            colTarget.GroupKeyGetter = delegate(object x) { return ((ApplicationJob)x).TargetPath.ToLower(); };

            colLastUpdate.AspectGetter = delegate(object x) { return ((ApplicationJob)x).LastUpdated; };
            colLastUpdate.AspectToStringFormat = "{0:g}";
            colLastUpdate.GroupKeyGetter = delegate(object x)
            {
                ApplicationJob job = (ApplicationJob)x;
                if (job.LastUpdated == null) return DateTime.MinValue;
                return job.LastUpdated.Value.Date;
            };
            colLastUpdate.GroupKeyToTitleConverter = delegate(object x)
            {
                if ((DateTime)x == DateTime.MinValue) return string.Empty;
                return ((DateTime)x).ToString("d");
            };

            colProgress.AspectGetter = delegate(object x) { return m_Updater.GetProgress(x as ApplicationJob); };
            colProgress.Renderer = new ProgressRenderer(m_Updater, 0, 100);

            m_Updater.ProgressChanged += new EventHandler<Updater.JobProgressChangedEventArgs>(m_Updater_ProgressChanged);
            m_Updater.StatusChanged += new EventHandler<Updater.JobStatusChangedEventArgs>(m_Updater_StatusChanged);
            m_Updater.UpdateCompleted += new EventHandler(m_Updater_UpdateCompleted);

            imlStatus.Images.Add(Properties.Resources.Document);
            imlStatus.Images.Add(Properties.Resources.Import);
            imlStatus.Images.Add(Properties.Resources.Symbol_Check);
            imlStatus.Images.Add(Properties.Resources.Symbol_Delete);
            imlStatus.Images.Add(Properties.Resources.Document_Restricted);
        }

        #region Updater events

        void m_Updater_UpdateCompleted(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                bRun.Text = "&Update now";
                bRun.Image = Properties.Resources.Restart;
                bAddNew.Enabled = true;

                if (m_Updater.Errors.Length > 0)
                {
                    ErrorsDialog dialog = new ErrorsDialog(m_Updater.Errors);
                    dialog.ShowDialog(this);
                }
            });
        }

        void m_Updater_StatusChanged(object sender, Updater.JobStatusChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate() {
                olvJobs.RefreshObject(e.ApplicationJob);
                olvJobs.EnsureVisible(olvJobs.IndexOf(e.ApplicationJob));
            });
        }

        void m_Updater_ProgressChanged(object sender, Updater.JobProgressChangedEventArgs e)
        {
            olvJobs.RefreshObject(e.ApplicationJob);
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cmnuShowGroups.Checked = Convert.ToBoolean(Settings.GetValue("Ketarin", "ShowGroups", true));
            olvJobs.ShowGroups = cmnuShowGroups.Checked;

            UpdateList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (m_Updater.IsBusy)
            {
                e.Cancel = true;
            }

            Settings.SetValue("Ketarin", "ShowGroups", olvJobs.ShowGroups);
        }

        private void bAddNew_Click(object sender, EventArgs e)
        {
            using (ApplicationJobDialog dialog = new ApplicationJobDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    dialog.ApplicationJob.Save();
                    UpdateList();
                }
            }
        }

        private void UpdateList()
        {
            m_Jobs = new List<ApplicationJob>(DbManager.GetJobs()).ToArray();
            olvJobs.SetObjects(m_Jobs);
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy)
            {
                m_Updater.Cancel();
            }
            else
            {
                RunJobs(m_Jobs);
            }
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            using (AboutDialog dialog = new AboutDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void RunJobs(ApplicationJob[] jobs)
        {
            bRun.Text = "Cancel";
            bRun.Image = null;
            bAddNew.Enabled = false;

            m_Updater.Run(jobs);
        }

        #region Context menu

        private void cmnuShowGroups_Click(object sender, EventArgs e)
        {
            if (cmnuShowGroups.Checked)
            {
                olvJobs.ShowGroups = false;
                cmnuShowGroups.Checked = false;
            }
            else
            {
                olvJobs.ShowGroups = true;
                olvJobs.BuildGroups();
                cmnuShowGroups.Checked = true;
            }
        }

        private void cmuUpdate_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy) return;

            if (olvJobs.SelectedObjects.Count == 0)
            {
                RunJobs(m_Jobs);
            }
            else
            {
                List<ApplicationJob> jobs = new List<ApplicationJob>();
                foreach (ApplicationJob job in olvJobs.SelectedObjects)
                {
                    jobs.Add(job);
                }
                RunJobs(jobs.ToArray());
            }
        }

        private void cmnuEdit_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            EditJob(job);
        }

        private void cmnuDelete_Click(object sender, EventArgs e)
        {
            string msg = "Do you really want to delete the selected applications from the list?";
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            foreach (ApplicationJob job in olvJobs.SelectedObjects)
            {
                job.Delete();
            }
            UpdateList();
        }

        private void cmnuJobs_Popup(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
            cmnuEdit.Enabled = (job != null);
            cmnuDelete.Enabled = (olvJobs.SelectedIndices.Count > 0 && !m_Updater.IsBusy);
            cmnuUpdate.Enabled = (!m_Updater.IsBusy);
        }

        private void olvJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmnuJobs_Popup(sender, e);
        }

        #endregion

        #region Edit jobs

        private void olvJobs_DoubleClick(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            EditJob(job);
        }

        private void olvJobs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
                EditJob(job);
            }
        }

        private void EditJob(ApplicationJob job)
        {
            if (job == null) return;

            using (ApplicationJobDialog dialog = new ApplicationJobDialog())
            {
                dialog.ApplicationJob = job;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    dialog.ApplicationJob.Save();
                }
            }
            olvJobs.RefreshObject(job);
        }

        #endregion


    }
}
