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

        private bool appexit;

        private void keyboardHandler_ShortcutPressed(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }

            this.WindowState = WindowState.Normal;
            this.Show();
            this.Activate();
            tbSearch.SelectAll();
            tbSearch.Focus();
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
            this.Hide();
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

            var mi = new MenuItem { Name = "Lipa", Text = "Lipa" };
            mi.Click += mi_Click;
            icon.ContextMenu = new ContextMenu(new[] { mi });
        }

        private void mi_Click(object sender, EventArgs e)
        {
            this.appexit = true;
            this.Close();
        }

        private void TrayIconDoubleClick(object sender, System.EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Show();
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
            if (this.appexit)
            {
                base.OnClosing(e);
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
#endif
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
