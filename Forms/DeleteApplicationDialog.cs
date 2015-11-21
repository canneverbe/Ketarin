using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CDBurnerXP.IO;

namespace Ketarin.Forms
{
    public partial class DeleteApplicationDialog : Form
    {
        private DeleteApplicationDialog()
        {
            InitializeComponent();

            CancelButton = bCancel;
            AcceptButton = bDeleteApplication;

            picIcon.Image = SystemIcons.Question.ToBitmap();
        }

        /// <summary>
        /// Shows a dialog asking the user whether or not to delete the applications
        /// and deletes them if wanted.
        /// </summary>
        /// <returns>true if the application has been deleted, false otherwise</returns>
        public static bool Show(IWin32Window owner, ArrayList applications)
        {
            return Show(owner, applications.Cast<ApplicationJob>().ToArray());
        }

        /// <summary>
        /// Shows a dialog asking the user whether or not to delete the applications
        /// and deletes them if wanted.
        /// </summary>
        /// <returns>true if the application has been deleted, false otherwise</returns>
        public static bool Show(IWin32Window owner, ApplicationJob[] applications)
        {
            if (applications == null || applications.Length == 0) return false;

            using (DeleteApplicationDialog dialog = new DeleteApplicationDialog())
            {
                if (applications.Length == 1)
                {
                    dialog.lblQuestion.Text = string.Format(dialog.lblQuestion.Text, applications[0].Name);
                }
                else
                {
                    dialog.lblQuestion.Text = "Do you really want to delete the selected applications from the list?";
                }

                switch (dialog.ShowDialog(owner))
                {
                    case DialogResult.OK: // "Delete application"
                        foreach (ApplicationJob application in applications)
                        {
                            application.Delete();
                        }
                        return true;

                    case DialogResult.Yes: // "Delete application and file"
                        foreach (ApplicationJob application in applications)
                        {
                            PathEx.TryDeleteFiles(application.CurrentLocation);
                            application.Delete();
                        }
                        return true;

                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
        }
    }
}
