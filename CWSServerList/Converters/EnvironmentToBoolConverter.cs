using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CWSServerList.Converters
{
    public class EnvironmentToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string environment && parameter is string targetEnvironment)
            {
                return environment == targetEnvironment;
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string targetEnvironment)
            {
                return targetEnvironment;
            }
            return Binding.DoNothing;
        }
    }
}
