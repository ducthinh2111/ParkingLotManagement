using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using Prism.Commands;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace User.ViewModels
{
    public class StaffViewModel : BindableBase, INavigationAware
    {
        private BitmapImage _path;
        private string _userFullName;
        private string _userType;
        private Staff staff;
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

        public StaffViewModel(IRegionManager regionManager)
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
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "AddSlotView");
                        break;
                    case "Manage Slot":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "ManageSlotView");
                        break;
                    case "Check-in":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "CheckInView");
                        break;
                    case "Check-out":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "CheckOutView");
                        break;
                    case "Manage Parking":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "ManageParkingView");
                        break;
                    case "Add Account":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "StaffCreateAccountView");
                        break;
                    case "Manage Account":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "ManageAccountView");
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
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "RateView");
                        break;
                    case "Logout":
                        _regionManager.RequestNavigate("ContentRegion", "LoginView");
                        temp.IsSelected = false;
                        break;
                    case "Report":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "ReportView");
                        break;
                    case "Search":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "SearchView");
                        break;
                    case "Dashboard":
                        _regionManager.RequestNavigate("StaffWorkspaceRegion", "DashboardView");
                        break;

                }
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            staff = navigationContext.Parameters["Staff"] as Staff;

            if (staff != null)
            {
                UserFullName = $"{staff.FirstName} {staff.LastName}";
                UserType = "Staff";
                if (staff.Gender == "Male")
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
