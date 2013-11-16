using System.Windows.Media.Imaging;
using NML.Core.Interfaces;

namespace NML.Core.Results
{
    public abstract class BaseSearchResult : ISearchResult
    {
        public string Title { get; set; }
        public BitmapImage SearchIcon { get; set; }
        public abstract bool HasResult { get; }
    }
}
