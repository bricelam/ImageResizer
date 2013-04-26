//------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Properties
{
    using BriceLambson.ImageResizer.Models;

    internal sealed partial class Settings
    {
        public ResizeSize SelectedSize
        {
            get
            {
                var defaultSizes = AdvancedSettings.Default.DefaultSizes;

                if (SelectedIndex < 0 || SelectedIndex >= defaultSizes.Count)
                {
                    return CustomSize;
                }

                return defaultSizes[SelectedIndex];
            }

            set
            {
                var defaultSizes = AdvancedSettings.Default.DefaultSizes;
                var index = defaultSizes.IndexOf(value);

                if (index == -1)
                {
                    SelectedIndex = defaultSizes.Count;
                }
                else
                {
                    SelectedIndex = index;
                }
            }
        }
    }
}