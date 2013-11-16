using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using NML.Core;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.FilesSystem
{
    public class FileSearch : ISearchEngine
    {
        public FileSearch()
        {
            SearchIcon = GetSearchIcon();
        }

        public BitmapImage SearchIcon { get; set; }

        public ISearchResult Search(string phrase)
        {
            phrase = Utils.PhraseWithoutPrefix(phrase, Prefix);

            var result = FileIndexSearch.ExecuteQuery(phrase);
            var searchResult = new ListSearchResult(result.Select(fsr => MapFileSearchRresult(fsr)), Name);
            searchResult.SearchIcon = SearchIcon;
            return searchResult;
        }

        private SearchResultListItem MapFileSearchRresult(FileSearchResult fsr)
        {
            var lastSlashIndex = fsr.Path.LastIndexOf('/');
            if (lastSlashIndex > -1)
            {
                fsr.Path = fsr.Path.Substring(0, lastSlashIndex);
            }

            return new SearchResultListItem
            {
                Text = string.Format("{0} - {1}", fsr.Name, fsr.Path),
                Url = fsr.Path
            };
        }

        private BitmapImage GetSearchIcon()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.FilesSystem.Images.folder.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public string Prefix
        {
            get { return "e"; }
        }

        public bool IsConfigurable
        {
            get { return false; }
        }

        public void Configure()
        {
        }

        public string Name {
            get { return "File system"; }
        }
    }
}
