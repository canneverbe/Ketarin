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
using CDBurnerXP.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace Ketarin
{
    public partial class MainForm : PersistentForm
    {
        private ApplicationJob[] m_Jobs = null;
        private Updater m_Updater = new Updater();
        // For caching purposes
        private string m_CustomColumn = string.Empty;
        private FormWindowState m_PreviousState = FormWindowState.Normal;

        #region ProgressRenderer

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
                if (m_Updater.GetProgress(job) == -1 || !job.Enabled)
                {
                    base.DrawBackground(g, r);
                    return;
                }

                base.Render(g, r);

                long fileSize = m_Updater.GetDownloadSize(job);
                // No file size has been determined yet
                if (fileSize == -2) return;

                using (Brush fontBrush = new SolidBrush(SystemColors.WindowText))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    string text = FormatFileSize.Format(fileSize);
                    if (fileSize < 0)
                    {
                        text = "(unknown)";
                    }
                    g.DrawString(text, new Font(this.Font, FontStyle.Bold), fontBrush, r, format);
                }
            }
        }

        #endregion

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap =
               new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original,
               new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height,
               GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
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
            // Gray out disabled jobs
            olvJobs.RowFormatter = delegate(OLVListItem item)
            {
                if (!((ApplicationJob)item.RowObject).Enabled)
                {
                    item.ForeColor = Color.Gray;
                }
                else
                {
                    item.ForeColor = olvJobs.ForeColor;
                }
            };
            colName.ImageGetter = delegate(object x) {
                ApplicationJob job = (ApplicationJob)x;
                
                // Gray icon if disabled
                if (!job.Enabled && !string.IsNullOrEmpty(job.PreviousLocation))
                {
                    try
                    {
                        string disabledKey = job.PreviousLocation + "|Disabled";
                        if (!imlStatus.Images.ContainsKey(disabledKey))
                        {
                            // No icon if no file exists
                            if (!File.Exists(job.PreviousLocation)) return 0;

                            Icon programIcon = IconReader.GetFileIcon(job.PreviousLocation, IconReader.IconSize.Small, false);
                            imlStatus.Images.Add(disabledKey, MakeGrayscale(programIcon.ToBitmap()));
                        }
                        return disabledKey;
                    }
                    catch (ArgumentException)
                    {
                        // no icon could be determined, use default
                    }
                }

                // If available and idle, use the program icon
                if (m_Updater.GetStatus(job) == Updater.Status.Idle && !string.IsNullOrEmpty(job.PreviousLocation))
                {
                    try
                    {
                        if (!imlStatus.Images.ContainsKey(job.PreviousLocation))
                        {
                            // No icon if no file exists
                            if (!File.Exists(job.PreviousLocation)) return 0;

                            Icon programIcon = IconReader.GetFileIcon(job.PreviousLocation, IconReader.IconSize.Small, false);
                            imlStatus.Images.Add(job.PreviousLocation, programIcon);
                        }
                        return job.PreviousLocation;
                    }
                    catch (ArgumentException)
                    {
                        // no icon could be determined, use default
                    }
                }

                return (int)m_Updater.GetStatus(job);
            };

            colTarget.AspectGetter = delegate(object x) {
                ApplicationJob job = x as ApplicationJob;
                return job.Variables.ReplaceAllInString(job.TargetPath, DateTime.MinValue, null, true);
            };
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
            colCustomValue.AspectGetter = delegate(object x)
            {
                if (string.IsNullOrEmpty(m_CustomColumn)) return null;

                m_CustomColumn = SettingsDialog.CustomColumnVariableName;
                string varFind = "{" + m_CustomColumn + "}";
                string value = ((ApplicationJob)x).Variables.ReplaceAllInString(varFind, DateTime.MinValue, null, true);
                return (varFind == value) ? string.Empty : value;
            };

            m_Updater.ProgressChanged += new EventHandler<Updater.JobProgressChangedEventArgs>(m_Updater_ProgressChanged);
            m_Updater.StatusChanged += new EventHandler<Updater.JobStatusChangedEventArgs>(m_Updater_StatusChanged);
            m_Updater.UpdateCompleted += new EventHandler(m_Updater_UpdateCompleted);
            m_Updater.UpdatesFound += new EventHandler<GenericEventArgs<string[]>>(m_Updater_UpdatesFound);

            LogDialog.Instance.VisibleChanged += new EventHandler(delegate(object sender, EventArgs e)
            {
                mnuLog.Checked = LogDialog.Instance.Visible;
            });
            
            imlStatus.Images.Add(Properties.Resources.Document);
            imlStatus.Images.Add(Properties.Resources.Import);
            imlStatus.Images.Add(Properties.Resources.New);
            imlStatus.Images.Add(Properties.Resources.Symbol_Check);
            imlStatus.Images.Add(Properties.Resources.Symbol_Delete);
            imlStatus.Images.Add(Properties.Resources.Document_Restricted);
        }

        #region Updater events

        private void m_Updater_UpdateCompleted(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                bRun.Text = "&Update now";
                bRun.SplitMenu = cmuRun;
                bRun.Image = Properties.Resources.Restart;
                cmnuImportFile.Enabled = true;
                mnuExportSelected.Enabled = true;
                mnuExportAll.Enabled = true;
                mnuImport.Enabled = true;
                olvJobs.Refresh();

                if (m_Updater.Errors.Length > 0)
                {
                    ErrorsDialog dialog = new ErrorsDialog(m_Updater.Errors);
                    dialog.ShowDialog(this);
                }
            });

            ntiTrayIcon.Text = "Ketarin (Idle)";
            ntiTrayIcon.ShowBalloonTip(5000, "Done", "Ketarin has finished the update check.", ToolTipIcon.Info);
        }

        private void m_Updater_StatusChanged(object sender, Updater.JobStatusChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate() {
                olvJobs.RefreshObject(e.ApplicationJob);
                int index = olvJobs.IndexOf(e.ApplicationJob);
                if (index >= 0)
                {
                    olvJobs.EnsureVisible(index);
                }

                // Icon text length limited to 64 chars
                string text = "Currently working on: " + e.ApplicationJob.Name;
                if (text.Length >= 64)
                {
                    text = text.Substring(0, 60) + "...";
                }
                ntiTrayIcon.Text = text;
            });
        }

        private void m_Updater_ProgressChanged(object sender, Updater.JobProgressChangedEventArgs e)
        {
            olvJobs.RefreshObject(e.ApplicationJob);
        }

        private void m_Updater_UpdatesFound(object sender, GenericEventArgs<string[]> e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                string msg = string.Format("Updates for {0} application definitions which you added from the online database have been found. Do you want to update these applications now?", e.Value.Length);
                if (MessageBox.Show(this, msg, "Updates found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (ApplicationJob job in m_Jobs)
                    {
                        if (job.UpdateFromXml(e.Value))
                        {
                            olvJobs.RefreshObject(job);
                        }
                    }
                }
            });
        }

        #endregion

        private void ntiTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Visible)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                cmnuShow.PerformClick();
            }
        }

        #region Drag and drop

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);

            CheckDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);

            CheckDragDrop(drgevent);
        }

        private static void CheckDragDrop(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.Copy;
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            string[] files = drgevent.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    ImportFromFile(file);
                }
                UpdateList();
            }
        }

        #endregion

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.WindowState == FormWindowState.Minimized)
            {
                if (Convert.ToBoolean(Settings.GetValue("MinimizeToTray", false)))
                {
                    ntiTrayIcon.Visible = true;
                    this.Hide();
                }
            }
            else
            {
                m_PreviousState = WindowState;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mnuShowGroups.Checked = Conversion.ToBoolean(Settings.GetValue("Ketarin", "ShowGroups", true));
            olvJobs.ShowGroups = mnuShowGroups.Checked;
            m_CustomColumn = Settings.GetValue("CustomColumn", "") as string;
            if (Conversion.ToBoolean(Settings.GetValue("Ketarin", "ShowStatusBar", false)))
            {
                mnuShowStatusBar.PerformClick();
            }

            UpdateList();

            if (Convert.ToBoolean(Settings.GetValue("Ketarin", "ShowLog", false)))
            {
                mnuLog.PerformClick();
            }

            if ((bool)Settings.GetValue("UpdateAtStartup", false))
            {
                RunJobs(false);
            }

            // Check applications for updates
            if ((bool)Settings.GetValue("UpdateOnlineDatabase", true))
            {
                m_Updater.BeginCheckForOnlineUpdates(m_Jobs);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Make shortcuts global
            switch (keyData)
            {
                case (Keys.Control | Keys.Enter):
                    cmnuOpenFolder.PerformClick();
                    return true;
            }

            foreach (MenuItem item in cmnuJobs.MenuItems)
            {
                if ((int)item.Shortcut == (int)keyData)
                {
                    item.PerformClick();
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.SetValue("Ketarin", "ShowGroups", olvJobs.ShowGroups);
            Settings.SetValue("Ketarin", "ShowStatusBar", statusBar.Visible);
            Settings.SetValue("Ketarin", "ShowLog", mnuLog.Checked);

            if (m_Updater.IsBusy)
            {
                e.Cancel = true;
            }
            else
            {
                LogDialog.Instance.Close();
            }
        }

        private void UpdateList()
        {
            m_Jobs = new List<ApplicationJob>(DbManager.GetJobs()).ToArray();
            olvJobs.SetObjects(m_Jobs);
        }

        #region Add button

        private void sbAddApplication_Click(object sender, EventArgs e)
        {
            cmnuAdd.PerformClick();
        }

        private void cmnuAdd_Click(object sender, EventArgs e)
        {
            using (ApplicationJobDialog dialog = new ApplicationJobDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SaveAndShowJob(dialog.ApplicationJob);
                }
            }
        }

        private void SaveAndShowJob(ApplicationJob job)
        {
            job.Save();
            olvJobs.AddObject(job);
            olvJobs.SelectedObject = job;
            olvJobs.EnsureVisible(olvJobs.SelectedIndex);
            UpdateStatusbar();
        }

        private void cmnuImport_Click(object sender, EventArgs e)
        {
            mnuImport.PerformClick();
        }

        private void cmnuImportOnline_Click(object sender, EventArgs e)
        {
            using (ImportFromDatabaseDialog dialog = new ImportFromDatabaseDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK && dialog.ImportedApplication != null)
                {
                    ApplicationJob existing = Array.Find<ApplicationJob>(m_Jobs, delegate(ApplicationJob x) { return x.Guid == dialog.ImportedApplication.Guid; });
                    if (existing == null) {
                        existing = dialog.ImportedApplication;
                        List<ApplicationJob> newJobs = new List<ApplicationJob>(m_Jobs);
                        newJobs.Add(existing);
                        m_Jobs = newJobs.ToArray();
                        olvJobs.AddObject(existing);
                        UpdateStatusbar();
                    }
                    olvJobs.SelectedObject = existing;
                    olvJobs.SelectedItem.EnsureVisible();
                }
            }
        }

        #endregion

        #region Run button

        private void bRun_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy)
            {
                m_Updater.Cancel();
            }
            else
            {
                RunJobs(false);
            }
        }

        private void cmnuCheckAndDownload_Click(object sender, EventArgs e)
        {
            bRun.PerformClick();
        }

        private void cmnuOnlyCheck_Click(object sender, EventArgs e)
        {
            RunJobs(true);
        }

        #endregion

        /// <summary>
        /// Updates all items, using the same order as the
        /// items in the list (considers sorting).
        /// </summary>
        private void RunJobs(bool onlyCheck)
        {
            List<ApplicationJob> jobs = new List<ApplicationJob>();
            OLVListItem startItem = null;

            do
            {
                startItem = olvJobs.GetNextItem(startItem) as OLVListItem;
                if (startItem != null)
                {
                    jobs.Add(startItem.RowObject as ApplicationJob);
                }
            } while (startItem != null);

            RunJobs(jobs.ToArray(), onlyCheck);
        }

        private void RunJobs(ApplicationJob[] jobs, bool onlyCheck)
        {
            bRun.Text = "Cancel";
            bRun.SplitMenu = null;
            bRun.Image = null;
            cmnuImportFile.Enabled = false;
            mnuExportSelected.Enabled = false;
            mnuExportAll.Enabled = false;
            mnuImport.Enabled = false;

            m_Updater.BeginUpdate(jobs, onlyCheck);
            olvJobs.RefreshObjects(jobs);
        }

        #region Context menu

        private void cmnuOpenFile_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy) return;

            try
            {
                ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
                System.Diagnostics.Process.Start(job.PreviousLocation);
            }
            catch (Exception)
            {
                // ignore if fails for whatever reason
            }
        }

        private void cmnuOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
                System.Diagnostics.Process.Start("explorer", " /select," + job.PreviousLocation);
            }
            catch (Exception)
            {
                // ignore if fails for whatever reason
            }
        }

        private void cmuUpdate_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy) return;

            if (olvJobs.SelectedObjects.Count == 0)
            {
                RunJobs(false);
            }
            else
            {
                List<ApplicationJob> jobs = new List<ApplicationJob>();
                foreach (ApplicationJob job in olvJobs.SelectedObjects)
                {
                    jobs.Add(job);
                }
                RunJobs(jobs.ToArray(), false);
            }
        }

        private void cmnuEdit_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            EditJob(job);
        }

        private void cmnuDelete_Click(object sender, EventArgs e)
        {
            if (DeleteApplicationDialog.Show(this, olvJobs.SelectedObjects))
            {
                olvJobs.RemoveObjects(olvJobs.SelectedObjects);
                m_Jobs = new List<ApplicationJob>(DbManager.GetJobs()).ToArray();
                UpdateStatusbar();
            }
        }

        private void cmnuJobs_Popup(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
            cmnuEdit.Enabled = (job != null);
            cmnuDelete.Enabled = (olvJobs.SelectedIndices.Count > 0 && !m_Updater.IsBusy);
            cmnuUpdate.Enabled = (!m_Updater.IsBusy);
            cmnuCheckForUpdate.Enabled = (!m_Updater.IsBusy);
            cmnuOpenFile.Enabled = (job != null && !m_Updater.IsBusy && !string.IsNullOrEmpty(job.PreviousLocation) && File.Exists(job.PreviousLocation));
            cmnuOpenFolder.Enabled = (job != null && !string.IsNullOrEmpty(job.PreviousLocation) && File.Exists(job.PreviousLocation));
            cmnuRename.Enabled = cmnuOpenFile.Enabled;
            cmnuCopy.Enabled = (job != null);
            cmnuPaste.Enabled = SafeClipboard.IsDataPresent(DataFormats.Text);
        }

        private void cmnuRename_Click(object sender, EventArgs e)
        {
            if (m_Updater.IsBusy) return;
            
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            using (RenameFileDialog dialog = new RenameFileDialog())
            {
                dialog.FileName = job.PreviousLocation;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.Move(job.PreviousLocation, dialog.FileName);
                        job.PreviousLocation = dialog.FileName;
                        job.Save();
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show(this, "The file to be renamed does not exist anymore.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmnuCheckForUpdate_Click(object sender, EventArgs e)
        {
            List<ApplicationJob> jobs = new List<ApplicationJob>();
            foreach (ApplicationJob job in olvJobs.SelectedObjects)
            {
                jobs.Add(job);
            }

            if (jobs.Count == 0)
            {
                RunJobs(true);
            }
            else
            {
                RunJobs(jobs.ToArray(), true);
            }
        }

        private void cmnuCopy_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
            if (job == null) return;

            SafeClipboard.SetData(job.GetXml(), false);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            olvJobs.SelectAll();
        }

        private void cmnuPaste_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationJob job = ApplicationJob.ImportFromXmlString(SafeClipboard.GetData(DataFormats.Text) as string);
                job.Guid = Guid.NewGuid();
                job.CanBeShared = true;
                job.Save();

                olvJobs.AddObject(job);
                olvJobs.EnsureVisible(olvJobs.IndexOf(job));
                olvJobs.SelectedObject = job;
                UpdateStatusbar();
            }
            catch (Exception) { }
        }

        private void olvJobs_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusbar();
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
            switch (e.KeyData)
            {
                case Keys.Enter:
                    ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
                    EditJob(job);
                    break;

                case Keys.Control | Keys.D:
                    foreach (ApplicationJob selectedJob in olvJobs.SelectedObjects)
                    {
                        selectedJob.Enabled = false;
                        selectedJob.Save();
                        olvJobs.RefreshObject(selectedJob);
                    }
                    break;

                case Keys.Control | Keys.E:
                    foreach (ApplicationJob selectedJob in olvJobs.SelectedObjects)
                    {
                        selectedJob.Enabled = true;
                        selectedJob.Save();
                        olvJobs.RefreshObject(selectedJob);
                    }
                    break;
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
                    olvJobs.RefreshObject(job);
                }
            }
        }

        #endregion

        #region Main menu

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            using (AboutDialog dialog = new AboutDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuShowGroups_Click(object sender, EventArgs e)
        {
            if (mnuShowGroups.Checked)
            {
                olvJobs.ShowGroups = false;
                mnuShowGroups.Checked = false;
            }
            else
            {
                olvJobs.ShowGroups = true;
                olvJobs.BuildGroups();
                mnuShowGroups.Checked = true;
            }
        }

        private void mnuShowStatusBar_Click(object sender, EventArgs e)
        {
            if (mnuShowStatusBar.Checked)
            {
                statusBar.Visible = false;
                mnuShowStatusBar.Checked = false;

                olvJobs.Bounds = new Rectangle(olvJobs.Left, olvJobs.Top, olvJobs.Width, olvJobs.Height + statusBar.Height);
                bRun.Top = olvJobs.Bottom + 7;
                bAddApplication.Top = olvJobs.Bottom + 7;
            }
            else
            {
                statusBar.Visible = true;
                mnuShowStatusBar.Checked = true;

                olvJobs.Bounds = new Rectangle(olvJobs.Left, olvJobs.Top, olvJobs.Width, olvJobs.Height - statusBar.Height);
                bRun.Top = olvJobs.Bottom + 7;
                bAddApplication.Top = olvJobs.Bottom + 7;
            }
        }

        private void mnuAddNew_Click(object sender, EventArgs e)
        {
            cmnuAdd.PerformClick();
        }

        private void mnuExportSelected_Click(object sender, EventArgs e)
        {
            if (olvJobs.SelectedIndices.Count == 0)
            {
                MessageBox.Show(this, "You did not select any jobs to export.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExportJobs(olvJobs.SelectedObjects);
        }

        private void ExportJobs(System.Collections.IEnumerable objects)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Application Definition|*.xml|Application Template|*.xml";
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(dialog.FileName, ApplicationJob.GetXml(objects, dialog.FilterIndex == 2));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to save the file: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mnuExportAll_Click(object sender, EventArgs e)
        {
            ExportJobs(m_Jobs);
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML file|*.xml";
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    ImportFromFile(dialog.FileName);

                    UpdateList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to import the file: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportFromFile(string file)
        {
            ApplicationJob padImport = ApplicationJob.ImportFromPad(file);
            if (padImport != null)
            {
                ApplicationJobDialog newJob = new ApplicationJobDialog();
                newJob.ApplicationJob = padImport;
                if (newJob.ShowDialog(this) == DialogResult.OK)
                {
                    SaveAndShowJob(newJob.ApplicationJob);
                }
            }
            else
            {
                ApplicationJob.ImportFromTemplateOrXml(this, file);
            }
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    m_CustomColumn = Settings.GetValue("CustomColumn", "") as string;
                    olvJobs.RefreshObjects(m_Jobs);
                    UpdateStatusbar();
                }
            }
        }

        private void UpdateStatusbar()
        {
            tbTotalApplications.Text = "Number of applications: " + olvJobs.Items.Count;
            tbSelectedApplications.Text = "Selected applications: " + olvJobs.SelectedIndices.Count;
        }

        private void mnuLog_Click(object sender, EventArgs e)
        {
            if (LogDialog.Instance.Visible)
            {
                LogDialog.Instance.Hide();
            }
            else
            {
                LogDialog.Instance.Show(this);
            }

            mnuLog.Checked = LogDialog.Instance.Visible;
        }

        private void mnuTutorial_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://cdburnerxp.se/help/kb/20");
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Tray Icon Menu

        private void cmnuShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            this.WindowState = m_PreviousState;
        }

        private void cmnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

    }
}
