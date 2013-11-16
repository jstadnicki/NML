using NML.Core.Interfaces;
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

namespace NML.Controls
{
    /// <summary>
    /// Interaction logic for ResultDisplay.xaml
    /// </summary>
    public partial class SettingsDisplay : ItemsControl
    {
        public SettingsDisplay()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in (this.DataContext as SettingsWindowViewModel).Results)
            {
                if (item.Name == ((Button)sender).Tag) { item.Configure(); }
                
            }
        }
    }
}
