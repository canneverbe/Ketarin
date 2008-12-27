using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    public partial class LogDialog : CDBurnerXP.Forms.PersistentForm
    {
        private static LogDialog m_Instance;
        private static Queue<string> m_Log = new Queue<string>();

        #region Properties

        public static LogDialog Instance
        {
            get {
                if (m_Instance == null)
                {
                    m_Instance = new LogDialog();
                }
                return LogDialog.m_Instance;
            }
            set { LogDialog.m_Instance = value; }
        }

        #endregion

        private LogDialog()
        {
            InitializeComponent();
            m_Instance = this;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;
            Hide();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
            {
                while (m_Log.Count > 0)
                {
                    txtLog.AppendText(m_Log.Dequeue());
                    txtLog.AppendText(Environment.NewLine);
                }

                m_Instance.txtLog.SelectionStart = m_Instance.txtLog.Text.Length;
            }
        }

        #region Public Log() functions

        public static void Log(ApplicationJob job, string text)
        {
            Log(job.Name + ": " + text);
        }

        public static void Log(ApplicationJob job, Exception ex)
        {
            Log(job.Name + ": Failed, " + ex.Message);
        }

        public static void Log(string text, Exception ex)
        {
            Log(text + " (" + ex.Message + ")");
        }

        public static void Log(UrlVariable var, string url, string replacement)
        {
            Log("Replacing {" + var.Name + "} in '" + url + "' with '" + replacement + "'");
        }

        public static void Log(ApplicationJob job, bool fileSizeMismatch, bool dateMismatch)
        {
            if (fileSizeMismatch)
            {
                Log(job, "Update required, file sizes do not match");
            }
            else if (dateMismatch)
            {
                Log(job, "Update required, file modified dates do not match");
            }
            else
            {
                Log(job, "Update not required");
            }
        }

        /// <summary>
        /// Writes a given text to the long, appended with the current date/time.
        /// </summary>
        public static void Log(string text)
        {
            text = DateTime.Now.ToString() + ": " + text;
            m_Log.Enqueue(text);

            if (m_Instance != null)
            {
                m_Instance.AddToTextBox(text);
            }
            else
            {
                Console.Error.WriteLine(text);
            }
        }

        private void AddToTextBox(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate()
                {
                    AddToTextBox(text);
                });
            }
            else
            {
                if (Visible) {
                    txtLog.AppendText(text);
                    txtLog.AppendText(Environment.NewLine);

                    txtLog.SelectionStart = m_Instance.txtLog.Text.Length;
                }
            }
        }

        #endregion
    }
}
