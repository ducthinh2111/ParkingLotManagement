using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DataAccess.DataModels;

namespace Slot.Helper
{
    public class ObjectToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var slot = value as DataAccess.DataModels.Slot;

            if(slot.Availability == "No" && slot.Status == "Inactive" || slot.Availability == "Yes" && slot.Status == "Active")
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
