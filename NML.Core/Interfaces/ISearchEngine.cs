namespace NML.Core.Interfaces
{
    public interface ISearchEngine
    {
        ISearchResult Search(string phrase);

        string Prefix { get; }
    }
}