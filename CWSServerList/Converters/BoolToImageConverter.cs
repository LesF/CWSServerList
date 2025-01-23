using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CWSServerList.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? "Images/green_tick.png" : "Images/red_cross.png";
            }
            return "Images/Warning.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

