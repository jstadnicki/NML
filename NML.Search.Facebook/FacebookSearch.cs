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
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Facebook.Images.fb.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            SearchIcon = image;
        }

        public ISearchResult Search(string phrase)
        {
            var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
            if (token == null)
            {
                return new TextSearchResult() { Text = "Please configure your facebook" };
            }

            var friendsList = GetFriendsResult();
            var feedList = GetFeedResult();

            if (friendsList == null || feedList == null)
            {
                ConfigurationHelper.SetConfiguration(FbSettings.TokenKey, "");
                return new TextSearchResult() { Text = "Please configure your facebook" };
            }

            List<SearchResultListItem> friendsResult = friendsList.Where(x => x.Name.Contains(phrase))
                  .Select(x => new SearchResultListItem { Text = x.Name, Url = x.Url, IconUrl = x.Picture }).ToList();

            List<SearchResultListItem> feedResult = feedList.Where(x => x.Name.Contains(phrase) || x.Caption.Contains(phrase) || x.Description.Contains(phrase))
                .Select(x => new SearchResultListItem { Text = x.Name, IconUrl = x.Picture, Url = x.Picture }).ToList();
            friendsResult.AddRange(feedResult);

            if ((DateTime.UtcNow - lastUpdateTime).TotalMinutes > 30)
            {
                friendsList = null;
                feedList = null;
            }
            return new ListSearchResult(friendsResult, "Facebook");

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
                catch (Exception ex)
                {
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
                        feedList.Add(new FacebookFeed
                        {
                            Name = f.name,
                            Id = f.id,
                            Caption = f.caption,
                            Description = f.description,
                            From = new FacebookUser { Name = f.from.name, Id = f.from.id },
                            Link = f.link,
                            Picture = f.picture

                        });
                    }
                }
                catch (Exception ex)
                {
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
    }
}
