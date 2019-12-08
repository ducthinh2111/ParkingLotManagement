using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using Prism.Commands;
using System.Diagnostics;
using System.Windows;
using System.Globalization;

namespace Parking.ViewModels
{
    public class CheckOutViewModel : BindableBase, INavigationAware
    {
        private IParkingsRepository _parkingsRepository;
        private ICustomersRepository _customersRepository;
        private IParkingLotsRepository _parkingLotsRepository;
        private IMonthlyIncomesRepository _monthlyIncomesRepository;
        private ISlotsRepository _slotsRepository;
        private List<DataAccess.DataModels.Parking> _parkingsList;
        private List<DataAccess.DataModels.Customer> _customersList;
        private List<DataAccess.DataModels.ParkingLot> _parkingLotsList;
        private List<DataAccess.DataModels.MonthlyIncome> _monthlyIncomesList;
        private List<DataAccess.DataModels.Slot> _slotsList;
        private string _CheckOutLicensePlate;
        private string _CheckOutSecurityCode;
        private string _customerID;
        private bool _enableCheckOutButton;
        private double _totalPrice;
        private string _isVIP;
        private string _VipDiscount;
        private DataAccess.DataModels.Parking parking;
        private Customer cus;
        private TimeSpan? _timeDiff;
        private double _totalUsingTime;


        public string CheckOutLicensePlate
        {
            get
            {
                return _CheckOutLicensePlate;
            }
            set
            {
                SetProperty(ref _CheckOutLicensePlate, value);
            }
        }
        public string CheckOutSecurityCode
        {
            get
            {
                return _CheckOutSecurityCode;
            }
            set
            {
                SetProperty(ref _CheckOutSecurityCode, value);
            }
        }

        public string CustomerID
        {
            get
            {
                return _customerID;
            }
            set
            {
                SetProperty(ref _customerID, value);
            }
        }

        public bool EnableCheckOutButton
        {
            get
            {
                return _enableCheckOutButton;
            }
            set
            {
                SetProperty(ref _enableCheckOutButton, value);
            }
        }

        public double TotalPrice
        {
            get
            {
                return _totalPrice;
            }
            set
            {
                SetProperty(ref _totalPrice, value);
            }
        }

        public string isVIP
        {
            get
            {
                return _isVIP;
            }
            set
            {
                SetProperty(ref _isVIP, value);
            }
        }

        public string VIPDiscount
        {
            get
            {
                return _VipDiscount;
            }
            set
            {
                SetProperty(ref _VipDiscount, value);
            }
        }

        public double TotalUsingTime
        {
            get
            {
                return _totalUsingTime;
            }
            set
            {
                SetProperty(ref _totalUsingTime, value);
            }
        }

        public DelegateCommand ConfirmCommand { get; private set; }

        public DelegateCommand CheckOutCommand { get; private set; }

        public CheckOutViewModel(IParkingsRepository parkingsRepository, ICustomersRepository customersRepository, IParkingLotsRepository parkingLotsRepository, IMonthlyIncomesRepository monthlyIncomesRepository, ISlotsRepository slotsRepository)
        {
            _parkingsRepository = parkingsRepository;
            _customersRepository = customersRepository;
            _parkingLotsRepository = parkingLotsRepository;
            _monthlyIncomesRepository = monthlyIncomesRepository;
            _slotsRepository = slotsRepository;

            Task.WaitAll(Task.Run(async () => await Load()));

            ConfirmCommand = new DelegateCommand(Confirm);

            CheckOutCommand = new DelegateCommand(CheckOut);

            ClearTheView();

        }

        private void CheckOut()
        {

            #region Update Customer Total Using Time And Total Using Money
            if (cus != null)
            {
                var _lot = _parkingLotsList.Find(param => param.ID == "HCMUTEPL");
                cus.TotalUsingMoney += TotalPrice;
                cus.TotalUsingTime += Math.Round((double)_timeDiff?.TotalHours, 2);
                if (cus.TotalUsingTime >= _lot.VIPRequiredTime)
                {
                    cus.isVIP = "Yes";
                }

                Task.WaitAll(Task.Run(async () => await _customersRepository.UpdateCustomerAsync(cus)));

            }
            #endregion

            #region Update Monthly Income

            MonthlyIncome monthlyIncome = _monthlyIncomesList.Find(param => param.ID == parking.CheckOutDateTime?.Month);

            monthlyIncome.Income += TotalPrice;

            Task.WaitAll(Task.Run(async () => await _monthlyIncomesRepository.UpdateMonthlyIncomeAsync(monthlyIncome)));


            #endregion

            #region Update Slot Availability

            var slot = _slotsList.Find(param => param.ID == parking.SlotID);

            slot.Availability = "Yes";

            Task.WaitAll(Task.Run(async () => await _slotsRepository.UpdateSlotAsync(slot)));

            #endregion

            #region Delete From Parking

            Task.WaitAll(Task.Run(async () => await _parkingsRepository.DeleteParkingAsync(parking)));

            #endregion

            MessageBox.Show("Customer has been checked out!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("EventLog.txt", true))
            {
                string temp = CustomerID == String.Empty ? "null" : CustomerID;
                file.WriteLine("----------");
                file.WriteLine($"[CheckOut] - Date:{DateTime.Now.ToShortDateString()}; Time:{DateTime.Now.ToLongTimeString()}; License Plate:{CheckOutLicensePlate}; Slot:{slot.Name}; UserID:{temp}");
            }

            ClearTheView();
        }

        private void ClearTheView()
        {
            EnableCheckOutButton = false;
            TotalPrice = 0;
            CheckOutLicensePlate = String.Empty;
            CheckOutSecurityCode = String.Empty;
            CustomerID = String.Empty;
            VIPDiscount = String.Empty;
            isVIP = String.Empty;
            parking = null;
            cus = null;
        }

        private void Confirm()
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            parking = _parkingsList.Find(param => param.LicensePlate == CheckOutLicensePlate);
            if (parking == null)
            {
                MessageBox.Show("Can not find license plate!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CheckOutSecurityCode == String.Empty)
            {
                MessageBox.Show("Security Code can not be NULL", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                if (parking.SecurityCode != CheckOutSecurityCode)
                {
                    MessageBox.Show("Security Code does not match, try again!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    var _lot = _parkingLotsList.Find(param => param.ID == "HCMUTEPL");
                    parking.CheckOutDateTime = DateTime.Now;

                    _timeDiff = parking.CheckOutDateTime?.Subtract((DateTime)parking.CheckInDateTime);

                    if (CustomerID != String.Empty)
                    {
                        cus = _customersList.Find(param => param.ID == CustomerID);
                        if (cus == null)
                        {
                            MessageBox.Show("Can not find data", "Notification", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            return;
                        }
                        else
                        {
                            isVIP = cus.isVIP;
                            if (isVIP == "Yes")
                            {
                                VIPDiscount = $"{_lot.VIPDiscount.ToString()}%";
                            }
                            else
                            {
                                VIPDiscount = "0%";
                            }
                        }
                    }
                    else
                    {
                        VIPDiscount = String.Empty;
                        isVIP = String.Empty;
                    }

                    if (VIPDiscount == String.Empty || VIPDiscount == "0%")
                    {
                        TotalPrice = Math.Round((double)(_timeDiff?.TotalHours * _lot.DayPrice), 2);
                    }
                    else
                    {
                        TotalPrice = Math.Round((double)(_timeDiff?.TotalHours * _lot.DayPrice), 2);
                        TotalPrice -= Math.Round((double)(_lot.VIPDiscount * TotalPrice / 100), 2);
                    }
                    if (TotalPrice <= 2)
                    {
                        TotalPrice = 2;
                    }

                    TotalUsingTime = Math.Round((double)_timeDiff?.TotalHours, 2);

                    EnableCheckOutButton = true;
                }
            }


        }

        private async Task Load()
        {
            _parkingsList = new List<DataAccess.DataModels.Parking>();
            _parkingsList = await _parkingsRepository.GetAllParkingsAsync();

            _customersList = new List<Customer>();
            _customersList = await _customersRepository.GetAllCustomersAsync();

            _parkingLotsList = new List<ParkingLot>();
            _parkingLotsList = await _parkingLotsRepository.GetAllParkingLotsAsync();

            _monthlyIncomesList = new List<MonthlyIncome>();
            _monthlyIncomesList = await _monthlyIncomesRepository.GetMonthlyIncomesAsync();

            _slotsList = new List<Slot>();
            _slotsList = await _slotsRepository.GetAllSlotsAsync();

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
            ClearTheView();
            var parking = navigationContext.Parameters["Parking"] as DataAccess.DataModels.Parking;
            if (parking != null)
            {
                CheckOutLicensePlate = parking.LicensePlate;
            }
        }
    }
}
