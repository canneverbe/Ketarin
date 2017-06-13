using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using CDBurnerXP;
using CDBurnerXP.IO;
using CookComputing.XmlRpc;
using FTPLib;
using Ketarin.Forms;
using MyDownloader.Core;
using MyDownloader.Extension.Protocols;
using Settings = CDBurnerXP.Settings;

namespace Ketarin
{
    /// <summary>
    /// Handles the updating process of a list of
    /// application jobs.
    /// </summary>
    public class Updater
    {
        public const SecurityProtocolType DefaultHttpProtocols = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        private ApplicationJob[] m_Jobs;
        private Dictionary<ApplicationJob, short> m_Progress;
        private readonly Dictionary<ApplicationJob, Status> m_Status = new Dictionary<ApplicationJob,Status>();
        private readonly Dictionary<ApplicationJob, long> m_Size = new Dictionary<ApplicationJob, long>();
        private bool m_CancelUpdates;
        private bool m_IsBusy;
        protected int m_LastProgress = -1;
        private List<ApplicationJobError> m_Errors;
        private byte m_NoProgressCounter;
        private bool m_OnlyCheck;
        private int m_ThreadLimit = 2;
        private readonly List<Thread> m_Threads = new List<Thread>();
        private readonly CookieContainer m_Cookies = new CookieContainer();
        private static readonly List<WebRequest> m_Requests = new List<WebRequest>();
        private bool m_InstallUpdated;

        #region Properties

        /// <summary>
        /// Forces all applications, even if no updates exist, to download.
        /// Also ignores the CheckForUpdatesOnly property of applications.
        /// </summary>
        public bool ForceDownload
        {
            get;
            set;
        }

        /// <summary>
        /// Ignores the CheckForUpdatesOnly property of applications (for setups).
        /// </summary>
        public bool IgnoreCheckForUpdatesOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of errors which happened after an update process.
        /// </summary>
        public ApplicationJobError[] Errors
        {
            get
            {
                return m_Errors.ToArray();
            }
        }

        /// <summary>
        /// Gets whether or not the updating process is still ongoing.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return m_IsBusy;
            }
        }

        #endregion

        /// <summary>
        /// Represents a download status of an application.
        /// </summary>
        public enum Status
        {
            Idle,
            Downloading,
            UpdateAvailable,
            UpdateSuccessful,
            NoUpdate,
            Failure,
        }

        #region JobProgressChangedEventArgs

        /// <summary>
        /// Holds all necessary information for the event that
        /// the download progress of an application changed.
        /// </summary>
        public class JobProgressChangedEventArgs : ProgressChangedEventArgs
        {
            private readonly ApplicationJob m_Job;

            #region Properties

            /// <summary>
            /// Gets the application for which the download progress changed.
            /// </summary>
            public ApplicationJob ApplicationJob
            {
                get
                {
                    return m_Job;
                }
            }

            #endregion

            public JobProgressChangedEventArgs(int progressPercentage, ApplicationJob job)
                : base(progressPercentage, null)
            {
                m_Job = job;
            }
        }

        #endregion

        #region JobStatusChangedEventArgs

        /// <summary>
        /// Holds all necessary information for the event that
        /// the update status of an application changed.
        /// </summary>
        public class JobStatusChangedEventArgs : EventArgs
        {
            private readonly ApplicationJob m_Job;
            private readonly Status m_NewStatus;

            #region Properties

            /// <summary>
            /// Gets the application of which the status has changed.
            /// </summary>
            public ApplicationJob ApplicationJob
            {
                get
                {
                    return m_Job;
                }
            }

            /// <summary>
            /// Gets the new status of the application.
            /// </summary>
            public Status NewStatus
            {
                get
                {
                    return m_NewStatus;
                }
            }

            #endregion

            public JobStatusChangedEventArgs(ApplicationJob job, Status newStatus)
            {
                m_Job = job;
                m_NewStatus = newStatus;
            }
        }

        #endregion

        /// <summary>
        /// Occurs when the download progress of an application changed.
        /// </summary>
        public event EventHandler<JobProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Occurs when the upgrade status of an application changed.
        /// </summary>
        public event EventHandler<JobStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Occurs when the updater has finished the whole upgrade process.
        /// </summary>
        public event EventHandler UpdateCompleted;

        /// <summary>
        /// Occurs when updates for applications downloaded from the online database
        /// have been found and provides a list of XML definitions for those applications.
        /// </summary>
        public event EventHandler<GenericEventArgs<string[]>> UpdatesFound;

        #region Public control methods

        /// <summary>
        /// Allows all routines involved in the update to
        /// store the corresponding WebRequest here. When the user
        /// cancels the process, these WebRequests wil be aborted,
        /// so that it finishes more or less instantly.
        /// </summary>
        internal static void AddRequestToCancel(WebRequest reqest)
        {
            lock (m_Requests)
            {
                m_Requests.Add(reqest);
            }
        }

        /// <summary>
        /// Cancels the updating progress.
        /// </summary>
        public void Cancel()
        {
            m_CancelUpdates = true;
            lock (m_Requests)
            {
                foreach (WebRequest req in m_Requests)
                {
                    try
                    {
                        req.Abort();
                    }
                    catch (NotSupportedException)
                    {
                        continue;
                    }
                }
                m_Requests.Clear();
            }
        }

        /// <summary>
        /// Returns the download size of a given application in bytes.
        /// </summary>
        /// <returns>-1 if the size cannot be determined, -2 if no file size has been determined yet</returns>
        public long GetDownloadSize(ApplicationJob job)
        {
            if (m_Size == null || !m_Size.ContainsKey(job)) return -1;

            return m_Size[job];
        }

        /// <summary>
        /// Returns the progress of the given application.
        /// </summary>
        /// <returns>-1 for no progress yet, otherwise 0 to 100</returns>
        public short GetProgress(ApplicationJob job)
        {
            if (m_Progress == null || !m_Progress.ContainsKey(job)) return -1;

            return m_Progress[job];
        }

        /// <summary>
        /// Returns the current status of a given application.
        /// </summary>
        /// <returns>Idle by default</returns>
        public Status GetStatus(ApplicationJob job)
        {
            if (m_Status == null || !m_Status.ContainsKey(job)) return Status.Idle;

            return m_Status[job];
        }

        /// <summary>
        /// Starts one or more threads which update the given
        /// applications asynchronously.
        /// </summary>
        /// <param name="onlyCheck">Specifies whether or not to download the updates</param>
        public void BeginUpdate(ApplicationJob[] jobs, bool onlyCheck, bool installUpdated)
        {
            m_IsBusy = true;
            m_Jobs = jobs;
            m_ThreadLimit = Convert.ToInt32(Settings.GetValue("ThreadCount", 2));
            m_OnlyCheck = onlyCheck;
            m_InstallUpdated = installUpdated;
            m_Requests.Clear();

            // Initialise progress and status
            m_Progress = new Dictionary<ApplicationJob, short>();

            foreach (ApplicationJob job in m_Jobs)
            {
                m_Progress[job] = (short)((ForceDownload || job.Enabled) ? 0 : -1);
                m_Status[job] = Status.Idle;
                m_Size[job] = -2;
            }
            m_Threads.Clear();

            Thread thread = new Thread(UpdateApplications);
            thread.Start();
        }

        /// <summary>
        /// Checks for which of the given applications updates
        /// are available asynchronously.
        /// </summary>
        public void BeginCheckForOnlineUpdates(ApplicationJob[] jobs)
        {
            DateTime lastUpdate = (DateTime)Settings.GetValue("LastUpdateCheck", DateTime.MinValue);
            if (lastUpdate.Date == DateTime.Now.Date)
            {
                // Only check once a day
                return;
            }

            Settings.SetValue("LastUpdateCheck", DateTime.Now);
            Thread thread = new Thread(this.CheckForOnlineUpdates) {IsBackground = true};
            thread.Start(jobs);
        }

        /// <summary>
        /// Checks for which of the given applications updates
        /// are available. Fires an event when finished.
        /// </summary>
        private void CheckForOnlineUpdates(object argument)
        {
            ApplicationJob[] jobs = argument as ApplicationJob[];

            // Build an array containing all GUIDs and dates
            List<RpcAppGuidAndDate> sendInfo = new List<RpcAppGuidAndDate>();
            foreach (ApplicationJob job in jobs.Where(job => !job.CanBeShared))
            {
                sendInfo.Add(new RpcAppGuidAndDate(job.Guid, job.DownloadDate));
            }

            if (sendInfo.Count == 0)
            {
                // Nothing to do
                return;
            }

            try
            {
                IKetarinRpc proxy = XmlRpcProxyGen.Create<IKetarinRpc>();
                string[] updatedApps = proxy.GetUpdatedApplications(sendInfo.ToArray());
                OnUpdatesFound(updatedApps);
            }
            catch (Exception ex)
            {
                // If updating fails, it does not hurt and should not annoy anyone.
                // Just write a log entry, just in case
                LogDialog.Log("Failed checking for online database updates", ex);
            }
        }

        #endregion

        /// <summary>
        /// Performs the actual update check for the current applications.
        /// Starts multiple threads if necessary.
        /// </summary>
        private void UpdateApplications()
        {
            m_CancelUpdates = false;
            m_Errors = new List<ApplicationJobError>();
            LogDialog.Log(string.Format("Update started with {0} application(s)", m_Jobs.Length));

            try
            {
                ApplicationJob previousJob = null;

                foreach (ApplicationJob job in m_Jobs)
                {
                    // Skip if disabled
                    if (!job.Enabled && m_Jobs.Length > 1) continue;

                    // Wait until we can start a new thread:
                    // - Thread limit is not reached
                    // - The next application is not to be downloaded exclusively
                    // - The application previously started is not to be downloaded exclusively
                    // - Setup is taking place
                    while (m_Threads.Count >= m_ThreadLimit || (m_Threads.Count > 0 && (m_InstallUpdated || job.ExclusiveDownload || (previousJob != null && previousJob.ExclusiveDownload))))
                    {
                        Thread.Sleep(200);

                        foreach (Thread activeThread in m_Threads)
                        {
                            if (!activeThread.IsAlive)
                            {
                                m_Threads.Remove(activeThread);
                                break;
                            }
                        }
                    }

                    // Stop if cancelled
                    if (m_CancelUpdates) break;

                    Thread newThread = new Thread(this.StartNewThread);
                    previousJob = job;
                    newThread.Start(job);
                    m_Threads.Add(newThread);
                }

                // Now, wait until all threads have finished
                while (m_Threads.Count > 0)
                {
                    Thread.Sleep(200);

                    foreach (Thread activeThread in m_Threads)
                    {
                        if (!activeThread.IsAlive)
                        {
                            m_Threads.Remove(activeThread);
                            break;
                        }
                    }
                }

                try
                {
                    string postUpdateCommand = Settings.GetValue("PostUpdateCommand", "") as string;
                    ScriptType postUpdateCommandType = Command.ConvertToScriptType(Settings.GetValue("PostUpdateCommandType", ScriptType.Batch.ToString()) as string);
                    new Command(postUpdateCommand, postUpdateCommandType).Execute(null);
                }
                catch (ApplicationException ex)
                {
                    LogDialog.Log("Post update command failed.", ex);
                }

                LogDialog.Log("Update finished");
            }
            finally
            {
                m_IsBusy = false;
                m_Progress.Clear();
                m_Size.Clear();
                OnUpdateCompleted();
            }
        }

        /// <summary>
        /// Performs the update process of a single application.
        /// Catches most exceptions and stores them for later use.
        /// </summary>
        private void StartNewThread(object paramJob)
        {
            ApplicationJob job = paramJob as ApplicationJob;

            m_Status[job] = Status.Downloading;
            OnStatusChanged(job);

            string requestedUrl = string.Empty;
            int numTries = 0;
            int maxTries = Convert.ToInt32(Settings.GetValue("RetryCount", 1));

            try
            {
                while (numTries < maxTries)
                {
                    try
                    {
                        numTries++;
                        m_Status[job] = DoDownload(job, out requestedUrl);

                        // If there is a custom column variable, and it has not been been downloaded yet,
                        // make sure that we fetch it now "unnecessarily" so that the column contains a current value.
                        Dictionary<string, string> customColumns = SettingsDialog.CustomColumns;
                        foreach (KeyValuePair<string, string> column in customColumns)
                        {
                            if (!string.IsNullOrEmpty(column.Value) && !job.Variables.HasVariableBeenDownloaded(column.Value))
                            {
                                job.Variables.ReplaceAllInString("{" + column.Value.TrimStart('{').TrimEnd('}') + "}");
                            }
                        }
                        if (customColumns.Count > 0)
                        {
                            job.Save(); // cached variable content
                        }

                        // Install if updated
                        if (m_InstallUpdated && m_Status[job] == Status.UpdateSuccessful)
                        {
                            job.Install(null);
                        }

                        // If no exception happened, we immediately leave the loop
                        break;
                    }
                    catch (SQLiteException ex)
                    {
                        // If "locked" exception (slow USB device eg.) continue trying
                        if (ex.ErrorCode == SQLiteErrorCode.Locked)
                        {
                            numTries--;
                            LogDialog.Log(job, ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        WebException webException = ex as WebException;
                        if (webException != null && webException.Status == WebExceptionStatus.RequestCanceled)
                        {
                            // User cancelled the process -> Do nothing
                            m_Status[job] = Status.Failure;
                            break;
                        }

                        // Only throw an exception if we have run out of tries
                        if (numTries == maxTries)
                        {
                            throw;
                        }
                        else
                        {
                            LogDialog.Log(job, ex);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex, (ex.Response != null) ? ex.Response.ResponseUri.ToString() : requestedUrl));
                m_Status[job] = Status.Failure;
            }
            catch (FileNotFoundException ex)
            {
                // Executing command failed
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
            }
            catch (Win32Exception ex)
            {
                // Executing command failed
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
            }
            catch (IOException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
                m_Status[job] = Status.Failure;
            }
            catch (UnauthorizedAccessException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
                m_Status[job] = Status.Failure;
            }
            catch (UriFormatException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex, requestedUrl));
                m_Status[job] = Status.Failure;
            }
            catch (NotSupportedException ex)
            {
                // Invalid URI prefix
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex, requestedUrl));
                m_Status[job] = Status.Failure;
            }
            catch (NonBinaryFileException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex, requestedUrl));
                m_Status[job] = Status.Failure;
            }
            catch (TargetPathInvalidException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
                m_Status[job] = Status.Failure;
            }
            catch (CommandErrorException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
                m_Status[job] = Status.Failure;
            }
            catch (ApplicationException ex)
            {
                // Error executing custom C# script
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
            }
            catch (SQLiteException ex)
            {
                LogDialog.Log(job, ex);
                m_Errors.Add(new ApplicationJobError(job, ex));
                m_Status[job] = Status.Failure;
            }

            m_Progress[job] = 100;
            OnStatusChanged(job);
        }

        /// <summary>
        /// Executes the actual download (determines the URL to download from). Does not handle exceptions,
        /// but takes care of proper cleanup.
        /// </summary>
        /// <param name="job">The job to process</param>
        /// <param name="requestedUrl">The URL from which has been downloaded</param>
        /// <returns>true, if a new update has been found and downloaded, false otherwise</returns>
        protected Status DoDownload(ApplicationJob job, out string requestedUrl)
        {
            string downloadUrl;
            if (job.DownloadSourceType == ApplicationJob.SourceType.FileHippo)
            {
                downloadUrl = ExternalServices.FileHippoDownloadUrl(job.FileHippoId, job.AvoidDownloadBeta);
            }
            else
            {
                downloadUrl = job.FixedDownloadUrl;
                // Now replace variables
                downloadUrl = job.Variables.ReplaceAllInString(downloadUrl);
            }

            requestedUrl = downloadUrl;

            if (string.IsNullOrEmpty(downloadUrl))
            {
                // No download URL specified, only check if update is required
                if (job.RequiresDownload(null, null))
                {
                    return Status.UpdateAvailable;
                }
                
                return Status.NoUpdate;
            }

            Uri url = new Uri(downloadUrl);

            return this.DoDownload(job, url);
        }

        /// <summary>
        /// Executes the actual download from an URL. Does not handle exceptions,
        /// but takes care of proper cleanup.
        /// </summary>
        /// <exception cref="NonBinaryFileException">This exception is thrown, if the resulting file is not of a binary type</exception>
        /// <exception cref="TargetPathInvalidException">This exception is thrown, if the resulting target path of an application is not valid</exception>
        /// <param name="job">The job to process</param>
        /// <param name="urlToRequest">URL from which should be downloaded</param>
        /// <returns>true, if a new update has been found and downloaded, false otherwise</returns>
        protected Status DoDownload(ApplicationJob job, Uri urlToRequest)
        {
            // Lower security policies
            try
            {
                ServicePointManager.CheckCertificateRevocationList = false;
            }
            catch (PlatformNotSupportedException)
            {
                // .NET bug under special circumstances
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate {
                return true;
            };

            ServicePointManager.SecurityProtocol = DefaultHttpProtocols;

            // If we want to download multiple files simultaneously
            // from the same server, we need to "remove" the connection limit.
            ServicePointManager.DefaultConnectionLimit = 50;

            // Determine number of segments to create
            int segmentCount = Convert.ToInt32(Settings.GetValue("SegmentCount", 1));

            job.Variables.ResetDownloadCount();

            WebRequest req = KetarinProtocolProvider.CreateRequest(urlToRequest, job, this.m_Cookies);
            AddRequestToCancel(req);

            using (WebResponse response = WebClient.GetResponse(req))
            {
                LogDialog.Log(job, "Server source file: " + req.RequestUri.AbsolutePath);

                // Occasionally, websites are not available and an error page is encountered
                // For the case that the content type is just plain wrong, ignore it if the size is higher than 500KB
                HttpWebResponse httpResponse = response as HttpWebResponse;
                if (httpResponse != null && response.ContentLength < 500000)
                {
                    if (response.ContentType.StartsWith("text/xml") || response.ContentType.StartsWith("application/xml"))
                    {
                        // If an XML file is served, maybe we have a PAD file
                        ApplicationJob padJob = ApplicationJob.ImportFromPad(httpResponse);
                        if (padJob != null)
                        {
                            job.CachedPadFileVersion = padJob.CachedPadFileVersion;
                            return this.DoDownload(job, new Uri(padJob.FixedDownloadUrl));
                        }
                    }
                    if (response.ContentType.StartsWith("text/html"))
                    {
                        throw NonBinaryFileException.Create(response.ContentType, httpResponse.StatusCode);
                    }
                }

                long fileSize = GetContentLength(response);
                if (fileSize == 0)
                {
                    throw new IOException("Source file on server is empty (ContentLength = 0).");
                }

                string targetFileName = job.GetTargetFile(response, urlToRequest.AbsoluteUri);

                LogDialog.Log(job, "Determined target file name: " + targetFileName);

                // Only download, if the file size or date has changed
                if (!ForceDownload && !job.RequiresDownload(response, targetFileName))
                {
                    // If file already exists (created by user),
                    // the download is not necessary. We still need to
                    // set the file name.
                    // If the file exists, but not at the target location
                    // (after renaming), do not reset the previous location.
                    if (File.Exists(targetFileName))
                    {
                        job.PreviousLocation = targetFileName;
                    }
                    job.Save();
                    return Status.NoUpdate;
                }

                // Skip downloading!
                // Installing also requires a forced download
                if (!ForceDownload && !m_InstallUpdated && (m_OnlyCheck || (job.CheckForUpdatesOnly && !IgnoreCheckForUpdatesOnly)))
                {
                    LogDialog.Log(job, "Skipped downloading updates");
                    return Status.UpdateAvailable;
                }

                // Execute: Default pre-update command
                string defaultPreCommand = Settings.GetValue("PreUpdateCommand", "") as string;
                // For starting external download managers: {preupdate-url}
                defaultPreCommand = UrlVariable.Replace(defaultPreCommand, "preupdate-url", urlToRequest.ToString(), job);
                ScriptType defaultPreCommandType = Command.ConvertToScriptType(Settings.GetValue("PreUpdateCommandType", ScriptType.Batch.ToString()) as string);

                int exitCode = new Command(defaultPreCommand, defaultPreCommandType).Execute(job, targetFileName);
                if (exitCode == 1)
                {
                    LogDialog.Log(job, "Default pre-update command returned '1', download aborted");
                    throw new CommandErrorException();
                }
                else if (exitCode == 2)
                {
                    LogDialog.Log(job, "Default pre-update command returned '2', download skipped");
                    return Status.UpdateAvailable;
                }

                // Execute: Application pre-update command
                exitCode = new Command(UrlVariable.Replace(job.ExecutePreCommand, "preupdate-url", urlToRequest.ToString(), job), job.ExecutePreCommandType).Execute(job, targetFileName);
                if (exitCode == 1)
                {
                    LogDialog.Log(job, "Pre-update command returned '1', download aborted");
                    throw new CommandErrorException();
                }
                else if (exitCode == 2)
                {
                    LogDialog.Log(job, "Pre-update command returned '2', download skipped");
                    return Status.UpdateAvailable;
                }
                else if (exitCode == 3)
                {
                    LogDialog.Log(job, "Pre-update command returned '3', external download");
                    job.LastUpdated = DateTime.Now;
                    job.Save();
                    return Status.UpdateSuccessful;
                }

                // Read all file contents to a temporary location
                string tmpLocation = Path.GetTempFileName();
                DateTime lastWriteTime = ApplicationJob.GetLastModified(response);

                // Only use segmented downloader with more than one segment.
                if (segmentCount > 1)
                {
                    // Response can be closed now, new one will be created.
                    response.Dispose();

                    m_Size[job] = fileSize;

                    Downloader d = new Downloader(new ResourceLocation { Url = urlToRequest.AbsoluteUri, ProtocolProvider = new KetarinProtocolProvider(job, m_Cookies) }, null, tmpLocation, segmentCount);
                    d.Start();
                    
                    while (d.State < DownloaderState.Ended)
                    {
                        if (m_CancelUpdates)
                        {
                            d.Pause();
                            break;
                        }

                        this.OnProgressChanged(d.Segments.Sum(x => x.Transfered), fileSize, job);
                        Thread.Sleep(250);
                    }

                    if (d.State == DownloaderState.EndedWithError)
                    {
                        throw d.LastError;
                    }
                }
                else
                {
                    // Read contents from the web and put into file
                    using (Stream sourceFile = response.GetResponseStream())
                    {
                        using (FileStream targetFile = File.Create(tmpLocation))
                        {
                            long byteCount = 0;
                            int readBytes;
                            m_Size[job] = fileSize;
                            
                            // Only create buffer once and re-use.
                            const int bufferSize = 1024 * 1024;
                            byte[] buffer = new byte[bufferSize];

                            do
                            {
                                if (m_CancelUpdates) break;

                                // Some adjustment for SCP download: Read only up to the max known bytes
                                int maxRead = (fileSize > 0) ? (int) Math.Min(fileSize - byteCount, bufferSize) : bufferSize;
                                if (maxRead == 0) break;

                                readBytes = sourceFile.Read(buffer, 0, maxRead);
                                if (readBytes > 0) targetFile.Write(buffer, 0, readBytes);
                                byteCount += readBytes;

                                this.OnProgressChanged(byteCount, fileSize, job);

                            } while (readBytes > 0);
                        }
                    }
                }

                if (m_CancelUpdates)
                {
                    m_Progress[job] = 0;
                    OnStatusChanged(job);
                    return Status.Failure;
                }

                // If each version has a different file name (version number),
                // we might only want to keep one of them. Also, we might
                // want to free some space on the target location.
                if (job.DeletePreviousFile)
                {
                    PathEx.TryDeleteFiles(job.PreviousLocation);
                }

                try
                {
                    File.SetLastWriteTime(tmpLocation, lastWriteTime);
                }
                catch (ArgumentException)
                {
                    // Invalid file date. Ignore and just use DateTime.Now
                }

                // File downloaded. Now let's check if the hash value is valid or abort otherwise!
                if (!string.IsNullOrEmpty(job.HashVariable) && job.HashType != HashType.None)
                {
                    string varName = job.HashVariable.Trim('{', '}');
                    string expectedHash = job.Variables.ReplaceAllInString("{" + varName + "}").Trim();

                    // Compare online hash with actual current hash.
                    if (!string.IsNullOrEmpty(expectedHash))
                    {
                        string currentHash = job.GetFileHash(tmpLocation);
                        if (string.Compare(expectedHash, currentHash, StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            LogDialog.Log(job, string.Format("File downloaded, but hash of downloaded file {0} does not match the expected hash {1}.", currentHash, expectedHash));
                            File.Delete(tmpLocation);
                            throw new IOException("Hash verification failed.");
                        }
                    }
                }

                try
                {
                    FileInfo downloadedFileInfo = new FileInfo(tmpLocation);
                    job.LastFileSize = downloadedFileInfo.Length;
                    job.LastFileDate = downloadedFileInfo.LastWriteTime;
                }
                catch (Exception ex)
                {
                    LogDialog.Log(job, ex);
                }

                try
                {
                    // Before copying, we might have to create the directory
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFileName));

                    // Copying might fail if variables have been replaced with bad values.
                    // However, we cannot rely on functions to clean up the path, since they
                    // might actually parse the path incorrectly and return an even worse path.
                    File.Copy(tmpLocation, targetFileName, true);
                }
                catch (ArgumentException)
                {
                    throw new TargetPathInvalidException(targetFileName);
                }
                catch (NotSupportedException)
                {
                    throw new TargetPathInvalidException(targetFileName);
                }

                File.Delete(tmpLocation);

                // At this point, the update is complete
                job.LastUpdated = DateTime.Now;
                job.PreviousLocation = targetFileName;
            }

            job.Save();

            job.ExecutePostUpdateCommands();

            return Status.UpdateSuccessful;
        }

        /// <summary>
        /// Determines the actual content length in a more reliable
        /// way for FTP downloads.
        /// </summary>
        /// <returns>-1 if no size could be determined</returns>
        private static long GetContentLength(WebResponse response)
        {
            HttpWebResponse http = response as HttpWebResponse;
            if (http != null)
            {
                return http.ContentLength;
            }

            FtpWebResponse ftp = response as FtpWebResponse;
            if (ftp != null)
            {
                if (ftp.ContentLength > 0)
                {
                    return ftp.ContentLength;
                }
                else
                {
                    // There is a problem with the .NET FTP implementation:
                    // "TYPE I" is never sent unless a file is requested, but is sometimes
                    // required by FTP servers to get the file size (otherwise error 550).
                    // Thus, we use a custom FTP library from code project for this task.
                    FTP ftpConnection = null;
                    try
                    {
                        ftpConnection = new FTP(response.ResponseUri.Host, "anonymous", "ketarin@canneverbe.com");
                        return ftpConnection.GetFileSize(response.ResponseUri.LocalPath);
                    }
                    catch (Exception)
                    {
                        // Limited trust in this code...
                        return -1;
                    }
                    finally
                    {
                        if (ftpConnection != null) ftpConnection.Disconnect();
                    }
                }
            }

            ScpWebResponse scp = response as ScpWebResponse;
            if (scp != null)
            {
                return scp.ContentLength;
            }

            return -1;
        }

        /// <summary>
        /// Fires the UpdateCompleted event.
        /// </summary>
        protected virtual void OnUpdateCompleted()
        {
            if (UpdateCompleted != null)
            {
                UpdateCompleted(this, null);
            }
        }

        /// <summary>
        /// Fires the StatusChanged event.
        /// </summary>
        protected virtual void OnStatusChanged(ApplicationJob job)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, new JobStatusChangedEventArgs(job, GetStatus(job)));
            }
        }

        /// <summary>
        /// Fires the UpdatesFound.
        /// </summary>
        protected virtual void OnUpdatesFound(string[] updatedApps)
        {
            if (UpdatesFound != null && updatedApps.Length > 0)
            {
                UpdatesFound(this, new GenericEventArgs<string[]>(updatedApps));
            }
        }

        /// <summary>
        /// Fires the ProgressChangedEvent. Only fires if the progress value
        /// has changed significantly to prevent (for example) too frequent GUI updates.
        /// </summary>
        /// <param name="pos">Current position of the stream</param>
        /// <param name="length">Total length of the stream</param>
        /// <param name="job">Current ApplicationJob</param>
        protected virtual void OnProgressChanged(long pos, long length, ApplicationJob job)
        {
            if (length == -1)
            {
                // Cannot report progress if no info given
                if (this.ProgressChanged != null)
                {
                    if (m_NoProgressCounter > 100) m_NoProgressCounter = 0;
                    m_Progress[job] = m_NoProgressCounter;
                    this.ProgressChanged(this, new JobProgressChangedEventArgs(m_NoProgressCounter++, job));
                }
                return;
            }

            double progress = (double)pos / length * 100.0;
            byte progressInt = Convert.ToByte(Math.Round(progress));

            if (progressInt != m_LastProgress)
            {
                if (this.ProgressChanged != null)
                {
                    this.ProgressChanged(this, new JobProgressChangedEventArgs(progressInt, job));
                }

                m_Progress[job] = progressInt;
                m_LastProgress = progressInt;
            }
        }
    }
}