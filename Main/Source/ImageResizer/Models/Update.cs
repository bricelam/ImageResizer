//------------------------------------------------------------------------------
// <copyright file="Update.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System;

    public class Update
    {
        public Version Version { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public Uri Url { get; set; }
        public ReleaseStatus ReleaseStatus { get; set; }
    }
}