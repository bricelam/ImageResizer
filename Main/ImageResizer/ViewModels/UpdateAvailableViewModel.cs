//------------------------------------------------------------------------------
// <copyright file="UpdateAvailableViewModel.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using Microsoft.Practices.Prism.Commands;

    internal class UpdateAvailableViewModel
    {
        private readonly Update _update;
        private readonly ICommand _downloadCommand;
        private readonly ICommand _closeCommand;

        public UpdateAvailableViewModel(Update update)
        {
            Contract.Requires(update != null);

            _downloadCommand = new DelegateCommand(Download);
            _closeCommand = new DelegateCommand(Close);

            _update = update;
        }

        public ICommand DownloadCommand
        {
            get { return _downloadCommand; }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public bool DontCheckForUpdates
        {
            get { return !AdvancedSettings.Default.CheckForUpdates; }
            set { AdvancedSettings.Default.CheckForUpdates = !value; }
        }

        private void Download()
        {
            var downloadUrl = _update.Url.ToString();

            // Open with the default browser
            Process.Start(downloadUrl);
        }

        private void Close()
        {
            AdvancedSettings.Default.Save();
        }
    }
}