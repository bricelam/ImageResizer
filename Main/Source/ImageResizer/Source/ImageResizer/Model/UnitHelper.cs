//------------------------------------------------------------------------------
// <copyright file="UnitHelper.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Model
{
    using System;
    using System.Globalization;

    internal static class UnitHelper
    {
        public static double ConvertToScale(double value, Unit unit, int originalPixels, double dpi)
        {
            switch (unit)
            {
                case Unit.Pixels:
                    return value / originalPixels;

                case Unit.Percent:
                    return value / 100;

                case Unit.Inches:
                    return (value * dpi) / originalPixels;

                case Unit.Centimeters:
                    return ConvertToScale(value * 50 / 127, Unit.Inches, originalPixels, dpi);

                default:
                    // TODO: Add message to the resource file
                    throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "The unit '{0}' is not yet supported.", unit));
            }
        }
    }
}
