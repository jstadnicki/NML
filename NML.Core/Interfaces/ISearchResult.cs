using System.Windows.Media.Imaging;

namespace NML.Core.Interfaces
{
    public interface ISearchResult
    {
        string Title { get; }
        BitmapImage SearchIcon { get; }
        bool HasResult { get; }
    }
}
