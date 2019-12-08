using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DataAccess.Interfaces;
using DataAccess.DataModels;
using System.Globalization;
using CommonServiceLocator;

namespace Slot.Helper
{
    public class SlotIDToSecurityCodeConverter : IValueConverter
    {
        IParkingsRepository _parkingsRepository;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _parkingsRepository = ServiceLocator.Current.GetInstance<IParkingsRepository>();

            List<Parking> parkingsList = new List<Parking>();

            Task.WaitAll(Task.Run(async () => parkingsList = await _parkingsRepository.GetAllParkingsAsync()));

            var pk = parkingsList.Find(param => param.SlotID == value.ToString());

            return pk.SecurityCode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
