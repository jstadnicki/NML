namespace NML.Core.Results
{
    public class TextSearchResult : BaseSearchResult
    {
        public TextSearchResult()
        { }

        public TextSearchResult(string title)
        {
            Title = title;
        }

        public string Text { get; set; }

        public override bool HasResult
        {
            get { return !string.IsNullOrWhiteSpace(Text); }
        }
    }
}
