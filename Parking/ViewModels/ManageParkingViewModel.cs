using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using System.Collections.ObjectModel;
using Prism.Commands;
using System.Windows;

namespace Parking.ViewModels
{
    public class ManageParkingViewModel : BindableBase, INavigationAware
    {
        private IParkingsRepository _parkingsRepository;
        private IRegionManager _regionManager;
        private List<DataAccess.DataModels.Parking> _parkingsList;
        private ObservableCollection<DataAccess.DataModels.Parking> _parkings;
        private Window _window;
        private double _maxHeight;
        private int _totalParking;

        public ObservableCollection<DataAccess.DataModels.Parking> Parkings
        {
            get
            {
                return _parkings;
            }
            set
            {
                SetProperty(ref _parkings, value);
            }
        }

        public double MaxHeight
        {
            get
            {
                return _maxHeight;
            }
            set
            {
                SetProperty(ref _maxHeight, value);
            }
        }

        public int TotalParking
        {
            get
            {
                return _totalParking;
            }
            set
            {
                SetProperty(ref _totalParking, value);
            }
        }

        public DelegateCommand<DataAccess.DataModels.Parking> CheckOutCommand { get; private set; }

        public DelegateCommand OrderByCheckInTimeCommand { get; private set; }
        public DelegateCommand OrderBySlotIDCommand { get; private set; }
        public DelegateCommand OrderByLicenseCommand { get; private set; }

        public ManageParkingViewModel(IParkingsRepository parkingsRepository, IRegionManager regionManager)
        {
            _parkingsRepository = parkingsRepository;
            _regionManager = regionManager;
            _window = Application.Current.MainWindow;

            CheckOutCommand = new DelegateCommand<DataAccess.DataModels.Parking>(CheckOut);
            OrderByCheckInTimeCommand = new DelegateCommand(OrderByCheckInTime);
            OrderBySlotIDCommand = new DelegateCommand(OrderBySlotID);
            OrderByLicenseCommand = new DelegateCommand(OrderByLicense);

            if (_window.WindowState == WindowState.Normal)
            {
                MaxHeight = 450;
            }
            else
            {
                MaxHeight = 700;
            }

            _window.StateChanged += (sender, e) =>
            {
                if (_window.WindowState == WindowState.Normal)
                {
                    MaxHeight = 450;
                }
                else
                {
                    MaxHeight = 700;
                }
            };
        }

        private void OrderByLicense()
        {
            Parkings = new ObservableCollection<DataAccess.DataModels.Parking>();
            var sort = from parking in _parkingsList orderby parking.LicensePlate ascending select parking;

            foreach (var parking in sort)
            {
                Parkings.Add(parking);
            }
        }

        private void OrderBySlotID()
        {
            Parkings = new ObservableCollection<DataAccess.DataModels.Parking>();
            var sort = from parking in _parkingsList orderby parking.SlotID ascending select parking;

            foreach (var parking in sort)
            {
                Parkings.Add(parking);
            }
        }

        private void OrderByCheckInTime()
        {
            Parkings = new ObservableCollection<DataAccess.DataModels.Parking>();
            var sort = from parking in _parkingsList orderby parking.CheckInDateTime descending select parking;

            foreach (var parking in sort)
            {
                Parkings.Add(parking);
            }
        }

        private void CheckOut(DataAccess.DataModels.Parking obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Parking", obj);

            if (obj != null)
            {
                _regionManager.RequestNavigate("ManagerWorkspaceRegion", "CheckOutView", parameters);
                _regionManager.RequestNavigate("StaffWorkspaceRegion", "CheckOutView", parameters);
            }
        }

        private void LoadUI()
        {
            Parkings = new ObservableCollection<DataAccess.DataModels.Parking>();
            _parkingsList = new List<DataAccess.DataModels.Parking>();
            var task = Task.Run(async () => _parkingsList = await _parkingsRepository.GetAllParkingsAsync());

            task.ContinueWith((t) =>
            {
                if(task.IsCompleted)
                {
                    foreach (var parking in _parkingsList)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Parkings.Add(parking);
                        });
                    }
                    TotalParking = _parkingsList.Count;
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadUI();
        }
    }
}
