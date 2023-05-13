using System;
using System.Globalization;
using System.Windows.Data;

namespace AudioBeta1._0.Converters
{
    public class TimespanToDouble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            double result = time.Seconds + time.Minutes * 60 + time.Hours * 3600;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
