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
using System.Linq;
using CodeProject.ReiMiyasaka;

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
        private string m_TargetPath = string.Empty;
        private DateTime? m_LastUpdated;
        private UrlVariableCollection m_Variables;
        private bool m_ShareApplication;
        private string m_VariableChangeIndicator = string.Empty;
        private string m_VariableChangeIndicatorLastContent;
        private string m_PreviousRelativeLocation = string.Empty;
        private List<SetupInstruction> setupInstructions;
        private static PropertyInfo[] applicationJobProperties;
        private string cachedCurrentLocation;
        private string previousLocation = string.Empty;

        /// <summary>
        /// Cached list of public properties of the type ApplicationJob.
        /// </summary>
        private static PropertyInfo[] ApplicationJobProperties
        {
            get {
                return applicationJobProperties ??
                       (applicationJobProperties =
                           typeof (ApplicationJob).GetProperties(BindingFlags.Public | BindingFlags.Instance));
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
                if (string.IsNullOrEmpty(this.SourceTemplate))
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
                    XmlDocument doc = new XmlDocument {PreserveWhitespace = true};
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
                return this.m_Variables.ReplaceAllInString(this.WebsiteUrl);
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
            get { return this.m_VariableChangeIndicator; }
            set
            {
                if (this.m_VariableChangeIndicator != value)
                {
                    this.m_VariableChangeIndicator = value;
                    this.m_VariableChangeIndicatorLastContent = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the variable which contains the hash value.
        /// </summary>
        public string HashVariable { get; set; }

        /// <summary>
        /// Gets or sets the kind of hash used for change detection.
        /// </summary>
        public HashType HashType { get; set; }

        /// <summary>
        /// Determines whether or not a user can
        /// share this application online.
        /// This is the case for all applications a user
        /// downloaded, which are not his own.
        /// </summary>
        /// <remarks>The actual permission check is done on the
        /// remote server, so this is not a security measure.</remarks>
        public bool CanBeShared { get; set; } = true;

        public bool ShareApplication
        {
            get { return this.m_ShareApplication; }
            set
            {
                this.m_ShareApplication = value && this.CanBeShared;
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
        public string HttpReferer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the globally unique identifier
        /// of this application.
        /// </summary>
        [XmlAttribute("Guid")]
        public Guid Guid { get; set; } = Guid.Empty;

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
                                    this.setupInstructions.Add(instruction);
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
            private bool m_VersionDownloaded;
            private FileInfo cachedInfo;

            #region Properties

            /// <summary>
            /// Gets or sets the application to which the collection belongs.
            /// </summary>
            [XmlIgnore()]
            public ApplicationJob Parent { get; set; }

            #endregion

            public UrlVariableCollection()
            {
            }
            
            public UrlVariableCollection(ApplicationJob parent)
            {
                this.Parent = parent;
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
                this.m_VersionDownloaded = false;
            }

            /// <summary>
            /// Determines whether or not a certain variable has
            /// been downloaded.
            /// </summary>
            /// <param name="name">Name of the variable, { and }.</param>
            public bool HasVariableBeenDownloaded(string name)
            {
                if (name == "version") return this.m_VersionDownloaded;

                if (!this.ContainsKey(name)) return false;

                UrlVariable var = this[name];
                return (var.DownloadCount > 0);
            }

            public virtual string ReplaceAllInString(string value)
            {
                return this.ReplaceAllInString(value, DateTime.MinValue, null, false);
            }

            public virtual string ReplaceAllInString(string value, DateTime fileDate, string filename, bool onlyCachedContent, bool skipGlobalVariables = false)
            {
                if (value == null) return null;

                if (this.Parent != null && !string.IsNullOrEmpty(this.Parent.CurrentLocation))
                {
                    try
                    {
                        if (!this.ContainsKey("file"))
                        {
                            value = UrlVariable.Replace(value, "file", this.Parent.CurrentLocation, this.Parent);
                        }

                        if (this.cachedInfo == null || this.cachedInfo.FullName != this.Parent.CurrentLocation)
                        {
                            this.cachedInfo = new FileInfo(this.Parent.CurrentLocation);
                        }
                        // Try to provide file date if missing
                        if (fileDate == DateTime.MinValue)
                        {
                            fileDate = this.cachedInfo.LastWriteTime;
                        }
                        // Provide file size
                        if (this.cachedInfo.Exists)
                        {
                            value = UrlVariable.Replace(value, "filesize", this.cachedInfo.Length.ToString(), this.Parent);
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
                    string[] dateTimeVars = { "dd", "ddd", "dddd", "hh", "HH", "mm", "MM", "MMM", "MMMM", "ss", "tt", "yy", "yyyy", "zz", "zzz" };
                    foreach (string dateTimeVar in dateTimeVars)
                    {
                        if (!this.ContainsKey(dateTimeVar))
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

                value = UrlVariable.Replace(value, "startuppath", PathEx.QualifyPath(Application.StartupPath), this.Parent);

                // Some date/time variables, only if they were not user defined
                string[] dateTimeVariables = { "dd", "ddd", "dddd", "hh", "HH", "mm", "MM", "MMM", "MMMM", "ss", "tt", "yy", "yyyy", "zz", "zzz" };
                foreach (string dateTimeVar in dateTimeVariables)
                {
                    if (!this.ContainsKey(dateTimeVar))
                    {
                        value = value.Replace("{" + dateTimeVar + "}", DateTime.Now.ToString(dateTimeVar));
                    }
                }

                // Unix timestamp
                value = UrlVariable.Replace(value, "time", RpcApplication.DotNetToUnix(DateTime.Now).ToString(), this.Parent);
                for (int i = 1; i <= 12; i++)
                {
                    value = UrlVariable.Replace(value, "time-" + i, RpcApplication.DotNetToUnix(DateTime.Now.AddHours(-i)).ToString(), this.Parent);
                }
                for (int i = 1; i <= 12; i++)
                {
                    value = UrlVariable.Replace(value, "time+" + i, RpcApplication.DotNetToUnix(DateTime.Now.AddHours(+i)).ToString(), this.Parent);
                }

                // Job-specific data / non global variables
                if (this.Parent != null)
                {
                    if (!string.IsNullOrEmpty(this.Parent.Category))
                    {
                        value = UrlVariable.Replace(value, "category", this.Parent.Category, this.Parent);
                    }

                    value = UrlVariable.Replace(value, "appname", this.Parent.Name, this.Parent);
                    value = UrlVariable.Replace(value, "appguid", DbManager.FormatGuid(this.Parent.Guid), this.Parent);
                    
                   
                    // Allow to access all public properties of the object per "property:X" variable.
                    foreach (PropertyInfo property in ApplicationJobProperties)
                    {
                        // Only make effort if variable is used
                        string varname = "property:" + property.Name;
                        if (UrlVariable.IsVariableUsedInString(varname, value))
                        {
                            if (!typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || property.PropertyType == typeof(string))
                            {
                                value = UrlVariable.Replace(value, varname, Convert.ToString(property.GetValue(this.Parent, null)), this.Parent);
                            }
                        }
                    }

                    if (!this.ContainsKey("version"))
                    {
                        // FileHippo version
                        if (this.Parent.DownloadSourceType == SourceType.FileHippo && UrlVariable.IsVariableUsedInString("version", value))
                        {
                            if (!onlyCachedContent)
                            {
                                this.Parent.FileHippoVersion = ExternalServices.FileHippoVersion(this.Parent.FileHippoId, this.Parent.AvoidDownloadBeta);
                                this.m_VersionDownloaded = true;
                            }
                            value = UrlVariable.Replace(value, "version", this.Parent.FileHippoVersion, this.Parent);
                        }
                        else if (!string.IsNullOrEmpty(this.Parent.CachedPadFileVersion))
                        {
                            // or PAD file version as alternative
                            value = UrlVariable.Replace(value, "version", this.Parent.CachedPadFileVersion, this.Parent);
                        }
                    }
                }

                foreach (UrlVariable var in this.Values)
                {
                    var.Parent = this; // make sure that value is set correctly
                    value = var.ReplaceInString(value, fileDate, onlyCachedContent);
                }

                // Global variables
                if (!skipGlobalVariables)
                {
                    value = UrlVariable.GlobalVariables.ReplaceAllInString(value, fileDate, null, true, skipGlobalVariables);
                }

                return value;
            }
        }

        #endregion

        [XmlElement("Variables")]
        public UrlVariableCollection Variables
        {
            get {
                return this.m_Variables;
            }
            set
            {
                if (value != null)
                {
                    this.m_Variables = value;
                    this.m_Variables.Parent = this;
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
        public SourceType DownloadSourceType { get; set; } = SourceType.FixedUrl;

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
                return !string.IsNullOrEmpty(this.CurrentLocation) && PathEx.TryGetFileSize(this.CurrentLocation) > 0;
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

                string result;

                if (!string.IsNullOrEmpty(this.PreviousLocation) && PathEx.TryGetFileSize(this.PreviousLocation) > 0)
                {
                    result = this.PreviousLocation;
                }
                else if (!string.IsNullOrEmpty(this.m_PreviousRelativeLocation))
                {
                    try
                    {
                        result = Path.GetFullPath(Path.Combine(Application.StartupPath, this.m_PreviousRelativeLocation));
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
            get; set;
        }

        [XmlElement("Enabled")]
        public bool Enabled { get; set; }

        public bool TargetIsFolder
        {
            get
            {
                if (string.IsNullOrEmpty(this.TargetPath)) return false;

                return this.TargetPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.CurrentCulture);
            }
        }

        [XmlElement("FileHippoId")]
        public string FileHippoId { get; set; }

        /// <summary>
        /// Contains the cached version information
        /// on FileHippo.
        /// </summary>
        [XmlIgnore()]
        public string FileHippoVersion { get; set; }

        [XmlElement("LastUpdated")]
        public DateTime? LastUpdated
        {
            get { return this.m_LastUpdated; }
            set
            {
                if (this.m_LastUpdated != value)
                {
                    this.m_LastUpdated = value;
                    this.cachedCurrentLocation = null;
                }
            }
        }

        [XmlElement("TargetPath")]
        public string TargetPath {
	    get { return m_TargetPath; }
	    set { m_TargetPath = PathEx.FixDirectorySeparator(value); }
       	}

        [XmlElement("FixedDownloadUrl")]
        public string FixedDownloadUrl { get; set; } = string.Empty;

        [XmlElement("Name")]
        public string Name
        {
            get { return this.m_Name; }
            set {
                this.m_Name = value.Length > 255 ? value.Substring(0, 255) : value;
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
                if (this.DownloadBeta == DownloadBetaType.Default)
                {
                    return defaultValue;
                }

                return (this.DownloadBeta == DownloadBetaType.Avoid);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of an application job.
        /// </summary>
        public ApplicationJob()
        {
            this.Enabled = true;
            this.FileHippoId = string.Empty;
            this.FileHippoVersion = string.Empty;
            this.m_Variables = new UrlVariableCollection(this);
            this.ExecuteCommandType = ScriptType.Batch;
            this.ExecutePreCommandType = ScriptType.Batch;
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
                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
                command.ExecuteNonQuery();
            }

            // Delete variables
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        /// <summary>
        /// Deletes this job from the database.
        /// </summary>
        public static void DeleteAll()
        {
            SQLiteTransaction transaction = DbManager.Connection.BeginTransaction();

            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"DELETE FROM jobs";
                command.ExecuteNonQuery();
            }

            // Delete variables
            using (IDbCommand command = DbManager.Connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM variables";
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
            if (this.CanBeShared) return false;

            foreach (string xml in xmlValues)
            {
                ApplicationJob job = LoadOneFromXml(xml);
                if (job.Guid == this.Guid)
                {
                    this.UpdateTemplatePropertiesFromApp(job); 
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
            this.DownloadDate = DateTime.Now;
            this.DownloadSourceType = job.DownloadSourceType;
            this.FileHippoId = job.FileHippoId;
            this.FixedDownloadUrl = job.FixedDownloadUrl;
            this.HttpReferer = job.HttpReferer;
            this.UserAgent = job.UserAgent;
            this.Name = job.Name;
            this.VariableChangeIndicator = job.VariableChangeIndicator;
            this.Variables = job.Variables;
            this.SetupInstructions = job.SetupInstructions;

            this.Save();
        }

        /// <summary>
        /// Updates the application based on a new version of its template.
        /// </summary>
        private void UpdateFromTemplate(string xml)
        {
            if (string.IsNullOrEmpty(this.SourceTemplate)) return;

            Dictionary<string, string> previousValues = new Dictionary<string, string>();

            // Extract previously used values
            XmlDocument sourceTemplateXml = new XmlDocument();
            sourceTemplateXml.LoadXml(this.SourceTemplate);

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
            this.UpdateTemplatePropertiesFromApp(newAppDefinition);
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

            ApplicationJob job = new ApplicationJob {DownloadSourceType = SourceType.FixedUrl};
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
            return GetXml(new[] { this }, false, Encoding.UTF8);
        }

        /// <summary>
        /// Returns an XML document containing this application job,
        /// but replaces all global variables with the actual values.
        /// </summary>
        public string GetXmlWithoutGlobalVariables()
        {
            // Replace global variables
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.GetXml());
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
        public static string GetXml(IEnumerable<ApplicationJob> jobs, bool isTemplate, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationJob));
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};

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
                XmlDocument doc = new XmlDocument {PreserveWhitespace = true};
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
                    try
                    {
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
                    catch (XmlException)
                    {
                        // Error loading SourceTemplate
                        continue;
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

                    List<string> options = new List<string>();
                    if (!string.IsNullOrEmpty(element.GetAttribute("options")))
                    {
                        options.AddRange(element.GetAttribute("options").Split('|'));
                    }
                    else
                    {
                        options.AddRange(element.GetElementsByTagName("option").OfType<XmlElement>().Select(optionElem => optionElem.InnerText));
                    }

                    dialog.AddPlaceHolder(name, options.ToArray(), element.GetAttribute("value"), element.GetAttribute("variable"));
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
            
            return null;
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
                        if (!DbManager.ApplicationExists(conn, this.Guid))
                        {
                            if (this.Guid == Guid.Empty) this.Guid = Guid.NewGuid();

                            // Insert stub, update afterwards.
                            using (IDbCommand command = conn.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"INSERT INTO jobs (JobGuid, CanBeShared) VALUES (@JobGuid, @CanBeShared)";
                                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
                                command.Parameters.Add(new SQLiteParameter("@CanBeShared", this.CanBeShared));
                                command.ExecuteNonQuery();
                            }
                        }

                        // Important: Once CanBeShared is set to false,
                        // it can never be true again (ownership does not change)
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "SELECT CanBeShared FROM jobs WHERE JobGuid = @JobGuid";
                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
                            bool canBeShared = Convert.ToBoolean(command.ExecuteScalar());
                            if (!canBeShared)
                            {
                                this.CanBeShared = false;
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
                                                   PreviousRelativeLocation = @PreviousRelativeLocation,
                                                   HashVariable = @HashVariable,
                                                   HashType = @HashType
                                             WHERE JobGuid = @JobGuid";

                            command.Parameters.Add(new SQLiteParameter("@ApplicationName", this.Name));
                            command.Parameters.Add(new SQLiteParameter("@FixedDownloadUrl", this.FixedDownloadUrl));
                            command.Parameters.Add(new SQLiteParameter("@TargetPath", this.TargetPath));
                            command.Parameters.Add(new SQLiteParameter("@LastUpdated", this.m_LastUpdated));
                            command.Parameters.Add(new SQLiteParameter("@IsEnabled", this.Enabled));
                            command.Parameters.Add(new SQLiteParameter("@FileHippoId", this.FileHippoId));
                            command.Parameters.Add(new SQLiteParameter("@DeletePreviousFile", this.DeletePreviousFile));
                            command.Parameters.Add(new SQLiteParameter("@PreviousLocation", this.PreviousLocation));
                            command.Parameters.Add(new SQLiteParameter("@SourceType", this.DownloadSourceType));
                            command.Parameters.Add(new SQLiteParameter("@ExecuteCommand", this.ExecuteCommand));
                            command.Parameters.Add(new SQLiteParameter("@ExecutePreCommand", this.ExecutePreCommand));
                            command.Parameters.Add(new SQLiteParameter("@Category", this.Category));
                            command.Parameters.Add(new SQLiteParameter("@CanBeShared", this.CanBeShared));
                            command.Parameters.Add(new SQLiteParameter("@ShareApplication", this.m_ShareApplication));
                            command.Parameters.Add(new SQLiteParameter("@HttpReferer", this.HttpReferer));
                            command.Parameters.Add(new SQLiteParameter("@FileHippoVersion", this.FileHippoVersion));
                            command.Parameters.Add(new SQLiteParameter("@DownloadBeta", (int) this.DownloadBeta));
                            command.Parameters.Add(new SQLiteParameter("@VariableChangeIndicator", this.m_VariableChangeIndicator));
                            command.Parameters.Add(new SQLiteParameter("@VariableChangeIndicatorLastContent", this.m_VariableChangeIndicatorLastContent));
                            command.Parameters.Add(new SQLiteParameter("@ExclusiveDownload", this.ExclusiveDownload));
                            command.Parameters.Add(new SQLiteParameter("@CheckForUpdateOnly", this.CheckForUpdatesOnly));
                            command.Parameters.Add(new SQLiteParameter("@CachedPadFileVersion", this.CachedPadFileVersion));
                            command.Parameters.Add(new SQLiteParameter("@LastFileDate", this.LastFileDate));
                            command.Parameters.Add(new SQLiteParameter("@LastFileSize", this.LastFileSize));
                            command.Parameters.Add(new SQLiteParameter("@IgnoreFileInformation", this.IgnoreFileInformation));
                            command.Parameters.Add(new SQLiteParameter("@UserNotes", this.UserNotes));
                            command.Parameters.Add(new SQLiteParameter("@WebsiteUrl", this.WebsiteUrl));
                            command.Parameters.Add(new SQLiteParameter("@UserAgent", this.UserAgent));
                            command.Parameters.Add(new SQLiteParameter("@ExecuteCommandType", this.ExecuteCommandType));
                            command.Parameters.Add(new SQLiteParameter("@ExecutePreCommandType", this.ExecutePreCommandType));
                            command.Parameters.Add(new SQLiteParameter("@SourceTemplate", this.SourceTemplate));
                            command.Parameters.Add(new SQLiteParameter("@HashVariable", this.HashVariable));
                            command.Parameters.Add(new SQLiteParameter("@HashType", (int)this.HashType));

                            // In order to find files if the drive letter has changed (portable USB stick), also remember the 
                            // last relative location.
                            if (!string.IsNullOrEmpty(this.PreviousLocation))
                            {
                                try
                                {
                                    Uri uri1 = new Uri(this.PreviousLocation);
                                    Uri uri2 = new Uri(Application.StartupPath + Path.DirectorySeparatorChar);

                                    Uri relativeUri = uri2.MakeRelativeUri(uri1);
                                    string relativePath = PathEx.FixDirectorySeparator(relativeUri.ToString());
                                    // If result returns out to be not relative, no need to save
                                    if (!Path.IsPathRooted(relativePath))
                                    {
                                        this.m_PreviousRelativeLocation = relativePath;
                                    }
                                }
                                catch (Exception)
                                {
                                    // Not critical if path cannot be determined.
                                }
                            }

                            command.Parameters.Add(new SQLiteParameter("@PreviousRelativeLocation", this.m_PreviousRelativeLocation));

                            if (this.DownloadDate.HasValue)
                            {
                                command.Parameters.Add(new SQLiteParameter("@DownloadDate", this.DownloadDate.Value));
                            }
                            else
                            {
                                command.Parameters.Add(new SQLiteParameter("@DownloadDate", DBNull.Value));
                            }

                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));

                            command.ExecuteNonQuery();
                        }

                        Dictionary<string, UrlVariable> variables = this.Variables;

                        // Save variables
                        using (IDbCommand command = conn.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "DELETE FROM variables WHERE JobGuid = @JobGuid";
                            command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
                            command.ExecuteNonQuery();
                        }

                        foreach (KeyValuePair<string, UrlVariable> pair in variables)
                        {
                            pair.Value.Save(transaction, this.Guid);
                        }

                        if (this.setupInstructions != null)
                        {
                            using (IDbCommand command = conn.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = "DELETE FROM setupinstructions WHERE JobGuid = @JobGuid";
                                command.Parameters.Add(new SQLiteParameter("@JobGuid", DbManager.FormatGuid(this.Guid)));
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
            this.m_Name = reader["ApplicationName"] as string;
            this.FixedDownloadUrl = reader["FixedDownloadUrl"] as string;
            this.TargetPath = reader["TargetPath"] as string;
            this.m_LastUpdated = reader["LastUpdated"] as DateTime?;
            this.Enabled = Convert.ToBoolean(reader["IsEnabled"]);
            this.FileHippoId = reader["FileHippoId"] as string;
            this.DeletePreviousFile = Convert.ToBoolean(reader["DeletePreviousFile"]);
            this.PreviousLocation = reader["PreviousLocation"] as string;
            this.DownloadSourceType = (SourceType)Convert.ToByte(reader["SourceType"]);
            this.ExecuteCommand = reader["ExecuteCommand"] as string;
            this.ExecutePreCommand = reader["ExecutePreCommand"] as string;
            this.Category = reader["Category"] as string;
            this.CanBeShared = Convert.ToBoolean(reader["CanBeShared"]);
            this.m_ShareApplication = Convert.ToBoolean(reader["ShareApplication"]);
            this.FileHippoVersion = reader["FileHippoVersion"] as string;
            this.HttpReferer = reader["HttpReferer"] as string;
            this.m_VariableChangeIndicator = reader["VariableChangeIndicator"] as string;
            this.HashVariable = reader["HashVariable"] as string;
            this.HashType = (HashType)Convert.ToByte(reader["HashType"]);
            this.m_VariableChangeIndicatorLastContent = reader["VariableChangeIndicatorLastContent"] as string;
            this.ExclusiveDownload = Convert.ToBoolean(reader["ExclusiveDownload"]);
            this.CheckForUpdatesOnly = Convert.ToBoolean(reader["CheckForUpdateOnly"]);
            this.CachedPadFileVersion = reader["CachedPadFileVersion"] as string;
            this.LastFileSize = Convert.ToInt64(reader["LastFileSize"]);
            this.LastFileDate = reader["LastUpdated"] as DateTime?;
            this.IgnoreFileInformation = Convert.ToBoolean(reader["IgnoreFileInformation"]);
            this.UserNotes = reader["UserNotes"] as string;
            this.WebsiteUrl = reader["WebsiteUrl"] as string;
            this.UserAgent = reader["UserAgent"] as string;
            this.SourceTemplate = reader["SourceTemplate"] as string;
            this.m_PreviousRelativeLocation = reader["PreviousRelativeLocation"] as string;

            string executeCommandType = reader["ExecuteCommandType"] as string;
            if (executeCommandType != null)
            {
                this.ExecuteCommandType = Command.ConvertToScriptType(executeCommandType);
            }

            string executePreCommandType = reader["ExecutePreCommandType"] as string;
            if (executePreCommandType != null)
            {
                this.ExecutePreCommandType = Command.ConvertToScriptType(executePreCommandType);
            }

            if (reader["DownloadBeta"] != DBNull.Value)
            {
                this.DownloadBeta = (DownloadBetaType)Convert.ToInt32(reader["DownloadBeta"]);
            }
            // An application has not been downloaded necessarily
            this.DownloadDate = (reader["DownloadDate"] != DBNull.Value) ? reader["DownloadDate"] as DateTime? : null;
            
            string guid = reader["JobGuid"] as string;
            this.Guid = new Guid(guid);
        }

        public string GetTargetFile(WebResponse netResponse, string alternateFileName)
        {
            string targetLocation = Environment.ExpandEnvironmentVariables(this.TargetPath);

            // Allow variables in target locations as well
            targetLocation = this.Variables.ReplaceAllInString(targetLocation, GetLastModified(netResponse), GetFileNameFromWebResponse(netResponse, alternateFileName), false);

            // If carried on a USB stick, allow using the drive name
            try
            {
                targetLocation = UrlVariable.Replace(targetLocation, "root", Path.GetPathRoot(Application.StartupPath), this);
            }
            catch (ArgumentException) { }

            if (this.TargetIsFolder || Directory.Exists(targetLocation))
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

        private static string GetSha1OfFile(string filename)
        {
            SHA1 hash = new SHA1CryptoServiceProvider();
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] localSha1 = hash.ComputeHash(stream);
                StringBuilder result = new StringBuilder(32);
                for (int i = 0; i < localSha1.Length; i++)
                {
                    result.Append(localSha1[i].ToString("X2"));
                }
                return result.ToString();
            }
        }

        private static string GetSha256OfFile(string filename)
        {
            SHA256 hash = new SHA256CryptoServiceProvider();
            using (FileStream stream = File.OpenRead(filename))
            {
                byte[] localSha256 = hash.ComputeHash(stream);
                StringBuilder result = new StringBuilder(64);
                for (int i = 0; i < localSha256.Length; i++)
                {
                    result.Append(localSha256[i].ToString("X2"));
                }
                return result.ToString();
            }
        }

        private static string GetSha512OfFile(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    byte[] hash = shaM.ComputeHash(stream);

                    StringBuilder result = new StringBuilder(128);
                    for (int i = 0; i < hash.Length; i++)
                    {
                        result.Append(hash[i].ToString("X2"));
                    }
                    return result.ToString();
                }
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

                if (!current.Exists && !string.IsNullOrEmpty(this.CurrentLocation) && this.DeletePreviousFile)
                {
                    // The file does not exist at the target location.
                    // Check if the previously downloaded file still matches.
                    current = new FileInfo(this.CurrentLocation);

                    if (current.Exists)
                    {
                        LogDialog.Log(this, $"Target file missing, comparing to previously downloaded file at {this.CurrentLocation}");
                    }
                }

                if (!this.IgnoreFileInformation && !current.Exists)
                {
                    LogDialog.Log(this, string.Format("Update required, '{0}' does not yet exist", targetFile));
                    return true;
                }
            }

            // Check a variable's contents?
            if (!string.IsNullOrEmpty(this.m_VariableChangeIndicator))
            {
                string varName = this.m_VariableChangeIndicator.Trim('{', '}');
                string content = this.Variables.ReplaceAllInString("{" + varName + "}");
                // Only return a result, if the variable has already been checked before,
                // that is, if there is an actual difference.
                if (this.m_VariableChangeIndicatorLastContent != null)
                {
                    bool update = (content != this.m_VariableChangeIndicatorLastContent);
                    if (update)
                    {
                        LogDialog.Log(this, string.Format("Update required, {0} has changed from '{1}' to '{2}'", "{" + varName + "}", this.m_VariableChangeIndicatorLastContent, content));
                    }
                    else
                    {
                        LogDialog.Log(this, string.Format("Update not required, {0} has not changed", "{" + varName + "}"));
                    }

                    this.m_VariableChangeIndicatorLastContent = content;
                    if (update) this.Save();
                    return update;
                }
                else
                {
                    LogDialog.Log(this, string.Format("No previous value for {0} available, ignoring this variable as indicator for changes", "{" + varName + "}"));
                    this.m_VariableChangeIndicatorLastContent = content;
                    this.Save();
                }
            }

            // Check hash value?
            if (!string.IsNullOrEmpty(this.HashVariable))
            {
                string varName = this.HashVariable.Trim('{', '}');
                string content = this.Variables.ReplaceAllInString("{" + varName + "}").Trim();
                
                // Compare online hash with actual current hash.
                if (!string.IsNullOrEmpty(content))
                {
                    if (string.IsNullOrEmpty(targetFile))
                    {
                        LogDialog.Log(this, "Unknown target file location, cannot compare hash value.");
                    }
                    else
                    {
                        string currentHash = this.GetFileHash(targetFile);
                        bool update = string.Compare(content, currentHash, StringComparison.OrdinalIgnoreCase) != 0;
                        if (update)
                        {
                            LogDialog.Log(this, string.Format("Update required, hash in {0} has changed from '{1}' to '{2}'", "{" + varName + "}", currentHash, content));
                        }
                        else
                        {
                            LogDialog.Log(this, string.Format("Update not required, hash in {0} has not changed", "{" + varName + "}"));
                        }

                        if (update) this.Save();
                        return update;
                    }
                }
                else
                {
                    LogDialog.Log(this, string.Format("Value of hash variable {0} cannot be determined, ignoring hash as indicator for changes", "{" + varName + "}"));
                }
            }

            // If using FileHippo, and previous file is available, check MD5
            if (!string.IsNullOrEmpty(this.FileHippoId) && this.DownloadSourceType == SourceType.FileHippo && this.FileExists)
            {
                string serverMd5 = ExternalServices.FileHippoMd5(this.FileHippoId, this.AvoidDownloadBeta);
                // It may happen, that the MD5 is not calculated
                if (serverMd5 != null)
                {
                    bool md5Result = string.Compare(serverMd5, GetMd5OfFile(this.CurrentLocation), true) != 0;
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

            if (this.IgnoreFileInformation)
            {
                if (this.LastFileDate.HasValue)
                {
                    TimeSpan diff = GetLastModified(netResponse) - this.LastFileDate.Value;
                    dateMismatch = (!disregardDate && diff > TimeSpan.Zero);
                }
                if (this.LastFileSize > 0)
                {
                    fileSizeMismatch = (this.LastFileSize != netResponse.ContentLength && netResponse.ContentLength >= 0);
                }
            }
            else
            {
                try
                {
                    fileSizeMismatch = (current.Length != netResponse.ContentLength && netResponse.ContentLength >= 0);

                    TimeSpan diff = GetLastModified(netResponse) - current.LastWriteTime;
                    dateMismatch = (!disregardDate && diff > TimeSpan.Zero);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Potential exception in LastWriteTime.
                    dateMismatch = true;
                }
            }

            bool result = (fileSizeMismatch || dateMismatch);
            LogDialog.Log(this, fileSizeMismatch, dateMismatch);

            return result;
        }

        /// <summary>
        /// Determines the hash value of a certain file.
        /// </summary>
        internal string GetFileHash(string targetFile)
        {
            switch (this.HashType)
            {
                case HashType.Md5:
                    return GetMd5OfFile(targetFile);

                case HashType.Sha1:
                    return GetSha1OfFile(targetFile);

                case HashType.Sha256:
                    return GetSha256OfFile(targetFile);

                case HashType.Sha512:
                    return GetSha512OfFile(targetFile);

                case HashType.Crc:
                    return CrcStream.GetCrcFromFile(targetFile).ToString("X2");
            }

            return string.Empty;
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
            return this.Name;
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

            StringBuilder fulltext = new StringBuilder(this.Name);
            fulltext.Append(this.Category);
            fulltext.Append(this.TargetPath);

            foreach (KeyValuePair<string, string> column in customColumns)
            {
                string value = this.Variables.ReplaceAllInString(column.Value, DateTime.MinValue, null, true);
                fulltext.Append(value);
            }

            string fulltextToLower = fulltext.ToString().ToLower();

            // Boolean search: All subjects must be matched
            return subjects.All(subject => this.MatchesSubject(subject, fulltextToLower));
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

        /// <summary>
        /// Executes the default post-update command and the application
        /// specific post-update command (if present).
        /// </summary>
        public void ExecutePostUpdateCommands()
        {
            // Execute a default command?
            string defaultCommand = Settings.GetValue("DefaultCommand") as string;
            ScriptType defaultCommandType = Command.ConvertToScriptType(Settings.GetValue("DefaultCommandType") as string);
            new Command(defaultCommand, defaultCommandType).Execute(this, this.PreviousLocation);

            // Do we need to execute a command after downloading?
            if (!string.IsNullOrEmpty(this.ExecuteCommand))
            {
                new Command(this.ExecuteCommand, this.ExecuteCommandType).Execute(this, this.PreviousLocation);
            }
        }
    }
}
