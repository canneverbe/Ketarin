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

        #region Properties

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

            // Item for all apps
            ApplicationList allApps = new ApplicationList("All applications", true);
            allApps.Applications.AddRange(DbManager.GetJobs());
            AddAppList(allApps);

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
                AddAppList(list);
            }
            
            olvLists.SetObjects(lists);
            olvLists.SelectedIndex = 0;
        }

        private void AddAppList(ApplicationList appList)
        {
            imlLists.Images.Add(appList.GetIcon());
            lists.Add(appList);
        }

        #region Events

        private void bNewList_Click(object sender, EventArgs e)
        {
            ApplicationList newList = new ApplicationList("New list", false);
            AddAppList(newList);
            newList.Save();
            olvLists.AddObject(newList);
            olvLists.SelectedObject = newList;

            OLVListItem selectedItem = olvLists.SelectedItem as OLVListItem;
            if (selectedItem != null)
            {
                selectedItem.EnsureVisible();
                olvLists.EditSubItem(selectedItem, 0);
            }
        }

        private void olvLists_CellEditStarting(object sender, ObjectListView.CellEditEventArgs e)
        {
            // Only allow editing of custom lists
            ApplicationList selectedList = olvLists.SelectedObject as ApplicationList;
            if (selectedList != null && selectedList.IsPredefined)
            {
                e.Cancel = true;
            }
        }

        private void olvLists_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.selectedApplications.Clear();
            foreach (ApplicationJob job in olvApps.CheckedObjects)
            {
                selectedApplications.Add(job);
            }
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
                    imlLists.Images[this.lists.IndexOf(list)] = list.GetIcon();
                    olvLists.RefreshObject(list);
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

        #endregion

    }
}
