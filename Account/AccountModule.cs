using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Views;

namespace Account
{
    public class AccountModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("CreateAccountRegion", typeof(CreateStaffAccountView));
            regionManager.RegisterViewWithRegion("ManageAccountRegion", typeof(ManageStaffAccountView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CreateAccountView>();
            containerRegistry.RegisterForNavigation<CreateCustomerAccountView>();
            containerRegistry.RegisterForNavigation<CreateStaffAccountView>();
            containerRegistry.RegisterForNavigation<ManageAccountView>();
            containerRegistry.RegisterForNavigation<ManageStaffAccountView>();
            containerRegistry.RegisterForNavigation<ManageCustomerAccountView>();
            containerRegistry.RegisterForNavigation<EditStaffView>();
            containerRegistry.RegisterForNavigation<EditCustomerView>();
            containerRegistry.RegisterForNavigation<StaffCreateAccountView>();
        }
    }
}
