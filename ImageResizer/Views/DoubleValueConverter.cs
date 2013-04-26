//------------------------------------------------------------------------------
// <copyright file="DoubleValueConverter.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(string))]
    internal class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;

            return d == 0
                ? String.Empty
                : d.ToString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;

            return string.IsNullOrWhiteSpace(text)
                ? 0
                : Double.Parse(text, culture);
        }
    }
}
