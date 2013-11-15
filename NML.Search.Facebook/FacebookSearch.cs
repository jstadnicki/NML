using System;
using NML.Core;
using NML.Core.Interfaces;
using NML.Core.Results;
using Facebook;

namespace NML.Search.Facebook
{
    public class FacebookSearch : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            var token = ConfigurationHelper.GetConfiguration(FbSettings.TokenKey);
              if (token == null)
              {
                  FacebookClient fc = new FacebookClient(token);
                  fc.AppId = FbSettings.AppId;
                  fc.AppSecret = FbSettings.AppSecret;

        
              }
              return new TextSearchResult() { Text = "Please configure your facebook" };
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
    }
}
