//------------------------------------------------------------------------------
// <copyright file="UnitValueLocalizer.cs" company="Brice Lambson">
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
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;

    [ValueConversion(typeof(Unit), typeof(string))]
    internal class UnitValueLocalizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = Resources.ResourceManager.GetString(Enum.GetName(typeof(Unit), value));

            if ((string)parameter == "ToLower")
            {
                output = output.ToLower(culture);
            }

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}