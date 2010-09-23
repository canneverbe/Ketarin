using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using Microsoft.Win32;
using CDBurnerXP.IO;
using System.Data.SQLite;
using System.ComponentModel;

namespace Ketarin
{
    /// <summary>
    /// Represents a list of applications.
    /// </summary>
    public class ApplicationList
    {
        #region ApplicationBindingList

        /// <summary>
        /// Keeps the list in a consitent state (no duplicate applications per list allowed).
        /// </summary>
        public class ApplicationBindingList : BindingList<ApplicationJob>
        {
            protected override void InsertItem(int index, ApplicationJob item)
            {
                // Do not allow duplicate apps in a list
                foreach (ApplicationJob app in this)
                {
                    if (app == item) return;
                }

                base.InsertItem(index, item);
            }

            internal void AddRange(ApplicationJob[] applicationJobs)
            {
                foreach (ApplicationJob job in applicationJobs)
                {
                    this.Add(job);
                }
            }

            internal ApplicationJob[] ToArray()
            {
                ApplicationJob[] jobs = new ApplicationJob[this.Count];
                for (int i = 0; i < this.Count; i++)
                {
                    jobs[i] = this[i];
                }
                return jobs;
            }
        }

        #endregion

        private bool isPredefined = false;
        private ApplicationBindingList applications = new ApplicationBindingList();

        /// <summary>
        /// Gets or sets the GUID of the list.
        /// </summary>
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets if the list is predefined (cannot be changed by the user).
        /// </summary>
        public bool IsPredefined
        {
            get { return isPredefined; }
        }

        /// <summary>
        /// Gets the list of applications that are on this list.
        /// </summary>
        public ApplicationBindingList Applications
        {
            get
            {
                return this.applications;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a comma separated list of all application names.
        /// </summary>
        public string ApplicationNames
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (ApplicationJob job in this.Applications)
                {
                    sb.Append(job.Name);
                    sb.Append(", ");
                }
                return sb.ToString().TrimEnd(',', ' ');
            }
        }

        internal ApplicationList()
        {
        }

        public ApplicationList(string name, bool isPredefined)
        {
            this.Name = name;
            this.isPredefined = isPredefined;
        }

        /// <summary>
        /// Generates an icon based on the contained applications.
        /// </summary>
        internal Image GetIcon()
        {
            // Check for application icons that can be extracted
            List<string> iconPaths = new List<string>();
            foreach (ApplicationJob app in this.Applications)
            {
                if (!string.IsNullOrEmpty(app.PreviousLocation) && PathEx.TryGetFileSize(app.PreviousLocation) > 0)
                {
                    iconPaths.Add(app.PreviousLocation);
                }
            }

            if (iconPaths.Count == 0)
            {
                return Properties.Resources.Setup32;
            }
            else
            {
                Bitmap bitmap = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    if (iconPaths.Count == 1)
                    {
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[0], IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), new Point(0, 0));
                        }
                    }
                    else if (iconPaths.Count == 2)
                    {
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[0], IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 22, 22);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[1], IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 9, 9, 22, 22);
                        }
                    }
                    else if (iconPaths.Count == 3)
                    {
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[0], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 16, 16);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[1], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 8, 8, 16, 16);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[2], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 16, 16, 16);
                        }
                    }
                    else
                    {
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[0], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 16, 16);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[1], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 0, 16, 16);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[2], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 16, 16, 16);
                        }
                        using (Icon programIcon = IconReader.GetFileIcon(iconPaths[3], IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 16, 16, 16);
                        }
                    }
                }

                return bitmap;
            }
        }

        /// <summary>
        /// Saves the application list into the database.
        /// </summary>
        public void Save()
        {
            using (SQLiteTransaction transaction = DbManager.Connection.BeginTransaction())
            {
                if (this.Guid == null || this.Guid == Guid.Empty)
                {
                    this.Guid = Guid.NewGuid();
                }

                // Insert or update list
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT OR REPLACE INTO setuplists (ListGuid, Name) VALUES (@ListGuid, @Name)";
                    command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(this.Guid)));
                    command.Parameters.Add(new SQLiteParameter("@Name", this.Name));
                    command.ExecuteNonQuery();
                }

                // Update applications
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"DELETE FROM setuplists_applications WHERE ListGuid = @ListGuid";
                    command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(this.Guid)));
                    command.ExecuteNonQuery();
                }
                foreach (ApplicationJob app in this.Applications)
                {
                    using (IDbCommand command = DbManager.Connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"INSERT INTO setuplists_applications (ListGuid, JobGuid) VALUES (@ListGuid, @JobGuid)";
                        command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(this.Guid)));
                        command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(app.Guid)));
                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        /// <summary>
        /// Reads the database fields from a data reader.
        /// </summary>
        internal void Hydrate(IDataReader reader)
        {
            this.Guid = new Guid(reader["ListGuid"] as string);
            this.Name = reader["Name"] as string;
        }

        /// <summary>
        /// Deletes the list from the database.
        /// </summary>
        public void Delete()
        {
            if (this.isPredefined) return;

            using (SQLiteTransaction transaction = DbManager.Connection.BeginTransaction())
            {
                // Insert or update list
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"DELETE FROM setuplists WHERE ListGuid = @ListGuid";
                    command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(this.Guid)));
                    command.ExecuteNonQuery();
                }

                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"DELETE FROM setuplists_applications WHERE ListGuid = @ListGuid";
                    command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(this.Guid)));
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }
    }
}
