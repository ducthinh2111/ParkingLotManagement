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
using System.Windows;

namespace Search.ViewModels
{
    public class SearchViewModel : BindableBase, INavigationAware
    {
        ICustomersRepository _customersRepository;
        IRegionManager _regionManager;
        private List<Customer> _customersList;
        private string _customerID;
        private string _customerName;
        private string _gender;
        private string _isVIP;
        private double _timeSpent;
        private double _totalAmount;
        private Customer cus;
        private Visibility _isResultFound;
        private string _input;

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

        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                SetProperty(ref _customerName, value);
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

        public string IsVIP
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

        public double TimeSpent
        {
            get
            {
                return _timeSpent;
            }
            set
            {
                SetProperty(ref _timeSpent, value);
            }
        }

        public double TotalAmount
        {
            get
            {
                return _totalAmount;
            }
            set
            {
                SetProperty(ref _totalAmount, value);
            }
        }

        public Visibility IsResultFound
        {
            get
            {
                return _isResultFound;
            }
            set
            {
                SetProperty(ref _isResultFound, value);
            }
        }

        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                SetProperty(ref _input, value);
            }
        }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand EditCustomerCommand { get; private set; }

        public DelegateCommand DeleteCustomerCommand { get; private set; }

        public SearchViewModel(ICustomersRepository customersRepository, IRegionManager regionManager)
        {
            _customersRepository = customersRepository;
            _regionManager = regionManager;

            Task.WaitAll(Task.Run(async () => await Load()));

            SearchCommand = new DelegateCommand(Search);

            EditCustomerCommand = new DelegateCommand(EditCustomer);

            DeleteCustomerCommand = new DelegateCommand(DeleteCustomer);

            ClearView();
        }

        private void DeleteCustomer()
        {
            if (cus != null)
            {
                Task.WaitAll(Task.Run(async () => await _customersRepository.DeleteCustomerAsync(cus)));

                ClearView();

                IsResultFound = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Can not find the customer", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearView();

                IsResultFound = Visibility.Collapsed;
            }

        }

        private void EditCustomer()
        {
            var parameters = new NavigationParameters();
            parameters.Add("Customer", cus);

            if (cus != null)
            {
                _regionManager.RequestNavigate("ManagerWorkspaceRegion", "EditCustomerView", parameters);
                _regionManager.RequestNavigate("StaffWorkspaceRegion", "EditCustomerView", parameters);
                ClearView();
            }
        }

        private void Search()
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            if (Input == String.Empty)
            {
                return;
            }

            cus = _customersList.Find(param => param.ID == Input || param.Username == Input);

            if (cus == null)
            {
                MessageBox.Show("No items match your search", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearView();

                IsResultFound = Visibility.Collapsed;
                return;
            }

            CustomerID = cus.ID;
            CustomerName = $"{cus.FirstName} {cus.LastName}";
            Gender = cus.Gender;
            IsVIP = cus.isVIP;
            TimeSpent = (double)cus.TotalUsingTime;
            TotalAmount = (double)cus.TotalUsingMoney;
            IsResultFound = Visibility.Visible;
        }

        private void ClearView()
        {
            CustomerID = String.Empty;
            CustomerName = String.Empty;
            Gender = String.Empty;
            IsVIP = String.Empty;
            TimeSpent = 0;
            TotalAmount = 0;
            IsResultFound = Visibility.Collapsed;
        }

        public async Task Load()
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
            Task.WaitAll(Task.Run(async () => await Load()));
            ClearView();
        }
    }
}
