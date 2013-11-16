namespace NML.Core
{
    public static class Utils
    {
        public static string PhraseWithoutPrefix(string phrase, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(phrase) && phrase.StartsWith(prefix + Constants.QueryPrefixSeparator))
            {
                return phrase.Remove(0, prefix.Length + Constants.QueryPrefixSeparator.Length);
            }

            return phrase;
        }
    }
}
