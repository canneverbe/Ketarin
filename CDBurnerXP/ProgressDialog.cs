using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CDBurnerXP.Forms
{
    /// <summary>
    /// A simple dialog, which shows the progress of an
    /// asynchronous operation.
    /// </summary>
    public partial class ProgressDialog : 
#if CDBXP
        BaseForm
#else
        PersistentForm
#endif
    {
        public delegate object AddingFilesDelegate();
        public delegate void CancelActionDelegate();
        public delegate string UpdateStatusDelegate();

        private object m_Result = null;
        private AddingFilesDelegate m_DoWorkDelegate;
        private CancelActionDelegate m_CancelActionDelegate;
        private UpdateStatusDelegate m_UpdateStatusDelegate;
        private bool m_Cancelled = false;
        private bool m_SelfCancelled = false;
        private Exception m_Error = null;
        private DateTime lastIdleReset = DateTime.MinValue;

        #region Properties

        /// <summary>
        /// If an error occured during the operation, its exception
        /// will be stored and accessible here.
        /// </summary>
        public Exception Error
        {
            get { return m_Error; }
            set { m_Error = value; }
        }

#if CDBXP
        protected override bool AutoTranslate
        {
            get
            {
                return true;
            }
        }
#endif

        public string StatusText
        {
            get
            {
                return lblStatus.Text;
            }
            set
            {
                lblStatus.Text = CompactString(value, tableLayoutPanel1.Width - 15, Font, "");
                lblStatus.Visible = !string.IsNullOrEmpty(lblStatus.Text);
            }
        }

        /// <summary>
        /// Gets whether or not the operation has been cancelled
        /// by the user before completion.
        /// </summary>
        public bool Cancelled
        {
            get
            {
                return m_Cancelled;
            }
        }

        /// <summary>
        /// A timer checks the result of this function
        /// a couple of times per second and displays the
        /// information (if available).
        /// </summary>
        public UpdateStatusDelegate OnUpdateStatus
        {
            get
            {
                return m_UpdateStatusDelegate;
            }
            set
            {
                m_UpdateStatusDelegate = value;
                if (value != null)
                {
                    StatusText = " "; // resize before the dialog has opened
                }
                else
                {
                    StatusText = string.Empty;
                }
            }
        }

        public CancelActionDelegate OnCancel
        {
            get
            {
                return m_CancelActionDelegate;
            }
            set
            {
                if (m_CancelActionDelegate != value)
                {
                    m_CancelActionDelegate = value;
                    // Enable or disable the cancel button if needed
                    CancelEnabled = (m_CancelActionDelegate != null);
                }
            }
        }

        protected bool CancelEnabled
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        CancelEnabled = value;
                    }));
                }
                else
                {
                    bCancel.Enabled = value;
                    Application.DoEvents();
                }
            }
        }

        public AddingFilesDelegate OnDoWork
        {
            get
            {
                return m_DoWorkDelegate;
            }
            set
            {
                m_DoWorkDelegate = value;
            }
        }

        public object Result
        {
            get
            {
                return m_Result;
            }
        }

        #endregion

        public ProgressDialog()
        {
            InitializeComponent();
            CancelButton = bCancel;
        }

        public ProgressDialog(string title, string description) : this()
        {
            this.Text = title;
            lblHeader.Text = description;
        }

        public void ReportProgress(int value)
        {
            if (this.prgProgress.InvokeRequired)
            {
                this.prgProgress.Invoke(new MethodInvoker(delegate { ReportProgress(value); }));
            }
            else
            {
                if (value > 0 && value <= 100)
                {
                    prgProgress.ShowMarquee = false;
                    prgProgress.Value = value;
                }
                else
                {
                    prgProgress.ShowMarquee = true;
                }
            }
        }

        /// <summary>
        /// Starts the background thread and shows the waiting dialog 
        /// if the process takes longer than 0.5 seconds.
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <returns>Result of the Dialog</returns>
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            Application.UseWaitCursor = true;

            try
            {
                DateTime start = DateTime.Now;
                m_Worker.RunWorkerAsync();

                while (m_Worker.IsBusy && (DateTime.Now - start) < TimeSpan.FromMilliseconds(500))
                {
                    Application.DoEvents();
                }

                if (m_Worker.IsBusy)
                {
                    // No wait cursor allowed in progress dialog
                    Application.UseWaitCursor = false;
                    // If still busy, show dialog
                    return base.ShowDialog(owner);
                }
                else
                {
                    // Process finished quickly, do not "flash" dialog
                    return DialogResult.OK;
                }
            }
            finally
            {
                Application.UseWaitCursor = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            prgProgress.ShowMarquee = true;
            base.OnLoad(e);
        }

        [System.Diagnostics.DebuggerNonUserCode()]
        private void m_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_DoWorkDelegate != null)
            {
                m_Result = this.OnDoWork();
            }
        }

        private void m_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            prgProgress.Value = 100;
            m_Error = e.Error;
            Close();
        }

        public void Cancel()
        {
            m_Worker.RunWorkerCompleted -= m_Worker_RunWorkerCompleted;
            m_Worker.Dispose();
            m_SelfCancelled = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (m_Worker.IsBusy && !m_SelfCancelled)
            {
                e.Cancel = true;
                if (bCancel.Enabled)
                {
                    bCancel.PerformClick();
                }
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            bCancel.Enabled = false;
            if (m_CancelActionDelegate != null) m_CancelActionDelegate();
            m_Cancelled = true;
        }

        private void m_StatusTimer_Tick(object sender, EventArgs e)
        {
            if (m_UpdateStatusDelegate != null)
            {
                StatusText = m_UpdateStatusDelegate();

#if CDBXP
                // Make sure that system does not enter standby mode adding files (large compilations might take a while).
                if (DateTime.UtcNow - this.lastIdleReset > TimeSpan.FromSeconds(30))
                {
                    Kernel32.SetThreadExecutionState(Kernel32.EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                    this.lastIdleReset = DateTime.UtcNow;
                }
#endif
            }
        }

        private string CompactString(string myString, int width, Font font, string otherText)
        {
            if (string.IsNullOrEmpty(myString))
            {
                return string.Empty;
            }

            string result = string.Copy(myString);
            width -= TextRenderer.MeasureText(otherText, font).Width;
            TextRenderer.MeasureText(result, font, new Size(width, 0), TextFormatFlags.PathEllipsis | TextFormatFlags.ModifyString);
            return result;
        }
    }
}
