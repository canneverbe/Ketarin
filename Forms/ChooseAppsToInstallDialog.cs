using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using Microsoft.Win32;
using CDBurnerXP.IO;
using CDBurnerXP.Controls;
using System.Data.SQLite;
using CDBurnerXP;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog that allows the user to select the applications that should be installed.
    /// </summary>
    public partial class ChooseAppsToInstallDialog : PersistentForm
    {
        private List<ApplicationList> lists = new List<ApplicationList>();
        private Dictionary<ApplicationJob, bool> checkedApps = new Dictionary<ApplicationJob, bool>();
        private List<ApplicationJob> selectedApplications = new List<ApplicationJob>();
        private ApplicationList lastDeletedList = null;
        private bool shouldUpdateApplications = false;

        #region Properties

        /// <summary>
        /// Gets whether or not applications should be updated before installing.
        /// </summary>
        public bool ShouldUpdateApplications
        {
            get { return shouldUpdateApplications; }
        }

        /// <summary>
        /// Gets the list of selected applications to install.
        /// </summary>
        public ApplicationJob[] SelectedApplications
        {
            get { return selectedApplications.ToArray(); }
        }

        #endregion

        public ChooseAppsToInstallDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            olvLists.ContextMenu = cmnuView;

            colListName.ImageGetter = delegate(object x)
            {
                return this.lists.IndexOf(x as ApplicationList);
            };
            olvApps.CheckStateGetter = delegate(object x)
            {
                return this.checkedApps.ContainsKey(x as ApplicationJob) && this.checkedApps[x as ApplicationJob];
            };
            olvApps.CheckStatePutter = delegate(object x, CheckState value)
            {
                this.checkedApps[x as ApplicationJob] = (value == CheckState.Checked);
                return value;
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (olvLists != null && olvLists.TileSize.Height > 0)
            {
                olvLists.TileSize = new Size(olvLists.Width - 23, olvLists.TileSize.Height);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Load view settings
            View view = (View)Settings.GetValue(this, "ListsView", View.Tile);
            if (view == View.Tile)
            {
                mnuTileView.PerformClick();
            }
            else
            {
                mnuDetailsView.PerformClick();
            }

            // Item for all apps
            ApplicationList allApps = new ApplicationList("All applications", true);
            allApps.Applications.AddRange(DbManager.GetJobs());
            AddAppToList(allApps);

            // By default, all apps should be selected
            foreach (ApplicationJob job in allApps.Applications)
            {
                this.checkedApps[job] = true;
            }

            // Item for each category
            Dictionary<string, ApplicationList> categoryLists = new Dictionary<string, ApplicationList>();
            foreach (ApplicationJob job in allApps.Applications)
            {
                if (string.IsNullOrEmpty(job.Category)) continue;

                if (!categoryLists.ContainsKey(job.Category))
                {
                    categoryLists[job.Category] = new ApplicationList(job.Category, true);
                    lists.Add(categoryLists[job.Category]);
                }

                categoryLists[job.Category].Applications.Add(job);
            }

            foreach (ApplicationList list in categoryLists.Values)
            {
                imlLists.Images.Add(list.GetIcon());
            }

            // Now add custom lists
            foreach (ApplicationList list in DbManager.GetSetupLists(allApps.Applications))
            {
                AddAppToList(list);
            }
            
            olvLists.SetObjects(lists);
            olvLists.SelectedIndex = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Settings.SetValue(this, "ListsView", olvLists.View);
        }

        private void EnableDisableButtons()
        {
            ApplicationList currentList = olvLists.SelectedObject as ApplicationList;
            bRemoveApp.Enabled = (currentList != null && !currentList.IsPredefined && olvApps.SelectedObjects.Count > 0);
            bAddApp.Enabled = (currentList != null && !currentList.IsPredefined);
            bSelectApp.Enabled = (currentList != null);

            // Allow remove if there is a non-predefined list
            bRemoveList.Enabled = false;
            foreach (ApplicationList list in olvLists.SelectedObjects)
            {
                if (!list.IsPredefined)
                {
                    bRemoveList.Enabled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Adds an application list to the list.
        /// Takes care of adding a new icon.
        /// </summary>
        private void AddAppToList(ApplicationList appList)
        {
            try
            {
                imlLists.Images.Add(appList.GetIcon());
            }
            catch (ArgumentException)
            {
                // Do not fault on invalid icons
            }

            lists.Add(appList);
        }

        /// <summary>
        /// Close the dialog with selected applications.
        /// </summary>
        private void InstallConfirm()
        {
            this.selectedApplications.Clear();
            foreach (ApplicationJob job in olvApps.CheckedObjects)
            {
                selectedApplications.Add(job);
            }

            // No applications selected -> not OK
            if (selectedApplications.Count == 0)
            {
                MessageBox.Show(this, "You did not select any applications to install.\r\n\r\nPlease select at least one application in order to proceed.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Updates the icon of an application list.
        /// </summary>
        private void UpdateAppList(ApplicationList list)
        {
            imlLists.Images[this.lists.IndexOf(list)] = list.GetIcon();
            olvLists.RefreshObject(list);
        }

        /// <summary>
        /// Integrates a new application list into the GUI and
        /// saves it to the database.
        /// </summary>
        /// <param name="newList"></param>
        private void CreateNewAppList(ApplicationList newList, bool edit)
        {
            AddAppToList(newList);
            newList.Save();
            olvLists.AddObject(newList);
            olvLists.SelectedObject = newList;

            OLVListItem selectedItem = olvLists.SelectedItem as OLVListItem;
            if (selectedItem != null)
            {
                selectedItem.EnsureVisible();
                if (edit) olvLists.EditSubItem(selectedItem, 0);
            }
        }

        #region Events

        private void mnuTileView_Click(object sender, EventArgs e)
        {
            olvLists.View = View.Tile;
            mnuTileView.Checked = true;
            mnuDetailsView.Checked = false;
        }

        private void mnuDetailsView_Click(object sender, EventArgs e)
        {
            olvLists.View = View.Details;
            mnuTileView.Checked = false;
            mnuDetailsView.Checked = true;
        }

        private void bNewList_Click(object sender, EventArgs e)
        {
            ApplicationList newList = new ApplicationList("New list", false);
            CreateNewAppList(newList, true);
        }

        private void olvLists_CellEditStarting(object sender, ObjectListView.CellEditEventArgs e)
        {
            // Only allow editing of custom lists
            ApplicationList selectedList = olvLists.SelectedObject as ApplicationList;
            if (selectedList != null && selectedList.IsPredefined)
            {
                e.Cancel = true;
            }
            else
            {
                System.Windows.Forms.TextBox txt = e.Control as System.Windows.Forms.TextBox;
                if (txt != null)
                {
                    txt.AutoCompleteMode = AutoCompleteMode.None;
                }
            }
        }

        private void olvLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();

            if (olvLists.SelectedObjects.Count == 0)
            {
                olvApps.EmptyListMsg = "Select a list in order to select applications.";
            }
            else
            {
                olvApps.EmptyListMsg = "No applications in list.";
            }

            // Show all selected apps in the app list
            List<ApplicationJob> apps = new List<ApplicationJob>();
            foreach (ApplicationList list in olvLists.SelectedObjects)
            {
                foreach (ApplicationJob job in list.Applications)
                {
                    if (!apps.Contains(job))
                    {
                        apps.Add(job);
                    }
                }
            }
            
            olvApps.SetObjects(apps);
        }

        private void olvLists_CellEditFinished(object sender, ObjectListView.CellEditEventArgs e)
        {
            ApplicationList list = e.RowObject as ApplicationList;
            if (list != null)
            {
                list.Save();
            }
        }


        private void bRemoveList_Click(object sender, EventArgs e)
        {
            foreach (ApplicationList list in olvLists.SelectedObjects)
            {
                this.lastDeletedList = list;
                lblUndoDelete.Text = string.Format("List \"{0}\" deleted. Undo", list.Name);
                lblUndoDelete.LinkArea = new LinkArea(lblUndoDelete.Text.Length - 4, 4);
                lblUndoDelete.Visible = true;
                list.Delete();
                olvLists.RemoveObject(list);
            }
        }

        private void olvLists_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                bRemoveList.PerformClick();
            }
        }

        private void bAddApp_Click(object sender, EventArgs e)
        {
            using (SelectApplicationDialog dialog = new SelectApplicationDialog())
            {
                dialog.Applications = this.lists[0].Applications.ToArray();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    ApplicationList list = olvLists.SelectedObject as ApplicationList;
                    foreach (ApplicationJob app in dialog.SelectedApplications)
                    {
                        list.Applications.Add(app);
                    }
                    list.Save();
                    UpdateAppList(list);
                    olvApps.SetObjects(list.Applications);
                }
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.shouldUpdateApplications = false;
            InstallConfirm();
        }

        private void mnuInstallOnly_Click(object sender, EventArgs e)
        {
            this.shouldUpdateApplications = false;
            InstallConfirm();
        }

        private void mnuUpdateAndInstall_Click(object sender, EventArgs e)
        {
            this.shouldUpdateApplications = true;
            InstallConfirm();
        }

        private void olvApps_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(((OLVListItem)e.Item).RowObject, DragDropEffects.Copy);
        }

        private void olvLists_DragDrop(object sender, DragEventArgs e)
        {
            ApplicationJob job = e.Data.GetData(typeof(ApplicationJob).FullName) as ApplicationJob;
            if (job != null)
            {
                OLVColumn column;
                Point actualPoint = olvLists.PointToClient(new Point(e.X, e.Y));
                OLVListItem targetItem = olvLists.GetItemAt(actualPoint.X, actualPoint.Y, out column);
                if (targetItem != null)
                {
                    ApplicationList list = targetItem.RowObject as ApplicationList;
                    list.Applications.Add(job);
                    list.Save();
                    UpdateAppList(list);
                }
            }
        }

        private void olvLists_DragEnter(object sender, DragEventArgs e)
        {
            CheckDropOnList(e);
        }

        private void olvLists_DragOver(object sender, DragEventArgs e)
        {
            CheckDropOnList(e);
        }

        private void CheckDropOnList(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ApplicationJob).FullName))
            {
                // Check if custom list is hovered
                OLVColumn column;
                Point actualPoint = olvLists.PointToClient(new Point(e.X, e.Y));
                OLVListItem targetItem = olvLists.GetItemAt(actualPoint.X, actualPoint.Y, out column);
                if (targetItem != null)
                {
                    ApplicationList list = targetItem.RowObject as ApplicationList;
                    e.Effect = list.IsPredefined ? DragDropEffects.None : DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void olvApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void olvApps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                bRemoveApp.PerformClick();
            }
        }

        private void bRemoveApp_Click(object sender, EventArgs e)
        {
            ApplicationList currentList = olvLists.SelectedObject as ApplicationList;
            if (currentList != null)
            {
                foreach (ApplicationJob app in olvApps.SelectedObjects)
                {
                    currentList.Applications.Remove(app);
                    olvApps.RemoveObject(app);
                    currentList.Save();
                    UpdateAppList(currentList);
                }
            }
        }

        private void lblUndoDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.lastDeletedList != null)
            {
                CreateNewAppList(this.lastDeletedList, false);
                this.lastDeletedList = null;
                lblUndoDelete.Visible = false;
            }
        }

        #endregion

        #region Selection button

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            olvApps.CheckedObjects = olvApps.Objects;
        }

        private void mnuSelectNone_Click(object sender, EventArgs e)
        {
            olvApps.CheckedObjects = null;
        }

        private void mnuInvertSelection_Click(object sender, EventArgs e)
        {
            List<object> allObjects = new List<object>();
            allObjects.AddRange(olvApps.Objects.ToArray());
            foreach (object o in olvApps.CheckedObjects)
            {
                allObjects.Remove(o);
            }
            olvApps.CheckedObjects = allObjects;
        }

        private void mnuSaveAsNewList_Click(object sender, EventArgs e)
        {
            ApplicationList newList = new ApplicationList("New list", false);
            foreach (ApplicationJob app in olvApps.CheckedObjects)
            {
                newList.Applications.Add(app);
            }
            CreateNewAppList(newList, true);
        }

        #endregion

    }
}
