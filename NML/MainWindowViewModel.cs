namespace NML
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    using NML.Core.Interfaces;

    public class MainWindowViewModel : INotifyPropertyChanged
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
            this.syncObject = new object();
            this.cancelTokens = new List<CancellationTokenSource>();
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

        private List<CancellationTokenSource> cancelTokens;

        private object syncObject;

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
            this.CancelPrevious();
            this.Results.Clear();

            var cancellationToken = new CancellationTokenSource();
            var cancel = cancellationToken.Token;

            lock (this.syncObject)
            {
                this.cancelTokens.Add(cancellationToken);
            }

            List<Task> tasks = new List<Task>();
            foreach (var engine in this.engines)
            {
                var queryTask = Task.Factory.StartNew(() => { return engine.Search(query); });

                var continueTask = queryTask.ContinueWith(
                    x =>
                    {
                        if (x.Result.HasResult)
                        {
                            this.Results.Add(x.Result);
                        }
                    }, cancel, TaskContinuationOptions.NotOnCanceled, TaskScheduler.FromCurrentSynchronizationContext());

                tasks.Add(continueTask);

            }

            Task.WhenAll(tasks).ContinueWith(
                _ =>
                {
                    lock (this.syncObject)
                    {
                        if (this.cancelTokens.Contains(cancellationToken))
                        {
                            this.cancelTokens.Remove(cancellationToken);
                        }
                    }
                });
        }

        private void CancelPrevious()
        {
            lock (this.syncObject)
            {
                foreach (var cancellationTokenSource in this.cancelTokens)
                {
                    cancellationTokenSource.Cancel();
                }

                this.cancelTokens.Clear();
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