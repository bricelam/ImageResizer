//------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    internal class ShellViewModel : ViewModelBase
    {
        private readonly string[] _args;
        private object _currentPage;

        public ShellViewModel(string[] args)
        {
            Debug.Assert(args != null);

            _args = args;

            Messenger.Default.Register<ShellLoadedMessage>(this, m => OnLoaded());
        }

        public object CurrentPage
        {
            get { return _currentPage; }
            set { Set(() => CurrentPage, ref _currentPage, value); }
        }

        private void OnLoaded()
        {
            // Upgrade settings from previous versions
            if (AdvancedSettings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                AdvancedSettings.Default.Upgrade();

                AdvancedSettings.Default.UpgradeRequired = false;
                AdvancedSettings.Default.Save();
            }

            var inputPage = new InputPageViewModel();
            inputPage.Completed += HandleInputPageCompleted;
            _currentPage = inputPage;

            if (AdvancedSettings.Default.CheckForUpdates)
            {
                CheckForUpdatesAsync();
            }
        }

        private static void Close()
        {
            Messenger.Default.Send(new CloseShellMessage());
        }

        private void HandleInputPageCompleted(object sender, InputPageCompletedEventArgs e)
        {
            Debug.Assert(e != null);

            if (e.Cancelled)
            {
                Close();
                return;
            }

            var progressPage = new ProgressPageViewModel(_args);
            progressPage.Completed += HandleProgressPageCompleted;

            CurrentPage = progressPage;
        }

        private void HandleProgressPageCompleted(object sender, ProgressPageCompletedEventArgs e)
        {
            Debug.Assert(e != null);

            if (!e.Errors.Any())
            {
                Close();
            }

            var resultsPage = new ResultsPageViewModel(e.Errors);
            resultsPage.Completed += HandleResultsPageCompleted;

            CurrentPage = resultsPage;
        }

        private void HandleResultsPageCompleted(object sender, EventArgs e)
        {
            Close();
        }

        private async void CheckForUpdatesAsync()
        {
            Update update = null;

            try
            {
                update = await new UpdaterService().CheckForUpdatesAsync(
                    AdvancedSettings.Default.UpdateUrl,
                    AdvancedSettings.Default.UpdateFilter);
            }
            catch
            {
            }

            if (update != null)
            {
                Messenger.Default.Send(new UpdateAvailableMessage(update));
            }
        }
    }
}