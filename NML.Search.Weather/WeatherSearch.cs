using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using NML.Core;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Weather
{
    public class WeatherSearch : ISearchEngine
    {
        public WeatherSearch()
        {
            SearchIcon = GetSearchIcon();
        }

        public ISearchResult Search(string phrase)
        {
            phrase = Utils.PhraseWithoutPrefix(phrase, Prefix);

            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var webResult = wc.DownloadString(string.Format("http://api.openweathermap.org/data/2.5/find?q={0}&units=metric&mode=json", HttpUtility.UrlEncode(phrase)));

            var weatherResults = ParseResult(webResult);
            var result = new ListTextSearchResult(weatherResults.Select(wsr => MapWeatherSearch(wsr)), "Weather");
            result.SearchIcon = SearchIcon;
            return result;
        }

        private SearchTextResultListItem MapWeatherSearch(WeatherSearchResult wsr)
        {
            return new SearchTextResultListItem
            {
                Text = wsr.Text
            };
        }

        private IEnumerable<WeatherSearchResult> ParseResult(string webResult)
        {
            var result = new List<WeatherSearchResult>();

            try
            {
                var jWeather = JObject.Parse(webResult);
                var places = jWeather["list"];

                foreach (var p in places)
                {
                    if (!string.IsNullOrWhiteSpace(p["name"].Value<string>()))
                    {
                        var wsr = new WeatherSearchResult();
                        
                        var country = p["sys"] != null && p["sys"]["country"] != null
                            ? p["sys"]["country"].Value<string>()
                            : string.Empty;

                        wsr.Text = string.Format("{0}{1}: {2}°C",
                            p["name"].Value<string>(), 
                            string.IsNullOrEmpty(country) ? string.Empty : string.Format(" ({0})", country),
                            Math.Round(p["main"]["temp"].Value<decimal>(), 1));

                        result.Add(wsr);
                    }
                }
            }
            catch { }

            return result;
        }

        public string Prefix { get { return "x"; } }
        
        public bool IsConfigurable
        {
            get { return false;  }
        }

        public void Configure()
        {
        }

        public BitmapImage SearchIcon { get; private set; }

        private BitmapImage GetSearchIcon()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Weather.Images.sun.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            image.Freeze();
            return image;
        }


        public string Name
        {
            get { return "Weather"; }
        }
    }
}
