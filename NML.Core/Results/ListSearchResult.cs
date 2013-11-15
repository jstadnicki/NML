using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NML.Core.Interfaces;

namespace NML.Core.Results
{
    public class ListSearchResult : BaseSearchResult
    {
        public ListSearchResult()
        {
        }

        public ListSearchResult(IEnumerable<SearchResultListItem> list)
        {
            ResultList = new ObservableCollection<SearchResultListItem>(list);
        }

        public ObservableCollection<SearchResultListItem> ResultList { get; set; }
    }
}
