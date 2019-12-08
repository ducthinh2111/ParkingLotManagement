using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using System.Windows.Controls;
using Login.Helper;
using System.Windows;
using CommonServiceLocator;

namespace Login.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private ICustomersRepository _customersRepository;
        private IStaffsRepository _staffsRepository;
        private IAdminAccountRepository _adminAccountRepository;
        private List<Customer> _customersList;
        private List<Staff> _staffsList;
        private List<AdminAccount> _adminAccountsList;
        private string _password;
        private string _username;

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

        public DelegateCommand LoginCommand { get; private set; }

       public DelegateCommand<object> PasswordChangeCommand { get; private set; }

        public LoginViewModel(IRegionManager regionManager, ICustomersRepository customersRepository, IAdminAccountRepository adminAccountRepository, IStaffsRepository staffsRepository)
        {
            _regionManager = regionManager;
            _customersRepository = customersRepository;
            _staffsRepository = staffsRepository;
            _adminAccountRepository = adminAccountRepository;

            Task.WaitAll(Task.Run(async () => await Load()));

            LoginCommand = new DelegateCommand(Navigate);
            PasswordChangeCommand = new DelegateCommand<object>(PasswordChange);
        }


        private async Task Load()
        {
            _customersList = new List<Customer>();
            _customersList = await _customersRepository.GetAllCustomersAsync();

            _staffsList = new List<Staff>();
            _staffsList = await _staffsRepository.GetAllStaffsAsync();

            _adminAccountsList = new List<AdminAccount>();
            _adminAccountsList = await _adminAccountRepository.GetAllAdminAccountsAsync();

            _password = String.Empty;

        }

        private void PasswordChange(object obj)
        {
            var temp = obj as PasswordBox;
            _password = temp.Password;
        }

        private void Navigate()
        {
            var admin = _adminAccountsList.Find(param => param.Username == Username);
            var parameters = new NavigationParameters();
            if (admin != null)
            {
                if (admin.Password == _password)
                {
                    parameters.Add("Admin", admin);

                    _regionManager.RequestNavigate("ContentRegion", "ManagerView", parameters);
                }
                else
                {
                    MessageBox.Show("User Name or Password is not correct!", "Try again", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                var pass = PasswordEncryption.sha256_hash(_password);
                var cus = _customersList.Find(param => param.Username == Username);
                if (cus != null)
                {
                    if (cus.Password == pass)
                    {
                        parameters.Add("Customer", cus);

                        _regionManager.RequestNavigate("ContentRegion", "CustomerView", parameters);
                    }
                    else
                    {
                        MessageBox.Show("User Name or Password is not correct!", "Try again", MessageBoxButton.OK, MessageBoxImage.Information);

                        return;
                    }
                }
                else
                {
                    var staff = _staffsList.Find(param => param.Username == Username);
                    if (staff != null)
                    {
                        if (staff.Password == pass)
                        {
                            parameters.Add("Staff", staff);

                            _regionManager.RequestNavigate("ContentRegion", "StaffView", parameters);
                        }
                        else
                        {
                            MessageBox.Show("User Name or Password is not correct!", "Try again", MessageBoxButton.OK, MessageBoxImage.Information);

                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data not found! Please try again", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
            }
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Task.WaitAll(Task.Run(async () => await Load()));
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
