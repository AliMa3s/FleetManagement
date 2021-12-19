using System;
using System.Globalization;
using System.Windows.Data;

namespace FleetManagement.WPF.Converters
{
    public class TankkaartActiefConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool? isActief = value as bool?;
                return (bool)isActief ? "Ja" : "Neen";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
