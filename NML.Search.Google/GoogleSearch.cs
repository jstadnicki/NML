using System;
using NML.Core;
using NML.Core.Interfaces;

namespace NML.Search.Google
{
    public class GoogleSearch : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            throw new NotImplementedException();
        }

        public string Prefix
        {
            get
            {
                return "g ";
            }
        }
    }
}
