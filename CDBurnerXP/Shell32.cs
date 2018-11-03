using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Microsoft.Win32
{
    /// This code has been left largely untouched from that in the CRC example. The main changes have been moving
    /// the icon reading code over to the IconReader type.
    public class Shell32
    {
        public const int MAX_PATH = 256;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHITEMID
        {
            public short cb;
            [MarshalAs(UnmanagedType.LPArray)]
            public byte[] abID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ITEMIDLIST
        {
            public SHITEMID mkid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            public int ulFlags;
            public IntPtr lpfn;
            public int lParam;
            public IntPtr iImage;
        }

        // Browsing for directory.
        public const int BIF_RETURNONLYFSDIRS = 1;
        public const int BIF_DONTGOBELOWDOMAIN = 2;
        public const int BIF_STATUSTEXT = 4;
        public const int BIF_RETURNFSANCESTORS = 8;
        public const int BIF_EDITBOX = 16;
        public const int BIF_VALIDATE = 32;
        public const int BIF_NEWDIALOGSTYLE = 64;
        public const int BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
        public const int BIF_BROWSEINCLUDEURLS = 128;
        public const int BIF_BROWSEFORCOMPUTER = 4096;
        public const int BIF_BROWSEFORPRINTER = 8192;
        public const int BIF_BROWSEINCLUDEFILES = 16384;
        public const int BIF_SHAREABLE = 32768;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        public const int SHGFI_ICON = 256;
        // get icon
        public const int SHGFI_DISPLAYNAME = 512;
        // get display name
        public const int SHGFI_TYPENAME = 1024;
        // get type name
        public const int SHGFI_ATTRIBUTES = 2048;
        // get attributes
        public const int SHGFI_ICONLOCATION = 4096;
        // get icon location
        public const int SHGFI_EXETYPE = 8192;
        // return exe type
        public const int SHGFI_SYSICONINDEX = 16384;
        // get system icon index
        public const int SHGFI_LINKOVERLAY = 32768;
        // put a link overlay on icon
        public const int SHGFI_SELECTED = 65536;
        // show icon in selected state
        public const int SHGFI_ATTR_SPECIFIED = 131072;
        // get only specified attributes
        public const int SHGFI_LARGEICON = 0;
        // get large icon
        public const int SHGFI_SMALLICON = 1;
        // get small icon
        public const int SHGFI_OPENICON = 2;
        // get open icon
        public const int SHGFI_SHELLICONSIZE = 4;
        // get shell size icon
        public const int SHGFI_PIDL = 8;
        // pszPath is a pidl
        public const int SHGFI_USEFILEATTRIBUTES = 16;
        // use passed dwFileAttribute
        public const int SHGFI_ADDOVERLAYS = 32;
        // apply the appropriate overlays
        public const int SHGFI_OVERLAYINDEX = 64;
        // Get the index of the overlay
        public const int FILE_ATTRIBUTE_DIRECTORY = 16;
        public const int FILE_ATTRIBUTE_NORMAL = 128;
        public const int SW_SHOW = 5;
        public const uint SEE_MASK_INVOKEIDLIST = 12;

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        /// <summary>
        /// Shows the Windows file property dialog for a given file.
        /// </summary>
        public static void ShowFileProperties(string filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern bool PathMatchSpecW(string pszFileParam, string pszSpec);

        /// <summary>
        /// Determines whether or not a file matches a given pattern.
        /// </summary>
        /// <param name="file">File path to match</param>
        /// <param name="filePattern">File pattern (like *.exe)</param>
        /// <param name="include">Whether the pattern specifies inclusion or exclusion of files</param>
        /// <returns>true, if the file should be included</returns>
        public static bool FileMatchesFilter(string file, string filePattern, bool include)
        {
            // Always include files if the filter is not set
            if (string.IsNullOrEmpty(filePattern)) return true;

            bool result = PathMatchSpecW(Path.GetFileName(file), filePattern);
            return include ? result : !result;
        }
    }
}
