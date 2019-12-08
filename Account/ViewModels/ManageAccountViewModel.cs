using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Account.ViewModels
{
    public class ManageAccountViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private object _selectedItem;

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        public DelegateCommand<object> ItemCommand { get; set; }

        public DelegateCommand AddAccountCommand { get; set; }

        public ManageAccountViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ItemCommand = new DelegateCommand<object>(OnSelectionChange);

            AddAccountCommand = new DelegateCommand(AddAccount);
        }

        private void AddAccount()
        {
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "CreateAccountView");
        }

        private void OnSelectionChange(object obj)
        {
            var temp = obj as ComboBoxItem;
            if (temp != null)
            {
                if (temp.Content.ToString() == "Staff")
                {
                    _regionManager.RequestNavigate("ManageAccountRegion", "ManageStaffAccountView");
                }
                if (temp.Content.ToString() == "Customer")
                {
                    _regionManager.RequestNavigate("ManageAccountRegion", "ManageCustomerAccountView");
                }
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedItem = null;
            _regionManager.Regions["ManageAccountRegion"].RemoveAll();
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
