using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CWSServerList.Converters
{
    public class LastUpdatedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime lastUpdated)
            {
                return (DateTime.Now - lastUpdated).TotalMinutes <= 5 ? Colors.Blue : Colors.Black;
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
