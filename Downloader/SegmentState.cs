// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

namespace MyDownloader.Core
{
    public enum SegmentState
    {
        Idle,
        Connecting,
        Downloading,
        Paused,
        Finished,
        Error,
    }
}
