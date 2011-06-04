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
using System.Runtime.InteropServices;

namespace Ketarin
{
    public partial class MainForm : PersistentForm
    {
        private ApplicationJob[] m_Jobs = null;
        private Updater m_Updater = new Updater();
        // For caching purposes
        private Dictionary<string, string> customColumns = new Dictionary<string, string>();
        private FormWindowState m_PreviousState = FormWindowState.Normal;
        private Dictionary<ApplicationJob, ApplicationJobDialog> openApps = new Dictionary<ApplicationJob, ApplicationJobDialog>();
        private List<Hotkey> hotkeys = new List<Hotkey>();

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
            olvJobs.Initialize();
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
                if (!job.Enabled && !string.IsNullOrEmpty(job.CurrentLocation) && m_Updater.GetStatus(job) == Updater.Status.Idle)
                {
                    try
                    {
                        string disabledKey = job.CurrentLocation + "|Disabled";
                        if (!imlStatus.Images.ContainsKey(disabledKey))
                        {
                            // No icon if no file exists
                            if (!File.Exists(job.CurrentLocation)) return 0;

                            Icon programIcon = IconReader.GetFileIcon(job.CurrentLocation, IconReader.IconSize.Small, false);
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
                if (m_Updater.GetStatus(job) == Updater.Status.Idle && !string.IsNullOrEmpty(job.CurrentLocation))
                {
                    try
                    {
                        if (!imlStatus.Images.ContainsKey(job.CurrentLocation))
                        {
                            // No icon if no file exists
                            if (!File.Exists(job.CurrentLocation)) return 0;

                            Icon programIcon = IconReader.GetFileIcon(job.CurrentLocation, IconReader.IconSize.Small, false);
                            imlStatus.Images.Add(job.CurrentLocation, programIcon);
                        }
                        return job.CurrentLocation;
                    }
                    catch (ArgumentException)
                    {
                        // no icon could be determined, use default
                    }
                }

                return (int)m_Updater.GetStatus(job);
            };

            colStatus.AspectGetter = delegate(object x)
            {
                return m_Updater.GetStatus(x as ApplicationJob);
            };
            colStatus.AspectToStringConverter = delegate(object x)
            {
                switch ((Updater.Status)x)
                {
                    case Updater.Status.Downloading: return "Downloading";
                    case Updater.Status.Failure: return "Failed";
                    case Updater.Status.Idle: return "Idle";
                    case Updater.Status.NoUpdate: return "No update";
                    case Updater.Status.UpdateSuccessful: return "Updated";
                    case Updater.Status.UpdateAvailable: return "Update available";
                }
                return string.Empty;
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
            colProgress.Renderer = new ApplicationJobsListView.ProgressRenderer(m_Updater, 0, 100);


            m_Updater.ProgressChanged += new EventHandler<Updater.JobProgressChangedEventArgs>(m_Updater_ProgressChanged);
            m_Updater.StatusChanged += new EventHandler<Updater.JobStatusChangedEventArgs>(m_Updater_StatusChanged);
            m_Updater.UpdateCompleted += new EventHandler(m_Updater_UpdateCompleted);
            m_Updater.UpdatesFound += new EventHandler<GenericEventArgs<string[]>>(m_Updater_UpdatesFound);

            LogDialog.Instance.VisibleChanged += new EventHandler(delegate(object sender, EventArgs e)
            {
                mnuLog.Checked = LogDialog.Instance.Visible;
            });

            this.olvJobs.FilterChanged += new EventHandler(olvJobs_FilterChanged);

            imlStatus.Images.Add(Properties.Resources.Document);
            imlStatus.Images.Add(Properties.Resources.Import);
            imlStatus.Images.Add(Properties.Resources.New);
            imlStatus.Images.Add(Properties.Resources.NewDownloaded);
            imlStatus.Images.Add(Properties.Resources.Symbol_Check);
            imlStatus.Images.Add(Properties.Resources.Symbol_Delete);
            imlStatus.Images.Add(Properties.Resources.Document_Restricted);
        }

        #region Updater events

        private void m_Updater_UpdateCompleted(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                bRun.Text = "&Update all";
                bRun.SplitMenu = cmuRun;
                bRun.Image = Properties.Resources.Restart;
                cmnuImportFile.Enabled = true;
                mnuExportSelected.Enabled = true;
                mnuExportAll.Enabled = true;
                mnuImport.Enabled = true;
                bInstall.Enabled = true;
                olvJobs.Refresh();
                // Refresh sorting (last updated column for example)
                // If groups are enabled, sorting causes the list to scroll back
                // to the top. We'd like to avoid that.
                if (!olvJobs.ShowGroups)
                {
                    olvJobs.Sort();
                }

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
                if (index >= 0 && mnuAutoScroll.Checked)
                {
                    olvJobs.EnsureVisible(index);
                }

                UpdateNumByStatus();

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
                List<string> appNames = new List<string>();
                foreach (string xml in e.Value)
                {
                    ApplicationJob job = ApplicationJob.LoadOneFromXml(xml);
                    appNames.Add(job.Name);
                }

                string msg = string.Format("Updates for the following application definitions which you added from the online database have been found:\r\n\r\n{0}\r\n\r\nDo you want to update these applications now?", string.Join("\r\n", appNames.ToArray()));
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
            if (DesignMode) return;

            RebuildCustomColumns();

            base.OnLoad(e);

            mnuShowGroups.Checked = Conversion.ToBoolean(Settings.GetValue("Ketarin", "ShowGroups", true));
            mnuAutoScroll.Checked = Conversion.ToBoolean(Settings.GetValue("Ketarin", "AutoScroll", true));
            olvJobs.ShowGroups = mnuShowGroups.Checked;

            if (Conversion.ToBoolean(Settings.GetValue("Ketarin", "ShowStatusBar", false)))
            {
                mnuShowStatusBar.PerformClick();
            }

            UpdateList();
            UpdateNumByStatus();

            if (Convert.ToBoolean(Settings.GetValue("Ketarin", "ShowLog", false)))
            {
                mnuLog.PerformClick();
            }

            if ((bool)Settings.GetValue("UpdateAtStartup", false))
            {
                RunJobs(false, false, false);
            }

            // Check applications for updates
            if ((bool)Settings.GetValue("UpdateOnlineDatabase", true))
            {
                m_Updater.BeginCheckForOnlineUpdates(m_Jobs);
            }

            this.hotkeys = Hotkey.GetHotkeys();
        }

        private void RebuildCustomColumns()
        {
            this.customColumns = SettingsDialog.CustomColumns;

            for (int i = olvJobs.AllColumns.Count - 1; i >= 0; i--)
            {
                if (this.olvJobs.AllColumns[i].Tag != null)
                {
                    this.olvJobs.AllColumns.RemoveAt(i);
                }
            }

            // Add custom columns to list
            foreach (KeyValuePair<string, string> column in this.customColumns)
            {
                OLVColumn newCol = new OLVColumn(column.Key, "");
                newCol.Name = column.Key;
                newCol.Tag = column.Value;
                this.olvJobs.AllColumns.Add(newCol);
                newCol.AspectGetter = delegate(object x)
                {
                    if (string.IsNullOrEmpty(newCol.Tag as string)) return null;

                    string varFind = "{" + ((string)newCol.Tag).TrimStart('{').TrimEnd('}') + "}";
                    string value = ((ApplicationJob)x).Variables.ReplaceAllInString(varFind, DateTime.MinValue, null, true);
                    return (varFind == value) ? string.Empty : value;
                };
            }

            olvJobs.RebuildColumns();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            // Check for user defined hotkeys
            foreach (Hotkey hotkey in this.hotkeys)
            {
                if (hotkey.IsMatch(keyData))
                {
                    ExecuteHotkey(hotkey, job);

                    return true;
                }
            }

            // Make shortcuts global
            switch (keyData)
            {
                case (Keys.Control | Keys.Enter):
                    cmnuOpenFolder.PerformClick();
                    return true;

                case (Keys.Control | Keys.Enter | Keys.Shift):
                    cmnuOpenFile.PerformClick();
                    return true;
            }

            foreach (MenuItem item in cmnuJobs.MenuItems)
            {
                if ((int)item.Shortcut == (int)keyData && item.Enabled)
                {
                    item.PerformClick();
                    return true;
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
            Settings.SetValue("Ketarin", "AutoScroll", mnuAutoScroll.Checked);

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
            Cursor = Cursors.WaitCursor;
            try
            {
                m_Jobs = new List<ApplicationJob>(DbManager.GetJobs()).ToArray();
                if (m_Jobs.Length > 0)
                {
                    olvJobs.EmptyListMsg = "Loading applications...";
                    Application.DoEvents();
                }
                else
                {
                    olvJobs.EmptyListMsg = ApplicationJobsListView.DefaultEmptyMessage;
                }
                olvJobs.SetObjects(m_Jobs);
                UpdateStatusbar();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
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
                RunJobs(false, false, false);
            }
        }

        private void cmnuCheckAndDownload_Click(object sender, EventArgs e)
        {
            bRun.PerformClick();
        }

        private void cmnuOnlyCheck_Click(object sender, EventArgs e)
        {
            RunJobs(true, false, false);
        }

        private void cmnuUpdateAndInstall_Click(object sender, EventArgs e)
        {
            RunJobs(true, false, true);
        }

        private void cmnuForceDownload_Click(object sender, EventArgs e)
        {
            if (olvJobs.SelectedObjects.Count == 0)
            {
                RunJobs(false, true, false);
            }
            else
            {
                RunJobs(olvJobs.SelectedApplications, false, true, false);
            }
        }

        #endregion

        #region Install button

        private void bInstall_Click(object sender, EventArgs e)
        {
            using (ChooseAppsToInstallDialog dialog = new ChooseAppsToInstallDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    using (InstallingApplicationsDialog setupDialog = new InstallingApplicationsDialog())
                    {
                        setupDialog.UpdateApplications = dialog.ShouldUpdateApplications;
                        setupDialog.Applications = dialog.SelectedApplications;
                        setupDialog.ShowDialog(this);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Updates all items, using the same order as the
        /// items in the list (considers sorting).
        /// </summary>
        private void RunJobs(bool onlyCheck, bool forceDownload, bool installUpdated)
        {
            if (m_Updater.IsBusy) return;

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

            RunJobs(jobs.ToArray(), onlyCheck, forceDownload, installUpdated);
        }

        private void RunJobs(ApplicationJob[] jobs, bool onlyCheck, bool forceDownload, bool installUpdated)
        {
            if (m_Updater.IsBusy) return;

            bRun.Text = "Cancel";
            bRun.SplitMenu = null;
            bRun.Image = null;
            bInstall.Enabled = false;
            cmnuImportFile.Enabled = false;
            mnuExportSelected.Enabled = false;
            mnuExportAll.Enabled = false;
            mnuImport.Enabled = false;

            m_Updater.ForceDownload = forceDownload;
            m_Updater.BeginUpdate(jobs, onlyCheck, installUpdated);
            olvJobs.RefreshObjects(jobs);
        }

        #region Context menu

        private void cmnuOpenFile_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
            OpenFile(job);
        }

        private void cmnuProperties_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
                Shell32.ShowFileProperties(job.CurrentLocation);
            }
            catch (Exception)
            {
                // ignore if fails for whatever reason
            }
        }

        private void cmnuOpenFolder_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;
            OpenDownloadFolder(job);
        }

        private void mnuInvert_Click(object sender, EventArgs e)
        {
            int[] selectedIndices = new int[olvJobs.SelectedIndices.Count];
            olvJobs.SelectedIndices.CopyTo(selectedIndices, 0);
            List<int> listSelectedIndices = new List<int>(selectedIndices);

            olvJobs.DeselectAll();

            for (int i = 0; i < olvJobs.Items.Count; i++)
            {
                if (!listSelectedIndices.Contains(i))
                {
                    olvJobs.SelectedIndices.Add(i);
                }
            }
        }

        private void cmnuInstall_Click(object sender, EventArgs e)
        {
            InstallSelectedApplications();
        }

        private void cmnuUpdateInstall_Click(object sender, EventArgs e)
        {
            using (InstallingApplicationsDialog setupDialog = new InstallingApplicationsDialog())
            {
                setupDialog.Applications = olvJobs.SelectedApplications;
                setupDialog.UpdateApplications = true;
                setupDialog.ShowDialog(this);
            }
        }

        private void cmuUpdate_Click(object sender, EventArgs e)
        {
            if (olvJobs.SelectedObjects.Count == 0)
            {
                RunJobs(false, false, false);
            }
            else
            {
                RunJobs(olvJobs.SelectedApplications, false, false, false);
            }
        }

        private void cmnuEdit_Click(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            EditJob(job);
        }

        private void cmnuDelete_Click(object sender, EventArgs e)
        {
            if (olvJobs.DeleteSelectedApplications())
            {
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
            cmnuForceDownload.Enabled = (!m_Updater.IsBusy);
            cmnuOpenFile.Enabled = (job != null && !(m_Updater.GetStatus(job) == Updater.Status.Downloading) && job.FileExists);
            cmnuProperties.Enabled = (job != null && job.FileExists);
            cmnuOpenFolder.Enabled = (job != null && job.FileExists);
            cmnuRename.Enabled = cmnuOpenFile.Enabled;
            cmnuCopy.Enabled = (olvJobs.SelectedObjects.Count != 0);
            cmnuPaste.Enabled = SafeClipboard.IsDataPresent(DataFormats.Text);
            cmnuInstall.Enabled = (!m_Updater.IsBusy);
            cmnuUpdateInstall.Enabled = (!m_Updater.IsBusy);
        }

        private void cmnuRename_Click(object sender, EventArgs e)
        {            
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            if (string.IsNullOrEmpty(job.CurrentLocation)) return;

            using (RenameFileDialog dialog = new RenameFileDialog())
            {
                dialog.FileName = job.PreviousLocation;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.Move(job.CurrentLocation, dialog.FileName);
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
            ApplicationJob[] jobs = olvJobs.SelectedApplications;
            if (jobs.Length == 0)
            {
                RunJobs(true, false, false);
            }
            else
            {
                RunJobs(jobs, true, false, false);
            }
        }

        private void cmnuCopy_Click(object sender, EventArgs e)
        {
            List<ApplicationJob> jobs = new List<ApplicationJob>();
            foreach (ApplicationJob job in olvJobs.SelectedObjects)
            {
                jobs.Add(job);
            }

            SafeClipboard.SetData(ApplicationJob.GetXml(jobs, false, Encoding.UTF8), false);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            olvJobs.SelectAll();
        }

        private void cmnuPaste_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationJob[] jobs = null;
                try
                {
                    jobs = ApplicationJob.LoadFromXml(SafeClipboard.GetData(DataFormats.Text) as string);
                }
                catch (Exception)
                {
                    jobs = new ApplicationJob[] { ApplicationJob.ImportFromTemplateOrXml(this, SafeClipboard.GetData(DataFormats.Text) as string, m_Jobs, true) };
                }
                if (jobs == null || jobs.Length == 0) return;

                foreach (ApplicationJob job in jobs)
                {
                    job.Guid = Guid.NewGuid();
                    job.PreviousLocation = null;
                    job.CanBeShared = true;
                    job.Save();
                    olvJobs.AddObject(job);
                }

                // Go to last job
                olvJobs.EnsureVisible(olvJobs.IndexOf(jobs[jobs.Length - 1]));
                olvJobs.SelectedObject = jobs[jobs.Length - 1];
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

        private void olvJobs_FilterChanged(object sender, EventArgs e)
        {
            bAddApplication.Enabled = olvJobs.IsDefaultFilter;
            mnuNew.Enabled = olvJobs.IsDefaultFilter;
            cmnuImportFile.Enabled = olvJobs.IsDefaultFilter && !m_Updater.IsBusy;
            mnuImport.Enabled = olvJobs.IsDefaultFilter && !m_Updater.IsBusy;
        }

        #endregion

        #region Edit jobs

        private void olvJobs_DoubleClick(object sender, EventArgs e)
        {
            ApplicationJob job = olvJobs.SelectedObject as ApplicationJob;

            // Check for custom hotkeys first
            foreach (Hotkey hotkey in this.hotkeys)
            {
                if (hotkey.IsDoubleClickMatch(Control.ModifierKeys))
                {
                    ExecuteHotkey(hotkey, job);
                    return;
                }
            }

            if (Control.ModifierKeys == Keys.Control)
            {
                OpenDownloadFolder(job);
            }
            else if (Control.ModifierKeys == Keys.Alt)
            {
                cmnuOpenFile.PerformClick();
            }
            else
            {
                bool openWebsite = (bool)Settings.GetValue("OpenWebsiteOnDoubleClick", false);
                if (openWebsite && !string.IsNullOrEmpty(job.WebsiteUrl))
                {
                    OpenWebsite(job);
                }
                else
                {
                    EditJob(job);
                }
            }
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
                        UpdateNumByStatus();
                    }
                    break;

                case Keys.Control | Keys.E:
                    foreach (ApplicationJob selectedJob in olvJobs.SelectedObjects)
                    {
                        selectedJob.Enabled = true;
                        selectedJob.Save();
                        olvJobs.RefreshObject(selectedJob);
                        UpdateNumByStatus();
                    }
                    break;
            }
        }

        /// <summary>
        /// Opens the website of a specified application.
        /// </summary>
        private static void OpenWebsite(ApplicationJob job)
        {
            try
            {
                System.Diagnostics.Process.Start(job.ExpandedWebsiteUrl);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Edits an application job. It is possible to edit multiple jobs at the same time.
        /// </summary>
        private void EditJob(ApplicationJob job)
        {
            if (job == null) return;

            if (this.openApps.ContainsKey(job))
            {
                this.openApps[job].BringToFront();
            }
            else
            {
                ApplicationJobDialog dialog = new ApplicationJobDialog();

                dialog.ApplicationJob = job;
                this.openApps[job] = dialog;
                dialog.Show(this);
                dialog.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs e)
                {
                    if (dialog.DialogResult == DialogResult.OK)
                    {
                        dialog.ApplicationJob.Save();
                        olvJobs.RefreshObject(job);
                        UpdateNumByStatus();
                    }
                    this.openApps.Remove(job);
                    dialog.Dispose();
                });
            }
        }

        private static void OpenFile(ApplicationJob job)
        {
            try
            {
                System.Diagnostics.Process.Start(job.CurrentLocation);
            }
            catch (Exception)
            {
                // ignore if fails for whatever reason
            }
        }

        private static void OpenDownloadFolder(ApplicationJob job)
        {
            try
            {
                if (job != null)
                {
                    System.Diagnostics.Process.Start("explorer", " /select," + job.CurrentLocation);
                }
            }
            catch (Exception)
            {
                // ignore if fails for whatever reason
            }
        }

        private void InstallSelectedApplications()
        {
            using (InstallingApplicationsDialog setupDialog = new InstallingApplicationsDialog())
            {
                setupDialog.Applications = olvJobs.SelectedApplications;
                setupDialog.ShowDialog(this);
            }
        }

        private void ExecuteHotkey(Hotkey hotkey, ApplicationJob job)
        {
            ApplicationJob[] jobs = olvJobs.SelectedApplications;

            switch (hotkey.Name)
            {
                case "OpenWebsite": OpenWebsite(job); break;
                case "Edit": EditJob(job); break;
                case "Update": RunJobs(jobs, false, false, false); break;
                case "ForceDownload": RunJobs(jobs, false, true, false); break;
                case "InstallSelected": InstallSelectedApplications(); break;
                case "OpenFile": OpenFile(job); break;
                case "OpenFolder": OpenDownloadFolder(job); break;
                case "CheckUpdate": RunJobs(jobs, true, false, false); break;
                case "UpdateAndInstall": RunJobs(jobs, false, false, true); break;
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
                bInstall.Top = olvJobs.Bottom + 7;
            }
            else
            {
                statusBar.Visible = true;
                mnuShowStatusBar.Checked = true;

                olvJobs.Bounds = new Rectangle(olvJobs.Left, olvJobs.Top, olvJobs.Width, olvJobs.Height - statusBar.Height);
                bRun.Top = olvJobs.Bottom + 7;
                bAddApplication.Top = olvJobs.Bottom + 7;
                bInstall.Top = olvJobs.Bottom + 7;
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
                    File.WriteAllText(dialog.FileName, ApplicationJob.GetXml(objects, dialog.FilterIndex == 2, Encoding.UTF8), Encoding.UTF8);
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
                ApplicationJob.ImportFromTemplateOrXml(this, file, m_Jobs);
            }
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (dialog.CustomColumnsChanged)
                    {
                        SaveDialogSettings();
                        RebuildCustomColumns();
                        LoadDialogSettings();
                    }
                    olvJobs.RefreshObjects(m_Jobs);
                    UpdateStatusbar();
                    this.hotkeys = Hotkey.GetHotkeys();
                }
            }
        }

        private void UpdateStatusbar()
        {
            tbTotalApplications.Text = "Number of applications: " + olvJobs.Items.Count;
            tbSelectedApplications.Text = "Selected applications: " + olvJobs.SelectedIndices.Count;
        }

        /// <summary>
        /// Updates the application count by status in the status bar.
        /// </summary>
        private void UpdateNumByStatus()
        {
            int idle = 0;
            int failed = 0;
            int finished = 0;
            int updated = 0;
            int disabled = 0;
            
            foreach (ApplicationJob job in m_Jobs)
            {
                if (!job.Enabled)
                {
                    disabled++;
                }

                switch (m_Updater.GetStatus(job))
                {
                    case Updater.Status.Idle: idle++; break;
                    case Updater.Status.Failure: failed++; break;
                    case Updater.Status.NoUpdate:
                        finished++;
                        break;
                    case Updater.Status.UpdateAvailable:
                        updated++;
                        finished++;
                        break;
                    case Updater.Status.UpdateSuccessful:
                        updated++;
                        finished++;
                        break;
                }
            }

            tbNumByStatus.Text = string.Format("By status: {0} Idle, {1} Updates, {2} Finished, {3} Failed ({4} Disabled)", idle, updated, finished, failed, disabled);
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
                System.Diagnostics.Process.Start("http://wiki.ketarin.org/index.php/Basics");
            }
            catch (Exception)
            {
            }
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            olvJobs.ShowSearch();            
        }


        private void mnuAutoScroll_Click(object sender, EventArgs e)
        {
            mnuAutoScroll.Checked = !mnuAutoScroll.Checked;
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
