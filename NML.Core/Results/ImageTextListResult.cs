using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Core.Results
{
    public class ImageTextListResult : BaseSearchResult
    {
        public ImageTextListResult(IEnumerable<ImageResultListItem> list, string title)
        {
            ResultList = new ObservableCollection<ImageResultListItem>(list);
            Title = title;
        }

        public ObservableCollection<ImageResultListItem> ResultList { get; set; }

        public override bool HasResult
        {
            get
            {
                return ResultList != null && ResultList.Any();
            }
        }
    }
}
