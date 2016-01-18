using System;
using System.Globalization;
using System.Windows.Data;
using ImageResizer.Properties;

namespace ImageResizer.Views
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    class TimeRemainingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeRemaining = (TimeSpan)value;

            // TODO: Do these localize right?
            return string.Format(
                culture,
                Resources.Progress_TimeRemaining,
                timeRemaining.Hours > 0
                    ? (GetCore(culture, Resources.Progress_Hour, Resources.Progress_Hours, timeRemaining.Hours) + ", " +
                        GetMinutes(culture, timeRemaining.Minutes))
                    : timeRemaining.Minutes > 0
                        ? (GetMinutes(culture, timeRemaining.Hours) + ", " + GetSeconds(culture, timeRemaining.Minutes))
                        : GetSeconds(culture, timeRemaining.Seconds));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static string GetMinutes(IFormatProvider formatProvider, int minutes)
            => GetCore(formatProvider, Resources.Progress_Minute, Resources.Progress_Minutes, minutes);

        static string GetSeconds(IFormatProvider formatProvider, int seconds)
            => GetCore(formatProvider, Resources.Progress_Second, Resources.Progress_Seconds, seconds);

        static string GetCore(IFormatProvider formatProvider, string singularFormat, string pluralFormat, int value)
            => value.ToString(formatProvider) + " " + (value == 1 ? singularFormat : pluralFormat);

    }
}
