using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;
using CDBurnerXP.IO;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which displays all errors 
    /// which occured during an update process.
    /// </summary>
    public partial class ErrorsDialog : PersistentForm
    {
        private ApplicationJobError[] m_Errors;

        #region Properties

        /// <summary>
        /// Gets or sets the errors which are to be shown in the dialog.
        /// </summary>
        internal ApplicationJobError[] Errors
        {
            get { return m_Errors; }
            set { m_Errors = value; }
        }

        #endregion

        internal ErrorsDialog(ApplicationJobError[] errors)
        {
            InitializeComponent();
            CancelButton = bClose;

            m_Errors = errors;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            olvErrors.SetObjects(m_Errors);
        }

        private void bCopyToClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            // Copy all errors to clipboard (separated with tabs and newlines)
            foreach (ApplicationJobError error in m_Errors)
            {
                sb.Append(error.ApplicationJob.Name);
                sb.Append("\t");
                sb.AppendLine(error.Message);
            }

            SafeClipboard.SetData(sb.ToString(), true);
        }
    }
}
