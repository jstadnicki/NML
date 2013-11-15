using NML.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
