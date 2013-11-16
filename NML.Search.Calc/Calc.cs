using System.Reflection;
using System.Windows.Media.Imaging;
using NML.Core.Interfaces;
using NML.Core.Results;

namespace NML.Search.Calc
{
    public class Calc : ISearchEngine
    {
        public ISearchResult Search(string phrase)
        {
            var result = new TextSearchResult("Calculator");

            if (!string.IsNullOrWhiteSpace(phrase))
            {
                var ex = new NCalc.Expression(phrase);
                try
                {
                    // TODO: allow sin, now only Sin works
                    result.Text = string.Format("{0} = {1}", phrase, ex.Evaluate());
                }
                catch
                {
                    result.Text = null;
                }
            }

            result.SearchIcon = GetSearchIcon();
            return result;
        }

        public string Prefix
        {
            get { return "c"; }
        }


        public bool IsConfigurable
        {
            get { return false; }
        }

        public void Configure()
        {
         
        }

        public BitmapImage SearchIcon { get; private set; }

        private BitmapImage GetSearchIcon()
        {
            var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("NML.Search.Calc.Images.calc.png");
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = s;
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}
