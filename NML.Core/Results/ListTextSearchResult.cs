using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NML.Core.Results
{
    public class ListTextSearchResult : BaseSearchResult
    {
        public ListTextSearchResult(IEnumerable<SearchTextResultListItem> list, string title)
        {
            ResultList = new ObservableCollection<SearchTextResultListItem>(list);
            Title = title;
        }

        public ObservableCollection<SearchTextResultListItem> ResultList { get; set; }

        public override bool HasResult
        {
            get
            {
                return ResultList != null && ResultList.Any();
            }
        }
    }
}
