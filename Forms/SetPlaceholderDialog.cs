using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog which allows the user to enter 
    /// a value for a placeholder within a template
    /// </summary>
    public partial class SetPlaceholderDialog : Form
    {
        #region Properties

        /// <summary>
        /// Returns the value which has been entered by the user.
        /// </summary>
        public string Value
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }

        #endregion

        public SetPlaceholderDialog(string placeholderName)
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
            lblPlaceholderName.Text = placeholderName;
        }
    }
}
