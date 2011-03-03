//------------------------------------------------------------------------------
// <copyright file="UpdaterService.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Services
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using BriceLambson.ImageResizer.Model;
    using BriceLambson.ImageResizer.Properties;

    internal class UpdaterService : IDisposable
    {
        private Settings settings;
        private BackgroundWorker backgroundWorker;

        public UpdaterService(Settings settings)
        {
            this.settings = settings;

            // TODO: What happens if this takes too long?
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += (_, e) => this.CheckForUpdates();
        }

        ~UpdaterService()
        {
            this.Dispose(false);
        }

        public event EventHandler<UpdateAvailableEventArgs> UpdateAvailable;

        public void CheckForUpdatesAsync()
        {
            if (!this.backgroundWorker.IsBusy && this.settings.CheckForUpdates)
            {
                this.backgroundWorker.RunWorkerAsync();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnUpdateAvaliable(SyndicationItem item)
        {
            if (this.UpdateAvailable != null)
            {
                this.UpdateAvailable(this, new UpdateAvailableEventArgs(item));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.backgroundWorker != null)
            {
                this.backgroundWorker.Dispose();
            }
        }

        // TODO: Handle more possible errors
        private void CheckForUpdates()
        {
            var updateUrl = this.settings.UpdateUrl;
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var updateFilter = this.settings.UpdateFilter;

            XmlReader reader;

            try
            {
                reader = XmlReader.Create(updateUrl);
            }
            catch (WebException)
            {
                // Bail on network errors
                return;
            }

            var formatter = new Atom10FeedFormatter();

            try
            {
                formatter.ReadFrom(reader);
            }
            catch (XmlException)
            {
                // Bail on invalid format
                return;
            }

            var item = formatter.Feed.Items.Where(i =>
                {
                    Version version;
                    var versionParsed = Version.TryParse(i.Title.Text, out version);

                    return versionParsed &&
                        version > currentVersion &&
                        i.Categories.Any(c => ((int)updateFilter & (int)Enum.Parse(typeof(ReleaseStatus), c.Name, true)) != 0);
                }).OrderByDescending(i => i.LastUpdatedTime).FirstOrDefault();

            if (item != null)
            {
                this.OnUpdateAvaliable(item);
            }
        }
    }
}
