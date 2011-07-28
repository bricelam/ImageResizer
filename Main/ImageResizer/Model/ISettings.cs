//------------------------------------------------------------------------------
// <copyright file="ISettings.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface ISettings
    {
        string FileNameFormat { get; set; }
        int QualityLevel { get; set; }
        bool ReplaceOriginals { get; set; }
        ResizeSize SelectedSize { get; set; }
        bool ShrinkOnly { get; set; }
    }
}
