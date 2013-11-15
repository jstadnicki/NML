namespace NML.Core.Interfaces
{
    public interface ISearchResult
    {
        string Title { get; }
        string IconUrl { get; }
        bool HasResult { get; }
    }
}
