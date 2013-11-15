using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Calc
{
    public class Calc : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            var ex = new NCalc.Expression(phrase);
            var result = new TextSearchResult();

            try
            {
                // TODO: allow sin, now only Sin works
                result.Text = ex.Evaluate().ToString();
            }
            catch
            {
                result.Text = null;
            }

            return result;
        }

        public string Prefix
        {
            get { return "c "; }
        }
    }
}
