using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Slot.Helper
{
    public class StatusToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            SolidColorBrush Orangebrush = (SolidColorBrush)Application.Current.Resources["OrangeBrush"];
            SolidColorBrush Greenbrush = (SolidColorBrush)Application.Current.Resources["GreenBrush"];

            if(status == "Active")
            {
                return Greenbrush;
            }
            return Orangebrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
