namespace NML
{
    using System.Windows.Forms;

    using NML.Core.Results;
    using NML.Utils;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private KeyboardHandler keyboardHandler;

        private NotifyIcon icon;

        private void keyboardHandler_ShortcutPressed(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;

            this.Activate();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            keyboardHandler.Dispose();
        }

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            /*
            var list = new ObservableCollection<BaseSearchResult>();
            list.Add(new TextSearchResult { Title = "TextSearchResult", Text = "Text", IconUrl = "icon" });
            rdSearchResults.ItemsSource = list;
             */

            // Registering shortcut
            keyboardHandler = new KeyboardHandler(this);
            keyboardHandler.ShortcutPressed += keyboardHandler_ShortcutPressed;

            // Displaying tray icon
            this.ShowTrayIcon();
        }

        private void ShowTrayIcon()
        {
            this.icon = new System.Windows.Forms.NotifyIcon();
            icon.Visible = true;
            icon.DoubleClick += this.TrayIconDoubleClick;
            icon.Icon = new System.Drawing.Icon("nml.ico");
        }

        private void TrayIconDoubleClick(object sender, System.EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
#if DEBUG
            base.OnClosing(e);
            this.icon.Visible = false;
#else
            e.Cancel = true;
            this.Hide();
#endif

        }
    }
}
