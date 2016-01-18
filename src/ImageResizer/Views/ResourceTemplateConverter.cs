using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using ImageResizer.Properties;

namespace ImageResizer.Views
{
    [ValueConversion(typeof(string), typeof(string))]
    class ResourceTemplateConverter : IValueConverter
    {
        static readonly IDictionary<string, string> _macros;

        static ResourceTemplateConverter()
        {
            _macros = new Dictionary<string, string>
            {
                ["$small$"] = Resources.Small,
                ["$medium$"] = Resources.Medium,
                ["$large$"] = Resources.Large,
                ["$phone$"] = Resources.Phone
            };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedValue;

            return _macros.TryGetValue((string)value, out convertedValue)
                ? convertedValue
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;
    }
}
