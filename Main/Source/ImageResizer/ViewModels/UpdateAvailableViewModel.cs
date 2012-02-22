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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using GalaSoft.MvvmLight.Command;

    internal class UpdateAvailableViewModel
    {
        private readonly Update _update;
        private readonly ICommand _downloadCommand;
        private readonly ICommand _closeCommand;

        public UpdateAvailableViewModel(Update update)
        {
            Contract.Requires(update != null);

            _downloadCommand = new RelayCommand(Download);
            _closeCommand = new RelayCommand(Close);

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

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Participates in data binding")]
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