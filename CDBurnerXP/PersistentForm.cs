using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CDBurnerXP.Controls;
using System.Drawing;
using System.ComponentModel;

namespace CDBurnerXP.Forms
{
    public class PersistentForm : Form
    {
        private bool m_SavePosition = false;

        #region Properties
        
        /// <summary>
        /// Gets or sets whether or not the position of a 
        /// dialog is saved additionally to the size.
        /// </summary>
        [DefaultValue(false)]
        public bool SavePosition
        {
            get { return m_SavePosition; }
            set { m_SavePosition = value; }
        }

        #endregion

        #region Settings

        /// <summary>
        /// Returns a list of all controls within the form.
        /// </summary>
        protected List<Control> GetAllControls()
        {
            return GetAllControls(this);
        }

        protected List<Control> GetAllControls(Control c)
        {
            List<Control> controls = new List<Control>();
            foreach (Control control in c.Controls)
            {
                controls.Add(control);
                controls.AddRange(GetAllControls(control));
            }
            return controls;
        }

        protected void SaveDialogSettings()
        {
            if (WindowState != FormWindowState.Maximized && WindowState != FormWindowState.Minimized)
            {
                Settings.SetValue(this, "Size", Size);
            }
            if (m_SavePosition && WindowState != FormWindowState.Minimized)
            {
                if (WindowState != FormWindowState.Maximized)
                {
                    Settings.SetValue(this, "Location", Location);
                }
                Settings.SetValue(this, "WindowState", WindowState);
            }

            foreach (Control c in GetAllControls())
            {
                ObjectListView olv = c as ObjectListView;
                // Save column widths and visibility
                if (olv != null)
                {
                    foreach (OLVColumn col in olv.AllColumns)
                    {
                        Settings.SetValue(olv, col.Text + ":Visibility", col.IsVisible);
                        Settings.SetValue(olv, col.Text + ":Width", col.Width);
                        Settings.SetValue(olv, col.Text + ":LastDisplayIndex", col.LastDisplayIndex);
                    }

                    Settings.SetValue(olv, "LastSortColumn", olv.LastSortColumn == null ? "" : olv.LastSortColumn.Text);
                    Settings.SetValue(olv, "LastSortOrder", olv.LastSortOrder);
                }
            }
        }

        protected virtual void LoadDialogSettings()
        {
            if (FormBorderStyle == FormBorderStyle.Sizable || FormBorderStyle == FormBorderStyle.SizableToolWindow)
            {
                Size = (Size)Settings.GetValue(this, "Size", Size);
            }

            object location = Settings.GetValue(this, "Location", null);
            if (m_SavePosition)
            {
                if (location != null)
                {
                    if (Utility.IsRectangleOnAnyScreen(new Rectangle((Point)location, Size)))
                    {
                        this.StartPosition = FormStartPosition.Manual;
                        Location = (Point)location;
                    }
                }
                WindowState = (FormWindowState)Settings.GetValue(this, "WindowState", FormWindowState.Normal);
            }

            foreach (Control c in GetAllControls())
            {
                ObjectListView olv = c as ObjectListView;
                // Save column widths and visibility
                if (olv != null)
                {
                    foreach (OLVColumn col in olv.AllColumns)
                    {
                        col.IsVisible = (bool)Settings.GetValue(olv, col.Text + ":Visibility", col.IsVisible);
                        if (!col.FillsFreeSpace)
                        {
                            col.Width = (int)Settings.GetValue(olv, col.Text + ":Width", col.Width);
                        }

                        col.LastDisplayIndex = (int)Settings.GetValue(olv, col.Text + ":LastDisplayIndex", col.LastDisplayIndex);
                    }

                    string sortColName = Settings.GetValue(olv, "LastSortColumn", olv.LastSortColumn == null ? null : olv.LastSortColumn.Name) as string;
                    if (!string.IsNullOrEmpty(sortColName))
                    {
                        foreach (OLVColumn col in olv.AllColumns)
                        {
                            if (col.Text == sortColName)
                            {
                                olv.LastSortColumn = col;
                                olv.LastSortOrder = (SortOrder)Conversion.ToInt(Settings.GetValue(olv, "LastSortOrder", olv.LastSortOrder));
                                olv.Sorting = olv.LastSortOrder;
                                break;
                            }
                        }
                    }

                    olv.RebuildColumns();
                }
            }
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                LoadDialogSettings();
            }
            catch (Exception)
            {
                // we don't want to see errors accessing the registry
            }

            base.OnLoad(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                SaveDialogSettings();
            }
            catch (Exception)
            {
                // we don't want to see errors accessing the registry
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Make sure that Window is actually visible
            if (!Utility.IsRectangleOnAnyScreen(new Rectangle(Location, Size)))
            {
                Location = new Point(0, 0);
            }
        }

        #endregion

    }
}
