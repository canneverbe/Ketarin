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
    /// <summary>
    /// A variable which can be used for download locations
    /// or commands and determines its content dynamically
    /// based on other webpages' content.
    /// </summary>
    [XmlRoot("UrlVariable")]
    public class UrlVariable : ICloneable
    {
        /// <summary>
        /// Defines the possible types for a variable.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// A variable which extracts content between
            /// a start and end text from a URL.
            /// </summary>
            StartEnd,
            /// <summary>
            /// A variable which extracts content using
            /// a regular expression from a URL.
            /// </summary>
            RegularExpression,
            /// <summary>
            /// A variable which is defined by plain text.
            /// The text may contain further variables.
            /// </summary>
            Textual
        }

        private Type m_VariableType;
        private string m_Url;
        private string m_StartText;
        private string m_EndText;
        private string m_TextualContent;
        private string m_Name;
        private string m_TempContent = string.Empty;
        private string m_Regex = string.Empty;
        private string m_CachedContent = string.Empty;
        private ApplicationJob.UrlVariableCollection m_Parent = null;
        private long m_JobId = 0;
        private int m_DownloadCount = 0;
        private static ApplicationJob.UrlVariableCollection m_GlobalVariables = null;
        /// <summary>
        /// Prevent recursion with the ExpandedUrl property.
        /// </summary>
        private bool m_Expanding = false;

        #region Properties

        /// <summary>
        /// Stores how often the variable has been downloaded.
        /// This value is per session and is not stored in the database.
        /// </summary>
        internal int DownloadCount
        {
            get { return m_DownloadCount; }
            set { m_DownloadCount = value; }
        }

        /// <summary>
        /// Gets or sets the UrlVariableCollection this variable belongs to.
        /// </summary>
        [XmlIgnore()]
        public ApplicationJob.UrlVariableCollection Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        /// <summary>
        /// Gets or sets the type of the variable.
        /// </summary>
        public Type VariableType
        {
            get { return m_VariableType; }
            set { m_VariableType = value; }
        }

        /// <summary>
        /// Collection of all global variables.
        /// </summary>
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
                if (m_Parent == null || m_Expanding || string.IsNullOrEmpty(m_Url))
                {
                    return m_Url;
                }

                m_Expanding = true;
                try
                {
                    return m_Parent.ReplaceAllInString(m_Url);
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

        /// <summary>
        /// For type 'Textual', this text represents the
        /// value which is to be used as variable content.
        /// Note: Variables are not replaced here.
        /// </summary>
        [XmlElement("TextualContent")]
        public string TextualContent
        {
            get { return m_TextualContent; }
            set { m_TextualContent = value; }
        }

        /// <summary>
        /// For type 'Textual', this text is to be
        /// used as replacement for the variable.
        /// </summary>
        [XmlIgnore()]
        public string ExpandedTextualContent
        {
            get
            {
                if (m_Parent == null || m_Expanding || string.IsNullOrEmpty(m_TextualContent))
                {
                    return m_TextualContent;
                }

                m_Expanding = true;
                try
                {
                    return m_Parent.ReplaceAllInString(m_TextualContent);
                }
                finally
                {
                    m_Expanding = false;
                }
            }
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

        /// <summary>
        /// Gets whether or not the variable is properly defined.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (string.IsNullOrEmpty(m_Url) && m_VariableType != Type.Textual);
            }
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
        internal UrlVariable(ApplicationJob.UrlVariableCollection job)
            : this(null, job)
        {
        }

        /// <summary>
        /// Creates a new variable for a given application.
        /// </summary>
        internal UrlVariable(string name, ApplicationJob.UrlVariableCollection collection)
        {
            m_Name = name;
            m_Parent = collection;
            if (collection != null && collection.Parent != null)
            {
                m_JobId = collection.Parent.Id;
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
            m_JobId = Convert.ToInt64(reader["JobId"]);
            m_VariableType = (Type)Convert.ToInt32(reader["VariableType"]);
            m_TextualContent = reader["TextualContent"] as string;
        }

        public void Save(IDbTransaction transaction, long parentJobId)
        {
            IDbConnection conn = (transaction != null) ? transaction.Connection : DbManager.NewConnection;
            using (IDbCommand command = conn.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO variables (JobId, VariableName, Url, StartText, EndText, RegularExpression, CachedContent, VariableType, TextualContent)
                                             VALUES (@JobId, @VariableName, @Url, @StartText, @EndText, @RegularExpression, @CachedContent, @VariableType, @TextualContent)";

                command.Parameters.Add(new SQLiteParameter("@JobId", parentJobId));
                command.Parameters.Add(new SQLiteParameter("@VariableName", m_Name));
                command.Parameters.Add(new SQLiteParameter("@Url", m_Url));
                command.Parameters.Add(new SQLiteParameter("@StartText", m_StartText));
                command.Parameters.Add(new SQLiteParameter("@EndText", m_EndText));
                command.Parameters.Add(new SQLiteParameter("@RegularExpression", m_Regex));
                command.Parameters.Add(new SQLiteParameter("@CachedContent", m_CachedContent));
                command.Parameters.Add(new SQLiteParameter("@VariableType", m_VariableType));
                command.Parameters.Add(new SQLiteParameter("@TextualContent", m_TextualContent));
                command.ExecuteNonQuery();
                m_JobId = parentJobId;
            }
        }

        /// <summary>
        /// Ensures that the global variables are read from
        /// the database when they are accessed for the next time.
        /// </summary>
        public static void ReloadGlobalVariables()
        {
            m_GlobalVariables = null;
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

        /// <summary>
        /// Quotes a string for insertion into a regular expression.
        /// Based on preg_quote() (PHP)
        /// </summary>
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
                case "split":
                    if (parts.Length >= 3)
                    {
                        string[] contentParts = content.Split(parts[1][0]);
                        int partNum;
                        if (Int32.TryParse(parts[2], out partNum) && partNum >= 0 && partNum < contentParts.Length)
                        {
                            return contentParts[partNum];
                        }
                    }
                    break;
                case "trim":
                    if (parts.Length >= 2)
                    {
                        return content.Trim(parts[1].ToCharArray());
                    }
                    else
                    {
                        return content.Trim();
                    }
                case "trimend":
                    if (parts.Length >= 2)
                    {
                        return content.TrimEnd(parts[1].ToCharArray());
                    }
                    else
                    {
                        return content.TrimEnd();
                    }
                case "trimstart":
                    if (parts.Length >= 2)
                    {
                        return content.TrimStart(parts[1].ToCharArray());
                    }
                    else
                    {
                        return content.TrimStart();
                    }

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

        /// <summary>
        /// Replaces this variable within a given string.
        /// </summary>
        public virtual string ReplaceInString(string value)
        {
            return ReplaceInString(value, false);
        }

        /// <summary>
        /// Replaces this variable within a given string.
        /// </summary>
        /// <param name="onlyCached">If true, no new content will be downloaded and only chached content will be used.</param>
        public virtual string ReplaceInString(string value, bool onlyCached)
        {
            if (!IsVariableUsedInString(m_Name, value)) return value;

            // Global variable only has static content
            if (onlyCached)
            {
                // We don't want to spam the log with chached only screen updates
                if (!onlyCached) LogDialog.Log(this, value, m_CachedContent);
                return string.IsNullOrEmpty(m_CachedContent) ? value : Replace(value, m_CachedContent);
            }

            // Ignore missing URLs etc.
            if (IsEmpty) return value;

            // Using textual content?
            if (m_VariableType == Type.Textual)
            {
                m_CachedContent = ExpandedTextualContent;
                LogDialog.Log(this, value, m_CachedContent);
                return Replace(value, ExpandedTextualContent);
            }

            string page = string.Empty;
            // Get the content we need to put in
            using (WebClient client = new WebClient())
            {
                page = client.DownloadString(ExpandedUrl);
                m_DownloadCount++;
            }

            // Normalise line-breaks
            page = page.Replace("\r\n", "\n");
            page = page.Replace("\r", "\n");

            // Using a regular expression?
            if (m_VariableType == Type.RegularExpression)
            {
                if (string.IsNullOrEmpty(m_Regex)) return value;

                Regex regex = new Regex(m_Regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                Match match = regex.Match(page);
                if (match.Success)
                {
                    if (match.Groups.Count == 1)
                    {
                        m_CachedContent = match.Value;
                        LogDialog.Log(this, value, m_CachedContent);
                        return Replace(value, match.Value);
                    }
                    else if (match.Groups.Count == 2)
                    {
                        m_CachedContent = match.Groups[1].Value;
                        LogDialog.Log(this, value, m_CachedContent);
                        return Replace(value, match.Groups[1].Value);
                    }
                }
            }

            // Use whole page if either start or end is missing
            if (string.IsNullOrEmpty(m_StartText) || string.IsNullOrEmpty(m_EndText))
            {
                m_CachedContent = page;
                LogDialog.Log(this, value, m_CachedContent);
                return Replace(value, page);
            }

            int startPos = page.IndexOf(m_StartText);
            if (startPos < 0) return value;

            int endOfStart = startPos + m_StartText.Length;

            int endPos = page.IndexOf(m_EndText, endOfStart);
            if (endPos < 0) return value;

            string result = page.Substring(endOfStart, endPos - endOfStart);

            m_CachedContent = result;
            LogDialog.Log(this, value, m_CachedContent);
            value = Replace(value, result);

            return value;
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