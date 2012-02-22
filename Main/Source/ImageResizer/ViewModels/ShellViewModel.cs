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
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    internal class ShellViewModel : ViewModelBase
    {
        private object _currentPage;
        private Task<Parameters> _parseArgsTask;

        public ShellViewModel(string[] args)
        {
            Contract.Requires(args != null);

            if (AdvancedSettings.Default.CheckForUpdates)
            {
                CheckForUpdatesAsync();
            }

            var inputPage = new InputPageViewModel();
            inputPage.Completed += HandleInputPageCompleted;

            _currentPage = inputPage;

            _parseArgsTask = ParametersHelper.ParseAsync(args);
        }

        public object CurrentPage
        {
            get { return _currentPage; }
            set { Set(() => CurrentPage, ref _currentPage, value); }
        }

        private static void Close()
        {
            Messenger.Default.Send(new CloseShellMessage());
        }

        private async void HandleInputPageCompleted(object sender, InputPageCompletedEventArgs e)
        {
            Contract.Requires(e != null);

            if (e.Cancelled)
            {
                Close();
                return;
            }

            var parameters = await _parseArgsTask;
            var progressPage = new ProgressPageViewModel(parameters);
            progressPage.Completed += HandleProgressPageCompleted;

            CurrentPage = progressPage;

            // Kick-off resizing
            progressPage.ResizeAsync();
        }

        private void HandleProgressPageCompleted(object sender, ProgressPageCompletedEventArgs e)
        {
            Contract.Requires(e != null);

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
                update
                    = await new UpdaterService().CheckForUpdatesAsync(
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