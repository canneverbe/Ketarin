using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CDBurnerXP.IO
{
    public static class FormatFileSize
    {
        private static string m_KBName = "KB";
        private static string m_MBName = "MB";
        private static string m_GBName = "GB";
        private static string m_BytesName = "Bytes";
        private static bool m_UseSingleUnit = false;

        private const int MODE1_LB_SIZE = 2048;
        private const int MODE2_LB_SIZE = 2352;

        #region Properties

        public static bool UseSingleUnit
        {
            get { return FormatFileSize.m_UseSingleUnit; }
            set { FormatFileSize.m_UseSingleUnit = value; }
        }

        public static string BytesName
        {
            get { return m_BytesName; }
            set { m_BytesName = value; }
        }

        public static string KBName
        {
            get
            {
                return m_KBName;
            }
            set
            {
                m_KBName = value;
            }
        }

        public static string MBName
        {
            get
            {
                return m_MBName;
            }
            set
            {
                m_MBName = value;
            }
        }

        public static string GBName
        {
            get
            {
                return m_GBName;
            }
            set
            {
                m_GBName = value;
            }
        }

        #endregion

        /// <summary>
        /// Formats a size in bytes as GB, MB or KB.
        /// </summary>
        /// <param name="length">Size in bytes</param>
        /// <returns>File size string with unit</returns>
        public static string Format(float length)
        {
            return Format(Convert.ToInt64(length));
        }

        /// <summary>
        /// Formats a size in bytes as GB, MB or KB.
        /// </summary>
        /// <param name="length">Size in bytes</param>
        /// <returns>File size string with unit</returns>
        public static string Format(long length)
        {
            long kbSize = 1024;
            long mbSize = 1048576;
            long gbSize = 1073741824;

            if (m_UseSingleUnit)
            {
                return FormatAsKB(length); 
            }

            if (length < kbSize)
                return FormatAsBytes(length); 

            if (length < mbSize)
                return FormatAsKB(length); 

            if (length < gbSize)
                return FormatAsMB(length);

            return FormatAsGB(length);
        }

        /// <summary>
        /// Formats a file size in bytes as GB, MB or KB.
        /// </summary>
        /// <param name="filePath">Path of file</param>
        /// <returns>File size string with unit</returns>
        public static string Format(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;

            try
            {
                FileInfo info = new FileInfo(filePath);
                return Format(info.Length);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Formats file sizes in a way, that the difference between both can be seen.
        /// If for eample "required" and "available" formatted size are the same (can happen 
        /// when using large discs and size is displayed in GB), it makes the application 
        /// look stupid, since it says there is a difference.
        /// </summary>
        /// <param name="length1">First length to format</param>
        /// <param name="length2">Second length to format</param>
        /// <param name="formattedLength1">First length, formatted</param>
        /// <param name="formattedLength2">Second length, formatted</param>
        public static void FormatForComparison(long length1, long length2, out string formattedLength1, out string formattedLength2)
        {
            formattedLength1 = Format(length1);
            formattedLength2 = Format(length2);
            if (formattedLength1 != formattedLength2)
            {
                // Difference can be displayed, no further action required
                return;
            }

            // Try MB
            formattedLength1 = FormatAsMB(length1);
            formattedLength2 = FormatAsMB(length2);
            if (formattedLength1 != formattedLength2)
            {
                // Difference can be displayed, no further action required
                return;
            }

            // Last resort KB
            formattedLength1 = FormatAsKB(length1);
            formattedLength2 = FormatAsKB(length2);
        }

        private static string FormatAsBytes(long length)
        {
            return String.Format("{0:0} " + BytesName, length);
        }

        private static string FormatAsKB(long size)
        {
            return String.Format("{0:0.00} " + KBName, size / 1024d);
        }

        private static string FormatAsMB(long size)
        {
            return string.Format("{0:0.00} " + MBName, size / 1024d / 1024d);
        }

        private static string FormatAsGB(long size)
        {
            return string.Format("{0:0.00} " + GBName, size / 1024d / 1024d / 1024d);
        }

        /// <summary>
        /// Formats a size in bytes as GB, MB or KB, using decimal conversion.
        /// </summary>
        /// <param name="length">Size in bytes</param>
        /// <returns>File size string with unit</returns>
        public static string FormatDecimal(long length)
        {
            double size = length / 1000d;
            long kbSize = 1000;
            long mbSize = 1000000;
            long gbSize = 1000000000;

            if (m_UseSingleUnit)
            {
                return String.Format("{0:0} " + KBName, size);
            }

            if (length < kbSize)
                return String.Format("{0:0} " + BytesName, length);

            if (length < mbSize)
                return String.Format("{0:0} " + KBName, size);

            size /= 1000d;

            if (length < gbSize)
                return string.Format("{0:0} " + MBName, size);

            size /= 1000d;

            return string.Format("{0} " + GBName, size);
        }

        /// <summary>
        /// Converts the given number of blocks to bytes (data mode).
        /// </summary>
        public static long BlocksToDataBytes(int blocks)
        {
            return blocks * (long)MODE1_LB_SIZE;
        }

        /// <summary>
        /// Converts the given number of blocks to bytes (audio mode).
        /// </summary>
        public static long BlocksToAudioBytes(int blocks)
        {
            return blocks * (long)MODE2_LB_SIZE;
        }

        /// <summary>
        /// Returns the number of bytes required for a given length in seconds.
        /// </summary>
        public static long SecondsToBytes(int seconds)
        {
            // The net byte rate of a Mode-1 CD-ROM, based on comparison to CDDA audio standards,
            // is 44100 Hz × 16 bits/sample × 2 channels × 2,048 / 2,352 /8 = 153.6 kB/s = 150 KiB/s.
            return 150 * seconds * 1024;
        }
    }
}
