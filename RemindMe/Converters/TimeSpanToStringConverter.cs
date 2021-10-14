using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RemindMe.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public string Format { get; set; } = "mm\\:ss";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan timeSpan)) return string.Empty;

            return timeSpan.ToString(Format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
