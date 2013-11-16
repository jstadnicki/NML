namespace NML
{
    using System.Windows.Forms;
    using System.Windows.Input;

    using NML.Core.Results;
    using NML.Utils;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using KeyEventArgs = System.Windows.Input.KeyEventArgs;

    public partial class MainWindow : Window
    {
        private KeyboardHandler keyboardHandler;

        private NotifyIcon icon;

        private void keyboardHandler_ShortcutPressed(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;

            this.Activate();

            tbSearch.SelectAll();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }   

        private void Window_Closed(object sender, EventArgs e)
        {
            keyboardHandler.Dispose();
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            
            // Registering shortcut
            keyboardHandler = new KeyboardHandler(this);
            keyboardHandler.ShortcutPressed += keyboardHandler_ShortcutPressed;

            // Displaying tray icon
            this.ShowTrayIcon();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            this.Width = workingArea.Width / 3;
            this.Left = workingArea.Width - this.Width;

            this.Top = workingArea.Top;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
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

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
    }
}
