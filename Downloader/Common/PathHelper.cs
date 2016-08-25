// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using System.IO;

namespace MyDownloader.Core.Common
{
    public static class PathHelper
    {
        public static string GetWithBackslash(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path += Path.DirectorySeparatorChar.ToString();
            }

            return path;
        }
    }
}
