using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Ketarin
{
    /// <summary>
    /// Represents a setup instruction that closes a process by first trying
    /// a soft exit and then a hard exit (kill) if necessary.
    /// </summary>
    [Serializable()]
    public class CloseProcessInstruction : SetupInstruction
    {
        /// <summary>
        /// In opposite to Process.Kill(), tries a "soft exit" of the process.
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern void ExitProcess(uint uExitCode);

        /// <summary>
        /// Specifies the name of the process to close.
        /// </summary>
        public string ProcessName
        {
            get;
            set;
        }

        public override string Name
        {
            get
            {
                return "Close process";
            }
        }

        public override void Execute()
        {
            foreach (Process process in Process.GetProcessesByName(ProcessName))
            {
                process.CloseMainWindow();

                // Wait 2 seconds until the process has closed
                DateTime startWait = DateTime.Now;
                while (DateTime.Now - startWait < TimeSpan.FromSeconds(2))
                {
                    if (process.HasExited) break;

                    Thread.Sleep(100);
                }

                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Close {0}", this.ProcessName);
        }
    }
}
