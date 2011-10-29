//------------------------------------------------------------------------------
// <copyright file="ReleaseStatus.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System;

    [Flags]
    public enum ReleaseStatus
    {
        None = 0,
        Stable = 1,
        Beta = 2,
        Alpha = 4
    }
}