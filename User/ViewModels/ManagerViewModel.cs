using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using DataAccess.DataModels;

namespace User.ViewModels
{
    public class ManagerViewModel : BindableBase, INavigationAware
    {
        private ImageSource _path;
        private string _userFullName;
        private string _userType;
        private AdminAccount admin;
        private IRegionManager _regionManager;
        private double _selectedIndex;

        public ImageSource Path
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

        public ManagerViewModel(IRegionManager regionManager)
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
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "AddSlotView");
                        break;
                    case "Manage Slot":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageSlotView");
                        break;
                    case "Check-in":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "CheckInView");
                        break;
                    case "Check-out":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "CheckOutView");
                        break;
                    case "Manage Parking":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageParkingView");
                        break;
                    case "Add Account":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "CreateAccountView");
                        break;
                    case "Manage Account":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageAccountView");
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
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "RateView");
                        break;
                    case "Logout":
                       _regionManager.RequestNavigate("ContentRegion", "LoginView");
                        temp.IsSelected = false;
                        break;
                    case "Report":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ReportView");
                        break;
                    case "Search":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "SearchView");
                        break;
                    case "Dashboard":
                        _regionManager.RequestNavigate("ManagerWorkspaceRegion", "DashboardView");
                        break;

                }
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            admin = navigationContext.Parameters["Admin"] as AdminAccount;
            if (admin != null)
            {
                UserFullName = $"{admin.Name}";
                UserType = "Admin";
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

