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
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel.Syndication;
    using System.Threading.Tasks;
    using System.Xml;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;

    internal class UpdaterService
    {
        public async Task<Update> CheckForUpdatesAsync(string updateUrl, UpdateFilter updateFilter)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(updateUrl));

            var reader = XmlReader.Create(updateUrl);
            var formatter = new Atom10FeedFormatter();

            await TaskEx.Run(() => formatter.ReadFrom(reader));

            return (from i in formatter.Feed.Items
                    let u = UpdateHelper.FromSyndicationItem(i)
                    where u.Version > Assembly.GetExecutingAssembly().GetName().Version
                        && ((int)updateFilter & (int)u.ReleaseStatus) != 0
                    orderby u.LastUpdatedTime descending
                    select u).FirstOrDefault();
        }
    }
}