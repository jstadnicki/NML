namespace NML
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using NML.Core.Interfaces;
    using NML.Search.Google;

    public class MainWindowViewModel :INotifyPropertyChanged
    {
        private ObservableCollection<ISearchResult> Results
        {
            get
            {
                return this.results;
            }
            set
            {
                this.results = value;
                this.OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            this.LoadPlugins();
            this.Results = new ObservableCollection<ISearchResult>();
        }

        private void LoadPlugins()
        {
//            var unityContainer = new UnityContainer();
//            unityContainer.LoadConfiguration();
//            this.engines = (IList<ISearchEngine>)unityContainer.ResolveAll<ISearchEngine>();

            this.engines = new List<ISearchEngine>();
            this.engines.Add(new GoogleSearch());
        }

        private string queryText;

        private IList<ISearchEngine> engines;

        private ObservableCollection<ISearchResult> results;

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
                Task.Factory.StartNew(
                    () =>
                        {
                            return engine.Search(query);
                        }).ContinueWith(x => this.Results.Add(x.Result), TaskScheduler.Current);

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