using System.Windows.Media.Imaging;

namespace NML.Core.Interfaces
{
    public interface ISearchEngine
    {
        ISearchResult Search(string phrase);

        string Prefix { get; }

        bool IsConfigurable { get; }

        void Configure();

        BitmapImage SearchIcon { get; }
    }
}