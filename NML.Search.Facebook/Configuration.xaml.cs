using NML.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NML.Search.Facebook
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration()
        {
            InitializeComponent();
            webBrowser1.Navigate(new Uri("https://www.facebook.com/dialog/oauth?client_id=" + FbSettings.AppId + "&redirect_uri=https://www.facebook.com/connect/login_success.html&display=popup&scope=publish_stream,user_status&response_type=token"));
        }

        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            if (webBrowser1.Source.Fragment.Contains("access_token"))
            {
                var fragments = webBrowser1.Source.Fragment.Split('&');
                foreach (var fragmnet in fragments)
                {
                    if (fragmnet.Contains("access_token"))
                    {
                        var token = fragmnet.Split('=')[1];
                        ConfigurationHelper.SetConfiguration(FbSettings.TokenKey, token);
                        this.Close();
                    }
                }
            }
        }
    }
}
