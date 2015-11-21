using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents an editor dialog, which allows
    /// to enter POST data for an URL.
    /// </summary>
    public partial class PostDataEditor : Form
    {
        private string m_PostData;
        private readonly DataTable m_Table = new DataTable();

        #region Properties

        /// <summary>
        /// Gets or sets the POST data.
        /// Example: a=b&amp;c=d
        /// </summary>
        [Browsable(false)]
        public string PostData
        {
            get { return m_PostData; }
            set
            {
                m_Table.Rows.Clear();

                if (value == null) return;

                // Fill table from the POST data string
                foreach (string[] keyValue in WebClient.GetKeyValuePairs(value))
                {
                    m_Table.Rows.Add(keyValue);
                }
            }
        }

        #endregion

        public PostDataEditor()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            m_Table.Columns.Add("Name");
            m_Table.Columns.Add("Value");

            gridData.DataSource = m_Table;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // Set POST result data
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in m_Table.Rows)
            {
                sb.Append(HttpUtility.UrlEncode(row[0] as string) + "=" + HttpUtility.UrlEncode(row[1] as string) + "&");
            }

            m_PostData = sb.ToString().TrimEnd('&');
        }
    }
}
