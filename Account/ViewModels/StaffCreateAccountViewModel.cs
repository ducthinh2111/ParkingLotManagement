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
using System.Windows.Controls;
using System.Windows;
using Account.Helper;

namespace Account.ViewModels
{
    public class StaffCreateAccountViewModel : BindableBase, INavigationAware
    {
        private ICustomersRepository _customersRepository;
        private List<Customer> _customersList;
        private string _id;
        private string _firstName;
        private string _lastName;
        private DateTime _birthday;
        private string _gender;
        private string _email;
        private string _phoneNumber;
        private string _username;
        private string _password;
        private double _maxHeight;
        private Window _window;

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

        public DelegateCommand CreateCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand<object> PasswordChangeCommand { get; set; }

        public StaffCreateAccountViewModel(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;

            Task.WaitAll(Task.Run(async () => await Load()));

            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);

            PasswordChangeCommand = new DelegateCommand<object>(PasswordChanged);

            ClearView();

            _window = Application.Current.MainWindow;

            MaxHeight = 550;

            _window.StateChanged += (sender, e) =>
            {
                if(_window.WindowState == WindowState.Normal)
                {
                    MaxHeight = 550;
                }
                else
                {
                    MaxHeight = 700;
                }
            };
            
        }

        private void Cancel()
        {
            ClearView();
        }

        private void PasswordChanged(object obj)
        {
            var temp = obj as PasswordBox;

            _password = temp.Password;
        }

        private void Create()
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            if (ID == String.Empty ||
                FirstName == String.Empty ||
                LastName == String.Empty ||
                Gender == String.Empty ||
                Email == String.Empty ||
                PhoneNumber == String.Empty ||
                Username == String.Empty)
            {
                MessageBox.Show("You must fill in all fields first", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            if (isDuplicatedStaff(ID, Email, PhoneNumber, Username))
            {
                return;
            }

            var pass = PasswordEncryption.sha256_hash(_password);

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
                Password = pass,
                ParkingLotID = "HCMUTEPL",
                isVIP = "No",
                TotalUsingTime = 0,
                TotalUsingMoney = 0

            };

            Task.WaitAll(Task.Run(async () => await _customersRepository.AddCustomerAsync(ct)));

            MessageBox.Show("Account has been created!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private bool isDuplicatedStaff(string ID, string email, string phoneNumber, string userName)
        {
            foreach (var cus in _customersList)
            {
                if (cus.ID == ID)
                {
                    MessageBox.Show("This ID has been registered by another customer!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
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

        private void ClearView()
        {
            ID = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            BirthDay = DateTime.Now;
            Gender = String.Empty;
            Email = String.Empty;
            PhoneNumber = String.Empty;
            Username = String.Empty;
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

        }
    }
}
