using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Parking.Helper
{
    public class TextToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush Orangebrush = (SolidColorBrush)Application.Current.Resources["OrangeBrush"];
            SolidColorBrush Greenbrush = (SolidColorBrush)Application.Current.Resources["GreenBrush"];

            if(value.ToString() == "No" || value.ToString() == "0%")
            {
                return Orangebrush;
            }
            return Greenbrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
