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

        public static SQLiteConnection Connection
        {
            get
            {
                if (m_DbConn == null)
                {
                    m_DbConn = NewConnection;
                    Settings.Provider = new DbManager.SettingsProvider();
                    WebRequest.DefaultWebProxy = Proxy;
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

        public static void LoadOrCreateDatabase()
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
                                         PreviousLocation   TEXT,
                                         ExecuteCommand     TEXT,
                                         HttpReferer        TEXT,
                                         JobGuid            TEXT UNIQUE,
                                         DeletePreviousFile INTEGER,
                                         CanBeShared        INTEGER DEFAULT 1,
                                         ShareApplication   INTEGER,
                                         SourceType         INTEGER,
                                         IsEnabled          INTEGER);";
                command.ExecuteNonQuery();
            }

            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS variables  
                                        (JobId              INTEGER,
                                         VariableName       TEXT,
                                         Url                TEXT,
                                         StartText          TEXT,
                                         RegularExpression  TEXT,
                                         CachedContent      TEXT,
                                         EndText            TEXT);";
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
            addColumns.Add("Category", "ALTER TABLE jobs ADD Category TEXT");
            addColumns.Add("JobGuid", "ALTER TABLE jobs ADD JobGuid TEXT");
            addColumns.Add("CanBeShared", "ALTER TABLE jobs ADD CanBeShared INTEGER DEFAULT 1");
            addColumns.Add("ShareApplication", "ALTER TABLE jobs ADD ShareApplication INTEGER DEFAULT 0");
            addColumns.Add("HttpReferer", "ALTER TABLE jobs ADD HttpReferer TEXT");
            addColumns.Add("FileHippoVersion", "ALTER TABLE jobs ADD FileHippoVersion TEXT");
            
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

            columns = GetColumns("variables");
            if (!columns.Contains("RegularExpression"))
            {
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = "ALTER TABLE variables ADD RegularExpression TEXT";
                    command.ExecuteNonQuery();
                }
            }
            if (!columns.Contains("CachedContent"))
            {
                using (IDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = "ALTER TABLE variables ADD CachedContent TEXT";
                    command.ExecuteNonQuery();
                }
            }
        }

        private static List<string> GetColumns(string table)
        {
            List<string> columns = new List<string>();
            using (IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(" + table +")";
                IDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(1));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return columns;
        }


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

    }
}
