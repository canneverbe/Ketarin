using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Microsoft.Win32
{
    public class Kernel32
    {
        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
        private const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
        private const uint LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;
        private const uint LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;
        public  const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;
        public IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public const uint PROCESS_ALL_ACCESS = (0x1F0FFF);
        public const uint STILL_ACTIVE = 259;
        public const uint INFINITE = 0xFFFFFFFF;

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            // Legacy flag, should not be used.
            // ES_USER_PRESENT   = 0x00000004,
            ES_CONTINUOUS = 0x80000000,
        }

        /// <summary>
        /// To load the dll - dllFilePath dosen't have to be const - so I can read path from registry
        /// </summary>
        /// <param name="dllFilePath">file path with file name</param>
        /// <param name="hFile">use IntPtr.Zero</param>
        /// <param name="dwFlags">What will happend during loading dll
        /// <para>LOAD_LIBRARY_AS_DATAFILE</para>
        /// <para>DONT_RESOLVE_DLL_REFERENCES</para>
        /// <para>LOAD_WITH_ALTERED_SEARCH_PATH</para>
        /// <para>LOAD_IGNORE_CODE_AUTHZ_LEVEL</para>
        /// </param>
        /// <returns>Pointer to loaded Dll</returns>
        [DllImport("kernel32")]
        private static extern IntPtr LoadLibraryEx(string dllFilePath, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32")]
        private static extern bool FreeLibrary(IntPtr dllPointer);
        /// <summary>
        /// To unload library
        /// </summary>
        /// <param name="dllPointer">Pointer to Dll witch was returned from LoadLibraryEx</param>
        /// <returns>If unloaded library was correct then true, else false</returns>
        public static bool FreeLibraryEx(IntPtr dllPointer)
        {
            if (dllPointer != null && dllPointer != IntPtr.Zero)
            {
                return FreeLibrary(dllPointer);
            }
            return false;
        }

        /// <summary>
        /// To get function pointer from loaded dll 
        /// </summary>
        /// <param name="dllPointer">Pointer to Dll witch was returned from LoadLibraryEx</param>
        /// <param name="functionName">Function name with you want to call</param>
        /// <returns>Pointer to function</returns>
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr dllPointer, string functionName);

        /// <summary>
        /// This will to load concret dll file
        /// </summary>
        /// <param name="dllFilePath">Dll file path</param>
        /// <returns>Pointer to loaded dll</returns>
        public static IntPtr LoadWin32Library(string dllFilePath)
        {
            return LoadLibraryEx(dllFilePath, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);
        }

        /// <summary>
        /// Retrieves information about the amount of space that is available on a disk volume, which is the total amount of space, the total amount of free space, and the total amount of free space available to the user that is associated with the calling thread.
        /// </summary>
        /// <param name="lpDirectoryname">
        /// A directory on the disk.
        /// If this parameter is a UNC name, it must include a trailing backslash, for example, "\\MyServer\MyShare\".</param>
        /// <param name="lpFreeBytesAvailableToCaller">A pointer to a variable that receives the total number of free bytes on a disk that are available to the user who is associated with the calling thread.</param>
        /// <param name="lpTotalNumberOfBytes">A pointer to a variable that receives the total number of bytes on a disk that are available to the user who is associated with the calling thread.</param>
        /// <param name="lpTotalNumberOfFreeBytes">A pointer to a variable that receives the total number of free bytes on a disk.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32")]
        public static extern int GetDiskFreeSpaceEx(string lpDirectoryname, ref long lpFreeBytesAvailableToCaller, ref long lpTotalNumberOfBytes, ref long lpTotalNumberOfFreeBytes);

        /// <summary>
        /// Retrieves the volume mount point where the specified path is mounted.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetVolumePathName(string fileName)
        {
            StringBuilder result = new StringBuilder(255);
            if (!GetVolumePathName(fileName, result, 255))
            {
                return Path.GetPathRoot(fileName);
            }            
            return result.ToString();
        }

        /// <summary>
        /// Retrieves the volume mount point where the specified path is mounted.
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool GetVolumePathName(string lpszFileName, [Out] StringBuilder lpszVolumePathName, uint cchBufferLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        /// <summary>
        /// Allows a Windows Forms application to attach to the
        /// command line. This is useful, if you want to provide
        /// command line output if your application is called with
        /// a specifc parameter (like /SILENT).
        /// </summary>
        /// <param name="dwProcessId">ID of the CMD window to attach.</param>
        /// <returns>true if attaching succeeded, false otherwise</returns>
        public static bool ManagedAttachConsole(uint dwProcessId)
        {
            try
            {
                return AttachConsole(dwProcessId);
            }
            catch (EntryPointNotFoundException)
            {
                // Requires WinXP at minimums
                return false;
            }
        }

        /// <summary>
        /// Detaches the calling process from its console
        /// </summary>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.
        /// To get extended error information, call Marshal.GetLastWin32Error.</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool FreeConsole();

        /// <summary>
        /// Enables an application to inform the system that it is in use, thereby preventing the 
        /// system from entering sleep or turning off the display while the application is running.
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        /// <summary>
        /// Retrieves a module handle for the specified module. The module must have been loaded by the calling process.
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Duplicates an object handle.
        /// </summary>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

        /// <summary>
        /// Retrieves a pseudo handle for the current process.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Retrieves the termination status of the specified process.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
    }

}