//------------------------------------------------------------------------------
// <copyright file="UpdateHelper.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using BriceLambson.ImageResizer.Models;

    internal static class UpdateHelper
    {
        public static Update FromSyndicationItem(SyndicationItem item)
        {
            Debug.Assert(item != null);

            var update = new Update();
            Version version;

            if (Version.TryParse(item.Title.Text, out version))
            {
                update.Version = version;
            }

            update.LastUpdatedTime = item.LastUpdatedTime;

            var link = item.Links.FirstOrDefault(
                l => String.IsNullOrWhiteSpace(l.RelationshipType)
                    || l.RelationshipType.Equals("alternate", StringComparison.OrdinalIgnoreCase));

            if (link != null)
            {
                update.Url = link.GetAbsoluteUri();
            }

            update.ReleaseStatus
                = item.Categories.Aggregate(
                    ReleaseStatus.None,
                    (rs, c) =>
                    {
                        ReleaseStatus releaseStatus;

                        if (Enum.TryParse<ReleaseStatus>(c.Name, true, out releaseStatus))
                        {
                            rs |= releaseStatus;
                        }

                        return rs;
                    });

            return update;
        }
    }
}