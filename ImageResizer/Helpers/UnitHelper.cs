//------------------------------------------------------------------------------
// <copyright file="UnitHelper.cs" company="Brice Lambson">
//     Copyright (c) 2011-2012 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using BriceLambson.ImageResizer.Models;

    internal static class UnitHelper
    {
        public static double ConvertToScale(double value, Unit unit, int originalPixels, double dpi)
        {
            Contract.Requires(originalPixels > 0);
            Contract.Requires(dpi > 0);
            Contract.Ensures(Contract.Result<double>() > 0);

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
            }

            throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "The unit '{0}' is not yet supported.", unit));
        }
    }
}