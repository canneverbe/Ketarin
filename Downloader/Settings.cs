namespace MyDownloader.Core
{
    internal class Settings
    {
        public static int MaxRetries { get; set; } = 2;

        public static double RetryDelay { get; set; } = 5;

        public static int MaxSegments { get; set; } = 5;

        public static double MinSegmentLeftToStartNewSegment { get; set; } = 30;

        public static long MinSegmentSize { get; set; } = 200000;

        public static string ProxyAddress { get; set; }

        public static string ProxyUserName { get; set; }

        public static string ProxyPassword { get; set; }

        public static string ProxyDomain { get; set; }

        public static bool UseProxy { get; set; }

        public static bool ProxyByPassOnLocal { get; set; }

        public static int ProxyPort { get; set; }
    }
}