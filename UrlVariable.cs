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
        private ApplicationJob m_Parent = null;
        private long m_JobId = 0;
        private static ApplicationJob.UrlVariableCollection m_GlobalVariables = null;
        /// <summary>
        /// Prevent recursion with the ExpandedUrl property.
        /// </summary>
        private bool m_Expanding = false;

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

        /// <summary>
        /// If the URL contains variables, this property
        /// will return the URL with all variables replaced.
        /// </summary>
        [XmlIgnore()]
        public string ExpandedUrl
        {
            get
            {
                if (m_Parent == null || m_Expanding) return m_Url;

                m_Expanding = true;
                try
                {
                    return m_Parent.Variables.ReplaceAllInString(m_Url);
                }
                finally
                {
                    m_Expanding = false;
                }
            }
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

        /// <summary>
        /// Creates a new global variable.
        /// </summary>
        internal UrlVariable()
        {
        }

        /// <summary>
        /// When loading variables for a given application job,
        /// this constructor can be used to prepare a variable for Hydrate().
        /// </summary>
        internal UrlVariable(ApplicationJob job) : this(null, job)
        {
        }

        /// <summary>
        /// Creates a new variable for a given application.
        /// </summary>
        internal UrlVariable(string name, ApplicationJob job)
        {
            m_Name = name;
            m_Parent = job;
            if (m_Parent != null)
            {
                m_JobId = m_Parent.Id;
            }
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

        /// <summary>
        /// Detertmines whether or not a variable needs to be downloaded for a 
        /// given string. This would be the case for custom column variables or
        /// variables used conventionally.
        /// </summary>
        /// <param name="name">Name of the variable without { and }</param>
        /// <param name="formatString">String to check</param>
        public static bool IsVariableDownloadNeeded(string name, string formatString)
        {
            // If variable is unused, don't make any efforts
            // Unless, of course, the variable is used for the custom column
            return (IsVariableUsedInString(name, formatString) || name == SettingsDialog.CustomColumnVariableName);
        }

        /// <summary>
        /// Determines whether or not a variable is used within a string.
        /// It also matches if functions like {variable:replace:a:b} are used.
        /// </summary>
        /// <param name="name">Name of the variable without { and }</param>
        /// <param name="formatString">String to check</param>
        private static bool IsVariableUsedInString(string name, string formatString)
        {
            Regex regex = new Regex(@"\{" + QuoteRegex(name) + @"(\:[^\}]+)?\}");
            return regex.IsMatch(formatString);
        }

        private static string QuoteRegex(string value)
        {
            value = value.Replace("\\", "\\\\");
            value = value.Replace("+", @"\+");
            value = value.Replace("*", @"\*");
            value = value.Replace("?", @"\?");
            value = value.Replace("[", @"\[");
            value = value.Replace("^", @"\^");
            value = value.Replace("]", @"\]");
            value = value.Replace("$", @"\$");
            value = value.Replace("(", @"\(");
            value = value.Replace(")", @"\)");
            value = value.Replace("{", @"\{");
            value = value.Replace("}", @"\}");
            value = value.Replace("=", @"\=");
            value = value.Replace("!", @"\!");
            value = value.Replace("<", @"\<");
            value = value.Replace(">", @"\>");
            value = value.Replace("|", @"\|");
            return value.Replace(":", @"\;");
        }

        /// <summary>
        /// Replaces this variable within a string with the given content.
        /// Applies functions if necessary.
        /// </summary>
        private string Replace(string formatString, string content)
        {
            Regex regex = new Regex(@"\{" + QuoteRegex(m_Name) + @"(\:[^\}]+)?\}", RegexOptions.Singleline);

            Match match = regex.Match(formatString);
            // We need to "rematch" multiple times if the string changes
            while (match.Success)
            {
                string functionPart = match.Groups[1].Value;
                formatString = formatString.Remove(match.Index, match.Length);
                formatString = formatString.Insert(match.Index, ReplaceFunction(functionPart, content));
                match = regex.Match(formatString);
            } 

            return formatString;
        }

        /// <summary>
        /// Applies a function (if given) to content and returns the
        /// modified content.
        /// </summary>
        /// <param name="function">A function specification, for example "replace:a:b"</param>
        /// <param name="content">The usual variable content</param>
        private string ReplaceFunction(string function, string content)
        {
            function = function.TrimStart(':');
            if (string.IsNullOrEmpty(function)) return content;

            string[] parts = SplitEscaped(function, ':');
            if (parts.Length == 0) return content;

            switch (parts[0])
            {
                case "toupper":
                    return content.ToUpper();
                case "tolower":
                    return content.ToLower();
                case "trim":
                    return content.Trim();

                case "replace":
                    if (parts.Length >= 3)
                    {
                        return content.Replace(parts[1], parts[2]);
                    }
                    break;
            }

            return content;
        }

        /// <summary>
        /// Splits a string at every occurence of 'splitter', but
        /// only if this character has not been escaped with a \.
        /// </summary>
        private string[] SplitEscaped(string value, char splitter)
        {
            List<string> result = new List<string>();
            StringBuilder temp = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != splitter || IsEscaped(value, i))
                {
                    if (IsEscaped(value, i))
                    {
                        temp.Remove(temp.Length - 1, 1);
                    }
                    temp.Append(value[i]);
                }
                else
                {
                    result.Add(temp.ToString());
                    temp = new StringBuilder();
                }
            }

            result.Add(temp.ToString());

            return result.ToArray();
        }

        /// <summary>
        /// Determines whether or not the character at the 
        /// given position is escaped.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool IsEscaped(string value, int pos)
        {
            return (pos > 0 && value[pos - 1] == '\\' && !IsEscaped(value, pos - 1));
        }

        public virtual string ReplaceInString(string url)
        {
            return ReplaceInString(url, false);
        }

        public virtual string ReplaceInString(string url, bool onlyCached)
        {
            if (!IsVariableDownloadNeeded(m_Name, url)) return url;

            // Global variable only has static content
            if (m_JobId == 0 || onlyCached)
            {
                LogDialog.Log(this, url, m_CachedContent);
                return string.IsNullOrEmpty(m_CachedContent) ? url : Replace(url, m_CachedContent);
            }

            // Ignore missing URLs
            if (string.IsNullOrEmpty(m_Url)) return url;

            string page = string.Empty;
            // Get the content we need to put in
            using (WebClient client = new WebClient())
            {
                page = client.DownloadString(ExpandedUrl);
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
                        LogDialog.Log(this, url, m_CachedContent);
                        return Replace(url, match.Value);
                    }
                    else if (match.Groups.Count == 2)
                    {
                        m_CachedContent = match.Groups[1].Value;
                        LogDialog.Log(this, url, m_CachedContent);
                        return Replace(url, match.Groups[1].Value);
                    }
                }
            }

            // Use whole page if either start or end is missing
            if (string.IsNullOrEmpty(m_StartText) || string.IsNullOrEmpty(m_EndText))
            {
                m_CachedContent = page;
                LogDialog.Log(this, url, m_CachedContent);
                return Replace(url, page);
            }

            int startPos = page.IndexOf(m_StartText);
            if (startPos < 0) return url;

            int endOfStart = startPos + m_StartText.Length;

            int endPos = page.IndexOf(m_EndText, endOfStart);
            if (endPos < 0) return url;

            string result = page.Substring(endOfStart, endPos - endOfStart);

            m_CachedContent = result;
            LogDialog.Log(this, url, m_CachedContent);
            url = Replace(url, result);

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