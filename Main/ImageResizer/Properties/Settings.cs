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
    using BriceLambson.ImageResizer.Model;

    // TODO: Can this be partitioned into interfaces?
    internal sealed partial class Settings : ISettings
    {
        public ResizeSize SelectedSize
        {
            get
            {
                if (this.SelectedIndex < 0 || this.SelectedIndex >= this.DefaultSizes.Count)
                {
                    return this.CustomSize;
                }

                return this.DefaultSizes[this.SelectedIndex];
            }

            set
            {
                var index = this.DefaultSizes.IndexOf(value);

                if (index == -1)
                {
                    this.SelectedIndex = this.DefaultSizes.Count;
                }
                else
                {
                    this.SelectedIndex = index;
                }
            }
        }
    }
}
