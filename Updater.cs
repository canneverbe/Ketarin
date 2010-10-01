using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.ComponentModel;
using CDBurnerXP.IO;
using System.Diagnostics;
using System.Windows.Forms;
using CDBurnerXP;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Ketarin.Forms;
using CookComputing.XmlRpc;

namespace Ketarin
{
    /// <summary>
    /// Handles the updating process of a list of
    /// application jobs.
    /// </summary>
    public class Updater
    {
        private ApplicationJob[] m_Jobs = null;
        private Dictionary<ApplicationJob, short> m_Progress = null;
        private Dictionary<ApplicationJob, Status> m_Status = new Dictionary<ApplicationJob,Status>();
        private Dictionary<ApplicationJob, long> m_Size = new Dictionary<ApplicationJob, long>();
        private bool m_CancelUpdates = false;
        private bool m_IsBusy = false;
        protected int m_LastProgress = -1;
        private List<ApplicationJobError> m_Errors;
        private byte m_NoProgressCounter = 0;
        private bool m_OnlyCheck = false;
        private bool m_ForceDownload = false;
        private int m_ThreadLimit = 2;
        private List<Thread> m_Threads = new List<Thread>();
        private List<string> m_NoAutoReferer = new List<string>(new string[] { "sourceforge.net" });
        private CookieContainer m_Cookies = new CookieContainer();
        private static List<WebRequest> m_Requests = new List<WebRequest>();
        private bool m_InstallUpdated = false;

        #region Properties

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
            private ApplicationJob m_Job = null;

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
            private ApplicationJob m_Job = null;
            private Status m_NewStatus = Status.Idle;

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
        public void BeginUpdate(ApplicationJob[] jobs, bool onlyCheck, bool forceDownload, bool installUpdated)
        {
            m_IsBusy = true;
            m_Jobs = jobs;
            m_ThreadLimit = Convert.ToInt32(Settings.GetValue("ThreadCount", 2));
            m_OnlyCheck = onlyCheck;
            m_ForceDownload = forceDownload;
            m_InstallUpdated = installUpdated;
            m_Requests.Clear();

            // Initialise progress and status
            m_Progress = new Dictionary<ApplicationJob, short>();

            foreach (ApplicationJob job in m_Jobs)
            {
                m_Progress[job] = (short)((forceDownload || job.Enabled) ? 0 : -1);
                bool res = m_Progress.ContainsKey(job);
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
            Thread thread = new Thread(new ParameterizedThreadStart(CheckForOnlineUpdates));
            thread.IsBackground = true;
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
            foreach (ApplicationJob job in jobs)
            {
                if (!job.CanBeShared)
                {
                    sendInfo.Add(new RpcAppGuidAndDate(job.Guid, job.DownloadDate));
                }
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

                    Thread newThread = new Thread(new ParameterizedThreadStart(StartNewThread));
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

                string postUpdateCommand = Settings.GetValue("PostUpdateCommand", "") as string;
                ScriptType postUpdateCommandType = Command.ConvertToScriptType(Settings.GetValue("PostUpdateCommandType", ScriptType.Batch.ToString()) as string);
                new Command(postUpdateCommand, postUpdateCommandType).Execute(null);

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
                        string customColumn1 = SettingsDialog.CustomColumnVariableName1;
                        string customColumn2 = SettingsDialog.CustomColumnVariableName2;
                        if (!string.IsNullOrEmpty(customColumn1) && !job.Variables.HasVariableBeenDownloaded(customColumn1))
                        {
                            job.Variables.ReplaceAllInString("{" + customColumn1 + "}");
                            job.Save(); // cached variable content
                        }
                        if (!string.IsNullOrEmpty(customColumn2) && !job.Variables.HasVariableBeenDownloaded(customColumn2))
                        {
                            job.Variables.ReplaceAllInString("{" + customColumn2 + "}");
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
                            throw ex;
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

            m_Progress[job] = 100;
            OnStatusChanged(job);
        }

        /// <summary>
        /// Determines the base host (TLD + server name) of an URI.
        /// </summary>
        /// <returns>Example: sourceforge.net</returns>
        private static string GetBaseHost(Uri uri)
        {
            string[] parts = uri.Host.Split('.');
            if (parts.Length <= 2)
            {
                return uri.Host;
            }

            return parts[parts.Length - 2] + "." + parts[parts.Length - 1];
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
            string downloadUrl = string.Empty;
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
                else
                {
                    return Status.NoUpdate;
                }
            }

            Uri url = new Uri(downloadUrl);

            return DoDownload(job, url);
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

            ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            // If we want to download multiple files simultaneously
            // from the same server, we need to "remove" the connection limit.
            ServicePointManager.DefaultConnectionLimit = 50;

            job.Variables.ResetDownloadCount();

            WebRequest req = WebRequest.CreateDefault(urlToRequest);
            AddRequestToCancel(req);
            req.Timeout = Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10)) * 1000; // 10 seconds by default

            HttpWebRequest httpRequest = req as HttpWebRequest;
            if (httpRequest != null)
            {
                // Store cookies for future requests. Some sites
                // check for previously stored cookies before allowing to download.
                httpRequest.CookieContainer = m_Cookies;

                // If we have an HTTP request, some sites may require a correct referer
                // for the download.
                // If there are variables defined (from which most likely the download link
                // or version is being extracted), we'll just use the first variable's URL as referer.
                // The user still has the option to set a custom referer.
                // Note: Some websites don't "like" certain referers
                if (!m_NoAutoReferer.Contains(GetBaseHost(req.RequestUri)))
                {
                    foreach (UrlVariable urlVar in job.Variables.Values)
                    {
                        httpRequest.Referer = urlVar.Url;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(job.HttpReferer))
                {
                    httpRequest.Referer = job.Variables.ReplaceAllInString(job.HttpReferer);
                }

                LogDialog.Log(job, "Using referer: " + (string.IsNullOrEmpty(httpRequest.Referer) ? "(none)" : httpRequest.Referer));
                httpRequest.UserAgent = (string.IsNullOrEmpty(job.UserAgent) ? WebClient.UserAgent : job.UserAgent);
            }

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
                            return DoDownload(job, new Uri(padJob.FixedDownloadUrl));
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
                if (!m_ForceDownload && !job.RequiresDownload(response, targetFileName))
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
                if (!m_ForceDownload && !m_InstallUpdated && (m_OnlyCheck || job.CheckForUpdatesOnly))
                {
                    LogDialog.Log(job, "Skipped downloading updates");
                    return Status.UpdateAvailable;
                }

                string defaultPreCommand = Settings.GetValue("PreUpdateCommand", "") as string;
                ScriptType defaultPreCommandType = Command.ConvertToScriptType(Settings.GetValue("PreUpdateCommandType", ScriptType.Batch.ToString()) as string);
                if (new Command(defaultPreCommand, defaultPreCommandType).Execute(job) == 1)
                {
                    LogDialog.Log(job, "Default pre-update command returned '1', download skipped");
                    throw new CommandErrorException();
                }
                if (new Command(job.ExecutePreCommand, job.ExecutePreCommandType).Execute(job) == 1)
                {
                    LogDialog.Log(job, "Pre-update command returned '1', download skipped");
                    throw new CommandErrorException();
                }

                // Read all file contents to a temporary location
                string tmpLocation = Path.GetTempFileName();

                // Read contents from the web and put into file
                using (Stream sourceFile = response.GetResponseStream())
                {
                    using (FileStream targetFile = File.Create(tmpLocation))
                    {
                        int byteCount = 0;
                        int readBytes = 0;
                        m_Size[job] = fileSize;

                        do
                        {
                            if (m_CancelUpdates) break;

                            byte[] buffer = new byte[1024];
                            readBytes = sourceFile.Read(buffer, 0, 1024);
                            if (readBytes > 0) targetFile.Write(buffer, 0, readBytes);
                            byteCount += readBytes;
                            OnProgressChanged(byteCount, fileSize, job);

                        } while (readBytes > 0);
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
                    File.SetLastWriteTime(tmpLocation, ApplicationJob.GetLastModified(response));
                }
                catch (ArgumentException)
                {
                    // Invalid file date. Ignore and just use DateTime.Now
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

            // Execute a default command?
            string defaultCommand = Settings.GetValue("DefaultCommand") as string;
            ScriptType defaultCommandType = Command.ConvertToScriptType(Settings.GetValue("DefaultCommandType") as string);
            new Command(defaultCommand, defaultCommandType).Execute(job);

            // Do we need to execute a command after downloading?
            if (!string.IsNullOrEmpty(job.ExecuteCommand))
            {
                new Command(job.ExecuteCommand, job.ExecuteCommandType).Execute(job);
            }

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
                    FTPLib.FTP ftpConnection = null;
                    try
                    {
                        ftpConnection = new FTPLib.FTP(response.ResponseUri.Host, "anonymous", "ketarin@canneverbe.com");
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
                if (ProgressChanged != null)
                {
                    if (m_NoProgressCounter > 100) m_NoProgressCounter = 0;
                    m_Progress[job] = m_NoProgressCounter;
                    ProgressChanged(this, new JobProgressChangedEventArgs(m_NoProgressCounter++, job));
                }
                return;
            }

            double progress = (double)pos / length * 100.0;
            byte progressInt = Convert.ToByte(Math.Round(progress));

            if (progressInt != m_LastProgress)
            {
                if (ProgressChanged != null)
                {
                    ProgressChanged(this, new JobProgressChangedEventArgs(progressInt, job));
                }

                m_Progress[job] = progressInt;
                m_LastProgress = progressInt;
            }
        }
    }
}