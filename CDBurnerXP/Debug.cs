using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CDBurnerXP.IO
{
    /// <summary>
    /// A class which provides methods for saving debug information.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Returns the location of the error log file.
        /// </summary>
        private static string LogFilePath
        {
            get
            {
                string dir = Path.Combine(PathEx.TryGetFolderPath(Environment.SpecialFolder.ApplicationData), "Canneverbe Limited", "CDBurnerXP");
                Directory.CreateDirectory(dir);

                return Path.Combine(dir, "Errors.log");
            }
        }

        /// <summary>
        /// Saves an exception the a log file.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogError(Exception ex)
        {
            try
            {
                if (ex == null) return;

                string textToAppend = string.Format("{0} ({1}): {2}", DateTime.Now, Application.ProductName, ex.ToString()) + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(LogFilePath, textToAppend);

                if (ex.InnerException != null)
                {
                    textToAppend = string.Format("Inner Exception: {0}", ex.InnerException.ToString()) + Environment.NewLine + Environment.NewLine;
                    File.AppendAllText(LogFilePath, textToAppend);
                }
            }
            catch (Exception)
            {
                // We don't want logging errors to cause even more problems
            } 
        }
    }
}
