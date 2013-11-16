using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        private BitmapImage GetSearchIcon()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Wikipedia.Images.wiki.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public ISearchResult Search(string phrase)
        {
            var hc = new HttpClient();
            var webResultHc = hc.GetStringAsync(string.Format("http://www.wikipedia.org/w/api.php?action=query&list=search&srsearch={0}&srprop=timestamp&format=json", HttpUtility.UrlEncode(phrase)));
            var webResult = webResultHc.Result;

            var wikiResults = ParseResult(webResult);
            var result = new ListSearchResult(wikiResults.Select(wsr => MapWikiSearch(wsr)), "Wikipedia");
            result.SearchIcon = GetSearchIcon();
            return result;
        }

        private SearchResultListItem MapWikiSearch(WikiSearchResult wsr)
        {
            return new SearchResultListItem
            {
                Text = wsr.Title,
                Url = wsr.Url
            };
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
            get { return "w"; }
        }


        public bool IsConfigurable
        {
            get { return false; }
        }

        public void Configure()
        {
            
        }

        public BitmapImage SearchIcon { get; private set; }


        public string Name
        {
            get { return "Wikipedia"; }
        }
    }
}
