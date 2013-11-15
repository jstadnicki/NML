using System;
using NML.Core.Interfaces;

namespace NML.Search.Wikipedia
{
    public class WikiSearch : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            throw new NotImplementedException();
            // http://en.wikipedia.org/w/api.php?action=query&list=search&srsearch=brad+pitt&srprop=timestamp&format=json
        }

        public string Prefix
        {
            get { return "w "; }
        }
    }
}
