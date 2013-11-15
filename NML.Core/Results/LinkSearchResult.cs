using NML.Core.Interfaces;

namespace NML.Core.Results
{
    public class LinkSearchResult : BaseSearchResult
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
    }
}
