using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Ketarin.Forms;
using CDBurnerXP;
using System.Data.SQLite;
using System.Web;

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
        #region GlobalUrlVariableCollection

        /// <summary>
        /// Represents a collection of all global variables.
        /// </summary>
        public class GlobalUrlVariableCollection : ApplicationJob.UrlVariableCollection
        {
            public override string ReplaceAllInString(string value, DateTime fileDate, string filename, bool onlyCachedContent, bool skipGlobalVariables)
            {
                // Replace until no further replacements have been made.
                string valueAfterReplacement = value;
                string valueBeforeReplacement;

                do
                {
                    valueBeforeReplacement = valueAfterReplacement;
                    valueAfterReplacement = base.ReplaceAllInString(valueAfterReplacement, fileDate, filename, onlyCachedContent, true);
                }
                while (valueBeforeReplacement != valueAfterReplacement);

                return valueAfterReplacement;
            }

            public override string ReplaceAllInString(string value)
            {
                return this.ReplaceAllInString(value, DateTime.MinValue, null, false, false);
            }

            /// <summary>
            /// Saves all global variables to the database.
            /// </summary>
            public void Save()
            {
                using (IDbTransaction transaction = DbManager.Connection.BeginTransaction())
                {
                    using (IDbCommand comm = DbManager.Connection.CreateCommand())
                    {
                        comm.Transaction = transaction;
                        comm.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                        comm.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(Guid.Empty)));
                        comm.ExecuteNonQuery();
                    }

                    foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
                    {
                        var.Save(transaction, Guid.Empty);
                    }

                    transaction.Commit();
                }
            }
        }

        #endregion

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
        private string m_CachedContent = null;
        private ApplicationJob.UrlVariableCollection m_Parent = null;
        private Guid m_JobGuid = Guid.Empty;
        private int m_DownloadCount = 0;
        private bool m_RegexRightToLeft = false;
        private string m_PostData;
        private static GlobalUrlVariableCollection m_GlobalVariables = null;
        /// <summary>
        /// Prevent recursion with the ExpandedUrl property.
        /// </summary>
        private bool m_Expanding = false;
        private static Random random = new Random();

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
        /// Gets or sets whether the regex has the 
        /// right-to-left match option.
        /// </summary>
        public bool RegexRightToLeft
        {
            get { return m_RegexRightToLeft; }
            set { m_RegexRightToLeft = value; }
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
        public static GlobalUrlVariableCollection GlobalVariables
        {
            get
            {
                if (m_GlobalVariables == null)
                {
                    m_GlobalVariables = new GlobalUrlVariableCollection();

                    using (SQLiteConnection conn = DbManager.NewConnection)
                    {
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.CommandText = @"SELECT * FROM variables WHERE JobGuid IS NULL OR JobGuid = @JobGuid";
                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(Guid.Empty)));
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
            set {
                m_Regex = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the POST data which is
        /// submitted along with the URL request.
        /// </summary>
        public string PostData
        {
            get { return m_PostData; }
            set { m_PostData = value; }
        }

        /// <summary>
        /// Returns the regular expression for this
        /// variable, with all variables replaced.
        /// </summary>
        [XmlIgnore()]
        public string ExpandedRegex
        {
            get
            {
                if (m_Parent == null || m_Expanding || string.IsNullOrEmpty(m_Regex))
                {
                    return m_Regex;
                }

                m_Expanding = true;
                try
                {
                    return m_Parent.ReplaceAllInString(m_Regex);
                }
                finally
                {
                    m_Expanding = false;
                }
            }
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
                m_JobGuid = collection.Parent.Guid;
            }
        }

        public void Hydrate(IDataReader reader)
        {
            m_Name = reader["VariableName"] as string;
            m_StartText = reader["StartText"] as string;
            m_EndText = reader["EndText"] as string;
            m_Url = reader["Url"] as string;
            Regex = reader["RegularExpression"] as string;
            m_CachedContent = reader["CachedContent"] as string;
            m_JobGuid = new Guid(reader["JobGuid"] as string);
            m_VariableType = (Type)Convert.ToInt32(reader["VariableType"]);
            m_TextualContent = reader["TextualContent"] as string;
            m_RegexRightToLeft = Conversion.ToBoolean(reader["RegexRightToLeft"]);
            m_PostData = reader["PostData"] as string;
        }

        public void Save(IDbTransaction transaction, Guid parentJobGuid)
        {
            IDbConnection conn = (transaction != null) ? transaction.Connection : DbManager.NewConnection;
            using (IDbCommand command = conn.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO variables (JobGuid, VariableName, Url, StartText, EndText, RegularExpression, CachedContent, VariableType, TextualContent, RegexRightToLeft, PostData)
                                             VALUES (@JobGuid, @VariableName, @Url, @StartText, @EndText, @RegularExpression, @CachedContent, @VariableType, @TextualContent, @RegexRightToLeft, @PostData)";

                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(parentJobGuid)));
                command.Parameters.Add(new SQLiteParameter("@VariableName", m_Name));
                command.Parameters.Add(new SQLiteParameter("@Url", m_Url));
                command.Parameters.Add(new SQLiteParameter("@StartText", m_StartText));
                command.Parameters.Add(new SQLiteParameter("@EndText", m_EndText));
                command.Parameters.Add(new SQLiteParameter("@RegularExpression", m_Regex));
                command.Parameters.Add(new SQLiteParameter("@RegexRightToLeft", m_RegexRightToLeft));
                command.Parameters.Add(new SQLiteParameter("@CachedContent", m_CachedContent));
                command.Parameters.Add(new SQLiteParameter("@VariableType", m_VariableType));
                command.Parameters.Add(new SQLiteParameter("@TextualContent", m_TextualContent));
                command.Parameters.Add(new SQLiteParameter("@PostData", m_PostData));
                
                command.ExecuteNonQuery();
                m_JobGuid = parentJobGuid;
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
        /// Find the position and length of a variable usage
        /// within a string.
        /// </summary>
        /// <param name="input">String to search for the variable</param>
        /// <param name="varname">Name of the variable to search</param>
        /// <param name="position">Position of the found variable</param>
        /// <param name="length">Length of the variable string</param>
        /// <returns>true, if the variable has been found, false otherwise</returns>
        private static bool GetVariablePosition(string input, string varname, int startAt, out int position, out int length, out string functionPart)
        {
            functionPart = "";
            position = -1;
            length = 0;

            for (int i = startAt; i < input.Length; i++)
            {
                bool fitsRemainingString = (input.Length - i >= varname.Length + 2);
                if (input[i] == '{' && !IsEscaped(input, i) && fitsRemainingString)
                {
                    // Start of variable detected. Is it the right variable?
                    string upcomingString = input.Substring(i + 1, varname.Length + 1);
                    if (upcomingString == varname + ":" || upcomingString == varname + "}")
                    {
                        position = i;
                        // Find end (consider functions)
                        int startPos = (input[i + varname.Length + 1] == ':') ? i + varname.Length + 2 : i + varname.Length + 1;

                        functionPart = "";
                        for (int j = startPos; j < input.Length; j++)
                        {
                            if (IsEscaped(input, j))
                            {
                                functionPart = functionPart.Substring(0, functionPart.Length - 1) + input[j];
                            }
                            else if (input[j] == '}')
                            {
                                length = j - i + 1;
                                return true;
                            }
                            else
                            {
                                functionPart += input[j];
                            }
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether or not a variable is used within a string.
        /// It also matches if functions like {variable:replace:a:b} are used.
        /// </summary>
        /// <param name="name">Name of the variable without { and }</param>
        /// <param name="formatString">String to check</param>
        public static bool IsVariableUsedInString(string name, string formatString)
        {
            int pos, length;
            string functionPart;
            return GetVariablePosition(formatString, name, 0, out pos, out length, out functionPart);
        }

        /// <summary>
        /// Replaces this variable within a string with the given content.
        /// Applies functions if necessary.
        /// </summary>
        private string Replace(string formatString, string content, ApplicationJob context = null)
        {
            return Replace(formatString, m_Name, content, context);
        }

        /// <summary>
        /// Replaces a variable with the given name within a string
        /// with the given content.
        /// Applies functions if necessary.
        /// </summary>
        public static string Replace(string formatString, string varname, string content, ApplicationJob context = null)
        {
            if (content == null)
            {
                content = string.Empty;
            }

            if (formatString == null)
            {
                return string.Empty;
            }

            int pos, length;
            string functionPart;
            int startAt = 0;

            // We need to "rematch" multiple times if the string changes
            while (GetVariablePosition(formatString, varname, startAt, out pos, out length, out functionPart))
            {
                formatString = formatString.Remove(pos, length);
                string replaceValue = ReplaceFunction(functionPart, content, context);
                startAt = pos + replaceValue.Length;
                formatString = formatString.Insert(pos, replaceValue);
            } 

            return formatString;
        }

        /// <summary>
        /// Applies a function (if given) to content and returns the
        /// modified content.
        /// </summary>
        /// <param name="function">A function specification, for example "replace:a:b"</param>
        /// <param name="content">The usual variable content</param>
        private static string ReplaceFunction(string function, string content, ApplicationJob context = null)
        {
            function = function.TrimStart(':');
            if (string.IsNullOrEmpty(function)) return content;

            string[] parts = SplitEscaped(function, ':');
            if (parts.Length == 0) return content;

            switch (parts[0])
            {
                case "empty":
                    // Useful, if you want to load, but not use a variable
                    return string.Empty;

                case "ifempty":
                    if (string.IsNullOrEmpty(content) && context != null && parts.Length > 1)
                    {
                        return context.Variables.ReplaceAllInString("{" + parts[1] + "}");
                    }

                    return content;

                case "regexreplace":
                    try
                    {
                        if (parts.Length > 2)
                        {
                            Regex regex = new Regex(parts[1], RegexOptions.Singleline | RegexOptions.IgnoreCase);
                            return regex.Replace(content, delegate(Match match) {
                                string result = parts[2];
                                for (int i = 0; i < match.Groups.Count; i++)
                                {
                                    result = result.Replace("$" + i, match.Groups[i].Value);
                                }
                                return result;                                
                            });
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        LogDialog.Log("Could not process the function 'regexreplace'.", ex);
                    }
                    return string.Empty;

                case "multireplace":
                case "multireplacei":
                    if (parts.Length > 3)
                    {
                        if (string.IsNullOrEmpty(parts[1])) break;
                        
                        // Exmaple: multireplace:,:a,b,c:1,2,3
                        char delimiter = parts[1][0];
                        
                        string[] search = parts[2].Split(delimiter);
                        string[] replace = parts[3].Split(delimiter);
                        for (int i = 0; i < search.Length; i++)
                        {
                            string replaceValue = (replace.Length > i) ? replace[i] : string.Empty;
                            if (parts[0] == "multireplacei")
                            {
                                content = ReplaceEx(content, search[i], replaceValue);
                            }
                            else
                            {
                                content = content.Replace(search[i], replaceValue);
                            }
                        }

                        return content;
                    }
                    break;

                case "regex":
                    try
                    {
                        Regex regex = new Regex(parts[1], RegexOptions.Singleline);
                        Match match = regex.Match(content);
                        if (parts.Length > 2)
                        {
                            int groupNum = Conversion.ToInt(parts[2]);
                            if (groupNum >= 0 && groupNum < match.Groups.Count)
                            {
                                return match.Groups[groupNum].Value;
                            }
                        }
                        return (match.Success) ? match.Value : string.Empty;
                    }
                    catch (ArgumentException ex)
                    {
                        LogDialog.Log("Could not process the function 'regex'.", ex);
                        return string.Empty;
                    }

                case "regexrandom":
                    try
                    {
                        Regex regex = new Regex(parts[1], RegexOptions.Singleline);
                        MatchCollection matches = regex.Matches(content);
                        if (matches.Count > 0)
                        {
                            int randomPos = random.Next(0, matches.Count - 1);
                            int groupNum = (parts.Length > 2) ? Conversion.ToInt(parts[2]) : -1;

                            if (groupNum >= 0 && groupNum < matches[randomPos].Groups.Count)
                            {
                                return matches[randomPos].Groups[groupNum].Value;
                            }
                            else
                            {
                                return matches[randomPos].Value;
                            }
                        }
                        return string.Empty;
                    }
                    catch (ArgumentException ex)
                    {
                        LogDialog.Log("Could not process the function 'regex'.", ex);
                        return string.Empty;
                    }
                case "ext":
                    return Path.GetExtension(content).TrimStart('.');
                case "basefile":
                    return Path.GetFileNameWithoutExtension(content);
                case "directory":
                    try
                    {
                        if (content.StartsWith("\"") && content.EndsWith("\""))
                        {
                            return "\"" + Path.GetDirectoryName(content.Trim('"')) + "\"";
                        }
                        else
                        {
                            return Path.GetDirectoryName(content.Trim('"'));
                        }
                    }
                    catch
                    {
                        return content;
                    }
                case "filename":
                    try
                    {
                        return Path.GetFileName(content);
                    }
                    catch
                    {
                        return content;
                    }
                case "filenameWithoutExtension":
                    try
                    {
                        return Path.GetFileNameWithoutExtension(content);
                    }
                    catch
                    {
                        return content;
                    }
                case "toupper":
                    return content.ToUpper();
                case "tolower":
                    return content.ToLower();
                case "split":
                    if (parts.Length >= 3)
                    {
                        string[] contentParts = content.Split(parts[1][0]);
                        int partNum;
                        if (Int32.TryParse(parts[2], out partNum))
                        {
                            if (partNum < 0)
                            {
                                // Negative number: Count from the end
                                partNum = contentParts.Length + partNum;
                            }
                            if (partNum >= 0 && partNum < contentParts.Length)
                            {
                                return contentParts[partNum];
                            }
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

                case "formatfilesize":
                    return CDBurnerXP.IO.FormatFileSize.Format(Conversion.ToInt(content));

                case "startuppath":
                    return System.Windows.Forms.Application.StartupPath;

                case "urldecode":
                    return HttpUtility.UrlDecode(content);

                case "urlencode":
                    return HttpUtility.UrlEncode(content);
            }

            return content;
        }

        /// <summary>
        /// Case insensitive replacement.
        /// http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
        /// </summary>
        private static string ReplaceEx(string original, string pattern, string replacement) 
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        /// <summary>
        /// Splits a string at every occurence of 'splitter', but
        /// only if this character has not been escaped with a \.
        /// </summary>
        private static string[] SplitEscaped(string value, char splitter)
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
        private static bool IsEscaped(string value, int pos)
        {
            // It has to be a special character...
            if (value[pos] != '\\' && value[pos] != ':' && value[pos] != '}') return false;
            return (pos > 0 && value[pos - 1] == '\\' && !IsEscaped(value, pos - 1));
        }

        /// <summary>
        /// Replaces this variable within a given string.
        /// </summary>
        public virtual string ReplaceInString(string value)
        {
            return ReplaceInString(value, DateTime.MinValue, false);
        }

        /// <summary>
        /// For type 'Textual', this text is to be
        /// used as replacement for the variable.
        /// </summary>
        public string GetExpandedTextualContent(DateTime fileDate)
        {
            if (m_Parent == null || m_Expanding || string.IsNullOrEmpty(m_TextualContent))
            {
                return m_TextualContent;
            }

            m_Expanding = true;
            try
            {
                return m_Parent.ReplaceAllInString(this.m_TextualContent, fileDate, string.Empty, false);
            }
            finally
            {
                m_Expanding = false;
            }
        }

        /// <summary>
        /// Creates the Regex used in this variable.
        /// </summary>
        /// <returns></returns>
        public Regex CreateRegex()
        {
            if (m_VariableType != Type.RegularExpression || string.IsNullOrEmpty(m_Regex))
            {
                return null;
            }

            try
            {
                RegexOptions options = RegexOptions.Singleline | RegexOptions.IgnoreCase;
                if (m_RegexRightToLeft)
                {
                    options |= RegexOptions.RightToLeft;
                }
                return new Regex(ExpandedRegex, options);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Replaces this variable within a given string.
        /// </summary>
        /// <param name="onlyCached">If true, no new content will be downloaded and only chached content will be used.</param>
        /// <param name="fileDate">Current file date, when downloading the modification date of the file being downloaded</param>
        public virtual string ReplaceInString(string value, DateTime fileDate, bool onlyCached)
        {
            if (!IsVariableUsedInString(m_Name, value)) return value;

            // Global variable only has static content
            if (onlyCached)
            {
                return (m_CachedContent == null) ? value : Replace(value, m_CachedContent, this.Parent == null ? null : this.Parent.Parent);
            }

            // Ignore missing URLs etc.
            if (IsEmpty) return value;

            // Using textual content?
            if (m_VariableType == Type.Textual)
            {
                m_CachedContent = GetExpandedTextualContent(fileDate);
                LogDialog.Log(this, value, m_CachedContent);
                return Replace(value, m_CachedContent, this.Parent == null ? null : this.Parent.Parent);
            }

            string page = string.Empty;
            // Get the content we need to put in
            string userAgent = (this.Parent == null) ? null : this.Parent.Parent.Variables.ReplaceAllInString(this.Parent.Parent.UserAgent);
            using (WebClient client = new WebClient(userAgent))
            {
                try
                {
                    string url = ExpandedUrl;
                    client.SetPostData(this);
                    page = client.DownloadString(url);
                }
                catch (ArgumentException)
                {
                    throw new UriFormatException("The URL '" + Url + "' of variable '" + m_Name + "' is not valid.");
                }
                m_DownloadCount++;
            }

            // Normalise line-breaks
            page = page.Replace("\r\n", "\n");
            page = page.Replace("\r", "\n");

            // Using a regular expression?
            if (m_VariableType == Type.RegularExpression)
            {
                Regex regex = CreateRegex();
                if (regex == null) return value;

                Match match = regex.Match(page);
                if (match.Success)
                {
                    if (match.Groups.Count == 1)
                    {
                        m_CachedContent = match.Value;
                        LogDialog.Log(this, value, m_CachedContent);
                        return Replace(value, match.Value);
                    }
                    else if (match.Groups.Count >= 2)
                    {
                        // Use first group (except index 0, which is complete match) with a match
                        for (int i = 1; i < match.Groups.Count; i++)
                        {
                            if (match.Groups[i].Success)
                            {
                                m_CachedContent = match.Groups[i].Value;
                                LogDialog.Log(this, value, m_CachedContent);
                                return Replace(value, match.Groups[i].Value);
                            }
                        }

                        // No group matches, use complete match
                        m_CachedContent = match.Groups[0].Value;
                        LogDialog.Log(this, value, m_CachedContent);
                        return Replace(value, match.Groups[0].Value);
                    }
                }
                else
                {
                    // No regex match should yield an empty result
                    return Replace(value, string.Empty);
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