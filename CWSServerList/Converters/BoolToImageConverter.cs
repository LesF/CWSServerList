using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CWSServerList.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? "green_tick.png" : "red_cross.png";
            }
            return "warning.png";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

