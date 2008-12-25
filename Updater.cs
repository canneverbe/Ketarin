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
    class Updater
    {
        private ApplicationJob[] m_Jobs = null;
        private Dictionary<ApplicationJob, short> m_Progress = null;
        private Dictionary<ApplicationJob, Status> m_Status = null;
        private Dictionary<ApplicationJob, long> m_Size = new Dictionary<ApplicationJob,long>();
        private bool m_CancelUpdates = false;
        private bool m_IsBusy = false;
        protected int m_LastProgress = -1;
        private List<ApplicationJobError> m_Errors;
        private byte m_NoProgressCounter = 0;
        private bool m_OnlyCheck = false;
        private int m_ThreadLimit = 2;
        private List<Thread> m_Threads = new List<Thread>();
        private List<string> m_NoAutoReferer = new List<string>(new string[] {"sourceforge.net"});

        #region Properties

        public ApplicationJobError[] Errors
        {
            get
            {
                return m_Errors.ToArray();
            }
        }

        public bool IsBusy
        {
            get
            {
                return m_IsBusy;
            }
        }

        #endregion

        public enum Status
        {
            Idle,
            Downloading,
            UpdateSuccessful,
            NoUpdate,
            Failure
        }

        #region JobProgressChangedEventArgs

        public class JobProgressChangedEventArgs : ProgressChangedEventArgs
        {
            private ApplicationJob m_Job = null;

            #region Properties

            public ApplicationJob ApplicationJob
            {
                get
                {
                    return m_Job;
                }
            }

            #endregion

            public JobProgressChangedEventArgs(int progressPercentage, ApplicationJob job) : base(progressPercentage, null)
            {
                m_Job = job;
            }
        }

        #endregion

        #region JobStatusChangedEventArgs

        public class JobStatusChangedEventArgs : EventArgs
        {
            private ApplicationJob m_Job = null;
            private Status m_NewStatus = Status.Idle;

            #region Properties

            public ApplicationJob ApplicationJob
            {
                get
                {
                    return m_Job;
                }
            }

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

        public event EventHandler<JobProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<JobStatusChangedEventArgs> StatusChanged;
        public event EventHandler UpdateCompleted;
        public event EventHandler<GenericEventArgs<string[]>> UpdatesFound;

        #region Public control methods

        public void Cancel()
        {
            m_CancelUpdates = true;
        }

        public long GetDownloadSize(ApplicationJob job)
        {
            if (m_Size == null || !m_Size.ContainsKey(job)) return -1;

            return m_Size[job];
        }

        public short GetProgress(ApplicationJob job)
        {
            if (m_Progress == null || !m_Progress.ContainsKey(job)) return -1;

            return m_Progress[job];
        }

        public Status GetStatus(ApplicationJob job)
        {
            if (m_Status == null || !m_Status.ContainsKey(job)) return Status.Idle;

            return m_Status[job];
        }

        public void BeginUpdate(ApplicationJob[] jobs, bool onlyCheck)
        {
            m_IsBusy = true;
            m_Jobs = jobs;
            m_ThreadLimit = Convert.ToInt32(Settings.GetValue("ThreadCount", 2));
            m_OnlyCheck = onlyCheck;

            // Initialise progress and status
            m_Progress = new Dictionary<ApplicationJob, short>();
            m_Status = new Dictionary<ApplicationJob, Status>();
            foreach (ApplicationJob job in m_Jobs)
            {
                m_Progress[job] = 0;
                bool res = m_Progress.ContainsKey(job);
                m_Status[job] = 0;
            }
            m_Threads.Clear();

            Thread thread = new Thread(UpdateApplications);
            thread.Start();
        }

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

        private void UpdateApplications()
        {
            m_CancelUpdates = false;
            m_Errors = new List<ApplicationJobError>();
            LogDialog.Log(string.Format("Update started with {0} application(s)", m_Jobs.Length));

            try
            {
                foreach (ApplicationJob job in m_Jobs)
                {
                    // Skip if disabled
                    if (!job.Enabled) continue;

                    // Wait until we can start a new thread
                    while (m_Threads.Count >= m_ThreadLimit)
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
                        m_Status[job] = DoDownload(job, out requestedUrl) ? Status.UpdateSuccessful : Status.NoUpdate;
                        // If no exception happened, we immediately leave the loop
                        break;
                    }
                    catch (Exception ex)
                    {
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
                m_Errors.Add(new ApplicationJobError(job, ex, requestedUrl));
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
            catch (UriFormatException ex)
            {
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

            m_Progress[job] = 100;
            OnStatusChanged(job);
        }

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
        /// Executes the actual download. Does not handle exceptions,
        /// but takes care of proper cleanup.
        /// </summary>
        /// <param name="job">The job to process</param>
        /// <returns>true, if a new update has been found and downloaded, false otherwise</returns>
        protected bool DoDownload(ApplicationJob job, out string requestedUrl)
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
            Uri url = new Uri(downloadUrl);

            // Lower security policies
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            // If we want to download multiple files simultaneously
            // from the same server, we need to "remove" the connection limit.
            ServicePointManager.DefaultConnectionLimit = 10;

            WebRequest req = WebRequest.CreateDefault(url);
            req.Timeout = Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10)) * 1000; // 10 seconds by default

            HttpWebRequest httpRequest = req as HttpWebRequest;
            if (httpRequest != null)
            {
                // If we have an HTTP request, some sites may require a correct referer
                // for the download.
                // If there there are variables defined (from which most likely the download link
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
                    httpRequest.Referer = job.HttpReferer;
                }

                httpRequest.UserAgent = WebClient.UserAgent;
            }

            using (WebResponse response = req.GetResponse())
            {
                // Occasionally, websites are not available and an error page is encountered
                // For the case that the content type is just plain wrong, ignore it if the size is higher than 500KB
                HttpWebResponse httpResponse = response as HttpWebResponse;
                if (httpResponse != null && response.ContentType.StartsWith("text/html") && response.ContentLength < 500000)
                {
                    throw NonBinaryFileException.Create(response.ContentType, HttpStatusCode.NotAcceptable);
                }

                string targetFileName = job.GetTargetFile(response);

                // Only download, if the file size or date has changed
                if (!job.RequiresDownload(response, targetFileName))
                {
                    m_Status[job] = Status.UpdateSuccessful;

                    // If file already exists (created by user),
                    // the download is not necessary. We still need to
                    // set the file name.
                    job.PreviousLocation = targetFileName;
                    job.Save();
                    return false;
                }

                // Skip downloading!
                if (m_OnlyCheck) return true;

                // Read all file contents to a temporary location
                string tmpLocation = Path.GetTempFileName();

                // Read contents from the web and put into file
                using (Stream sourceFile = response.GetResponseStream())
                {
                    using (FileStream targetFile = File.Create(tmpLocation))
                    {
                        int byteCount = 0;
                        int readBytes = 0;
                        long length = GetContentLength(response);
                        m_Size[job] = length;

                        do
                        {
                            if (m_CancelUpdates) break;

                            byte[] buffer = new byte[1024];
                            readBytes = sourceFile.Read(buffer, 0, 1024);
                            if (readBytes > 0) targetFile.Write(buffer, 0, readBytes);
                            byteCount += readBytes;
                            OnProgressChanged(byteCount, length, job);

                        } while (readBytes > 0);
                    }
                }

                if (m_CancelUpdates)
                {
                    m_Status[job] = Status.Failure;
                    m_Progress[job] = 0;
                    OnStatusChanged(job);
                    return false;
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
                    // Copying might fail if variables have been replaced with bad values.
                    // However, we cannot rely on functions to clean up the path, since they
                    // might actually parse the path incorrectly and return a even worse path.
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
            if (!string.IsNullOrEmpty(defaultCommand))
            {
                ExecuteCommand(job, defaultCommand);
            }

            // Do we need to execute a command after downloading?
            if (!string.IsNullOrEmpty(job.ExecuteCommand))
            {
                ExecuteCommand(job, job.ExecuteCommand);
            }

            return true;
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

        private static void ExecuteCommand(ApplicationJob job, string baseCommand)
        {
            baseCommand = baseCommand.Replace("\r\n", "\n");
            baseCommand = job.Variables.ReplaceAllInString(baseCommand);

            // Replace variable: file
            baseCommand = baseCommand.Replace("{file}", "\"" + job.PreviousLocation + "\"");
            // Replace variable: root
            try
            {
                baseCommand = baseCommand.Replace("{root}", Path.GetPathRoot(Application.StartupPath));
            }
            catch (ArgumentException) { }

            // Feed cmd.exe with our commands
            ProcessStartInfo cmdExe = new ProcessStartInfo("cmd.exe");
            cmdExe.RedirectStandardInput = true;
            cmdExe.UseShellExecute = false;
            cmdExe.CreateNoWindow = true;
            cmdExe.RedirectStandardOutput = true;
            cmdExe.RedirectStandardError = true;

            using (Process proc = Process.Start(cmdExe))
            {
                // Clean all the cmd.exe headers
                string clean = string.Empty;
                do
                {
                    clean = proc.StandardOutput.ReadLine();
                } while (!string.IsNullOrEmpty(clean));

                using (proc.StandardInput)
                {
                    string[] commands = baseCommand.Split('\n');
                    foreach (string command in commands)
                    {
                        LogDialog.Log(job, "Executing command: " + command);
                        proc.StandardInput.WriteLine(command);
                    }
                }
                proc.WaitForExit();

                string result = proc.StandardOutput.ReadToEnd();
                LogDialog.Log(job, "Command result: " + result);
            }
        }

        protected virtual void OnUpdateCompleted()
        {
            if (UpdateCompleted != null)
            {
                UpdateCompleted(this, null);
            }
        }

        protected virtual void OnStatusChanged(ApplicationJob job)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, new JobStatusChangedEventArgs(job, GetStatus(job)));
            }
        }

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
