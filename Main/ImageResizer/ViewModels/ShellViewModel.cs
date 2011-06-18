//------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.ServiceLocation;

    internal class ShellViewModel : IDisposable, INotifyPropertyChanged
    {
        private Settings settings;
        private IEventAggregator eventAggregator;
        private InputPageViewModel inputPage;
        private ProgressPageViewModel progressPage;
        private ResultsPageViewModel resultsPage;
        private UpdaterService updaterService;
        private object currentPage;

        public ShellViewModel(string[] args)
        {
            this.settings = Settings.Default;

            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            this.inputPage = new InputPageViewModel(this.settings, this.eventAggregator);
            this.inputPage.Completed += this.HandleInputPageCompleted;

            var parameterService = new ParameterService(args);
            this.progressPage = new ProgressPageViewModel(this.settings, parameterService);
            this.progressPage.Completed += this.HandleProgressPageCompleted;

            // TODO: This may not always be needed
            this.resultsPage = new ResultsPageViewModel();
            this.resultsPage.Completed += this.HandleResultsPageCompleted;

            this.updaterService = new UpdaterService(this.settings);
            this.updaterService.UpdateAvailable += this.HandleUpdateAvailable;

            // Kick-off updater
            this.updaterService.CheckForUpdatesAsync();

            this.currentPage = this.inputPage;
        }

        ~ShellViewModel()
        {
            this.Dispose(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object CurrentPage
        {
            get
            {
                return this.currentPage;
            }

            set
            {
                if (this.currentPage != value)
                {
                    this.currentPage = value;

                    // TODO: Extract this from a lambda for compile-time checking
                    this.OnPropertyChanged("CurrentPage");
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.progressPage != null)
                {
                    this.progressPage.Dispose();
                }

                if (this.updaterService != null)
                {
                    this.updaterService.Dispose();
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void HandleInputPageCompleted(object sender, InputPageCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Close();
                return;
            }

            this.CurrentPage = this.progressPage;

            // Kick-off resizing
            this.progressPage.ResizeAsync();
        }

        private void HandleProgressPageCompleted(object sender, ProgressPageCompletedEventArgs e)
        {
            if (e.Errors.Any())
            {
                this.resultsPage.Errors = e.Errors;
                this.currentPage = this.resultsPage;
            }
            else
            {
                this.Close();
            }
        }

        private void HandleResultsPageCompleted(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HandleUpdateAvailable(object sender, UpdateAvailableEventArgs e)
        {
            // TODO: Subscribe to this somewhere. Show notification?
            var updateAvailableViewModel = new UpdateAvailableViewModel(this.settings, e.Item);

            this.eventAggregator.GetEvent<UpdateAvailableEvent>().Publish(updateAvailableViewModel);
        }

        private void Close()
        {
            this.eventAggregator.GetEvent<CloseShellEvent>().Publish(null);
        }
    }
}
