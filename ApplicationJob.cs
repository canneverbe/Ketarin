using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Ketarin
{
    public class ApplicationJob
    {
        private string m_Name;
        private long m_Id;
        private string m_FixedDownloadUrl = string.Empty;
        private string m_TargetPath = string.Empty;
        private DateTime? m_LastUpdated = null;
        private bool m_Enabled = true;
        private string m_FileHippoId = string.Empty;
        private bool m_DeletePreviousFile = false;
        private string m_PreviousLocation = string.Empty;
        private SourceType m_SourceType = SourceType.FixedUrl;
        private Dictionary<string, UrlVariable> m_Variables = null;

        public enum SourceType
        {
            FixedUrl,
            FileHippo
        }

        #region Properties

        internal Dictionary<string, UrlVariable> Variables
        {
            get {
                // Load variables on demand
                if (m_Variables == null)
                {
                    m_Variables = new Dictionary<string, UrlVariable>();
                    if (m_Id != 0)
                    {
                        using (IDbCommand command = DbManager.Connection.CreateCommand())
                        {
                            command.CommandText = @"SELECT * FROM variables WHERE JobId = @JobId";
                            command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                            using (IDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UrlVariable variable = new UrlVariable();
                                    variable.Hydrate(reader);
                                    m_Variables.Add(variable.Name, variable);
                                }
                            }
                        }
                    }
                }
                return m_Variables;
            }
        }

        internal SourceType DownloadSourceType
        {
            get { return m_SourceType; }
            set { m_SourceType = value; }
        }

        public string PreviousLocation
        {
            get { return m_PreviousLocation; }
            set { m_PreviousLocation = value; }
        }

        public bool DeletePreviousFile
        {
            get { return m_DeletePreviousFile; }
            set { m_DeletePreviousFile = value; }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public bool TargetIsFolder
        {
            get
            {
                if (string.IsNullOrEmpty(m_TargetPath)) return false;

                return Directory.Exists(m_TargetPath);
            }
        }

        public string FileHippoId
        {
            get { return m_FileHippoId; }
            set { m_FileHippoId = value; }
        }

        public DateTime? LastUpdated
        {
            get { return m_LastUpdated; }
            set { m_LastUpdated = value; }
        }

        public string TargetPath
        {
            get { return m_TargetPath; }
            set { m_TargetPath = value; }
        }

        public string FixedDownloadUrl
        {
            get { return m_FixedDownloadUrl; }
            set { m_FixedDownloadUrl = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        #endregion

        /// <summary>
        /// Deletes this job from the database.
        /// </summary>
        public void Delete()
        {
            SQLiteTransaction transaction = DbManager.Connection.BeginTransaction();

            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"DELETE FROM jobs WHERE JobId = @JobId";
                command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                command.ExecuteNonQuery();
            }

            // Delete variables
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM variables WHERE JobId = @JobId";
                command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public void Save()
        {
            SQLiteTransaction transaction = DbManager.Connection.BeginTransaction();

            if (m_Id > 0)
            {
                // Update existing job
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"UPDATE jobs
                                               SET ApplicationName = @ApplicationName,
                                                   FixedDownloadUrl = @FixedDownloadUrl,
                                                   TargetPath = @TargetPath,
                                                   LastUpdated = @LastUpdated,
                                                   IsEnabled = @IsEnabled,
                                                   FileHippoId = @FileHippoId,
                                                   DeletePreviousFile = @DeletePreviousFile,
                                                   PreviousLocation = @PreviousLocation,
                                                   SourceType = @SourceType
                                             WHERE JobId = @JobId";

                    command.Parameters.Add(new SQLiteParameter("@ApplicationName", Name));
                    command.Parameters.Add(new SQLiteParameter("@FixedDownloadUrl", m_FixedDownloadUrl));
                    command.Parameters.Add(new SQLiteParameter("@TargetPath", m_TargetPath));
                    command.Parameters.Add(new SQLiteParameter("@LastUpdated", m_LastUpdated));
                    command.Parameters.Add(new SQLiteParameter("@IsEnabled", m_Enabled));
                    command.Parameters.Add(new SQLiteParameter("@FileHippoId", m_FileHippoId));
                    command.Parameters.Add(new SQLiteParameter("@DeletePreviousFile", m_DeletePreviousFile));
                    command.Parameters.Add(new SQLiteParameter("@PreviousLocation", m_PreviousLocation));
                    command.Parameters.Add(new SQLiteParameter("@SourceType", m_SourceType));
                    command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                    
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // Insert a new job
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT INTO jobs (ApplicationName, FixedDownloadUrl, DateAdded, TargetPath, LastUpdated, IsEnabled, FileHippoId, DeletePreviousFile, SourceType)
                                                 VALUES (@ApplicationName, @FixedDownloadUrl, @DateAdded, @TargetPath, @LastUpdated, @IsEnabled, @FileHippoId, @DeletePreviousFile, @SourceType)";

                    command.Parameters.Add(new SQLiteParameter("@ApplicationName", Name));
                    command.Parameters.Add(new SQLiteParameter("@FixedDownloadUrl", m_FixedDownloadUrl));
                    command.Parameters.Add(new SQLiteParameter("@DateAdded", DateTime.Now));
                    command.Parameters.Add(new SQLiteParameter("@LastUpdated", m_LastUpdated));
                    command.Parameters.Add(new SQLiteParameter("@TargetPath", m_TargetPath));
                    command.Parameters.Add(new SQLiteParameter("@IsEnabled", m_Enabled));
                    command.Parameters.Add(new SQLiteParameter("@DeletePreviousFile", m_DeletePreviousFile));
                    command.Parameters.Add(new SQLiteParameter("@FileHippoId", m_FileHippoId));
                    command.Parameters.Add(new SQLiteParameter("@SourceType", m_SourceType));
                    
                    command.ExecuteNonQuery();
                }

                // Get ID
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = "SELECT last_insert_rowid()";
                    m_Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            Dictionary<string, UrlVariable> variables = Variables;

            // Save variables
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM variables WHERE JobId = @JobId";
                command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                command.ExecuteNonQuery();
            }

            foreach (KeyValuePair<string, UrlVariable> pair in variables)
            {
                using (IDbCommand command = DbManager.Connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT INTO variables (JobId, VariableName, Url, StartText, EndText)
                                                 VALUES (@JobId, @VariableName, @Url, @StartText, @EndText)";

                    command.Parameters.Add(new SQLiteParameter("@JobId", m_Id));
                    command.Parameters.Add(new SQLiteParameter("@VariableName", pair.Key));
                    command.Parameters.Add(new SQLiteParameter("@Url", pair.Value.Url));
                    command.Parameters.Add(new SQLiteParameter("@StartText", pair.Value.StartText));
                    command.Parameters.Add(new SQLiteParameter("@EndText", pair.Value.EndText));
                    command.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }

        public void Hydrate(IDataReader reader)
        {
            m_Id = (long)reader["JobId"];
            m_Name = reader["ApplicationName"] as string;
            m_FixedDownloadUrl = reader["FixedDownloadUrl"] as string;
            m_TargetPath = reader["TargetPath"] as string;
            m_LastUpdated = reader["LastUpdated"] as DateTime?;
            m_Enabled = Convert.ToBoolean(reader["IsEnabled"]);
            m_FileHippoId = reader["FileHippoId"] as string;
            m_DeletePreviousFile = Convert.ToBoolean(reader["DeletePreviousFile"]);
            m_PreviousLocation = reader["PreviousLocation"] as string;
            m_SourceType = (SourceType)Convert.ToByte(reader["SourceType"]);
        }

        public string GetTargetFile(WebResponse netResponse)
        {
            string targetLocation = TargetPath;

            // Allow variables in target locations as well
            foreach (UrlVariable variable in Variables.Values) {
                targetLocation = variable.ReplaceInString(targetLocation);
            }

            if (TargetIsFolder)
            {
                // If directory does not yet exist, create it
                Directory.CreateDirectory(targetLocation);

                string fileName = Path.GetFileName(netResponse.ResponseUri.AbsolutePath);

                // Look for alternative file name
                string disposition = netResponse.Headers.Get("content-disposition") as string;
                if (!string.IsNullOrEmpty(disposition))
                {
                    string token = "filename=";
                    int pos = disposition.IndexOf(token);
                    if (pos >= 0)
                    {
                        fileName = disposition.Substring(pos + token.Length);
                    }
                }

                fileName = fileName.Replace("%20", " ");
                targetLocation = Path.Combine(targetLocation, fileName);
            }
            return targetLocation;
        }

        private static string GetMd5OfFile(string filename)
        {
            MD5 hash = new MD5CryptoServiceProvider();
            byte[] localMd5 = hash.ComputeHash(File.ReadAllBytes(filename));
            StringBuilder result = new StringBuilder(32);
            for (int i = 0; i < localMd5.Length; i++)
            {
                result.Append(localMd5[i].ToString("X2"));
            }
            return result.ToString();
        }

        /// <summary>
        /// Determines whether or not it is required to (re-)download the file.
        /// </summary>
        /// <param name="netResponse">Response of the webserver containing information about the current file</param>
        /// <returns>true, if downloading is required</returns>
        public bool RequiresDownload(WebResponse netResponse)
        {
            FileInfo current = new FileInfo(GetTargetFile(netResponse));
            if (!current.Exists) return true;

            // If using FileHippo, and previous file is available, check MD5
            if (!string.IsNullOrEmpty(m_FileHippoId) && m_SourceType == SourceType.FileHippo && !string.IsNullOrEmpty(m_PreviousLocation) && File.Exists(m_PreviousLocation))
            {
                string serverMd5 = ExternalServices.FileHippoMd5(m_FileHippoId);
                return string.Compare(serverMd5, GetMd5OfFile(m_PreviousLocation)) != 0;
            }

            // Remote date must be greater than our local date
            
            TimeSpan diff = GetLastModified(netResponse) - current.LastWriteTime;
            return (current.Length != netResponse.ContentLength || diff > TimeSpan.Zero);
        }

        /// <summary>
        /// Returns the LastModified value of either a FTP or HTTP web response.
        /// </summary>
        /// <returns>DateTime.MaxValue if it cannot be determined</returns>
        public static DateTime GetLastModified(WebResponse netResponse)
        {
            DateTime lastModified = DateTime.MaxValue;
            FtpWebResponse ftpResponse = netResponse as FtpWebResponse;
            if (ftpResponse != null)
            {
                lastModified = ftpResponse.LastModified;
            }
            HttpWebResponse httpResponse = netResponse as HttpWebResponse;
            if (httpResponse != null)
            {
                lastModified = httpResponse.LastModified;
            }
            return lastModified;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
