//------------------------------------------------------------------------------
// <copyright file="ModeValueLocalizer.cs" company="Brice Lambson">
//     Copyright (c) 2011-2012 Brice Lambson. All rights reserved.
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

    [ValueConversion(typeof(Mode), typeof(string))]
    internal class ModeValueLocalizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceName = Enum.GetName(typeof(Mode), value);

            if ((string)parameter == "ToThirdPersonSingular")
            {
                resourceName += "_ThirdPersonSingular";
            }

            return Resources.ResourceManager.GetString(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}