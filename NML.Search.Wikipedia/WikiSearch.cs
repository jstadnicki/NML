using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Wikipedia
{
    public class WikiSearch : ISearchEngine
    {
        private const string wikiUrl = "http://en.wikipedia.org/wiki/{0}";

        public WikiSearch()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Wikipedia.Images.wiki.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            SearchIcon = image;
        }

        public ISearchResult Search(string phrase)
        {
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var webResult = wc.DownloadString(string.Format("http://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={0}&srprop=timestamp&format=json", HttpUtility.UrlEncode(phrase)));

            var wikiResults = ParseResult(webResult);
            return new ListSearchResult(wikiResults.Select(wsr => MapWikiSearch(wsr)), "Wikipedia");
        }

        private SearchResultListItem MapWikiSearch(WikiSearchResult wsr)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<WikiSearchResult> ParseResult(string webResult)
        {
            var result = new List<WikiSearchResult>();

            try
            {
                var jWikis = JObject.Parse(webResult);
                var wikis = jWikis["query"]["search"];
                
                foreach (var w in wikis)
                {
                    var wsr = new WikiSearchResult();

                    wsr.Title = w["title"].Value<string>();
                    wsr.Url = string.Format(wikiUrl, wsr.Title);

                    Uri uri;
                    if (Uri.TryCreate(wsr.Url, UriKind.Absolute, out uri))
                    {
                        result.Add(wsr);
                    }
                }
            }
            catch { }

            return result;
        }

        public string Prefix
        {
            get { return "w "; }
        }


        public bool IsConfigurable
        {
            get { return false; }
        }

        public void Configure()
        {
            
        }

        public BitmapImage SearchIcon { get; private set; }
    }
}
