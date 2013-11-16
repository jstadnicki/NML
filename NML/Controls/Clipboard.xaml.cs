using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NML.Controls
{
    public partial class Clipboard : UserControl
    {
        public Clipboard()
        {
            InitializeComponent();
        }

        private void Clipboard_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Clipboard.SetText(Text);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Clipboard), new PropertyMetadata(string.Empty));

        
    }
}
