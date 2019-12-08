using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Account.ViewModels
{
    public class CreateAccountViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        public DelegateCommand<object> ItemCommand { get; set; }

        public CreateAccountViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ItemCommand = new DelegateCommand<object>(OnSelectionChange);
        }

        

        private void OnSelectionChange(object obj)
        {
            var temp = obj as ComboBoxItem;
            if(temp != null)
            {
                if(temp.Content.ToString() == "Staff")
                {
                    _regionManager.RequestNavigate("CreateAccountRegion", "CreateStaffAccountView");
                }
                if (temp.Content.ToString() == "Customer")
                {
                    _regionManager.RequestNavigate("CreateAccountRegion", "CreateCustomerAccountView");
                }
            }
        }
    }
}
