using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using CDBurnerXP;
using System.Net;

namespace Ketarin
{
    /// <summary>
    /// This class contains a collection of functions
    /// for reading from the database.
    /// </summary>
    class DbManager
    {
        #region SettingsProvider

        /// <summary>
        /// Saves all of Ketarin's settings to the database to be more self contained.
        /// </summary>
        public class SettingsProvider : ISettingsProvider
        {
            #region ISettingsProvider Member

            private string GetPath(string[] path)
            {
                return String.Join("/", path).Trim('/');
            }

            public object GetValue(params string[] path)
            {
                lock (Connection)
                {
                    using (IDbCommand command = Connection.CreateCommand())
                    {
                        command.CommandText = "SELECT SettingValue FROM settings WHERE SettingPath = @SettingPath";
                        command.Parameters.Add(new SQLiteParameter("@SettingPath", GetPath(path)));
                        return command.ExecuteScalar();
                    }
                }
            }

            public void SetValue(string value, params string[] path)
            {
                lock (Connection)
                {
                    using (IDbCommand command = Connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE settings SET SettingValue = @SettingValue WHERE SettingPath = @SettingPath";
                        command.Parameters.Add(new SQLiteParameter("@SettingValue", value));
                        command.Parameters.Add(new SQLiteParameter("@SettingPath", GetPath(path)));
                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.CommandText = @"INSERT INTO settings (SettingPath, SettingValue)
                                                 VALUES (@SettingPath, @SettingValue)";
                            command.Parameters.Add(new SQLiteParameter("@SettingValue", value));
                            command.Parameters.Add(new SQLiteParameter("@SettingPath", GetPath(path)));
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            #endregion
        }

        #endregion

        private static SQLiteConnection m_DbConn;
        private static string m_DatabasePath;

        /// <summary>
        /// Sets a predefined database path if necessary.
        /// </summary>
        public static string DatabasePath
        {
            set { DbManager.m_DatabasePath = value; }
        }

        /// <summary>
        /// Builds a new proxy object from the settings.
        /// Returns null if no valid proxy exists.
        /// </summary>
        public static WebProxy Proxy
        {
            get
            {
                string server = Settings.GetValue("ProxyServer") as string;
                Int16 port = Convert.ToInt16(Settings.GetValue("ProxyPort", 0));

                if (string.IsNullOrEmpty(server) || port <= 0) return null;

                string username = Settings.GetValue("ProxyUser") as string;
                string password = Settings.GetValue("ProxyPassword") as string;

                string address = "http://" + server + ":" + port;

                try
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        return new WebProxy(address, true);
                    }
                    else
                    {
                        NetworkCredential credentials = new NetworkCredential(username, password);

                        return new WebProxy(address, true, null, credentials);
                    }
                }
                catch (UriFormatException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns an application wide database connection.
        /// </summary>
        public static SQLiteConnection Connection
        {
            get
            {
                if (m_DbConn == null)
                {
                    m_DbConn = NewConnection;
                    Settings.Provider = new DbManager.SettingsProvider();
                }
                return m_DbConn;
            }
        }

        /// <summary>
        /// For all database operations that might be executed at the same
        /// time within different threads (for example, .Save() for multiple
        /// application jobs), we need a "unique" connection.
        /// </summary>
        public static SQLiteConnection NewConnection
        {
            get
            {
                if (m_DatabasePath == null)
                {
                    // Only determine the path once
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ketarin\\jobs.db";
                    // Is a special path set in the registry?
                    string regPath = CDBurnerXP.Settings.GetValue("Ketarin", "DatabasePath", "") as string;
                    if (!string.IsNullOrEmpty(regPath) && File.Exists(regPath))
                    {
                        path = regPath;
                    }
                    // Or is there a database file in the startup directory?
                    string localPath = Path.Combine(Application.StartupPath, "jobs.db");
                    if (File.Exists(localPath))
                    {
                        path = localPath;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    m_DatabasePath = path;
                }

                SQLiteConnection connection;
                string connString = string.Format("Data Source={0};Version=3;", m_DatabasePath);
                if (!File.Exists(m_DatabasePath))
                {
                    connString += "New=True;";
                }
                connection = new SQLiteConnection(connString);
                connection.Open();
                return connection;
            }
        }

        /// <summary>
        /// Creates or upgrades the database if necessary.
        /// </summary>
        public static void CreateOrUpgradeDatabase()
        {
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS jobs  
                                        (JobId              INTEGER PRIMARY KEY,
                                         ApplicationName    TEXT,
                                         TargetPath         TEXT,
                                         FixedDownloadUrl   TEXT,
                                         FileHippoId        TEXT,
                                         FileHippoVersion   TEXT,
                                         DateAdded          DATE,
                                         LastUpdated        DATE,
                                         DownloadDate       DATE,
                                         PreviousLocation   TEXT,
                                         ExecuteCommand     TEXT,
                                         HttpReferer        TEXT,
                                         VariableChangeIndicator TEXT,
                                         VariableChangeIndicatorLastContent TEXT,
                                         JobGuid            TEXT UNIQUE,
                                         DeletePreviousFile INTEGER DEFAULT 0,
                                         DownloadBeta       INTEGER DEFAULT 0,
                                         CanBeShared        INTEGER DEFAULT 1,
                                         ShareApplication   INTEGER DEFAULT 0,
                                         SourceType         INTEGER DEFAULT 0,
                                         IsEnabled          INTEGER);";
                command.ExecuteNonQuery();
            }

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS variables  
                                        (JobId              INTEGER,
                                         VariableName       TEXT,
                                         VariableType       INTEGER DEFAULT 0,
                                         Url                TEXT,
                                         StartText          TEXT,
                                         EndText            TEXT,
                                         RegularExpression  TEXT,
                                         TextualContent     TEXT,
                                         CachedContent      TEXT);";
                command.ExecuteNonQuery();
            }

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS settings  
                                        (SettingPath        TEXT UNIQUE,
                                         SettingValue       TEXT);";
                command.ExecuteNonQuery();
            }

            // Upgrade tables
            List<string> columns = GetColumns("jobs");

            Dictionary<string, string> addColumns = new Dictionary<string, string>();
            addColumns.Add("ExecuteCommand", "ALTER TABLE jobs ADD ExecuteCommand TEXT");
            addColumns.Add("ExecutePreCommand", "ALTER TABLE jobs ADD ExecutePreCommand TEXT");
            addColumns.Add("Category", "ALTER TABLE jobs ADD Category TEXT");
            addColumns.Add("JobGuid", "ALTER TABLE jobs ADD JobGuid TEXT");
            addColumns.Add("DownloadDate", "ALTER TABLE jobs ADD DownloadDate Date");
            addColumns.Add("CanBeShared", "ALTER TABLE jobs ADD CanBeShared INTEGER DEFAULT 1");
            addColumns.Add("ShareApplication", "ALTER TABLE jobs ADD ShareApplication INTEGER DEFAULT 0");
            addColumns.Add("HttpReferer", "ALTER TABLE jobs ADD HttpReferer TEXT");
            addColumns.Add("FileHippoVersion", "ALTER TABLE jobs ADD FileHippoVersion TEXT");
            addColumns.Add("DownloadBeta", "ALTER TABLE jobs ADD DownloadBeta INTEGER DEFAULT 0");
            addColumns.Add("VariableChangeIndicator", "ALTER TABLE jobs ADD VariableChangeIndicator TEXT");
            addColumns.Add("VariableChangeIndicatorLastContent", "ALTER TABLE jobs ADD VariableChangeIndicatorLastContent TEXT");

            ExecuteUpgradeQueries(columns, addColumns);

            columns = GetColumns("variables");
            addColumns = new Dictionary<string, string>();
            addColumns.Add("RegularExpression", "ALTER TABLE variables ADD RegularExpression TEXT");
            addColumns.Add("CachedContent", "ALTER TABLE variables ADD CachedContent TEXT");
            addColumns.Add("VariableType", "ALTER TABLE variables ADD VariableType INTEGER DEFAULT 0");
            addColumns.Add("TextualContent", "ALTER TABLE variables ADD TextualContent TEXT");

            ExecuteUpgradeQueries(columns, addColumns);

            // Compatibility: Set all regular expression variables to the new type
            if (!columns.Contains("VariableType"))
            {
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE variables SET VariableType = 1 WHERE RegularExpression IS NOT NULL AND RegularExpression != ''";
                    command.ExecuteNonQuery();
                }
            }

            // Upgrade to GUIDs
            if (!columns.Contains("JobGuid"))
            {
                // Add new columns, cleanup of existing GUIDs
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = @"ALTER TABLE variables ADD JobGuid TEXT; UPDATE jobs SET JobGuid = upper(JobGuid);";
                    command.ExecuteNonQuery();
                }
                
                // Make sure that all applications have a GUID
                ApplicationJob[] jobs = GetJobs();
                foreach (ApplicationJob job in jobs)
                {
                    job.Save();
                }

                // Upgrade variables
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE variables SET JobGuid = (SELECT JobGuid FROM jobs WHERE jobs.JobId = variables.JobId);
                                            UPDATE variables SET JobGuid = @JobGuid WHERE JobId = 0;
                                            DELETE FROM variables WHERE JobGuid IS NULL";
                    command.Parameters.Add(new SQLiteParameter("@JobGuid", FormatGuid(Guid.Empty)));
                    command.ExecuteNonQuery();
                }

                // Delete variables.JobId
                DeleteColumnFromTable("variables", "JobId");
                DeleteColumnFromTable("jobs", "JobId");

                // Create important indices
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = @"CREATE INDEX IF NOT EXISTS VarJobGuid ON variables (JobGuid);
                                            CREATE UNIQUE INDEX IF NOT EXISTS JobGuid ON jobs (JobGuid);";
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes the specified column from the given table.
        /// </summary>
        private static void DeleteColumnFromTable(string table, string colToDelete)
        {
            List<string> columns = new List<string>();
            List<string> types = new List<string>();

            // Read all columns and types from the table
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(" + table + ")";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(1));
                        types.Add(reader.GetString(2));
                    }
                }
            }

            // Column does not exist, no action necessary
            if (!columns.Contains(colToDelete)) return;

            string cols = String.Join(",", columns.ToArray());
            int index = columns.IndexOf(colToDelete);
            columns.RemoveAt(index);
            types.RemoveAt(index);
            string colsWithoutDeleted = String.Join(",", columns.ToArray());
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i] == colToDelete) continue;

                sb.Append(columns[i] + " " + types[i] + ",");
            }
            string colsWithTypes = sb.ToString().TrimEnd(',');

            // Actual deletion
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = string.Format(@"BEGIN TRANSACTION;
                    CREATE TEMPORARY TABLE {0}_backup({1});
                    INSERT INTO {0}_backup SELECT {1} FROM {0};
                    DROP TABLE {0};
                    CREATE TABLE {0}({3});
                    INSERT INTO {0} SELECT {2} FROM {0}_backup;
                    DROP TABLE {0}_backup;
                    COMMIT;", table, cols, colsWithoutDeleted, colsWithTypes);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds all columns (keys of addColumns) using the query (values of addColumns)
        /// which are not contained in the parameter 'columns'.
        /// </summary>
        /// <param name="columns">Existing columns</param>
        /// <param name="addColumns">Columns to add (with queries)</param>
        private static void ExecuteUpgradeQueries(List<string> columns, Dictionary<string, string> addColumns)
        {
            foreach (KeyValuePair<string, string> column in addColumns)
            {
                if (!columns.Contains(column.Key))
                {
                    using (IDbCommand command = Connection.CreateCommand())
                    {
                        command.CommandText = column.Value;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static List<string> GetColumns(string table)
        {
            List<string> columns = new List<string>();
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(" + table +")";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(1));
                    }
                }
            }
            return columns;
        }

        /// <summary>
        /// Formats a GUID as string the way it should be stored within the database.
        /// </summary>
        public static string FormatGuid(Guid guid)
        {
            return guid.ToString("D").ToUpper();
        }

        /// <summary>
        /// Returns a sorted list of all existing application jobs.
        /// </summary>
        public static ApplicationJob[] GetJobs()
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM jobs ORDER BY ApplicationName";

            List<ApplicationJob> result = new List<ApplicationJob>();

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ApplicationJob job = new ApplicationJob();
                    job.Hydrate(reader);
                    result.Add(job);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns an application from the database which has the specified GUID.
        /// </summary>
        public static ApplicationJob GetJob(Guid appGuid)
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM jobs WHERE JobGuid = @JobGuid";
            command.Parameters.Add(new SQLiteParameter("@JobGuid", FormatGuid(appGuid)));

            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    ApplicationJob job = new ApplicationJob();
                    job.Hydrate(reader);
                    return job;
                }
            }

            return null;
        }


        /// <summary>
        /// Determines whether or not an application with the given GUID exists.
        /// </summary>
        public static bool ApplicationExists(Guid appGuid)
        {
            return ApplicationExists(Connection, appGuid);
        }

        /// <summary>
        /// Determines whether or not an application with the given GUID exists.
        /// </summary>
        public static bool ApplicationExists(IDbConnection conn, Guid appGuid)
        {
            if (appGuid == null || appGuid == Guid.Empty) return false;

            using (IDbCommand command = conn.CreateCommand())
            {
                command.CommandText = "SELECT JobGuid FROM jobs WHERE JobGuid = @JobGuid";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", FormatGuid(appGuid)));
                return (command.ExecuteScalar() != null);
            }
        }

        /// <summary>
        /// Returns a sorted list of all used URLs for variables.
        /// </summary>
        public static string[] GetVariableUrls()
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT Url FROM variables WHERE Url <> '' AND Url IS NOT NULL ORDER BY Url";

            List<string> urls = new List<string>();

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string url = reader["Url"] as string;
                    if (!string.IsNullOrEmpty(url))
                    {
                        urls.Add(url);
                    }
                }
            }

            return urls.ToArray();
        }

        /// <summary>
        /// Returns a sorted list of all used category names.
        /// </summary>
        public static string[] GetCategories()
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT Category FROM jobs ORDER BY Category";

            List<string> categories = new List<string>();

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string cat = reader["Category"] as string;
                    if (!string.IsNullOrEmpty(cat))
                    {
                        categories.Add(cat);
                    }
                }
            }

            return categories.ToArray();
        }

        /// <summary>
        /// Returns a list of the 10 most used variable names,
        /// ordered by usage count.
        /// </summary>
        public static string[] GetMostUsedVariableNames()
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT VariableName, COUNT(*) AS cnt FROM variables GROUP BY VariableName ORDER BY cnt DESC LIMIT 10";

            List<string> names = new List<string>();

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader["VariableName"] as string;
                    if (!string.IsNullOrEmpty(name))
                    {
                        names.Add(name);
                    }
                }
            }

            return names.ToArray();
        }

    }
}
