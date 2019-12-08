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

namespace Account.ViewModels
{
    public class ManageCustomerAccountViewModel : BindableBase, INavigationAware
    {
        private ICustomersRepository _customersRepository;
        private IRegionManager _regionManager;
        private List<Customer> _customersList;
        private ObservableCollection<Customer> _customersView;
        private Window _window;
        private double _maxHeight;


        public ObservableCollection<Customer> CustomersView
        {
            get
            {
                return _customersView;
            }
            set
            {
                SetProperty(ref _customersView, value);
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

        public DelegateCommand<Customer> EditCustomerCommand { get; private set; }

        public DelegateCommand<Customer> DeleteCustomerCommand { get; private set; }

        public DelegateCommand OrderByFirstNameCommand { get; private set; }

        public DelegateCommand OrderByLastNameCommand { get; private set; }

        public DelegateCommand OrderByMoneyCommand { get; private set; }

        public ManageCustomerAccountViewModel(ICustomersRepository customersRepository, IRegionManager regionManager)
        {
            _customersRepository = customersRepository;
            _regionManager = regionManager;
            _window = Application.Current.MainWindow;

            EditCustomerCommand = new DelegateCommand<Customer>(EditCustomer);

            DeleteCustomerCommand = new DelegateCommand<Customer>(DeleteCustomer);

            OrderByFirstNameCommand = new DelegateCommand(OrderByFirstName);

            OrderByLastNameCommand = new DelegateCommand(OrderByLastName);

            OrderByMoneyCommand = new DelegateCommand(OrderByMoney);

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

        private void OrderByMoney()
        {
            CustomersView = new ObservableCollection<DataAccess.DataModels.Customer>();
            var sort = from cus in _customersList orderby cus.TotalUsingMoney descending select cus;

            foreach (var cus in sort)
            {
                CustomersView.Add(cus);
            }
        }

        private void OrderByLastName()
        {
            CustomersView = new ObservableCollection<DataAccess.DataModels.Customer>();
            var sort = from cus in _customersList orderby cus.LastName select cus;

            foreach (var cus in sort)
            {
                CustomersView.Add(cus);
            }
        }

        private void OrderByFirstName()
        {
            CustomersView = new ObservableCollection<DataAccess.DataModels.Customer>();
            var sort = from cus in _customersList orderby cus.FirstName select cus;

            foreach (var cus in sort)
            {
                CustomersView.Add(cus);
            }
        }

        private void DeleteCustomer(Customer obj)
        {
            Task.WaitAll(Task.Run(async () => await _customersRepository.DeleteCustomerAsync(obj)));

            LoadUI();
        }

        private void EditCustomer(Customer obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Customer", obj);

            if (obj != null)
            {
                _regionManager.RequestNavigate("ManagerWorkspaceRegion", "EditCustomerView", parameters);
            }
        }

        private void LoadUI()
        {
            CustomersView = new ObservableCollection<Customer>();
            _customersList = new List<Customer>();
            var task = Task.Run(async () => _customersList = await _customersRepository.GetAllCustomersAsync());

            task.ContinueWith((t) =>
            {
                if(task.IsCompleted)
                {
                    foreach (var cus in _customersList)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CustomersView.Add(cus);
                        });
                    }
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
