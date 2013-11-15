using NML.Core.Interfaces;

namespace NML.Core.Results
{
    public abstract class BaseSearchResult : ISearchResult
    {
        public string Title { get; set; }
        public string IconUrl { get; set; }
        public abstract bool HasResult { get; }
    }
}
