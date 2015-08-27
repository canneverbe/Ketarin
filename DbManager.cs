using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using CDBurnerXP;
using CDBurnerXP.IO;
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

            public void SetValueRaw(string value, string path, IDbTransaction transaction)
            {
                lock (Connection)
                {
                    using (IDbCommand command = Connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "UPDATE settings SET SettingValue = @SettingValue WHERE SettingPath = @SettingPath";
                        command.Parameters.Add(new SQLiteParameter("@SettingValue", value));
                        command.Parameters.Add(new SQLiteParameter("@SettingPath", path));
                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.CommandText = @"INSERT INTO settings (SettingPath, SettingValue)
                                                 VALUES (@SettingPath, @SettingValue)";
                            command.Parameters.Add(new SQLiteParameter("@SettingValue", value));
                            command.Parameters.Add(new SQLiteParameter("@SettingPath", path));
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            public void SetValue(string value, params string[] path)
            {
                SetValueRaw(value, GetPath(path), null);
            }

            #endregion
        }

        #endregion

        private static SQLiteConnection m_DbConn;
        private static string m_DatabasePath;
        private static bool m_BackupDone = false;

        /// <summary>
        /// Sets a predefined database path if necessary.
        /// </summary>
        public static string DatabasePath
        {
            set { DbManager.m_DatabasePath = (value == null) ? null : Path.GetFullPath(value); }
            get { return DbManager.m_DatabasePath; }
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

                    try
                    {
                        if (!m_DbConn.ConnectionString.Contains("New=True;"))
                        {
                            MakeBackups();
                        }
                    }
                    catch (Exception)
                    {
                        // We can ignore these kind of errors
                        Ketarin.Forms.LogDialog.Log("Creating database backup failed.");
                    }
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
        /// Makes sure that a couple of database backups 
        /// are kept automatically. SQLite is reliable, 
        /// but better safe than sorry.
        /// </summary>
        private static void MakeBackups()
        {
            // Only try to create a backup once per instance
            if (m_BackupDone || !(bool)Settings.GetValue("CreateDatabaseBackups", true)) return;
            m_BackupDone = true;

            DateTime oldestBackup = DateTime.MaxValue;
            string oldestBackupFile = null;
            int backupCount = 0;

            string fullPath = m_DatabasePath;
            // Determine the number of backups and the oldest backup
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(fullPath)))
            {
                if (file.StartsWith(fullPath) && Path.GetExtension(file) == ".bak")
                {
                    // Extract date
                    string date = Path.GetFileNameWithoutExtension(file);
                    if (date.Length < 10) continue;

                    backupCount++;
                    DateTime backupDate = File.GetLastWriteTime(file);
                    if (backupDate < oldestBackup)
                    {
                        oldestBackup = backupDate;
                        oldestBackupFile = file;
                    }
                }
            }

            // If there are more then 6 backups, delete the oldest one
            if (backupCount > 6)
            {
                PathEx.TryDeleteFiles(oldestBackupFile);
            }

            // Create a backup, 7 backups, daily
            string dateAppend = "_" + DateTime.Now.ToString("yyyy-MM-dd");
            string backupName = m_DatabasePath + dateAppend + ".bak";
            if (!File.Exists(backupName))
            {
                File.Copy(m_DatabasePath, backupName, true);
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

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS setupinstructions  
                                        (JobGuid            TEXT,
                                         Position           INTEGER DEFAULT 0,
                                         Data               TEXT);";
                command.ExecuteNonQuery();
            }
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE INDEX IF NOT EXISTS SetupinstructionsJobGuid ON setupinstructions (JobGuid)";
                command.ExecuteNonQuery();
            }

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS setuplists  
                                        (ListGuid           TEXT UNIQUE,
                                         Name               TEXT);";
                command.ExecuteNonQuery();
            }
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS setuplists_applications  
                                        (ListGuid           TEXT,
                                         JobGuid            TEXT,
                                        CONSTRAINT setuplists_applications_unique UNIQUE (ListGuid, JobGuid));";
                command.ExecuteNonQuery();
            }
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS snippets
                                        (SnippetGuid         TEXT,
                                         Name               TEXT,
                                         Type               TEXT,
                                         Text               TEXT,
                                        CONSTRAINT snippets_unique UNIQUE (SnippetGuid));";
                command.ExecuteNonQuery();
            }

            // Upgrade tables
            List<string> columns = GetColumns("jobs");

            Dictionary<string, string> addColumns = new Dictionary<string, string>();
            addColumns.Add("ExecuteCommand", "ALTER TABLE jobs ADD ExecuteCommand TEXT");
            addColumns.Add("ExecuteCommandType", "ALTER TABLE jobs ADD ExecuteCommandType TEXT");
            addColumns.Add("ExecutePreCommand", "ALTER TABLE jobs ADD ExecutePreCommand TEXT");
            addColumns.Add("ExecutePreCommandType", "ALTER TABLE jobs ADD ExecutePreCommandType TEXT");
            addColumns.Add("SourceTemplate", "ALTER TABLE jobs ADD SourceTemplate TEXT");
            addColumns.Add("Category", "ALTER TABLE jobs ADD Category TEXT");
            addColumns.Add("JobGuid", "ALTER TABLE jobs ADD JobGuid TEXT");
            addColumns.Add("DownloadDate", "ALTER TABLE jobs ADD DownloadDate Date");
            addColumns.Add("CanBeShared", "ALTER TABLE jobs ADD CanBeShared INTEGER DEFAULT 1");
            addColumns.Add("ExclusiveDownload", "ALTER TABLE jobs ADD ExclusiveDownload INTEGER DEFAULT 0");
            addColumns.Add("CheckForUpdateOnly", "ALTER TABLE jobs ADD CheckForUpdateOnly INTEGER DEFAULT 0");
            addColumns.Add("ShareApplication", "ALTER TABLE jobs ADD ShareApplication INTEGER DEFAULT 0");
            addColumns.Add("HttpReferer", "ALTER TABLE jobs ADD HttpReferer TEXT");
            addColumns.Add("FileHippoVersion", "ALTER TABLE jobs ADD FileHippoVersion TEXT");
            addColumns.Add("DownloadBeta", "ALTER TABLE jobs ADD DownloadBeta INTEGER DEFAULT 0");
            addColumns.Add("VariableChangeIndicator", "ALTER TABLE jobs ADD VariableChangeIndicator TEXT");
            addColumns.Add("VariableChangeIndicatorLastContent", "ALTER TABLE jobs ADD VariableChangeIndicatorLastContent TEXT");
            addColumns.Add("CachedPadFileVersion", "ALTER TABLE jobs ADD CachedPadFileVersion TEXT");
            addColumns.Add("LastFileSize", "ALTER TABLE jobs ADD LastFileSize INTEGER DEFAULT 0");
            addColumns.Add("LastFileDate", "ALTER TABLE jobs ADD LastFileDate Date");
            addColumns.Add("IgnoreFileInformation", "ALTER TABLE jobs ADD IgnoreFileInformation INTEGER DEFAULT 0");
            addColumns.Add("UserNotes", "ALTER TABLE jobs ADD UserNotes TEXT");
            addColumns.Add("WebsiteUrl", "ALTER TABLE jobs ADD WebsiteUrl TEXT");
            addColumns.Add("UserAgent", "ALTER TABLE jobs ADD UserAgent TEXT");
            addColumns.Add("PreviousRelativeLocation", "ALTER TABLE jobs ADD PreviousRelativeLocation TEXT");
            addColumns.Add("HashType", "ALTER TABLE jobs ADD HashType INTEGER DEFAULT 0");
            addColumns.Add("HashVariable", "ALTER TABLE jobs ADD HashVariable TEXT");

            ExecuteUpgradeQueries(columns, addColumns);

            columns = GetColumns("variables");
            addColumns = new Dictionary<string, string>();
            addColumns.Add("RegularExpression", "ALTER TABLE variables ADD RegularExpression TEXT");
            addColumns.Add("RegexRightToLeft", "ALTER TABLE variables ADD RegexRightToLeft INTEGER DEFAULT 0");
            addColumns.Add("CachedContent", "ALTER TABLE variables ADD CachedContent TEXT");
            addColumns.Add("VariableType", "ALTER TABLE variables ADD VariableType INTEGER DEFAULT 0");
            addColumns.Add("TextualContent", "ALTER TABLE variables ADD TextualContent TEXT");
            addColumns.Add("PostData", "ALTER TABLE variables ADD PostData TEXT");

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
            Dictionary<Guid, List<UrlVariable>> allVariables = new Dictionary<Guid, List<UrlVariable>>();
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM variables";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UrlVariable variable = new UrlVariable();
                        variable.Hydrate(reader);

                        Guid jobGuid = new Guid(reader["JobGuid"] as string);

                        if (!allVariables.ContainsKey(jobGuid))
                        {
                            allVariables[jobGuid] = new List<UrlVariable>();
                        }

                        allVariables[jobGuid].Add(variable);
                    }
                }
            }

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM jobs ORDER BY ApplicationName";

                List<ApplicationJob> result = new List<ApplicationJob>();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ApplicationJob job = new ApplicationJob();
                        job.Hydrate(reader);

                        if (allVariables.ContainsKey(job.Guid))
                        {
                            foreach (UrlVariable var in allVariables[job.Guid])
                            {
                                job.Variables[var.Name] = var;
                            }
                        }

                        result.Add(job);
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Returns an application from the database which has the specified GUID.
        /// </summary>
        public static ApplicationJob GetJob(Guid appGuid)
        {
            ApplicationJob job = null;

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM jobs WHERE JobGuid = @JobGuid";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", FormatGuid(appGuid)));

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        job = new ApplicationJob();
                        job.Hydrate(reader);
                    }
                }
            }

            if (job != null)
            {
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM variables WHERE JobGuid = @JobGuid";
                    command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(appGuid)));
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UrlVariable variable = new UrlVariable(job.Variables);
                            variable.Hydrate(reader);
                            job.Variables.Add(variable.Name, variable);
                        }
                    }
                }
            }

            return job;
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
        /// Returns a list of all code snippets.
        /// </summary>
        public static Snippet[] GetSnippets()
        {
            List<Snippet> snippets = new List<Snippet>();

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM snippets ORDER BY Name";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Snippet appList = new Snippet();
                        appList.Hydrate(reader);
                        snippets.Add(appList);
                    }
                }
            }

            return snippets.ToArray();
        }

        /// <summary>
        /// Returns a sorted list of all setup lists.
        /// </summary>
        public static ApplicationList[] GetSetupLists(IEnumerable<ApplicationJob> applicationsToAttach)
        {
            List<ApplicationList> lists = new List<ApplicationList>();

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM setuplists ORDER BY Name";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ApplicationList appList = new ApplicationList();
                        appList.Hydrate(reader);
                        lists.Add(appList);
                    }
                }
            }

            // Attach applications to lists
            foreach (ApplicationList list in lists)
            {
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM setuplists_applications WHERE ListGuid = @ListGuid";
                    command.Parameters.Add(new SQLiteParameter("@ListGuid", DbManager.FormatGuid(list.Guid)));

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid jobGuid = new Guid(reader["JobGuid"] as string);
                            // Find application and add to list
                            foreach (ApplicationJob app in applicationsToAttach)
                            {
                                if (app.Guid == jobGuid)
                                {
                                    list.Applications.Add(app);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return lists.ToArray();
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

        /// <summary>
        /// Returns a list of all settings in the database
        /// (key-value pairs).
        /// </summary>
        public static SerializableDictionary<string, string> GetSettings()
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM settings";

            SerializableDictionary<string, string> settings = new SerializableDictionary<string, string>();

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    settings.Add(reader["SettingPath"] as string, reader["SettingValue"] as string);
                }
            }

            return settings;
        }

        /// <summary>
        /// Saves all settings specified to the database.
        /// </summary>
        /// <param name="settings">Dictionary, which holds the settings as key-value pairs</param>
        public static void SetSettings(Dictionary<string, string> settings)
        {
            if (settings == null) return;

            SettingsProvider provider = new SettingsProvider();

            using (IDbTransaction transaction = Connection.BeginTransaction())
            {
                foreach (KeyValuePair<string, string> setting in settings)
                {
                    provider.SetValueRaw(setting.Value, setting.Key, transaction);
                }

                transaction.Commit();
            }
        }

    }
}
