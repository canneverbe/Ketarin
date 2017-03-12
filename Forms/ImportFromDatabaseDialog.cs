using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using CDBurnerXP;
using CookComputing.XmlRpc;

namespace Ketarin.Forms
{
    public partial class ImportFromDatabaseDialog : ApplicationDatabaseBaseDialog
    {
        private static RpcApplication[] m_LastLoadedApplications;
        private static string m_LastSearchText = string.Empty;

        #region Properties

        public List<ApplicationJob> ImportedApplications { get; } = new List<ApplicationJob>();

        #endregion

        public ImportFromDatabaseDialog()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (m_LastLoadedApplications != null)
            {
                this.Applications = m_LastLoadedApplications;
                this.txtSearchSubject.Text = m_LastSearchText;
            }
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                m_LastLoadedApplications = proxy.GetApplications(this.txtSearchSubject.Text);
                m_LastSearchText = this.txtSearchSubject.Text;
                this.Applications = m_LastLoadedApplications;
                this.olvApplications.EmptyListMsg = "No applications found";
            }
            catch (XmlRpcException ex)
            {
                MessageBox.Show(this, "An error occured while accessing the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WebException ex)
            {
                MessageBox.Show(this, "Could not connect to the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected override void OnSelectedApplicationChanged(object sender, EventArgs e)
        {
            base.OnSelectedApplicationChanged(sender, e);

            this.AcceptButton = this.bOK.Enabled ? this.bOK : this.bSearch;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                foreach (RpcApplication app in this.olvApplications.SelectedObjects)
                {
                    IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                    string xml = proxy.GetApplication(app.ShareId);
                    ApplicationJob resultJob = ApplicationJob.LoadOneFromXml(xml);
                    // For security reasons, we remove some of the properties
                    // if it's not the users own job
                    if (!DbManager.ApplicationExists(resultJob.Guid))
                    {
                        resultJob.CanBeShared = false;
                    }

                    resultJob.Save();
                    this.ImportedApplications.Add(resultJob);

                    // Real value is determined while saving
                    if (!resultJob.CanBeShared)
                    {
                        resultJob.DownloadDate = app.UpdatedAtDate;
                        resultJob.ExecuteCommand = string.Empty;
                        resultJob.ExecutePreCommand = string.Empty;
                        resultJob.TargetPath = string.Empty;
                        resultJob.PreviousLocation = string.Empty;

                        // If possible, determine some values based on the default
                        // values for a new application.
                        string defaultXml = Settings.GetValue("DefaultApplication", "") as string;
                        if (!string.IsNullOrEmpty(defaultXml))
                        {
                            ApplicationJob defaultApp = ApplicationJob.LoadOneFromXml(defaultXml);
                            if (defaultApp != null)
                            {
                                resultJob.TargetPath = defaultApp.TargetPath;
                                resultJob.ExecuteCommand = defaultApp.ExecuteCommand;
                                resultJob.ExecutePreCommand = defaultApp.ExecutePreCommand;
                            }
                        }

                        resultJob.Save();
                    }
                }
            }
            catch (XmlRpcException ex)
            {
                MessageBox.Show(this, "An error occured while importing applications: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            catch (WebException ex)
            {
                MessageBox.Show(this, "Could not connect to the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtSearchSubject_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = this.bSearch;
        }

        private void bTop50_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                m_LastLoadedApplications = proxy.GetMostDownloadedApplications();
                m_LastSearchText = string.Empty;
                this.olvApplications.Sort(this.colUseCount);
                this.Applications = m_LastLoadedApplications;
            }
            catch (XmlRpcException ex)
            {
                MessageBox.Show(this, "An error occured while accessing the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WebException ex)
            {
                MessageBox.Show(this, "Could not connect to the online database: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

    }
}
