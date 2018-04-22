using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    public partial class LogDialog : PersistentForm
    {
        private static LogDialog m_Instance;
        private static readonly Queue<string> m_Log = new Queue<string>();
        private static readonly List<string> m_FullLog = new List<string>();

        #region Properties

        public static LogDialog Instance
        {
            get {
                if (m_Instance == null)
                {
                    m_Instance = new LogDialog();
                }
                return m_Instance;
            }
            set { m_Instance = value; }
        }

        #endregion

        private LogDialog()
        {
            this.InitializeComponent();
            m_Instance = this;
            this.txtLog.ContextMenu = this.contextMenu;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;
            this.Hide();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                while (m_Log.Count > 0)
                {
                    this.AppendText(m_Log.Dequeue());
                }

                m_Instance.txtLog.SelectionStart = m_Instance.txtLog.Text.Length;
            }
        }

        public static void SaveLogToFile(string filename)
        {
            File.WriteAllText(filename, string.Join(Environment.NewLine, m_FullLog.ToArray()));
        }

        #region Public Log() functions

        public static void Log(ApplicationJob job, string text)
        {
            if (job == null)
            {
                Log(text);
            }
            else
            {
                Log(job.Name + ": " + text);
            }
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
            string prepend = (var.Parent?.Parent == null) ? "" : var.Parent.Parent.Name + ": ";
            Log(prepend + "Replacing {" + var.Name + "} in '" + url + "' with '" + replacement + "'");
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
                Log(job, "Update not required, since date and file size remain unchanged");
            }
        }

        /// <summary>
        /// Writes a given text to the long, appended with the current date/time.
        /// </summary>
        public static void Log(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            lock (m_Log)
            {
                text = DateTime.Now + ": " + text;
                m_Log.Enqueue(text);
                m_FullLog.Add(text);

                if (m_Instance != null)
                {
                    m_Instance.AddToTextBox(text);
                }
                else
                {
                    Console.Error.WriteLine(text);
                }
            }
        }

        #endregion

        private void AddToTextBox(string text)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.AddToTextBox(text);
                });
            }
            else
            {
                if (this.Visible) {
                    this.AppendText(text);

                    this.txtLog.SelectionStart = m_Instance.txtLog.Text.Length;
                }
            }
        }

        private void AppendText(string text)
        {
            if (text != null)
            {
                this.txtLog.AppendText(text);
                this.txtLog.AppendText(Environment.NewLine);
            }
        }

        private void mnuClearLog_Click(object sender, EventArgs e)
        {
            lock (m_Log)
            {
                m_Log.Clear();
                m_FullLog.Clear();
                this.txtLog.Clear();
            }
        }
    }
}
