using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Ketarin.Forms;
using System.Data.SQLite;

namespace Ketarin
{
    [XmlRoot("UrlVariable")]
    public class UrlVariable : ICloneable
    {
        private string m_Url;
        private string m_StartText;
        private string m_EndText;
        private string m_Name;
        private string m_TempContent = string.Empty;
        private string m_Regex = string.Empty;
        private string m_CachedContent = string.Empty;
        private long m_JobId = 0;
        private static ApplicationJob.UrlVariableCollection m_GlobalVariables = null;

        #region Properties

        /// <summary>
        /// Ensures that the global variables are read from
        /// the database when they are accessed for the next time.
        /// </summary>
        public static void ReloadGlobalVariables()
        {
            m_GlobalVariables = null;
        }

        public static ApplicationJob.UrlVariableCollection GlobalVariables
        {
            get
            {
                if (m_GlobalVariables == null)
                {
                    m_GlobalVariables = new ApplicationJob.UrlVariableCollection();

                    using (SQLiteConnection conn = DbManager.NewConnection)
                    {
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.CommandText = @"SELECT * FROM variables WHERE JobId = 0";
                            using (IDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UrlVariable variable = new UrlVariable();
                                    variable.Hydrate(reader);
                                    m_GlobalVariables.Add(variable.Name, variable);
                                }
                            }
                        }
                    }
                }
                return m_GlobalVariables;
            }
        }

        [XmlElement("Regex")]
        public string Regex
        {
            get { return m_Regex; }
            set { m_Regex = value; }
        }

        [XmlElement("Url")]
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        [XmlElement("StartText")]
        public string StartText
        {
            get { return m_StartText; }
            set { m_StartText = value; }
        }

        [XmlElement("EndText")]
        public string EndText
        {
            get { return m_EndText; }
            set { m_EndText = value; }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Temporarily store content related to this
        /// variable for short term caching purposes.
        /// </summary>
        [XmlIgnore()]
        internal string TempContent
        {
            get { return m_TempContent; }
            set { m_TempContent = value; }
        }

        /// <summary>
        /// Stores the content of the variable
        /// in the database. This can be used for a 
        /// custom column, without the need for web requests.
        /// </summary>
        [XmlIgnore()]
        public string CachedContent
        {
            get { return m_CachedContent; }
            set { m_CachedContent = value; }
        }

        #endregion

        public UrlVariable()
        {
        }

        public UrlVariable(string name, long jobId)
        {
            m_Name = name;
            m_JobId = jobId;
        }

        public void Hydrate(IDataReader reader)
        {
            m_Name = reader["VariableName"] as string;
            m_StartText = reader["StartText"] as string;
            m_EndText = reader["EndText"] as string;
            m_Url = reader["Url"] as string;
            m_Regex = reader["RegularExpression"] as string;
            m_CachedContent = reader["CachedContent"] as string;
            m_JobId = Convert.ToInt32(reader["JobId"]);
        }

        public void Save(IDbTransaction transaction, long parentJobId)
        {
            IDbConnection conn = (transaction != null) ? transaction.Connection : DbManager.NewConnection;
            using (IDbCommand command = conn.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO variables (JobId, VariableName, Url, StartText, EndText, RegularExpression, CachedContent)
                                             VALUES (@JobId, @VariableName, @Url, @StartText, @EndText, @RegularExpression, @CachedContent)";

                command.Parameters.Add(new SQLiteParameter("@JobId", parentJobId));
                command.Parameters.Add(new SQLiteParameter("@VariableName", m_Name));
                command.Parameters.Add(new SQLiteParameter("@Url", m_Url));
                command.Parameters.Add(new SQLiteParameter("@StartText", m_StartText));
                command.Parameters.Add(new SQLiteParameter("@EndText", m_EndText));
                command.Parameters.Add(new SQLiteParameter("@RegularExpression", m_Regex));
                command.Parameters.Add(new SQLiteParameter("@CachedContent", m_CachedContent));
                command.ExecuteNonQuery();
                m_JobId = parentJobId;
            }
        }

        public static bool IsVariableDownloadNeeded(string name, string formatString)
        {
            string find = "{" + name + "}";
            string customColumnVariable = "{" + SettingsDialog.CustomColumnVariableName + "}";
            // If variable is unused, don't make any efforts
            // Unless, of course, the variable is used for the custom column
            return (formatString.Contains(find) || customColumnVariable == find);
        }

        public virtual string ReplaceInString(string url)
        {
            if (!IsVariableDownloadNeeded(m_Name, url)) return url;
            
            string find = "{" + m_Name + "}";

            // Global variable only has static content
            if (m_JobId == 0)
            {
                return url.Replace(find, m_CachedContent);
            }

            // Ignore missing URLs
            if (string.IsNullOrEmpty(m_Url)) return url;

            string page = string.Empty;
            // Get the content we need to put in
            using (WebClient client = new WebClient())
            {
                page = client.DownloadString(m_Url);
            }

            // Normalise line-breaks
            page = page.Replace("\r\n", "\n");
            page = page.Replace("\r", "\n");

            // Using a regular expression?
            if (!string.IsNullOrEmpty(m_Regex))
            {
                Regex regex = new Regex(m_Regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = regex.Match(page);
                if (match.Success)
                {
                    if (match.Groups.Count == 1)
                    {
                        m_CachedContent = match.Value;
                        return url.Replace(find, match.Value);
                    }
                    else if (match.Groups.Count == 2)
                    {
                        m_CachedContent = match.Groups[1].Value;
                        return url.Replace(find, match.Groups[1].Value);
                    }
                }
            }

            // Use whole page if either start or end is missing
            if (string.IsNullOrEmpty(m_StartText) || string.IsNullOrEmpty(m_EndText))
            {
                m_CachedContent = page;
                return url.Replace(find, page);
            }

            int startPos = page.IndexOf(m_StartText);
            if (startPos < 0) return url;

            int endOfStart = startPos + m_StartText.Length;

            int endPos = page.IndexOf(m_EndText, endOfStart);
            if (endPos < 0) return url;

            string result = page.Substring(endOfStart, endPos - endOfStart);

            m_CachedContent = result;
            url = url.Replace(find, result);

            return url;
        }

        #region ICloneable Member

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {
            return m_Name;
        }
    }
}