using NML.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NML.Core.Results
{
    public abstract class BaseSearchResult : ISearchResult
    {
        public string Title { get; set; }
        public string IconUrl { get; set; }
    }
}
