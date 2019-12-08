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
using System.Windows.Controls;
using System.Windows;
using Account.Helper;

namespace Account.ViewModels
{
    public class EditCustomerViewModel : BindableBase, INavigationAware
    {
        private ICustomersRepository _customersRepository;
        private IRegionManager _regionManager;
        private List<Customer> _customersList;
        private string _id;
        private string _firstName;
        private string _lastName;
        private DateTime _birthday;
        private string _gender;
        private string _email;
        private string _phoneNumber;
        private string _username;
        private string _oldPassword;
        private string _newPassword;
        private Customer cus;
        private string _isVIP;
        private double _totalUsingTime;
        private double _totalUsingMoney;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetProperty(ref _firstName, value);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        public DateTime BirthDay
        {
            get
            {
                return _birthday;
            }
            set
            {
                SetProperty(ref _birthday, value);
            }
        }

        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                SetProperty(ref _gender, value);
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                SetProperty(ref _email, value);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                SetProperty(ref _phoneNumber, value);
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                SetProperty(ref _username, value);
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

        public double TotalUsingMoney
        {
            get
            {
                return _totalUsingMoney;
            }
            set
            {
                SetProperty(ref _totalUsingMoney, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand<object> PasswordChangeCommand { get; set; }

        public EditCustomerViewModel(ICustomersRepository customersRepository, IRegionManager regionManager)
        {
            _customersRepository = customersRepository;
            _regionManager = regionManager;

            Task.WaitAll(Task.Run(async () => await Load()));

            SaveCommand = new DelegateCommand(Save);

            PasswordChangeCommand = new DelegateCommand<object>(PasswordChanged);

            CancelCommand = new DelegateCommand(Cancel);

            _newPassword = "";


        }

        private void Cancel()
        {
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageAccountView");
        }

        private void PasswordChanged(object obj)
        {
            var temp = obj as PasswordBox;

            _newPassword = temp.Password;
        }

        private void Save()
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            if (ID == String.Empty ||
                FirstName == String.Empty ||
                LastName == String.Empty ||
                Gender == String.Empty ||
                Email == String.Empty ||
                PhoneNumber == String.Empty ||
                Username == String.Empty ||
                isVIP == String.Empty)
            {
                MessageBox.Show("You must fill in all fields first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Email != cus.Email || PhoneNumber != cus.PhoneNumber || Username != cus.Username)
            {
                if (isDuplicatedStaff(Email, PhoneNumber, Username))
                {
                    return;
                }
            }

            if (_newPassword.Length != 0 && _newPassword != _oldPassword)
            {
                _oldPassword = PasswordEncryption.sha256_hash(_newPassword);
            }

            Customer ct = new Customer
            {
                ID = this.ID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthDay = this.BirthDay,
                Gender = this.Gender,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Username = this.Username,
                Password = _oldPassword,
                ParkingLotID = "HCMUTEPL",
                isVIP = this.isVIP,
                TotalUsingTime = this.TotalUsingTime,
                TotalUsingMoney = this.TotalUsingMoney

            };

            Task.WaitAll(Task.Run(async () => await _customersRepository.UpdateCustomerAsync(ct)));

            MessageBox.Show("Data have been saved!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private bool isDuplicatedStaff(string email, string phoneNumber, string userName)
        {
            foreach (var cus in _customersList)
            {
                if (cus.Email == email)
                {
                    MessageBox.Show("This email has been registered by another customer!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
                if (cus.PhoneNumber == phoneNumber)
                {
                    MessageBox.Show("This phoneNumber has been registered by another customer!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
                if (cus.Username == userName)
                {
                    MessageBox.Show("This userName has been registered by another customer!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
            }
            return false;
        }

        private async Task Load()
        {
            _customersList = new List<Customer>();
            _customersList = await _customersRepository.GetAllCustomersAsync();
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
            cus = navigationContext.Parameters["Customer"] as DataAccess.DataModels.Customer;
            if (cus != null)
            {
                ID = cus.ID;
                FirstName = cus.FirstName;
                LastName = cus.LastName;
                BirthDay = (DateTime)cus.BirthDay;
                Gender = cus.Gender;
                Email = cus.Email;
                PhoneNumber = cus.PhoneNumber;
                Username = cus.Username;
                _oldPassword = cus.Password;
                isVIP = cus.isVIP;
                TotalUsingTime = (double)cus.TotalUsingTime;
                TotalUsingMoney = (double)cus.TotalUsingMoney;
            }

            _newPassword = "";
        }
    }
}
