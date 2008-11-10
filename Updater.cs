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

namespace Ketarin
{
    class Updater
    {
        private IEnumerable<ApplicationJob> m_Jobs = null;
        private Dictionary<ApplicationJob, short> m_Progress = null;
        private Dictionary<ApplicationJob, Status> m_Status = null;
        private Dictionary<ApplicationJob, long> m_Size = new Dictionary<ApplicationJob,long>();
        private bool m_CancelUpdates = false;
        private bool m_IsBusy = false;
        protected int m_LastProgress = -1;
        private List<ApplicationJobError> m_Errors;
        private byte m_NoProgressCounter = 0;

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
            Success,
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

        public void Run(IEnumerable<ApplicationJob> jobs)
        {
            m_IsBusy = true;
            m_Jobs = jobs;
            
            // Initialise progress and status
            m_Progress = new Dictionary<ApplicationJob, short>();
            m_Status = new Dictionary<ApplicationJob, Status>();
            foreach (ApplicationJob job in m_Jobs)
            {
                m_Progress[job] = 0;
                bool res = m_Progress.ContainsKey(job);
                m_Status[job] = 0;
            }

            Thread thread = new Thread(Run);
            thread.Start();
        }

        #endregion

        private void Run()
        {
            m_CancelUpdates = false;
            m_Errors = new List<ApplicationJobError>();

            try
            {
                foreach (ApplicationJob job in m_Jobs)
                {
                    // Stop if cancelled
                    if (m_CancelUpdates) break;

                    // Skip if disabled
                    if (!job.Enabled) continue;

                    m_Status[job] = Status.Downloading;
                    OnStatusChanged(job);

                    try
                    {
                        DoDownload(job);
                        m_Status[job] = Status.Success;
                    }
                    catch (WebException ex)
                    {
                        m_Errors.Add(new ApplicationJobError(job, ex));
                        m_Status[job] = Status.Failure;
                    }
                    catch (FileNotFoundException ex)
                    {
                        // Executing command failed
                        m_Errors.Add(new ApplicationJobError(job, ex));
                    }
                    catch (IOException ex)
                    {
                        m_Errors.Add(new ApplicationJobError(job, ex));
                        m_Status[job] = Status.Failure;
                    }
                    catch (UriFormatException ex)
                    {
                        m_Errors.Add(new ApplicationJobError(job, ex));
                        m_Status[job] = Status.Failure;
                    }
                    catch (NonBinaryFileException ex)
                    {
                        m_Errors.Add(new ApplicationJobError(job, ex));
                        m_Status[job] = Status.Failure;
                    }
                    catch (Win32Exception ex)
                    {
                        // Executing command failed
                        m_Errors.Add(new ApplicationJobError(job, ex));
                    }

                    m_Progress[job] = 100;
                    OnStatusChanged(job);
                }
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
        /// Executes the actual download. Does not handle exceptions,
        /// but takes care of proper cleanup.
        /// </summary>
        /// <param name="job">The job to process</param>
        protected void DoDownload(ApplicationJob job)
        {
            string downloadUrl = string.Empty;
            if (job.DownloadSourceType == ApplicationJob.SourceType.FileHippo)
            {
                downloadUrl = ExternalServices.FileHippoDownloadUrl(job.FileHippoId);
            }
            else
            {
                downloadUrl = job.FixedDownloadUrl;
                // Now replace variables
                foreach (UrlVariable var in job.Variables.Values)
                {
                    downloadUrl = var.ReplaceInString(downloadUrl);
                }
            }
            Uri url = new Uri(downloadUrl);

            WebRequest req = WebRequest.CreateDefault(url);
            req.Timeout = 10000; // 10 seconds
            using (WebResponse response = req.GetResponse())
            {
                // Occasionally, websites are not available and an error page is encountered
                // For the case that the content type is just plain wrong, ignore it if the size is higher than 500KB
                if (response is HttpWebResponse && response.ContentType.StartsWith("text/") && response.ContentLength < 500000)
                {
                    throw new NonBinaryFileException(response.ContentType);
                }

                string targetFileName = job.GetTargetFile(response);

                // Only download, if the file size or date has changed
                if (!job.RequiresDownload(response))
                {
                    m_Status[job] = Status.Success;

                    // If file already exists (created by user),
                    // the download is not necessary. We still need to
                    // set the file name.
                    job.PreviousLocation = targetFileName;
                    job.Save();
                    return;
                }

                // Read all file contents to a temporary location
                string tmpLocation = Path.GetTempFileName();

                // Read contents from the web and put into file
                using (Stream sourceFile = response.GetResponseStream())
                {
                    using (FileStream targetFile = File.Create(tmpLocation))
                    {
                        int resByte = -1;
                        int byteCount = 0;
                        long length = Convert.ToInt64(response.ContentLength);
                        m_Size[job] = length;

                        do
                        {
                            if (m_CancelUpdates) break;

                            resByte = sourceFile.ReadByte();
                            if (resByte >= 0) targetFile.WriteByte((byte)resByte);
                            byteCount++;
                            OnProgressChanged(byteCount, length, job);

                        } while (resByte >= 0);
                    }
                }

                if (m_CancelUpdates)
                {
                    m_Status[job] = Status.Failure;
                    m_Progress[job] = 0;
                    OnStatusChanged(job);
                    return;
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
                File.Copy(tmpLocation, targetFileName, true);
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
        }

        private static void ExecuteCommand(ApplicationJob job, string baseCommand)
        {
            // Replace variable: file
            string command = baseCommand.Replace("{file}", "\"" + job.PreviousLocation + "\"");
            // Replace variable: root
            try
            {
                command = command.Replace("{root}", Path.GetPathRoot(Application.StartupPath));
            }
            catch (ArgumentException) { }
            // Tell cmd to execute the given command (/c)
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd", "/c " + command);
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;
            Process.Start(processInfo);
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
