using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CDBurnerXP.Controls
{
    public class WebLink : LinkLabel
    {
        private string m_Url = string.Empty;

        public string Url
        {
            get
            {
                return m_Url;
            }
            set
            {
                m_Url = value;
            }
        }

        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            base.OnLinkClicked(e);

            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    System.Diagnostics.Process.Start(m_Url);
                }
                catch (FileNotFoundException)
                {
                    // occurs if browser cannot be started or shows a message before starting
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // occurs if browser cannot be started or shows a message before starting
                }
            }
            else
            {
                try
                {
                    Clipboard.SetData(DataFormats.Text, m_Url);
                }
                catch (Exception) { }
            }
        }
    }
}
