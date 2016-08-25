// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using System;

namespace MyDownloader.Core.Common
{
    public static class TimeSpanFormatter
    {
        public static string ToString(TimeSpan ts)
        {
            if (ts == TimeSpan.MaxValue)
            {
                return "?";
            }

            string str = ts.ToString();
            int index = str.LastIndexOf('.');
            if (index > 0)
            {
                return str.Remove(index);
            }
            else
            {
                return str;
            }
        }
    }
}
