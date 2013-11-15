namespace NML
{
    using NML.Core.Results;
    using NML.Utils;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media.Imaging;

    public partial class MainWindow : Window
    {
        private KeyboardHandler keyboardHandler;

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

            // Registering shortcut
            keyboardHandler = new KeyboardHandler(this);
            keyboardHandler.ShortcutPressed += keyboardHandler_ShortcutPressed;

            // Displaying tray icon
            this.ShowTrayIcon();
        }

        private void ShowTrayIcon()
        {
            var ni = new System.Windows.Forms.NotifyIcon();
            ni.Visible = true;
            ni.DoubleClick += this.TrayIconDoubleClick;
            ni.Icon = new System.Drawing.Icon("nml.ico");
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
#if !DEBUG
            e.Cancel = true;
            this.Hide();
#endif
        }
    }
}
