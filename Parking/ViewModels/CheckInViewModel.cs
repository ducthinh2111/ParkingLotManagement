using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.DataModels;
using Prism.Commands;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace Parking.ViewModels
{
    public class CheckInViewModel : BindableBase, INavigationAware
    {
        private IParkingsRepository _parkingsRepository;
        private ISlotsRepository _slotsRepository;
        private List<DataAccess.DataModels.Slot> _slotsList;
        private List<DataAccess.DataModels.Parking> _parkingsList;
        private string _checkInLicensePlate;
        private string _checkInSecurityCode;
        private bool _isDialogOpen;
        private string _licensePlatePopUp;
        private string _slotNamePopUp;
        private string _checkInTimePopUp;

        public string CheckInLicensePlate
        {
            get
            {
                return _checkInLicensePlate;
            }
            set
            {
                SetProperty(ref _checkInLicensePlate, value);
            }
        }

        public string CheckInSecurityCode
        {
            get
            {
                return _checkInSecurityCode;
            }
            set
            {
                SetProperty(ref _checkInSecurityCode, value);
            }
        }

        public bool IsDialogOpen
        {
            get
            {
                return _isDialogOpen;
            }
            set
            {
                SetProperty(ref _isDialogOpen, value);
            }
        }

        public string LicensePlatePopUp
        {
            get
            {
                return _licensePlatePopUp;
            }
            set
            {
                SetProperty(ref _licensePlatePopUp, value);
            }
        }
        public string SlotNamePopUp
        {
            get
            {
                return _slotNamePopUp;
            }
            set
            {
                SetProperty(ref _slotNamePopUp, value);
            }
        }
        public string CheckInTimePopUp
        {
            get
            {
                return _checkInTimePopUp;
            }
            set
            {
                SetProperty(ref _checkInTimePopUp, value);
            }
        }

        public DelegateCommand ConfirmCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public CheckInViewModel(IParkingsRepository parkingsRepository, ISlotsRepository slotsRepository)
        {
            _parkingsRepository = parkingsRepository;
            _slotsRepository = slotsRepository;

            Task.WaitAll(Task.Run(async () => await Load()));

            ConfirmCommand = new DelegateCommand(Confirm);

            CancelCommand = new DelegateCommand(Cancel);

            ClearView();

            SecurityCodeGenerator();
        }

        private void Cancel()
        {
            CheckInLicensePlate = String.Empty;
        }

        private void Confirm()
        {
            if (CheckInLicensePlate == String.Empty)
            {
                MessageBox.Show("License Plate can not be NULL!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Task.WaitAll(Task.Run(async () => await Load()));
            var slot = FindSlot();
            if (slot != null)
            {

                if (isDuplicatedLicensePlate(CheckInLicensePlate))
                {
                    MessageBox.Show("License has been existing!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    DataAccess.DataModels.Parking pk = new DataAccess.DataModels.Parking
                    {
                        LicensePlate = CheckInLicensePlate,
                        SecurityCode = CheckInSecurityCode,
                        CheckInDateTime = DateTime.Now,
                        CheckOutDateTime = null,
                        SlotID = slot.ID
                    };

                    Task.WaitAll(Task.Run(async () => await _parkingsRepository.AddParkingAsync(pk)));

                    slot.Availability = "No";

                    Task.WaitAll(Task.Run(async () => await _slotsRepository.UpdateSlotAsync(slot)));

                    LicensePlatePopUp = CheckInLicensePlate;
                    SlotNamePopUp = slot.Name;
                    CheckInTimePopUp = DateTime.Now.ToString();
                    IsDialogOpen = true;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("EventLog.txt", true))
                    {
                        file.WriteLine("----------");
                        file.WriteLine($"[CheckIn] - Date:{DateTime.Now.ToShortDateString()}; Time:{DateTime.Now.ToLongTimeString()}; License Plate:{CheckInLicensePlate}; Slot:{slot.Name}");
                    }
                    ClearView();
                }
            }
            else
            {
                MessageBox.Show("All slots are being occupied, please come back later!", "apologize", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private async Task Load()
        {
            _slotsList = new List<DataAccess.DataModels.Slot>();
            _slotsList = await _slotsRepository.GetAllSlotsAsync();

            _parkingsList = new List<DataAccess.DataModels.Parking>();
            _parkingsList = await _parkingsRepository.GetAllParkingsAsync();
        }

        private DataAccess.DataModels.Slot FindSlot()
        {
            foreach (var slot in _slotsList)
            {
                if (slot.Availability == "Yes" && slot.Status == "Active")
                {
                    return slot;
                }
            }
            return null;
        }

        private void SecurityCodeGenerator()
        {
            Random rnd = new Random();
            CheckInSecurityCode = rnd.Next(1000, 9999).ToString();
            while (isDuplicatedCode(CheckInSecurityCode))
            {
                CheckInSecurityCode = rnd.Next(1000, 9999).ToString();
            }
        }

        private bool isDuplicatedCode(string securityCode)
        {
            foreach (var parking in _parkingsList)
            {
                if (securityCode == parking.SecurityCode)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isDuplicatedLicensePlate(string licensePlate)
        {
            foreach (var parking in _parkingsList)
            {
                if (licensePlate.ToLower() == parking.LicensePlate.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearView()
        {
            CheckInLicensePlate = String.Empty;
            SecurityCodeGenerator();
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
            Task.WaitAll(Task.Run(async () => await Load()));
            IsDialogOpen = false;
            ClearView();
        }
    }
}
