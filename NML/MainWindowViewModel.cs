namespace NML
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using NML.Annotations;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string queryText;

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.ShouldTriggerSearch(value);
                this.queryText = value;
                this.OnPropertyChanged();
            }
        }

        private void ShouldTriggerSearch(string query)
        {
            if (query.Length > 3)
            {
                MessageBox.Show("go");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}