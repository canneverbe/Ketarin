using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    public partial class SimilarApplicationsDialog : ApplicationDatabaseBaseDialog
    {
        private ApplicationJob m_ApplicationJob;

        #region Properties

        public ApplicationJob ApplicationJob
        {
            get { return m_ApplicationJob; }
            set { m_ApplicationJob = value; }
        }

        #endregion

        public SimilarApplicationsDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bOK.Enabled = false;
            txtNewName.Text = m_ApplicationJob.Name;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            m_ApplicationJob.Name = txtNewName.Text;
        }

        private void txtNewName_TextChanged(object sender, EventArgs e)
        {
            // Only enable if the name is not empty and not the
            // exact same as one in the list.
            foreach (RpcApplication job in olvApplications.Objects)
            {
                if (string.Compare(job.ApplicationName, txtNewName.Text, true) == 0)
                {
                    bOK.Enabled = false;
                    return;
                }
            }

            bOK.Enabled = !string.IsNullOrEmpty(txtNewName.Text);
        }
    }
}
