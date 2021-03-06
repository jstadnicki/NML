﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NML.Core.Results
{
    public class ListSearchResult : BaseSearchResult
    {
        public ListSearchResult(IEnumerable<SearchResultListItem> list, string title)
        {
            ResultList = new ObservableCollection<SearchResultListItem>(list);
            Title = title;
        }

        public ObservableCollection<SearchResultListItem> ResultList { get; set; }

        public override bool HasResult
        {
            get
            {
                return ResultList != null && ResultList.Any();
            }
        }
    }
}
