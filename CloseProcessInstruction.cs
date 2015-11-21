using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;

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
                if (process.CloseMainWindow())
                {
                    // Wait 2 seconds until the process has closed
                    DateTime startWait = DateTime.Now;
                    while (DateTime.Now - startWait < TimeSpan.FromSeconds(2))
                    {
                        if (process.HasExited) break;

                        Thread.Sleep(100);
                    }
                }

                if (!process.HasExited)
                {
                    // Safe termination
                    if (!SafeTerminateProcess(process.Handle, 0))
                    {
                        process.Kill();
                    }
                }
            }
        }

        /// <summary>
        /// Safely terminate a process by creating a remote thread in the process that calls ExitProcess.
        /// </summary>
        /// <remarks>http://www.drdobbs.com/184416547;?pgno=3</remarks>
        private bool SafeTerminateProcess(IntPtr hProcess, uint uExitCode)
        {
            uint dwCode;
            IntPtr hProcessDup;
            IntPtr hRT = IntPtr.Zero;
            IntPtr hKernel = Kernel32.GetModuleHandle("Kernel32");
            bool success = false;

            bool bDup = Kernel32.DuplicateHandle(Kernel32.GetCurrentProcess(), hProcess, Kernel32.GetCurrentProcess(), out hProcessDup, Kernel32.PROCESS_ALL_ACCESS, false, 0);

            // Detect the special case where the process is 
            // already dead...
            if (Kernel32.GetExitCodeProcess(bDup ? hProcessDup : hProcess, out dwCode) && dwCode == Kernel32.STILL_ACTIVE)
            {
                IntPtr pfnExitProc = Kernel32.GetProcAddress(hKernel, "ExitProcess");
                uint dwTID;
                hRT = Kernel32.CreateRemoteThread(bDup ? hProcessDup : hProcess, IntPtr.Zero, 0, pfnExitProc, new IntPtr(uExitCode), 0, out dwTID);
            }

            if (hRT != IntPtr.Zero)
            {
                // Must wait process to terminate to 
                // guarantee that it has exited...
                Kernel32.WaitForSingleObject(bDup ? hProcessDup : hProcess, Kernel32.INFINITE);
                Kernel32.CloseHandle(hRT);
                success = true;
            }

            if (bDup)
            {
                Kernel32.CloseHandle(hProcessDup);
            }

            return success;
        }

        public override string ToString()
        {
            return string.Format("Close {0}", this.ProcessName);
        }
    }
}
