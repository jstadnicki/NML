namespace NML
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ShowTrayIcon();

            this.DataContext = new MainWindowViewModel();
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
