using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CookComputing.XmlRpc;

namespace Ketarin.Forms
{
    public partial class ApplicationDatabaseDialog : PersistentForm
    {
        public ApplicationDatabaseDialog()
        {
            InitializeComponent();

            AcceptButton = bImport;
            CancelButton = bCancel;
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                olvApplications.SetObjects(proxy.GetApplications(txtSearchSubject.Text));
                olvApplications.EmptyListMsg = "No applications found";
            }
            catch (XmlRpcException ex)
            {
                MessageBox.Show(this, "An error occured while accessing the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void olvApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            bImport.Enabled = (olvApplications.SelectedIndices.Count > 0);
            AcceptButton = bImport.Enabled ? bImport : bSearch;
        }

        private void txtSearchSubject_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = bSearch;
        }

        private void olvApplications_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bImport.PerformClick();
        }

        private void bImport_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                foreach (RpcApplication app in olvApplications.SelectedObjects)
                {
                    IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                    string xml = proxy.GetApplication(app.ShareId);
                    ApplicationJob resultJob = ApplicationJob.LoadFromXml(xml);
                    // For security reasons, we remove some of the properties
                    // if it's not the users own job
                    if (!resultJob.SetIdByGuid(resultJob.Guid))
                    {
                        resultJob.CanBeShared = false;
                    }

                    resultJob.Save();

                    // Real value is determined while saving
                    if (!resultJob.CanBeShared)
                    {
                        resultJob.ExecuteCommand = string.Empty;
                        resultJob.TargetPath = string.Empty;
                        resultJob.PreviousLocation = string.Empty;
                        resultJob.Save();
                    }
                }
            }
            catch (XmlRpcException ex)
            {
                MessageBox.Show(this, "An error occured while importing applications: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
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
                ApplicationJob resultJob = ApplicationJob.LoadFromXml(xml);

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
            finally
            {
                Cursor = Cursors.Default;
            }
        }


    }
}
