using DataAccess.DataModels;
using DataAccess.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Account.Helper;
using System.Windows.Controls;

namespace Account.ViewModels
{
    public class CreateStaffAccountViewModel : BindableBase, INavigationAware
    {
        private IStaffsRepository _staffsRepository;
        private IRegionManager _regionManager;
        private List<Staff> _staffsList;
        private string _id;
        private string _firstName;
        private string _lastName;
        private DateTime _birthday;
        private string _gender;
        private string _email;
        private string _phoneNumber;
        private string _username;
        private string _password;
        private Window _window;
        private double _maxHeight;

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

        public CreateStaffAccountViewModel(IStaffsRepository staffsRepository, IRegionManager regionManager)
        {
            _staffsRepository = staffsRepository;
            _regionManager = regionManager;

            Task.WaitAll(Task.Run(async () => await Load()));

            CreateCommand = new DelegateCommand(Create);

            CancelCommand = new DelegateCommand(Cancel);

            PasswordChangeCommand = new DelegateCommand<object>(PasswordChanged);

            ClearView();

            _window = Application.Current.MainWindow;

            MaxHeight = 550;

            _window.StateChanged += (sender, e) =>
            {
                if (_window.WindowState == WindowState.Normal)
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
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageAccountView");
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
                MessageBox.Show("You must fill in all fields first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isDuplicatedStaff(ID, Email, PhoneNumber, Username))
            {
                return;
            }

            var pass = PasswordEncryption.sha256_hash(_password);

            Staff st = new Staff
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
                Salary = 1000
            };

            Task.WaitAll(Task.Run(async () => await _staffsRepository.AddStaffAsync(st)));

            MessageBox.Show("Account has been created!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private bool isDuplicatedStaff(string ID, string email, string phoneNumber, string userName)
        {
            foreach (var staff in _staffsList)
            {
                if (staff.ID == ID)
                {
                    MessageBox.Show("This ID has been registered by another staff!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
                if (staff.Email == email)
                {
                    MessageBox.Show("This email has been registered by another staff!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
                if (staff.PhoneNumber == phoneNumber)
                {
                    MessageBox.Show("This phoneNumber has been registered by another staff!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
                if (staff.Username == userName)
                {
                    MessageBox.Show("This userName has been registered by another staff!", "Data in use", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return true;
                }
            }
            return false;
        }

        private async Task Load()
        {
            _staffsList = new List<Staff>();
            _staffsList = await _staffsRepository.GetAllStaffsAsync();
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