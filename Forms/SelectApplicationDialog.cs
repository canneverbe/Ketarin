using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Allows the user to select one or more applications from a list of applications.
    /// </summary>
    public partial class SelectApplicationDialog : PersistentForm
    {
        #region Properties

        /// <summary>
        /// Gets the selected (checked) applications.
        /// </summary>
        public ApplicationJob[] SelectedApplications
        {
            get
            {
                return this.olvApplications.CheckedObjects.Cast<ApplicationJob>().ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the applications among which to choose.
        /// </summary>
        public ApplicationJob[] Applications { get; set; }

        #endregion

        public SelectApplicationDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            olvApplications.Initialize();
            olvApplications.SetObjects(this.Applications);
        }

        private void olvApplications_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            bOK.Enabled = (olvApplications.CheckedObjects.Count > 0);
        }
    }
}
