using System;
using System.Globalization;
using System.Windows.Data;

namespace FleetManagement.WPF.Converters
{
    public class RijksregisterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string rijksregister = value as string;

                return rijksregister.Substring(0, 2) + "." 
                    + rijksregister.Substring(2, 2) + "." 
                    + rijksregister.Substring(4, 2) + "-"
                    + rijksregister.Substring(6, 3) + "."
                    + rijksregister.Substring(9, 2);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
