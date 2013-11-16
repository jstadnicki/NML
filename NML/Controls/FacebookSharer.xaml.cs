using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NML.Controls
{
    public partial class FacebookSharer : UserControl
    {
        public FacebookSharer()
        {
            InitializeComponent();
        }

        private void FacebookSharer_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = new Process();
            p.StartInfo.FileName = string.Format("https://www.facebook.com/sharer.php?u={0}", HttpUtility.UrlEncode(ShareUrl));
            p.Start();
        }

        public string ShareUrl
        {
            get { return (string)GetValue(ShareUrlProperty); }
            set { SetValue(ShareUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShareUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShareUrlProperty =
            DependencyProperty.Register("ShareUrl", typeof(string), typeof(FacebookSharer), new PropertyMetadata(string.Empty));

        
    }
}
