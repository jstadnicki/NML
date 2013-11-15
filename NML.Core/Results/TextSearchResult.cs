using NML.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Core.Results
{
    public class TextSearchResult : BaseSearchResult
    {
        public string Text { get; set; }

        public override bool HasResult
        {
            get { return !string.IsNullOrWhiteSpace(Text); }
        }
    }
}
