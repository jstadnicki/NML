namespace NML.Core.Results
{
    public class LinkSearchResult : BaseSearchResult
    {
        public string Text { get; set; }
        public string Url { get; set; }

        public override bool HasResult
        {
            get { return !string.IsNullOrWhiteSpace(Text) && !string.IsNullOrWhiteSpace(Url); }
        }
    }
}
