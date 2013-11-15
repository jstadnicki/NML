using System.Reflection;
using System.Windows.Media.Imaging;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Calc
{
    public class Calc : ISearchEngine
    {
        public Calc()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Calc.Images.calc.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            SearchIcon = image;
        }

        public ISearchResult Search(string phrase)
        {
            var ex = new NCalc.Expression(phrase);
            var result = new TextSearchResult("Calculator");

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


        public bool IsConfigurable
        {
            get { return false; }
        }

        public void Configure()
        {
         
        }

        public BitmapImage SearchIcon { get; private set; }
    }
}
