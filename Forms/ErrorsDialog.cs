using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    public partial class ErrorsDialog : PersistentForm
    {
        private ApplicationJobError[] m_Errors;

        #region Properties

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
    }
}
