using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Google
{
    public class GoogleSearch : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var result = wc.DownloadString(string.Format("http://www.google.pl/search?q={0}", HttpUtility.UrlEncode(phrase)));

            var googleResults = ParseResult(result);
            return new ListSearchResult(googleResults.Select(gsr => MapGoogleSearch(gsr)));
        }

        public string Prefix
        {
            get
            {
                return "g ";
            }
        }

        private const string HTML_TAG_PATTERN = "<.*?>";

        private static string StripHTML(string inputString)
        {
            return Regex.Replace(inputString, HTML_TAG_PATTERN, string.Empty);
        }

        private SearchResultListItem MapGoogleSearch(GoogleSearchResult gsr)
        {
            return new SearchResultListItem
            {
                Text = gsr.Title,
                Url = gsr.Url
            };
        }

        private IList<GoogleSearchResult> ParseResult(string webResult)
        {
            var result = new List<GoogleSearchResult>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(webResult);
                var nodes = doc.DocumentNode.SelectNodes("//li[@class='g']");
                foreach (var n in nodes)
                {
                    var gsr = new GoogleSearchResult();

                    var aNode = n.SelectSingleNode(".//h3[@class='r']/a");
                    gsr.Title = StripHTML(aNode.InnerText);
                    gsr.Url = GetUrl(aNode.GetAttributeValue("href", string.Empty));

                    Uri uri;
                    if (Uri.TryCreate(gsr.Url, UriKind.Absolute, out uri))
                    {
                        result.Add(gsr);
                    }
                }
            }
            catch { }
            
            return result;
        }

        private string GetUrl(string badUrl)
        {
            var better = HttpUtility.HtmlDecode(badUrl);
            better = better.Remove(0, better.IndexOf("?") + 1);
            return HttpUtility.UrlDecode(HttpUtility.ParseQueryString(better)["q"]);
        }
    }
}
