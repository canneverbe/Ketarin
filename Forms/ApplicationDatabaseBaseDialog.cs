using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CookComputing.XmlRpc;

namespace Ketarin.Forms
{
    public partial class ApplicationDatabaseBaseDialog : PersistentForm
    {
        #region Properties

        public IEnumerable<RpcApplication> Applications
        {
            set
            {
                olvApplications.SetObjects(value);
            }
        }

        #endregion

        public ApplicationDatabaseBaseDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        protected virtual void OnSelectedApplicationChanged(object sender, EventArgs e)
        {
            bOK.Enabled = (olvApplications.SelectedIndices.Count > 0);
        }

        private void olvApplications_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bOK.PerformClick();
        }

        private void cmnuProperties_Click(object sender, EventArgs e)
        {
            if (olvApplications.SelectedObject == null) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                RpcApplication app = (RpcApplication)olvApplications.SelectedObject;

                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                string xml = proxy.GetApplication(app.ShareId);
                ApplicationJob resultJob = ApplicationJob.LoadOneFromXml(xml);

                using (ApplicationJobDialog dialog = new ApplicationJobDialog())
                {
                    dialog.ApplicationJob = resultJob;
                    dialog.ReadOnly = true;
                    dialog.ShowDialog(this);
                }
            }
            catch (XmlRpcException)
            {
                MessageBox.Show(this, "Failed loading the selected application.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WebException)
            {
                MessageBox.Show(this, "Failed loading the selected application.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

    }
}
