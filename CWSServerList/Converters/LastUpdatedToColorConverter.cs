using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CWSServerList.Converters
{
    public class LastUpdatedToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // AppTheme theme = Application.Current?.RequestedTheme ?? AppTheme.Light;
            // If system theme changes the app themes switch for most display element, except the Convert method
            // does not get recalled so colors won't change until the elements using this converter get refreshed.

            if (value is DateTime lastUpdated)
            {
                return (DateTime.Now - lastUpdated).TotalMinutes <= 5 ? FontAttributes.Bold : FontAttributes.None;

                //if (theme == AppTheme.Dark)
                //    return (DateTime.Now - lastUpdated).TotalMinutes <= 5 ? Colors.Cyan : Colors.White;
                //else
                //    return (DateTime.Now - lastUpdated).TotalMinutes <= 5 ? Colors.BlueViolet : Colors.Black;
            }
            return FontAttributes.None;
            //return (theme == AppTheme.Dark) ? Colors.White : Colors.Black;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
