using System;
using NML.Core;
using NML.Core.Interfaces;
using NML.Core.Results;
using Facebook;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Reflection;

namespace NML.Search.Facebook
{
    public class FacebookSearch : ISearchEngine
    {
        public FacebookSearch()
        {
            SearchIcon = GetSearchIcon();
        }

        private const string ConfigureMessage = "Please configure your Facebook account";

        public ISearchResult Search(string phrase)
        {
            var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
            if (token == null)
            {
                return new TextSearchResult { Text = ConfigureMessage, Title = Name, SearchIcon = SearchIcon };
            }

            phrase = Utils.PhraseWithoutPrefix(phrase, Prefix);

            var friendsList = GetFriendsResult();
            var feedList = GetFeedResult();

            if (friendsList == null || feedList == null)
            {
                ConfigurationHelper.SetConfiguration(FbSettings.TokenKey, "");
                return new TextSearchResult() { Text = ConfigureMessage, Title = Name, SearchIcon = SearchIcon };
            }

            List<ImageResultListItem> friendsResult = friendsList.Where(x => x.Name.ToLowerInvariant().Contains(phrase.ToLowerInvariant()))
                  .Select(x => new ImageResultListItem { Text = x.Name, Url = x.Url, IconUrl = x.Picture }).ToList();

            List<ImageResultListItem> feedResult = feedList.Where(x => x.Name.ToLowerInvariant().Contains(phrase.ToLowerInvariant()) || 
                x.Caption.ToLowerInvariant().Contains(phrase.ToLowerInvariant()) || x.Description.ToLowerInvariant().Contains(phrase.ToLowerInvariant()))
                .Select(x => new ImageResultListItem { Text = x.Name, IconUrl = x.Picture, Url = x.Picture }).ToList();
            friendsResult.AddRange(feedResult);

            if ((DateTime.UtcNow - lastUpdateTime).TotalMinutes > 30)
            {
                friendsList = null;
                feedList = null;
            }
            var result = new ImageTextListResult(friendsResult, "Facebook");
            result.SearchIcon = SearchIcon;
            return result;
        }

        private static List<FacebookUser> friendsList = null;
        private static List<FacebookUser> GetFriendsResult()
        {
            if (friendsList == null)
            {
                try
                {
                    var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
                    FacebookClient fc = new FacebookClient(token);
                    fc.AppId = FbSettings.AppId;
                    fc.AppSecret = FbSettings.AppSecret;
                    friendsList = new List<FacebookUser>();
                    dynamic friends = fc.Get("me/friends");
                    foreach (var f in friends.data)
                    {
                        friendsList.Add(new FacebookUser { Name = f.name, Id = f.id });
                    }
                    lastUpdateTime = DateTime.UtcNow;
                }
                catch
                {
                    friendsList = null;
                    return null;
                }
            }
            return friendsList;
        }

        private static DateTime lastUpdateTime = DateTime.MinValue;
        private static List<FacebookFeed> feedList = null;
        private static List<FacebookFeed> GetFeedResult()
        {
            if (feedList == null)
            {
                try
                {
                    var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
                    FacebookClient fc = new FacebookClient(token);
                    fc.AppId = FbSettings.AppId;
                    fc.AppSecret = FbSettings.AppSecret;
                    feedList = new List<FacebookFeed>();

                    dynamic feed = fc.Get("me/feed");

                    foreach (var f in feed.data)
                    {
                        if (!string.IsNullOrEmpty(f.name))
                        {
                            feedList.Add(new FacebookFeed
                            {
                                Name = f.name ?? string.Empty,
                                Id = f.id,
                                Caption = f.caption ?? string.Empty,
                                Description = f.description ?? string.Empty,
                                From = new FacebookUser { Name = f.from.name, Id = f.from.id },
                                Link = f.link ?? string.Empty,
                                Picture = f.picture ?? string.Empty

                            });
                        }
                    }
                }
                catch
                {
                    feedList = null;
                    return null;
                }
                lastUpdateTime = DateTime.UtcNow;
            }
            return feedList;
        }

        public string Prefix
        {
            get
            {
                return "f";
            }
        }

        public bool IsConfigurable
        {
            get { return true; }
        }

        public void Configure()
        {
            var s = new NML.Search.Facebook.Configuration();
            s.Show();
        }

        public BitmapImage SearchIcon { get; private set; }

        public string Name
        {
            get { return "Facebook"; }
        }

        private BitmapImage GetSearchIcon()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Facebook.Images.fb.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}
