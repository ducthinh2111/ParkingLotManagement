using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DataAccess.DataModels;
using Prism.Commands;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace User.ViewModels
{
    public class CustomerViewModel : BindableBase, INavigationAware
    {
        private BitmapImage _path;
        private string _userFullName;
        private string _userType;
        private Customer cus;
        private IRegionManager _regionManager;
        private double _selectedIndex;

        public BitmapImage Path
        {
            get
            {
                return _path;
            }
            set
            {
                SetProperty(ref _path, value);
            }
        }
        public string UserFullName
        {
            get
            {
                return _userFullName;
            }
            set
            {
                SetProperty(ref _userFullName, value);
            }
        }
        public string UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                SetProperty(ref _userType, value);
            }
        }
        public double SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                SetProperty(ref _selectedIndex, value);
            }
        }


        public DelegateCommand<object> ItemCommand { get; private set; }

        public DelegateCommand<string> SubItemCommand { get; private set; }

        public CustomerViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            ItemCommand = new DelegateCommand<object>(OnItemSelected);

            SubItemCommand = new DelegateCommand<string>(OnSubItemSelected);
        }

        private void OnSubItemSelected(string obj)
        {
            if (obj != null)
            {
                Debug.WriteLine(obj);
                switch (obj)
                {
                    case "Add Slot":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "AddSlotView");
                        break;
                    case "Manage Slot":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "ManageSlotView");
                        break;
                    case "Check-in":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "CheckInView");
                        break;
                    case "Check-out":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "CheckOutView");
                        break;
                    case "Manage Parking":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "ManageParkingView");
                        break;
                    case "Add Account":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "CreateAccountView");
                        break;
                    case "Manage Account":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "ManageAccountView");
                        break;

                }
            }
        }

        private void OnItemSelected(object obj)
        {
            var temp = obj as ListBoxItem;
            if (temp != null)
            {
                Debug.WriteLine(temp.Name);
                switch (temp.Name)
                {
                    case "Infrastructure":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "RateView");
                        break;
                    case "Logout":
                        _regionManager.RequestNavigate("ContentRegion", "LoginView");
                        temp.IsSelected = false;
                        break;
                    case "Report":
                        var parameters = new NavigationParameters();
                        parameters.Add("Customer",cus);
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "CustomerReportView", parameters);
                        break;
                    case "Search":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "SearchView");
                        break;
                    case "Dashboard":
                        _regionManager.RequestNavigate("CustomerWorkspaceRegion", "DashboardView");
                        break;

                }
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            cus = navigationContext.Parameters["Customer"] as Customer;

            if (cus != null)
            {
                UserFullName = $"{cus.FirstName} {cus.LastName}";
                UserType = "Customer";
                if(cus.Gender=="Male")
                {
                    Path = new BitmapImage(new Uri("pack://application:,,,/Infrastructure;component/Images/BoyAvatar.png"));
                }
                else
                {
                    Path = new BitmapImage(new Uri("pack://application:,,,/Infrastructure;component/Images/GirlAvatar.png"));

                }
            }
            SelectedIndex = 0;
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
