// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

namespace MyDownloader.Core.Common
{
    public static class BoolFormatter
    {
        private const string Yes = "Yes";
        private const string No = "No";

        public static bool FromString(string s)
        {
            if (s == Yes) return true;
            return false;
        }

        public static string ToString(bool v)
        {
            if (v) return Yes;
            return No;
        }
    }
}
