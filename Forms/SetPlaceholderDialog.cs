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
                if (tblMain.Controls.Contains(cboValue))
                {
                    return cboValue.Text as string;
                }
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
                cboValue.Text = value;
            }
        }

        /// <summary>
        /// Allows to specify a number of possible options (pipe separated).
        /// </summary>
        public string Options
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    cboValue.Visible = false;
                    txtValue.Visible = true;
                }
                else
                {
                    cboValue.Visible = true;
                    cboValue.Bounds = txtValue.Bounds;
                    tblMain.Controls.Remove(txtValue);
                    tblMain.Controls.Add(cboValue);
                    txtValue.Visible = false;

                    cboValue.Items.AddRange(value.Split('|'));
                }
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
