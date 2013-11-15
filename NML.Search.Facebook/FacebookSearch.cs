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
            if (token != null)
            {

                var friendsList = GetFriendsResult();
                 List<SearchResultListItem> friendsResult = friendsList.Where(x => x.Name.Contains(phrase))
                       .Select(x => new SearchResultListItem{Text = x.Name,Url=x.Url, IconUrl=x.Picture }).ToList();
                var feedList = GetFeedResult();
                List<SearchResultListItem> feedResult = feedList.Where(x => x.Name.Contains(phrase) || x.Caption.Contains(phrase) || x.Description.Contains(phrase))
                    .Select(x => new SearchResultListItem{ Text = x.Name, IconUrl=x.Picture, Url = x.Picture}).ToList();
                friendsResult.AddRange(feedResult);
                return new ListSearchResult(friendsResult, "Facebook");
            }
            return new TextSearchResult() { Text = "Please configure your facebook" };
        }

        private static List<FacebookUser> GetFriendsResult()
        {
            var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
            FacebookClient fc = new FacebookClient(token);
            fc.AppId = FbSettings.AppId;
            fc.AppSecret = FbSettings.AppSecret;
            var friendsList = new List<FacebookUser>();
            dynamic friends = fc.Get("me/friends");
            foreach (var f in friends.data)
            {
                friendsList.Add(new FacebookUser { Name = f.name, Id = f.id });
            }
            return friendsList;
        }

        private static List<FacebookFeed> GetFeedResult()
        {
            var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
            FacebookClient fc = new FacebookClient(token);
            fc.AppId = FbSettings.AppId;
            fc.AppSecret = FbSettings.AppSecret;
            var feedList = new List<FacebookFeed>();
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
            return feedList;
        }

        public string Prefix
        {
            get
            {
                return "f ";
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
