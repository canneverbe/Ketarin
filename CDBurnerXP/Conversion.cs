using System;
using System.Collections.Generic;
using System.Text;

namespace CDBurnerXP
{
    public static class Conversion
    {
        public static bool ToBoolean(object value)
        {
            if (value is DBNull)
            {
                return false;
            }
            try
            {
                return Convert.ToBoolean(Convert.ToInt32(value));
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string ToXmlDate(DateTime date)
        {
            return date.ToString("yyy-MM-dd HH:mm:ss");
        }

        public static int ToInt(object number)
        {
            try
            {
                return Convert.ToInt32(number);
            }
            catch
            {
                return 0;
            }
        }

        public static int ToInt(string number)
        {
            int result;
            if (Int32.TryParse(number, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static long ToLong(string number)
        {
            long result;
            if (Int64.TryParse(number, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Casts an object to DateTime, and returns DateTime.MinValue
        /// if the cast fails.
        /// </summary>
        public static DateTime ToDateTime(object value)
        {
            try
            {
                return (DateTime)value;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Returns the original string, cut to maxLength of it exceeds the length.
        /// </summary>
        public static string Limit(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength);
        }
    }
}
