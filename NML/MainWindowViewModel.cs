namespace NML
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using NML.Core.Interfaces;

    public class MainWindowViewModel :INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            this.LoadPlugins();
        }

        private void LoadPlugins()
        {
            var unityContainer = new UnityContainer();
            unityContainer.LoadConfiguration();
            this.engines = (IList<ISearchEngine>)unityContainer.ResolveAll<ISearchEngine>();
        }

        private string queryText;

        private IList<ISearchEngine> engines;

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
            }
        }

        private void ShouldTriggerSearch(string query)
        {
            if (query.Length > 3)
            {
                this.TriggerSearchMechanism(query);
            }
        }

        private void TriggerSearchMechanism(string query)
        {
            foreach (var engine in this.engines)
            {
                var r = engine.Search(query);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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