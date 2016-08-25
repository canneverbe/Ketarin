// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

namespace MyDownloader.Core.Extensions
{
    public interface IExtension
    {
        string Name { get; }

        IUIExtension UIExtension { get; }
    }
}
