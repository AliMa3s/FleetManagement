using System;
using System.Globalization;
using System.Windows.Data;

namespace FleetManagement.WPF.Converters
{
    public class NummerplaatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                string nummerplaat = value as string;

                return nummerplaat.Substring(0, 1) + "-" 
                    + nummerplaat.Substring(1, 3) + "-" 
                    + nummerplaat.Substring(4, 3);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
