//------------------------------------------------------------------------------
// <copyright file="UpdateAvailableEventArgs.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Services
{
    using System;
    using System.ServiceModel.Syndication;

    internal class UpdateAvailableEventArgs : EventArgs
    {
        public UpdateAvailableEventArgs(SyndicationItem item)
        {
            this.Item = item;
        }

        // TODO: Probably ought to send a domain-specific object
        public SyndicationItem Item { get; set; }
    }
}
