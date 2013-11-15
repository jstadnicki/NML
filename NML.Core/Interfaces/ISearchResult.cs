namespace NML.Core.Interfaces
{
    public interface ISearchResult
    {
        string Title { get; }
        string IconUrl { get; }
    }

    public abstract class BaseSearchResult : ISearchResult
    {
        public string Title { get; private set; }
        public string IconUrl { get; private set; }
    }
}
