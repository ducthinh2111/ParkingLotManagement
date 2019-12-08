using DataAccess.DataModels;
using DataAccess.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rate.ViewModels
{
    public class RateViewModel : BindableBase, INavigationAware
    {
        private IParkingLotsRepository _parkingLotsRepository;
        private List<ParkingLot> list;
        private string _dayPrice = String.Empty;
        private string _nightPrice = String.Empty;
        private string _vipRequiredTime = String.Empty;
        private string _vipDiscount = String.Empty;

        public string DayPrice
        {
            get
            {
                return _dayPrice;
            }
            set
            {
                SetProperty(ref _dayPrice, value);
            }
        }

        public string NightPrice
        {
            get
            {
                return _nightPrice;
            }
            set
            {
                SetProperty(ref _nightPrice, value);
            }
        }

        public string VIPDiscount
        {
            get
            {
                return _vipDiscount;
            }
            set
            {
                SetProperty(ref _vipDiscount, value);
            }
        }

        public string VIPRequiredTime
        {
            get
            {
                return _vipRequiredTime;
            }
            set
            {
                SetProperty(ref _vipRequiredTime, value);
            }
        }

        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public RateViewModel(IParkingLotsRepository parkingLotsRepository)
        {
            _parkingLotsRepository = parkingLotsRepository;

            LoadData();

            UpdateCommand = new DelegateCommand(Update);

            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            LoadData();
        }

        private void Update()
        {
            if (DayPrice == String.Empty && NightPrice == String.Empty && VIPDiscount == String.Empty && VIPRequiredTime == String.Empty)
            {
                System.Windows.MessageBox.Show("Nothing has been updated", "Notification", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
            }
            else
            {
                var lot = list.Find(param => param.ID == "HCMUTEPL");
                lot.DayPrice = Double.Parse(DayPrice);
                lot.NightPrice = Double.Parse(NightPrice);
                lot.VIPRequiredTime = Double.Parse(VIPRequiredTime);
                lot.VIPDiscount = Double.Parse(VIPDiscount);
                var task = Task.Run(async () => await _parkingLotsRepository.UpdateParkingLotAsync(lot));
                task.ContinueWith((t) =>
                {
                    System.Windows.MessageBox.Show("Data has been updated!", "Successful", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                });
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadData();
        }

        private void LoadData()
        {
            list = new List<ParkingLot>();
            var task = Task.Run(async () => list = await _parkingLotsRepository.GetAllParkingLotsAsync());

            task.ContinueWith((t) =>
            {
                if(task.IsCompleted)
                {
                    DayPrice = list.Find(param => param.ID == "HCMUTEPL").DayPrice.ToString();
                    NightPrice = list.Find(param => param.ID == "HCMUTEPL").NightPrice.ToString();
                    VIPRequiredTime = list.Find(param => param.ID == "HCMUTEPL").VIPRequiredTime.ToString();
                    VIPDiscount = list.Find(param => param.ID == "HCMUTEPL").VIPDiscount.ToString();
                }
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}