using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using CDBurnerXP.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog, which visualises the HTML code
    /// loaded by a variable.
    /// </summary>
    public partial class BrowserPreviewDialog : PersistentForm, IComponent
    {
        public BrowserPreviewDialog()
        {
            InitializeComponent();
        }

        public void ShowPreview(IWin32Window owner, string url, string postData)
        {
            if (!Visible)
            {
                Show(owner);
            }

            if (String.IsNullOrEmpty(postData))
            {
                webBrowser.Navigate(url, "", null, null);
            }
            else
            {
                webBrowser.Navigate(url, "", Encoding.ASCII.GetBytes(postData), "Content-Type: application/x-www-form-urlencoded");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // Prevent disposing
            e.Cancel = true;
            Hide();
        }
    }
}
