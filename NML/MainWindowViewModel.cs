﻿using System;
using System.Linq;
using NML.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NML.Core.Interfaces;

namespace NML
{
    using Timer = System.Timers.Timer;

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
            this.searchInProgress = false;
            this.timer = new System.Timers.Timer();
            this.timer.Interval = 500;
            this.timer.Elapsed += timer_Elapsed;
            this.timer.AutoReset = false;
            this.dispatcher = Dispatcher.CurrentDispatcher;

        }

        private bool searchInProgress;

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.dispatcher.Invoke(this.TriggerSearchMechanism);
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

        private Timer timer;

        private Dispatcher dispatcher;

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.queryText = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    this.timer.Stop();
                    this.Results.Clear();
                }
                else
                {
                    this.UpdateTimes();
                }
            }
        }

        private void UpdateTimes()
        {
            this.timer.Stop();
            this.timer.Start();
        }

        private void TriggerSearchMechanism()
        {
            this.SearchInProgress = true;
            this.CancelPrevious();
            this.Results.Clear();

            var cancellationToken = new CancellationTokenSource();
            var cancel = cancellationToken.Token;

            lock (this.syncObject)
            {
                this.cancelTokens.Add(cancellationToken);
            }

            List<Task> tasks = new List<Task>();

            var filteredEngines = GetFilteredEngines(this.QueryText);

            foreach (var engine in filteredEngines)
            {
                var queryTask = Task.Factory.StartNew(() => { return engine.Search(this.QueryText); });

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
                            this.SearchInProgress = false;
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

        public bool SearchInProgress
        {
            get
            {
                return searchInProgress;
            }
            set
            {
                searchInProgress = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchInProgress"));
                }
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
    }
}