using System;

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
            // Only enable if the name is not empty. Allow same name,
            // badd apps should be purged using a voting mechanism.
            bOK.Enabled = !string.IsNullOrEmpty(txtNewName.Text);
        }

        protected override void OnSelectedApplicationChanged(object sender, EventArgs e)
        {
            // Do not enable OK
        }
    }
}
