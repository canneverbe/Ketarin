using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CDBurnerXP;
using CDBurnerXP.IO;
using Ketarin.Forms;
using System.Reflection;
using System.Collections;

namespace Ketarin
{
    /// <summary>
    /// Represents an application which can be kept up
    /// to date according to user defined rules.
    /// It is the main object of Ketarin.
    /// </summary>
    [XmlRoot("ApplicationJob")]
    public class ApplicationJob
    {
        private string m_Name;
        private string m_FixedDownloadUrl = string.Empty;
        private string m_TargetPath = string.Empty;
        private DateTime? m_LastUpdated = null;
        private bool m_Enabled = true;
        private string m_FileHippoId = string.Empty;
        private string m_FileHippoVersion = string.Empty;
        private SourceType m_SourceType = SourceType.FixedUrl;
        private UrlVariableCollection m_Variables = null;
        private Guid m_Guid = Guid.Empty;
        private bool m_CanBeShared = true;
        private bool m_ShareApplication = false;
        private string m_HttpReferer = string.Empty;
        private string m_VariableChangeIndicator = string.Empty;
        private string m_VariableChangeIndicatorLastContent = null;
        private string m_PreviousRelativeLocation = string.Empty;
        private List<SetupInstruction> setupInstructions = null;
        private static PropertyInfo[] applicationJobProperties = null;
        private string cachedCurrentLocation = null;
        private string previousLocation = string.Empty;

        /// <summary>
        /// Cached list of public properties of the type ApplicationJob.
        /// </summary>
        private static PropertyInfo[] ApplicationJobProperties
        {
            get
            {
                if (applicationJobProperties == null)
                {
                    applicationJobProperties = typeof(ApplicationJob).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                }
                return applicationJobProperties;
            }
        }

        public enum SourceType
        {
            FixedUrl,
            FileHippo
        }

        public enum DownloadBetaType
        {
            Default = 0,
            Avoid,
            AlwaysDownload
        }

        #region Properties

        /// <summary>
        /// Gets or sets the template from which the application has been created.
        /// </summary>
        [XmlIgnore()]
        public string SourceTemplate { get; set; }

        /// <summary>
        /// Source template property for XML serialization as CDATA.
        /// </summary>
        [XmlElement("SourceTemplate", typeof(XmlCDataSection))]
        public XmlCDataSection SourceTemplateCdata
        {
            get
            {
                // Prevent unnecessary CDATA elements
                if (string.IsNullOrEmpty(SourceTemplate))
                {
                    return null;
                }

                XmlDocument doc = new XmlDocument();
                return doc.CreateCDataSection(this.SourceTemplate);
            }
            set
            {
                if (value == null)
                {
                    this.SourceTemplate = string.Empty;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    if (string.IsNullOrEmpty(value.InnerText))
                    {
                        this.SourceTemplate = string.Empty;
                        return;
                    }
                    doc.LoadXml(value.InnerText);
                    // Make sure that no nested source templates are saved
                    foreach (XmlElement e in doc.GetElementsByTagName("SourceTemplate"))
                    {
                        e.ParentNode.RemoveChild(e);
                        break;
                    }
                    if (doc.FirstChild is XmlDeclaration)
                    {
                        doc.RemoveChild(doc.FirstChild);
                    }
                    this.SourceTemplate = doc.OuterXml.Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the website of the application.
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets the website of an application with all variables replaced.
        /// </summary>
        public string ExpandedWebsiteUrl
        {
            get
            {
                return m_Variables.ReplaceAllInString(WebsiteUrl);
            }
        }

        /// <summary>
        /// Gets or sets a custom user agent to use for downloads.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the custom notes for an application.
        /// </summary>
        public string UserNotes { get; set; }

        /// <summary>
        /// Gets or sets the last size of the file which
        /// has been downloaded for the application.
        /// </summary>
        public long LastFileSize { get; set; }

        /// <summary>
        /// Gets or sets the last write time of the file which
        /// has been downloaded for the application.
        /// </summary>
        public DateTime? LastFileDate { get; set; }

        /// <summary>
        /// Specifies whether or not to ignore the file based information.
        /// If true, only the information in database will be compared, and Ketarin
        /// will not re-download if the file is missing.
        /// </summary>
        public bool IgnoreFileInformation { get; set; }

        public DownloadBetaType DownloadBeta  { get; set; }

        /// <summary>
        /// Gets or sets the version information
        /// scraped from a PAD file.
        /// </summary>
        [XmlIgnore()]
        internal string CachedPadFileVersion { get; set; }

        /// <summary>
        /// The last updated date of the application
        /// in the online database.
        /// </summary>
        public DateTime? DownloadDate { get; set; }

        /// <summary>
        /// Gets or sets whether or not the application should
        /// not be downloaded.
        /// For example, you might not want to include downloading
        /// huge applications.
        /// </summary>
        public bool CheckForUpdatesOnly { get; set; }

        /// <summary>
        /// Gets or sets the variable, which is used as change indicator.
        /// If this value is not set, the file modification date / size is used.
        /// </summary>
        public string VariableChangeIndicator
        {
            get { return m_VariableChangeIndicator; }
            set
            {
                if (m_VariableChangeIndicator != value)
                {
                    m_VariableChangeIndicator = value;
                    m_VariableChangeIndicatorLastContent = null;
                }
            }
        }

        /// <summary>
        /// Determines whether or not a user can
        /// share this application online.
        /// This is the case for all applications a user
        /// downloaded, which are not his own.
        /// </summary>
        /// <remarks>The actual permission check is done on the
        /// remote server, so this is not a security measure.</remarks>
        public bool CanBeShared
        {
            get { return m_CanBeShared; }
            set { m_CanBeShared = value; }
        }

        public bool ShareApplication
        {
            get { return m_ShareApplication; }
            set
            {
                m_ShareApplication = value && CanBeShared;
            }
        }

        /// <summary>
        /// Gets or sets whether or not the application
        /// may be downloaded simultaneously with other
        /// applications.
        /// </summary>
        public bool ExclusiveDownload { get; set; }

        /// <summary>
        /// Gets or sets a referer which is used 
        /// for HTTP requests when downloading
        /// the application.
        /// </summary>
        public string HttpReferer
        {
            get { return m_HttpReferer; }
            set { m_HttpReferer = value; }
        }

        /// <summary>
        /// Gets or sets the globally unique identifier
        /// of this application.
        /// </summary>
        [XmlAttribute("Guid")]
        public Guid Guid
        {
            get
            {
                return m_Guid;
            }
            set
            {
                m_Guid = value;
            }
        }

        /// <summary>
        /// Gets the list of setup instructions that need to be executed in order to install the application.
        /// </summary>
        public List<SetupInstruction> SetupInstructions
        {
            get
            {
                if (this.setupInstructions == null)
                {
                    this.setupInstructions = new List<SetupInstruction>();

                    using (IDbCommand command = DbManager.Connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM setupinstructions WHERE JobGuid = @JobGuid ORDER BY Position";
                        command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string xmlInstructions = reader["Data"] as string;
                                if (string.IsNullOrEmpty(xmlInstructions)) continue;

                                // Needed to determine appropriate type
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xmlInstructions);

                                using (StringReader xmlReader = new StringReader(xmlInstructions))
                                {
                                    XmlSerializer serializer = new XmlSerializer(Type.GetType("Ketarin." + doc.DocumentElement.Name));
                                    SetupInstruction instruction = (SetupInstruction)serializer.Deserialize(xmlReader);
                                    instruction.Application = this;
                                    setupInstructions.Add(instruction);
                                }
                            }
                        }
                    }
                }

                return this.setupInstructions;
            }
            set
            {
                this.setupInstructions = value;
            }
        }

        #region UrlVariableCollection

        public class UrlVariableCollection : SerializableDictionary<string, UrlVariable>
        {
            private ApplicationJob m_Parent;
            private bool m_VersionDownloaded = false;
            private FileInfo cachedInfo = null;

            #region Properties

            /// <summary>
            /// Gets or sets the application to which the collection belongs.
            /// </summary>
            [XmlIgnore()]
            public ApplicationJob Parent
            {
                get { return m_Parent; }
                set { m_Parent = value; }
            }

            #endregion

            public UrlVariableCollection()
            {
            }
            
            public UrlVariableCollection(ApplicationJob parent)
            {
                m_Parent = parent;
            }

            /// <summary>
            /// Resets the download count of all variables to 0.
            /// </summary>
            public void ResetDownloadCount()
            {
                foreach (UrlVariable var in this.Values)
                {
                    var.DownloadCount = 0;
                }
                m_VersionDownloaded = false;
            }

            /// <summary>
            /// Determines whether or not a certain variable has
            /// been downloaded.
            /// </summary>
            /// <param name="name">Name of the variable, { and }.</param>
            public bool HasVariableBeenDownloaded(string name)
            {
                if (name == "version") return m_VersionDownloaded;

                if (!ContainsKey(name)) return false;

                UrlVariable var = this[name];
                return (var.DownloadCount > 0);
            }

            public string ReplaceAllInString(string value)
            {
                return ReplaceAllInString(value, DateTime.MinValue, null, false);
            }

            public string ReplaceAllInString(string value, DateTime fileDate, string filename, bool onlyCachedContent)
            {
                if (value == null) return value;

                if (m_Parent != null && !string.IsNullOrEmpty(m_Parent.CurrentLocation))
                {
                    try
                    {
                        if (!ContainsKey("file"))
                        {
                            value = UrlVariable.Replace(value, "file", m_Parent.CurrentLocation);
                        }

                        if (cachedInfo == null || cachedInfo.FullName != m_Parent.CurrentLocation)
                        {
                            cachedInfo = new FileInfo(m_Parent.CurrentLocation);
                        }
                        // Try to provide file date if missing
                        if (fileDate == DateTime.MinValue)
                        {
                            fileDate = cachedInfo.LastWriteTime;
                        }
                        // Provide file size
                        if (cachedInfo.Exists)
                        {
                            value = UrlVariable.Replace(value, "filesize", cachedInfo.Length.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        // Ignore all errors. If no information can be retrieved, 
                        // only expand the usual variables.
                    }
                }

                // Ignore invalid dates
                if (fileDate > DateTime.MinValue)
                {
                    // Some date/time variables, only if they were not user defined
                    string[] dateTimeVars = new string[] { "dd", "ddd", "dddd", "hh", "HH", "mm", "MM", "MMM", "MMMM", "ss", "tt", "yy", "yyyy", "zz", "zzz" };
                    foreach (string dateTimeVar in dateTimeVars)
                    {
                        if (!ContainsKey(dateTimeVar))
                        {
                            value = value.Replace("{f:" + dateTimeVar + "}", fileDate.ToString(dateTimeVar));
                        }
                    }
                }

                // Provide {url:ext} and {url:basefile}
                try
                {
                    if (filename != null)
                    {
                        value = value.Replace("{url:ext}", Path.GetExtension(filename).TrimStart('.'));
                        value = value.Replace("{url:basefile}", Path.GetFileNameWithoutExtension(filename));
                        value = value.Replace("{url:filename}", Path.GetFileName(filename));
                    }
                }
                catch (ArgumentException ex)
                {
                    LogDialog.Log("Could not determine {url:*} variables", ex);
                }

                value = UrlVariable.Replace(value, "startuppath", Application.StartupPath);

                // Some date/time variables, only if they were not user defined
                string[] dateTimeVariables = new string[] { "dd", "ddd", "dddd", "hh", "HH", "mm", "MM", "MMM", "MMMM", "ss", "tt", "yy", "yyyy", "zz", "zzz" };
                foreach (string dateTimeVar in dateTimeVariables)
                {
                    if (!ContainsKey(dateTimeVar))
                    {
                        value = value.Replace("{" + dateTimeVar + "}", DateTime.Now.ToString(dateTimeVar));
                    }
                }

                // Unix timestamp
                value = UrlVariable.Replace(value, "time", RpcApplication.DotNetToUnix(DateTime.Now).ToString());
                for (int i = 1; i <= 12; i++)
                {
                    value = UrlVariable.Replace(value, "time-" + i, RpcApplication.DotNetToUnix(DateTime.Now.AddHours(-i)).ToString());
                }
                for (int i = 1; i <= 12; i++)
                {
                    value = UrlVariable.Replace(value, "time+" + i, RpcApplication.DotNetToUnix(DateTime.Now.AddHours(+i)).ToString());
                }

                // Job-specific data / non global variables
                if (m_Parent != null)
                {
                    if (!string.IsNullOrEmpty(m_Parent.Category))
                    {
                        value = UrlVariable.Replace(value, "category", m_Parent.Category);
                    }

                    value = UrlVariable.Replace(value, "appname", m_Parent.Name);
                    value = UrlVariable.Replace(value, "appguid", DbManager.FormatGuid(m_Parent.Guid));
                    
                   
                    // Allow to access all public properties of the object per "property:X" variable.
                    foreach (PropertyInfo property in ApplicationJob.ApplicationJobProperties)
                    {
                        // Only make effort if variable is used
                        string varname = "property:" + property.Name;
                        if (UrlVariable.IsVariableUsedInString(varname, value))
                        {
                            if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType == typeof(string))
                            {
                                value = UrlVariable.Replace(value, varname, Convert.ToString(property.GetValue(m_Parent, null)));
                            }
                        }
                    }

                    if (!ContainsKey("version"))
                    {
                        // FileHippo version
                        if (m_Parent.DownloadSourceType == SourceType.FileHippo && UrlVariable.IsVariableUsedInString("version", value))
                        {
                            if (!onlyCachedContent)
                            {
                                m_Parent.FileHippoVersion = ExternalServices.FileHippoVersion(m_Parent.FileHippoId, m_Parent.AvoidDownloadBeta);
                                m_VersionDownloaded = true;
                            }
                            value = UrlVariable.Replace(value, "version", m_Parent.FileHippoVersion);
                        }
                        else if (!string.IsNullOrEmpty(m_Parent.CachedPadFileVersion))
                        {
                            // or PAD file version as alternative
                            value = UrlVariable.Replace(value, "version", m_Parent.CachedPadFileVersion);
                        }
                    }
                }

                foreach (UrlVariable var in Values)
                {
                    var.Parent = this; // make sure that value is set correctly
                    value = var.ReplaceInString(value, fileDate, onlyCachedContent);
                }

                // Global variables
                foreach (UrlVariable var in UrlVariable.GlobalVariables.Values)
                {
                    value = var.ReplaceInString(value, fileDate, true);
                }

                return value;
            }
        }

        #endregion

        [XmlElement("Variables")]
        public UrlVariableCollection Variables
        {
            get {
                return m_Variables;
            }
            set
            {
                if (value != null)
                {
                    m_Variables = value;
                    m_Variables.Parent = this;
                }
            }
        }

        /// <summary>
        /// A command to be executed after downloading.
        /// {file} is a placeholder for PreviousLocation.
        /// </summary>
        [XmlElement("ExecuteCommand")]
        public string ExecuteCommand
        {
            get;
            set;
        }

        /// <summary>
        /// A command to be executed before downloading.
        /// {file} is a placeholder for PreviousLocation.
        /// </summary>
        [XmlElement("ExecutePreCommand")]
        public string ExecutePreCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the post download command.
        /// </summary>
        [XmlElement("ExecuteCommandType")]
        public ScriptType ExecuteCommandType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the pre download command.
        /// </summary>
        [XmlElement("ExecutePreCommandType")]
        public ScriptType ExecutePreCommandType
        {
            get;
            set;
        }

        [XmlElement("Category")]
        public string Category
        {
            get;
            set;
        }

        [XmlElement("SourceType")]
        public SourceType DownloadSourceType
        {
            get { return m_SourceType; }
            set { m_SourceType = value; }
        }

        /// <summary>
        /// Gets or sets the last location the application has been downloaded to.
        /// </summary>
        public string PreviousLocation
        {
            get { return this.previousLocation; }
            set
            {
                if (this.previousLocation != value)
                {
                    this.previousLocation = value;
                    this.cachedCurrentLocation = null;
                }
            }
        }

        /// <summary>
        /// Determines whether or not the file exists.
        /// </summary>
        public bool FileExists
        {
            get
            {
                return !string.IsNullOrEmpty(CurrentLocation) && PathEx.TryGetFileSize(CurrentLocation) > 0;
            }
        }

        /// <summary>
        /// Determines the current location of the file, using the relative URI if necessary.
        /// </summary>
        [XmlIgnore()]
        public string CurrentLocation
        {
            get
            {
                if (this.cachedCurrentLocation != null)
                {
                    return this.cachedCurrentLocation;
                }

                string result = null;

                if (!string.IsNullOrEmpty(PreviousLocation) && PathEx.TryGetFileSize(PreviousLocation) > 0)
                {
                    result = PreviousLocation;
                }
                else if (!string.IsNullOrEmpty(m_PreviousRelativeLocation))
                {
                    try
                    {
                        result = Path.GetFullPath(Path.Combine(Application.StartupPath, m_PreviousRelativeLocation));
                    }
                    catch (NotSupportedException)
                    {
                        result = string.Empty;
                    }
                }
                else
                {
                    result = string.Empty;
                }

                this.cachedCurrentLocation = result;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets if the previously downloaded file should be deleted
        /// when downloading a new update.
        /// </summary>
        [XmlElement("DeletePreviousFile")]
        public bool DeletePreviousFile
        {
            get;
            set;
        }

        [XmlElement("Enabled")]
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

                return TargetPath.EndsWith("\\") || Directory.Exists(m_TargetPath);
            }
        }

        [XmlElement("FileHippoId")]
        public string FileHippoId
        {
            get { return m_FileHippoId; }
            set { m_FileHippoId = value; }
        }

        /// <summary>
        /// Contains the cached version information
        /// on FileHippo.
        /// </summary>
        [XmlIgnore()]
        public string FileHippoVersion
        {
            get { return m_FileHippoVersion; }
            set { m_FileHippoVersion = value; }
        }

        [XmlElement("LastUpdated")]
        public DateTime? LastUpdated
        {
            get { return m_LastUpdated; }
            set
            {
                if (m_LastUpdated != value)
                {
                    m_LastUpdated = value;
                    this.cachedCurrentLocation = null;
                }
            }
        }

        [XmlElement("TargetPath")]
        public string TargetPath
        {
            get { return m_TargetPath; }
            set { m_TargetPath = value; }
        }

        [XmlElement("FixedDownloadUrl")]
        public string FixedDownloadUrl
        {
            get { return m_FixedDownloadUrl; }
            set { m_FixedDownloadUrl = value; }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (value.Length > 255)
                {
                    m_Name = value.Substring(0, 255);
                }
                else
                {
                    m_Name = value;
                }
            }
        }

        /// <summary>
        /// Determines whether or not to download beta versions
        /// of this application by using the default and per
        /// application setting.
        /// </summary>
        public bool AvoidDownloadBeta
        {
            get
            {
                bool defaultValue = (bool)Settings.GetValue("AvoidFileHippoBeta", false);
                if (DownloadBeta == DownloadBetaType.Default)
                {
                    return defaultValue;
                }

                return (DownloadBeta == DownloadBetaType.Avoid);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of an application job.
        /// </summary>
        public ApplicationJob()
        {
            m_Variables = new ApplicationJob.UrlVariableCollection(this);
            ExecuteCommandType = ScriptType.Batch;
            ExecutePreCommandType = ScriptType.Batch;
        }

        /// <summary>
        /// Deletes this job from the database.
        /// </summary>
        public void Delete()
        {
            SQLiteTransaction transaction = DbManager.Connection.BeginTransaction();

            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"DELETE FROM jobs WHERE JobGuid = @JobGuid";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                command.ExecuteNonQuery();
            }

            // Delete variables
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        /// <summary>
        /// Updates an application downloaded from the online
        /// database based on the return value of the web service.
        /// </summary>
        /// <returns>true, if the applciation has been updated</returns>
        public bool UpdateFromXml(string[] xmlValues)
        {
            // No update possible
            if (CanBeShared) return false;

            foreach (string xml in xmlValues)
            {
                ApplicationJob job = LoadOneFromXml(xml);
                if (job.Guid == Guid)
                {
                    UpdateTemplatePropertiesFromApp(job); 
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Transfers all download relevant properties of an application
        /// to the current application and saves it.
        /// </summary>
        private void UpdateTemplatePropertiesFromApp(ApplicationJob job)
        {
            // Basically, we are only interested in properties
            // that change if a different method needs to be used
            // in order to download the file (changed website for example).
            DownloadDate = DateTime.Now;
            DownloadSourceType = job.DownloadSourceType;
            FileHippoId = job.FileHippoId;
            FixedDownloadUrl = job.FixedDownloadUrl;
            HttpReferer = job.HttpReferer;
            UserAgent = job.UserAgent;
            Name = job.Name;
            VariableChangeIndicator = job.VariableChangeIndicator;
            Variables = job.Variables;
            SetupInstructions = job.SetupInstructions;

            Save();
        }

        /// <summary>
        /// Updates the application based on a new version of its template.
        /// </summary>
        private void UpdateFromTemplate(string xml)
        {
            if (string.IsNullOrEmpty(SourceTemplate)) return;

            Dictionary<string, string> previousValues = new Dictionary<string, string>();

            // Extract previously used values
            XmlDocument sourceTemplateXml = new XmlDocument();
            sourceTemplateXml.LoadXml(SourceTemplate);

            XmlNodeList placeholdersList = sourceTemplateXml.GetElementsByTagName("placeholder");
            foreach (XmlElement element in placeholdersList)
            {
                previousValues[element.GetAttribute("name")] = element.GetAttribute("value");
            }

            XmlDocument newTemplate = new XmlDocument();
            newTemplate.LoadXml(xml);

            SetPlaceholders(newTemplate, previousValues);

            // Any placeholders left? Template cannot be applied
            placeholdersList = newTemplate.GetElementsByTagName("placeholder");
            if (placeholdersList.Count > 0)
            {
                throw new ApplicationException("The new template does not use the same placeholders.\r\n\r\nThe application cannot be updated.");
            }

            ApplicationJob newAppDefinition = LoadOneFromXml(newTemplate.OuterXml);
            UpdateTemplatePropertiesFromApp(newAppDefinition);
        }        

        /// <summary>
        /// Imports one (incomplete) ApplicationJob from a HTTP WebRequest.
        /// </summary>
        /// <returns>The incomplete ApplicationJob. Completiton by user required.</returns>
        public static ApplicationJob ImportFromPad(HttpWebResponse response)
        {
            using (Stream sourceFile = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(sourceFile))
                {
                    return ImportFromPadXml(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Imports one (incomplete) ApplicationJob from a PAD file.
        /// </summary>
        /// <returns>The incomplete ApplicationJob. Completiton by user required.</returns>
        public static ApplicationJob ImportFromPad(string fileName)
        {
            return ImportFromPadXml(File.ReadAllText(fileName));
        }

        /// <summary>
        /// Imports one (incomplete) ApplicationJob from a PAD file.
        /// </summary>
        /// <returns>null, if no application could be extracted</returns>
        private static ApplicationJob ImportFromPadXml(string xml)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xml);
            }
            catch (XmlException)
            {
                return null;
            }

            XmlNodeList progNames = doc.GetElementsByTagName("Program_Name");
            XmlNodeList downloadUrls = doc.GetElementsByTagName("Primary_Download_URL");
            XmlNodeList versionInfos = doc.GetElementsByTagName("Program_Version");
            XmlNodeList versionInfos2 = doc.GetElementsByTagName("Filename_Versioned");

            if (progNames.Count == 0 && downloadUrls.Count == 0) return null;

            ApplicationJob job = new ApplicationJob();
            job.DownloadSourceType = SourceType.FixedUrl;
            if (progNames.Count > 0)
            {
                job.Name = doc.GetElementsByTagName("Program_Name")[0].InnerText;
            }
            if (downloadUrls.Count > 0)
            {
                job.FixedDownloadUrl = doc.GetElementsByTagName("Primary_Download_URL")[0].InnerText;
            }
            if (versionInfos.Count > 0)
            {
                job.CachedPadFileVersion = doc.GetElementsByTagName("Program_Version")[0].InnerText;
            }
            else if (versionInfos2.Count > 0)
            {
                job.CachedPadFileVersion = doc.GetElementsByTagName("Filename_Versioned")[0].InnerText;
            }

            return job;
        }

        /// <summary>
        /// Imports one or more ApplicationJobs from an XML file.
        /// </summary>
        /// <returns>List of imported ApplicationJobs</returns>
        public static ApplicationJob[] ImportFromXml(string fileName)
        {
            return ImportFromXmlString(File.ReadAllText(fileName), true);
        }

        /// <summary>
        /// Imports one or more ApplicationJobs from a piece of XML.
        /// </summary>
        /// <returns>List of imported ApplicationJobs</returns>
        public static ApplicationJob[] ImportFromXmlString(string xml, bool save)
        {
            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(textReader))
                {
                    return ImportFromXml(reader, save);
                }
            }
        }

        /// <summary>
        /// Returns an XML document containing this application job.
        /// </summary>
        public string GetXml()
        {
            return GetXml(new ApplicationJob[] { this }, false, Encoding.UTF8);
        }

        /// <summary>
        /// Returns an XML document containing this application job,
        /// but replaces all global variables with the actual values.
        /// </summary>
        public string GetXmlWithoutGlobalVariables()
        {
            // Replace global variables
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(GetXml());
            // Adjust download URL
            XmlNodeList downloadUrlElements = doc.GetElementsByTagName("FixedDownloadUrl");
            if (downloadUrlElements.Count > 0)
            {
                XmlElement downloadUrlElement = downloadUrlElements[0] as XmlElement;
                downloadUrlElement.InnerText = UrlVariable.GlobalVariables.ReplaceAllInString(downloadUrlElement.InnerText);
            }
            // Adjust variables
            XmlNodeList urlVariableElements = doc.GetElementsByTagName("UrlVariable");
            foreach (XmlElement urlVariable in urlVariableElements)
            {
                foreach (XmlElement subElement in urlVariable.ChildNodes)
                {
                    if (subElement.Name == "Url" || subElement.Name == "TextualContent")
                    {
                        subElement.InnerText = UrlVariable.GlobalVariables.ReplaceAllInString(subElement.InnerText);
                    }
                }
            }

            // Change encoding
            if (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                XmlDeclaration xmlDeclaration = (XmlDeclaration)doc.FirstChild;
                xmlDeclaration.Encoding = "utf-8";
            }

            return doc.InnerXml;
        }

        /// <summary>
        /// Returns an XML document from the given jobs.
        /// </summary>
        /// <param name="jobs">The jobs which should be included in the XML</param>
        /// <param name="isTemplate">Determines whether or not a template should be generated. Templates lack a few properties.</param>
        public static string GetXml(System.Collections.IEnumerable jobs, bool isTemplate, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationJob));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            StringBuilder output = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
            {
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='" + encoding.WebName + "'");
                xmlWriter.WriteStartElement("Jobs");
                foreach (ApplicationJob job in jobs)
                {
                    // Before exporting, make sure that it got a Guid
                    if (job.Guid == Guid.Empty && !isTemplate) job.Save();
                    serializer.Serialize(xmlWriter, job);
                }
                xmlWriter.WriteEndElement();
            }

            string xmlResult = output.ToString();

            // Remove a couple of properties:
            // Guid, DownloadDate, LastUpdated, PreviousLocation
            if (isTemplate)
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xmlResult);

                XmlNodeList nodes = doc.GetElementsByTagName("ApplicationJob");
                foreach (XmlElement element in nodes)
                {
                    // Remove attribute Guid
                    element.RemoveAttribute("Guid");
                    // Remove element DownloadDate
                    element.RemoveChild(element["DownloadDate"]);
                    // Remove element LastUpdated
                    element.RemoveChild(element["LastUpdated"]);
                    // Remove element PreviousLocation
                    if (element["PreviousLocation"] != null)
                    {
                        element.RemoveChild(element["PreviousLocation"]);
                    }
                }

                xmlResult = doc.InnerXml;
            }

            return xmlResult;
        }

        /// <summary>
        /// Creates one or more application jobs from the given XML,
        /// without saving it.
        /// </summary>
        /// <returns>The last imported ApplicationJob</returns>
        public static ApplicationJob[] LoadFromXml(string xml)
        {
            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(textReader))
                {
                    return ImportFromXml(reader, false);
                }
            }
        }

        /// <summary>
        /// Creates a single application job from the given XML,
        /// without saving it.
        /// </summary>
        /// <returns>The last imported ApplicationJob</returns>
        public static ApplicationJob LoadOneFromXml(string xml)
        {
            ApplicationJob[] jobs = LoadFromXml(xml);
            return (jobs.Length == 0) ? null : jobs[0];
        }

        /// <summary>
        /// Imports an application job from an XML file. If the XML
        /// contains any place holders, a dialog will be shown and ask
        /// the user for additional information.
        /// Also checks for possible template updates.
        /// </summary>
        /// <param name="owner">Handle of the parent window</param>
        /// <param name="filename">File name of the XML file</param>
        public static ApplicationJob ImportFromTemplateOrXml(IWin32Window owner, string filename, ApplicationJob[] appsToCheckForUpdates)
        {
            return ImportFromTemplateOrXml(owner, File.ReadAllText(filename), appsToCheckForUpdates, true);
        }

        public static ApplicationJob ImportFromTemplateOrXml(IWin32Window owner, string xml, ApplicationJob[] appsToCheckForUpdates, bool isString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Guid templateGuid = Guid.Empty;

            // Determine GUID of current template
            XmlNodeList appElements = doc.GetElementsByTagName("ApplicationJob");
            foreach (XmlElement appElement in appElements)
            {
                if (!string.IsNullOrEmpty(appElement.GetAttribute("Guid")))
                {
                    templateGuid = new Guid(appElement.GetAttribute("Guid"));
                    break;
                }
            }

            List<ApplicationJob> appsToUpdate = new List<ApplicationJob>();

            // Check if any applications have been created from this template
            if (templateGuid != Guid.Empty)
            {
                foreach (ApplicationJob app in appsToCheckForUpdates)
                {
                    if (string.IsNullOrEmpty(app.SourceTemplate)) continue;

                    XmlDocument templateDoc = new XmlDocument();
                    templateDoc.LoadXml(app.SourceTemplate);
                    XmlNodeList sourceTemplateAppElements = templateDoc.GetElementsByTagName("ApplicationJob");
                    foreach (XmlElement sourceTemplateAppElement in sourceTemplateAppElements)
                    {
                        // Only use templates that have a GUID
                        if (!string.IsNullOrEmpty(sourceTemplateAppElement.GetAttribute("Guid")))
                        {
                            Guid sourceTemplateGuid = new Guid(sourceTemplateAppElement.GetAttribute("Guid"));
                            if (sourceTemplateGuid == templateGuid && !AreTemplatesEqual(doc, templateDoc))
                            {
                                appsToUpdate.Add(app);
                                break;
                            }
                        }
                    }
                }
            }

            if (appsToUpdate.Count > 0)
            {
                string msg = string.Format("{0} applications have been created from this template.\r\n\r\nDo you want to update these applications based on the new template?", appsToUpdate.Count);
                if (MessageBox.Show(owner, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
                {
                    foreach (ApplicationJob app in appsToUpdate)
                    {
                        app.UpdateFromTemplate(xml);
                    }
                    return null;
                }
            }

            XmlNodeList placeholdersList = doc.GetElementsByTagName("placeholder");

            // First, grather all values. A placeholder might occur twice.
            Dictionary<string, string> values = new Dictionary<string, string>();

            using (SetPlaceholderDialog dialog = new SetPlaceholderDialog(xml))
            {
                foreach (XmlElement element in placeholdersList)
                {
                    string name = element.GetAttribute("name");

                    if (string.IsNullOrEmpty(name) || values.ContainsKey(name)) continue;

                    dialog.AddPlaceHolder(name, element.GetAttribute("options"), element.GetAttribute("value"), element.GetAttribute("variable"));
                }

                // Abort importing if cancelled
                if (dialog.ShowDialog(owner) == DialogResult.Cancel) return null;

                values = dialog.Placeholders;
            }

            SetPlaceholders(doc, values);

            ApplicationJob[] jobs = ImportFromXmlString(doc.OuterXml, false);
            if (jobs.Length > 0)
            {
                // Attach used values to placeholders
                if (values.Count > 0)
                {
                    XmlDocument templateXml = new XmlDocument();
                    templateXml.PreserveWhitespace = true;
                    templateXml.LoadXml(xml);

                    foreach (XmlElement placeholder in templateXml.GetElementsByTagName("placeholder"))
                    {
                        placeholder.SetAttribute("value", values[placeholder.GetAttribute("name")]);
                    }

                    jobs[0].SourceTemplate = templateXml.OuterXml;
                    // Templates always create new applications. Ensure null GUID.
                    jobs[0].Guid = Guid.Empty;
                }

                foreach (ApplicationJob app in jobs)
                {
                    app.Save();
                }

                return jobs[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether or not two application templates with placeholders are equal.
        /// This is done by removing all placeholder elements and comparing what is left.
        /// </summary>
        private static bool AreTemplatesEqual(XmlDocument template1, XmlDocument template2)
        {
            XmlDocument templateA = template1.Clone() as XmlDocument;
            XmlDocument templateB = template2.Clone() as XmlDocument;

            XmlNodeList nodes = templateA.GetElementsByTagName("placeholder");
            XmlNode[] placeholders = new XmlNode[nodes.Count];
            for (int i = 0; i < placeholders.Length; i++)
            {
                placeholders[i] = nodes[i];
            }

            foreach (XmlElement element in placeholders)
            {
                element.ParentNode.RemoveChild(element);
            }

            nodes = templateB.GetElementsByTagName("placeholder");
            placeholders = new XmlNode[nodes.Count];
            for (int i = 0; i < placeholders.Length; i++)
            {
                placeholders[i] = nodes[i];
            }
            foreach (XmlElement element in placeholders)
            {
                element.ParentNode.RemoveChild(element);
            }

            return templateA.OuterXml == templateB.OuterXml;
        }

        /// <summary>
        /// Replaces all placeholders in a given template with the given values.
        /// </summary>
        /// <param name="doc">Template to update</param>
        /// <param name="values">Values to replace the placeholders with (name, value)</param>
        internal static void SetPlaceholders(XmlDocument doc, Dictionary<string, string> values)
        {
            XmlNodeList placeholdersList = doc.GetElementsByTagName("placeholder");
            // Prevent changing collection!
            XmlNode[] placeholders = new XmlNode[placeholdersList.Count];
            for (int i = 0; i < placeholders.Length; i++)
            {
                placeholders[i] = placeholdersList[i];
            }

            foreach (XmlElement element in placeholders)
            {
                string name = element.GetAttribute("name");

                if (!values.ContainsKey(name)) continue;

                // Apply regex on the value entered by the user?
                string regex = element.GetAttribute("regex");
                if (!string.IsNullOrEmpty(regex))
                {
                    try
                    {
                        Regex valueRegex = new Regex(regex);
                        Match match = valueRegex.Match(values[name]);
                        if (match.Success)
                        {
                            values[name] = match.Value;
                        }
                    }
                    catch (ArgumentException)
                    {
                        // invalid regex, do not use it
                        LogDialog.Log("Invalid regular expression for template placeholder '" + name + "'");
                    }
                }

                // Replace the placeholder with a text node
                element.ParentNode.ReplaceChild(doc.CreateTextNode(values[name]), element);
            }
        }

        /// <summary>
        /// Imports one or more ApplicationJobs from a piece of XML, 
        /// provided by an XmlReader.
        /// </summary>
        /// <returns>All imported ApplicationJobs</returns>
        private static ApplicationJob[] ImportFromXml(XmlReader reader, bool save)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationJob));
            // Find the start position
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationJob")
                {
                    break;
                }
            }

            List<ApplicationJob> importedJobs = new List<ApplicationJob>();

            // Read each job
            while (true)
            {
                ApplicationJob importedJob = (ApplicationJob)serializer.Deserialize(reader);
                if (importedJob == null) break;

                // If a job already exists, only update it!
                if (save) importedJob.Save();
                importedJobs.Add(importedJob);
            }

            return importedJobs.ToArray();
        }

        /// <summary>
        /// Saves the application job including all variables
        /// to the database.
        /// </summary>
        public void Save()
        {
            lock (this)
            {
                using (SQLiteConnection conn = DbManager.NewConnection)
                {
                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        if (!DbManager.ApplicationExists(conn, m_Guid))
                        {
                            if (m_Guid == Guid.Empty) m_Guid = Guid.NewGuid();

                            // Insert stub, update afterwards.
                            using (IDbCommand command = conn.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"INSERT INTO jobs (JobGuid, CanBeShared) VALUES (@JobGuid, @CanBeShared)";
                                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                                command.Parameters.Add(new SQLiteParameter("@CanBeShared", m_CanBeShared));
                                command.ExecuteNonQuery();
                            }
                        }

                        // Important: Once CanBeShared is set to false,
                        // it can never be true again (ownership does not change)
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "SELECT CanBeShared FROM jobs WHERE JobGuid = @JobGuid";
                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                            bool canBeShared = Convert.ToBoolean(command.ExecuteScalar());
                            if (!canBeShared)
                            {
                                m_CanBeShared = false;
                            }
                        }

                        // Update existing job
                        using (IDbCommand command = conn.CreateCommand())
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
                                                   SourceType = @SourceType,
                                                   ExecuteCommand = @ExecuteCommand,
                                                   ExecutePreCommand = @ExecutePreCommand, 
                                                   Category = @Category,
                                                   CanBeShared = @CanBeShared,
                                                   ShareApplication = @ShareApplication,
                                                   HttpReferer = @HttpReferer,
                                                   FileHippoVersion = @FileHippoVersion,
                                                   DownloadBeta = @DownloadBeta,
                                                   DownloadDate = @DownloadDate,
                                                   VariableChangeIndicator = @VariableChangeIndicator,
                                                   VariableChangeIndicatorLastContent = @VariableChangeIndicatorLastContent,
                                                   ExclusiveDownload = @ExclusiveDownload,
                                                   CheckForUpdateOnly = @CheckForUpdateOnly,
                                                   CachedPadFileVersion = @CachedPadFileVersion,
                                                   LastFileDate = @LastFileDate,
                                                   LastFileSize = @LastFileSize,
                                                   IgnoreFileInformation = @IgnoreFileInformation,
                                                   UserNotes = @UserNotes,
                                                   WebsiteUrl = @WebsiteUrl,
                                                   UserAgent = @UserAgent,
                                                   ExecuteCommandType = @ExecuteCommandType,
                                                   ExecutePreCommandType = @ExecutePreCommandType,
                                                   SourceTemplate = @SourceTemplate,
                                                   PreviousRelativeLocation = @PreviousRelativeLocation
                                             WHERE JobGuid = @JobGuid";

                            command.Parameters.Add(new SQLiteParameter("@ApplicationName", Name));
                            command.Parameters.Add(new SQLiteParameter("@FixedDownloadUrl", m_FixedDownloadUrl));
                            command.Parameters.Add(new SQLiteParameter("@TargetPath", m_TargetPath));
                            command.Parameters.Add(new SQLiteParameter("@LastUpdated", m_LastUpdated));
                            command.Parameters.Add(new SQLiteParameter("@IsEnabled", m_Enabled));
                            command.Parameters.Add(new SQLiteParameter("@FileHippoId", m_FileHippoId));
                            command.Parameters.Add(new SQLiteParameter("@DeletePreviousFile", DeletePreviousFile));
                            command.Parameters.Add(new SQLiteParameter("@PreviousLocation", PreviousLocation));
                            command.Parameters.Add(new SQLiteParameter("@SourceType", m_SourceType));
                            command.Parameters.Add(new SQLiteParameter("@ExecuteCommand", ExecuteCommand));
                            command.Parameters.Add(new SQLiteParameter("@ExecutePreCommand", ExecutePreCommand));
                            command.Parameters.Add(new SQLiteParameter("@Category", Category));
                            command.Parameters.Add(new SQLiteParameter("@CanBeShared", m_CanBeShared));
                            command.Parameters.Add(new SQLiteParameter("@ShareApplication", m_ShareApplication));
                            command.Parameters.Add(new SQLiteParameter("@HttpReferer", m_HttpReferer));
                            command.Parameters.Add(new SQLiteParameter("@FileHippoVersion", m_FileHippoVersion));
                            command.Parameters.Add(new SQLiteParameter("@DownloadBeta", (int)DownloadBeta));
                            command.Parameters.Add(new SQLiteParameter("@VariableChangeIndicator", m_VariableChangeIndicator));
                            command.Parameters.Add(new SQLiteParameter("@VariableChangeIndicatorLastContent", m_VariableChangeIndicatorLastContent));
                            command.Parameters.Add(new SQLiteParameter("@ExclusiveDownload", ExclusiveDownload));
                            command.Parameters.Add(new SQLiteParameter("@CheckForUpdateOnly", CheckForUpdatesOnly));
                            command.Parameters.Add(new SQLiteParameter("@CachedPadFileVersion", CachedPadFileVersion));
                            command.Parameters.Add(new SQLiteParameter("@LastFileDate", LastFileDate));
                            command.Parameters.Add(new SQLiteParameter("@LastFileSize", LastFileSize));
                            command.Parameters.Add(new SQLiteParameter("@IgnoreFileInformation", IgnoreFileInformation));
                            command.Parameters.Add(new SQLiteParameter("@UserNotes", UserNotes));
                            command.Parameters.Add(new SQLiteParameter("@WebsiteUrl", WebsiteUrl));
                            command.Parameters.Add(new SQLiteParameter("@UserAgent", UserAgent));
                            command.Parameters.Add(new SQLiteParameter("@ExecuteCommandType", ExecuteCommandType));
                            command.Parameters.Add(new SQLiteParameter("@ExecutePreCommandType", ExecutePreCommandType));
                            command.Parameters.Add(new SQLiteParameter("@SourceTemplate", SourceTemplate));

                            // In order to find files if the drive letter has changed (portable USB stick), also remember the 
                            // last relative location.
                            if (!string.IsNullOrEmpty(PreviousLocation))
                            {
                                try
                                {
                                    Uri uri1 = new Uri(PreviousLocation);
                                    Uri uri2 = new Uri(Application.StartupPath + "\\");

                                    Uri relativeUri = uri2.MakeRelativeUri(uri1);
                                    string relativePath = relativeUri.ToString().Replace("/", "\\");
                                    // If result returns out to be not relative, no need to save
                                    if (!Path.IsPathRooted(relativePath))
                                    {
                                        m_PreviousRelativeLocation = relativePath;
                                    }
                                }
                                catch (Exception)
                                {
                                    // Not critical if path cannot be determined.
                                }
                            }

                            command.Parameters.Add(new SQLiteParameter("@PreviousRelativeLocation", m_PreviousRelativeLocation));

                            if (DownloadDate.HasValue)
                            {
                                command.Parameters.Add(new SQLiteParameter("@DownloadDate", DownloadDate.Value));
                            }
                            else
                            {
                                command.Parameters.Add(new SQLiteParameter("@DownloadDate", DBNull.Value));
                            }

                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));

                            command.ExecuteNonQuery();
                        }

                        Dictionary<string, UrlVariable> variables = Variables;

                        // Save variables
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                            command.ExecuteNonQuery();
                        }

                        foreach (KeyValuePair<string, UrlVariable> pair in variables)
                        {
                            pair.Value.Save(transaction, m_Guid);
                        }

                        if (this.setupInstructions != null)
                        {
                            using (IDbCommand command = conn.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = "DELETE FROM setupinstructions WHERE JobGuid = @JobGuid";
                                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(m_Guid)));
                                command.ExecuteNonQuery();
                            }

                            int pos = 0;
                            foreach (SetupInstruction instruction in this.setupInstructions)
                            {
                                instruction.Application = this;
                                instruction.Save(transaction, pos++);
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
        }

        public void Hydrate(IDataReader reader)
        {
            m_Name = reader["ApplicationName"] as string;
            m_FixedDownloadUrl = reader["FixedDownloadUrl"] as string;
            m_TargetPath = reader["TargetPath"] as string;
            m_LastUpdated = reader["LastUpdated"] as DateTime?;
            m_Enabled = Convert.ToBoolean(reader["IsEnabled"]);
            m_FileHippoId = reader["FileHippoId"] as string;
            DeletePreviousFile = Convert.ToBoolean(reader["DeletePreviousFile"]);
            PreviousLocation = reader["PreviousLocation"] as string;
            m_SourceType = (SourceType)Convert.ToByte(reader["SourceType"]);
            ExecuteCommand = reader["ExecuteCommand"] as string;
            ExecutePreCommand = reader["ExecutePreCommand"] as string;
            Category = reader["Category"] as string;
            m_CanBeShared = Convert.ToBoolean(reader["CanBeShared"]);
            m_ShareApplication = Convert.ToBoolean(reader["ShareApplication"]);
            m_FileHippoVersion = reader["FileHippoVersion"] as string;
            m_HttpReferer = reader["HttpReferer"] as string;
            m_VariableChangeIndicator = reader["VariableChangeIndicator"] as string;
            m_VariableChangeIndicatorLastContent = reader["VariableChangeIndicatorLastContent"] as string;
            ExclusiveDownload = Convert.ToBoolean(reader["ExclusiveDownload"]);
            CheckForUpdatesOnly = Convert.ToBoolean(reader["CheckForUpdateOnly"]);
            CachedPadFileVersion = reader["CachedPadFileVersion"] as string;
            LastFileSize = Convert.ToInt64(reader["LastFileSize"]);
            LastFileDate = reader["LastUpdated"] as DateTime?;
            IgnoreFileInformation = Convert.ToBoolean(reader["IgnoreFileInformation"]);
            UserNotes = reader["UserNotes"] as string;
            WebsiteUrl = reader["WebsiteUrl"] as string;
            UserAgent = reader["UserAgent"] as string;
            SourceTemplate = reader["SourceTemplate"] as string;
            m_PreviousRelativeLocation = reader["PreviousRelativeLocation"] as string;

            string executeCommandType = reader["ExecuteCommandType"] as string;
            if (executeCommandType != null)
            {
                ExecuteCommandType = Command.ConvertToScriptType(executeCommandType);
            }

            string executePreCommandType = reader["ExecutePreCommandType"] as string;
            if (executePreCommandType != null)
            {
                ExecutePreCommandType = Command.ConvertToScriptType(executePreCommandType);
            }

            if (reader["DownloadBeta"] != DBNull.Value)
            {
                DownloadBeta = (DownloadBetaType)Convert.ToInt32(reader["DownloadBeta"]);
            }
            // An application has not been downloaded necessarily
            DownloadDate = (reader["DownloadDate"] != DBNull.Value) ? reader["DownloadDate"] as DateTime? : null;
            
            string guid = reader["JobGuid"] as string;
            m_Guid = new Guid(guid);
        }

        public string GetTargetFile(WebResponse netResponse, string alternateFileName)
        {
            string targetLocation = Environment.ExpandEnvironmentVariables(TargetPath);

            // Allow variables in target locations as well
            targetLocation = Variables.ReplaceAllInString(targetLocation, GetLastModified(netResponse), GetFileNameFromWebResponse(netResponse, alternateFileName), false);

            // If carried on a USB stick, allow using the drive name
            try
            {
                targetLocation = UrlVariable.Replace(targetLocation, "root", Path.GetPathRoot(Application.StartupPath));
            }
            catch (ArgumentException) { }

            if (TargetIsFolder || Directory.Exists(targetLocation))
            {
                string fileName = GetFileNameFromWebResponse(netResponse, alternateFileName);
                targetLocation = Path.Combine(targetLocation, fileName);
            }

            // If relative file names are used, add the startup path
            try
            {
                if (!Path.IsPathRooted(targetLocation))
                {
                    targetLocation = Path.Combine(Application.StartupPath, targetLocation);
                }
            }
            catch (ArgumentException) { }

            return targetLocation;
        }

        /// <summary>
        /// Returns the result file name of a web response. If possible, the
        /// content disposition headers are considered as well.
        /// </summary>
        private static string GetFileNameFromWebResponse(WebResponse netResponse, string alternateFileName)
        {
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
                    fileName = fileName.Replace("\"", "");
                    int endPos = fileName.IndexOf(';');
                    if (endPos > 0)
                    {
                        fileName = fileName.Substring(0, endPos);
                    }

                    // Make sure that no relative paths are being injected (security issue)
                    fileName = Path.GetFileName(fileName);

                    fileName = PathEx.ReplaceInvalidFileNameChars(fileName);
                }
            }

            if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(alternateFileName))
            {
                fileName = Path.GetFileName(alternateFileName);
            }

            fileName = fileName.Replace("%20", " ");

            return fileName;
        }

        private static string GetMd5OfFile(string filename)
        {
            MD5 hash = new MD5CryptoServiceProvider();
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] localMd5 = hash.ComputeHash(stream);
                StringBuilder result = new StringBuilder(32);
                for (int i = 0; i < localMd5.Length; i++)
                {
                    result.Append(localMd5[i].ToString("X2"));
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// Determines whether or not it is required to (re-)download the file.
        /// </summary>
        /// <param name="netResponse">Response of the webserver containing information about the current file</param>
        /// <returns>true, if downloading is required</returns>
        public bool RequiresDownload(WebResponse netResponse, string targetFile)
        {
            LogDialog.Log(this, "Checking if update is required...");

            FileInfo current = null;

            if (!string.IsNullOrEmpty(targetFile))
            {
                try
                {
                    current = new FileInfo(targetFile);
                }
                catch (ArgumentException)
                {
                    throw new TargetPathInvalidException(targetFile);
                }
                catch (NotSupportedException)
                {
                    throw new TargetPathInvalidException(targetFile);
                }

                if (!current.Exists && !string.IsNullOrEmpty(CurrentLocation) && DeletePreviousFile)
                {
                    // The file does not exist at the target location.
                    // Check if the previously downloaded file still matches.
                    current = new FileInfo(CurrentLocation);
                }

                if (!IgnoreFileInformation && !current.Exists)
                {
                    LogDialog.Log(this, string.Format("Update required, '{0}' does not yet exist", targetFile));
                    return true;
                }
            }

            // Check a variable's contents?
            if (!string.IsNullOrEmpty(m_VariableChangeIndicator))
            {
                string varName = m_VariableChangeIndicator.Trim('{', '}');
                string content = Variables.ReplaceAllInString("{" + varName + "}");
                // Only return a result, if the variable has already been checked before,
                // that is, if there is an actual difference.
                if (m_VariableChangeIndicatorLastContent != null)
                {
                    bool update = (content != m_VariableChangeIndicatorLastContent);
                    if (update)
                    {
                        LogDialog.Log(this, string.Format("Update required, {0} has changed from '{1}' to '{2}'", "{" + varName + "}", m_VariableChangeIndicatorLastContent, content));
                    }
                    else
                    {
                        LogDialog.Log(this, string.Format("Update not required, {0} has not changed", "{" + varName + "}"));
                    }

                    m_VariableChangeIndicatorLastContent = content;
                    return update;
                }
                else
                {
                    LogDialog.Log(this, string.Format("No previous value for {0} available, ignoring this variable as indicator for changes", "{" + varName + "}"));
                    m_VariableChangeIndicatorLastContent = content;
                }
            }

            // If using FileHippo, and previous file is available, check MD5
            if (!string.IsNullOrEmpty(m_FileHippoId) && m_SourceType == SourceType.FileHippo && FileExists)
            {
                string serverMd5 = ExternalServices.FileHippoMd5(m_FileHippoId, AvoidDownloadBeta);
                // It may happen, that the MD5 is not calculated
                if (serverMd5 != null)
                {
                    bool md5Result = string.Compare(serverMd5, GetMd5OfFile(CurrentLocation), true) != 0;
                    LogDialog.Log(this, md5Result ? "Update required, MD5 does not match" : "Update not required");
                    return md5Result;
                }
            }

            // Remote date must be greater than our local date
            // However, if the remote date is very close to DateTime.Now, it is incorrect (scripts)
            TimeSpan toNowDiff = DateTime.Now - GetLastModified(netResponse);
            bool disregardDate = Math.Abs(toNowDiff.TotalSeconds) <= 0.1;

            if (current == null) return false;

            bool fileSizeMismatch = false, dateMismatch = false;

            if (IgnoreFileInformation)
            {
                if (LastFileDate.HasValue)
                {
                    TimeSpan diff = GetLastModified(netResponse) - LastFileDate.Value;
                    dateMismatch = (!disregardDate && diff > TimeSpan.Zero);
                }
                if (LastFileSize > 0)
                {
                    fileSizeMismatch = (LastFileSize != netResponse.ContentLength && netResponse.ContentLength >= 0);
                }
            }
            else
            {
                TimeSpan diff = GetLastModified(netResponse) - current.LastWriteTime;
                fileSizeMismatch = (current.Length != netResponse.ContentLength && netResponse.ContentLength >= 0);
                dateMismatch = (!disregardDate && diff > TimeSpan.Zero);
            }

            bool result = (fileSizeMismatch || dateMismatch);
            LogDialog.Log(this, fileSizeMismatch, dateMismatch);

            return result;
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
                try
                {
                    lastModified = httpResponse.LastModified;
                }
                catch (ProtocolViolationException)
                {
                    return DateTime.Now;
                }
            }
            ScpWebResponse scpResponse = netResponse as ScpWebResponse;
            if (scpResponse != null)
            {
                lastModified = scpResponse.LastModified;
            }
            return lastModified;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether or not this application matches the given search critera.
        /// <param name="subjects">Lowercased search subjects</param>
        /// </summary>
        public bool MatchesSearchCriteria(string[] subjects, Dictionary<string, string> customColumns)
        {
            // Always matches if no subject is given
            if (subjects.Length == 0 || (subjects.Length == 1 && string.IsNullOrEmpty(subjects[0])))
            {
                return true;
            }

            StringBuilder fulltext = new StringBuilder(Name);
            fulltext.Append(Category);
            fulltext.Append(TargetPath);

            foreach (KeyValuePair<string, string> column in customColumns)
            {
                string value = Variables.ReplaceAllInString(column.Value, DateTime.MinValue, null, true);
                fulltext.Append(value);
            }

            string fulltextToLower = fulltext.ToString().ToLower();

            // Boolean search: All subjects must be matched
            foreach (string subject in subjects)
            {
                if (!MatchesSubject(subject, fulltextToLower))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether or not this application matches a specific search subject.
        /// </summary>
        /// <param name="fulltextToLower">All textual data of the application combined in a lowercased string</param>
        private bool MatchesSubject(string subject, string fulltextToLower)
        {
            int colonPos = subject.IndexOf(':');
            if (colonPos > 0)
            {
                string keyword = subject.Substring(0, colonPos);
                string searchSubject = subject.Substring(colonPos + 1);

                if (!string.IsNullOrEmpty(searchSubject))
                {
                    switch (keyword)
                    {
                        case "inurl":
                            // Search for URLs with a certain content
                            return this.FixedDownloadUrl.ToLower().Contains(searchSubject);

                        case "incommand":
                            // Search for commands with a certain content
                            return (this.ExecuteCommand != null && this.ExecuteCommand.ToLower().Contains(searchSubject)) || (this.ExecutePreCommand != null && this.ExecutePreCommand.ToLower().Contains(searchSubject));

                        case "invarname":
                            // Check for variables with a certain name
                            StringBuilder varNames = new StringBuilder();
                            foreach (UrlVariable variable in this.Variables.Values)
                            {
                                varNames.Append(variable.Name);
                            }

                            return varNames.ToString().ToLower().Contains(searchSubject);

                        case "invarurl":
                            // Check for variables with certain URLs
                            StringBuilder varUrls = new StringBuilder();
                            foreach (UrlVariable variable in this.Variables.Values)
                            {
                                varUrls.Append(variable.Url);
                            }

                            return varUrls.ToString().ToLower().Contains(searchSubject);
                    }
                }
            }

            return fulltextToLower.Contains(subject);
        }

        /// <summary>
        /// Installs the application, if setup instructions are defined.
        /// </summary>
        /// <param name="bgwSetup">Parent background worker, checks for cancellation</param>
        public void Install(System.ComponentModel.BackgroundWorker bgwSetup)
        {
            foreach (SetupInstruction instruction in this.SetupInstructions)
            {
                if (bgwSetup != null && bgwSetup.CancellationPending) return;

                instruction.Execute();
            }
        }
    }
}
