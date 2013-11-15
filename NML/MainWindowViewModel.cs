using System;
using System.Linq;
using NML.Core;

namespace NML
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using NML.Core.Interfaces;

    public class MainWindowViewModel :INotifyPropertyChanged
    {
        public ObservableCollection<ISearchResult> Results
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
            var unityContainer = new UnityContainer();
            unityContainer.LoadConfiguration();
            this.engines = new List<ISearchEngine>(unityContainer.ResolveAll<ISearchEngine>());
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

        private IEnumerable<ISearchEngine> GetFilteredEngines(string query)
        {
            var prefixIndex = query.IndexOf(Constants.QueryPrefixSeparator);
            
            if (prefixIndex > 0)
            {
                var queryPrefix = query.Substring(0, prefixIndex);
                return engines.Where(e => e.Prefix.Equals(queryPrefix, StringComparison.InvariantCultureIgnoreCase));
            }

            return this.engines;
        }

        private void TriggerSearchMechanism(string query)
        {
            var filteredEngines = GetFilteredEngines(query);

            foreach (var engine in filteredEngines)
            {
                Task.Factory.StartNew(
                    () =>
                        {
                            return engine.Search(query);
                        }).ContinueWith(x => this.Results.Add(x.Result), TaskScheduler.FromCurrentSynchronizationContext());

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