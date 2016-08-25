// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

namespace MyDownloader.Core
{
    public enum DownloaderState: byte 
    {
        NeedToPrepare = 0,
        Preparing,
        WaitingForReconnect,
        Prepared,
        Working,
        Pausing,
        Paused,
        Ended,
        EndedWithError
    }
}
