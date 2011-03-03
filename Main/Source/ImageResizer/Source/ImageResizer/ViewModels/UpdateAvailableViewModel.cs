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
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Properties;
    using Microsoft.Practices.Prism.Commands;

    internal class UpdateAvailableViewModel
    {
        private Settings settings;
        private SyndicationItem item;

        public UpdateAvailableViewModel(Settings settings, SyndicationItem item)
        {
            this.settings = settings;
            this.item = item;

            this.DownloadCommand = new DelegateCommand(this.Download);
            this.CloseCommand = new DelegateCommand(this.Close);
        }

        public ICommand DownloadCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public bool DontCheckForUpdates
        {
            get { return !this.settings.CheckForUpdates; }
            set { this.settings.CheckForUpdates = !value; }
        }

        private void Download()
        {
            var downloadUrl = this.item.Links.Single().Uri.ToString();

            // Open with the default browser
            Process.Start(downloadUrl);
        }

        private void Close()
        {
            this.settings.Save();
        }
    }
}
