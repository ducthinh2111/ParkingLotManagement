using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using System.Security;
using Prism.Commands;
using System.Windows;
using Account.Helper;
using System.Windows.Controls;

namespace Account.ViewModels
{
    public class EditStaffViewModel : BindableBase , INavigationAware
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
        private double _salary;
        private string _oldPassword;
        private string _newPassword;
        private Staff staff;

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

        public double Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                SetProperty(ref _salary, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand<object> PasswordChangeCommand { get; set; }

        public EditStaffViewModel(IStaffsRepository staffsRepository, IRegionManager regionManager)
        {
            _staffsRepository = staffsRepository;
            _regionManager = regionManager;

            Task.WaitAll(Task.Run(async () => await Load()));

            SaveCommand = new DelegateCommand(Save);

            CancelCommand = new DelegateCommand(Cancel);

            PasswordChangeCommand = new DelegateCommand<object>(PasswordChanged);

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

            if (FirstName == String.Empty ||
                LastName == String.Empty ||
                Gender == String.Empty ||
                Email == String.Empty ||
                PhoneNumber == String.Empty ||
                Username == String.Empty ||
                Salary == 0)
            {
                MessageBox.Show("You must fill in all fields first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(Email != staff.Email || PhoneNumber != staff.PhoneNumber || Username != staff.Username)
            {
                if (isDuplicatedStaff(Email, PhoneNumber, Username))
                {
                    return;
                }
            }

            if(_newPassword.Length != 0 && _newPassword != _oldPassword)
            {
                _oldPassword = PasswordEncryption.sha256_hash(_newPassword);
            }

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
                Password = _oldPassword,
                ParkingLotID = "HCMUTEPL",
                Salary = this.Salary
            };

            Task.WaitAll(Task.Run(async () => await _staffsRepository.UpdateStaffAsync(st)));

            MessageBox.Show("Data have been saved!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private bool isDuplicatedStaff(string email, string phoneNumber, string userName)
        {
            foreach (var staff in _staffsList)
            {
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            staff = navigationContext.Parameters["Staff"] as DataAccess.DataModels.Staff;
            if (staff != null)
            {
                ID = staff.ID;
                FirstName = staff.FirstName;
                LastName = staff.LastName;
                BirthDay = (DateTime)staff.BirthDay;
                Gender = staff.Gender;
                Email = staff.Email;
                PhoneNumber = staff.PhoneNumber;
                Username = staff.Username;
                _oldPassword = staff.Password;
                Salary = (double)staff.Salary;
            }

            _newPassword = "";
        }
    }
}
