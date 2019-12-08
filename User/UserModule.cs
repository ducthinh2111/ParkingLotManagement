using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Views;

namespace User
{
    public class UserModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ManagerView>();
            containerRegistry.RegisterForNavigation<StaffView>();
            containerRegistry.RegisterForNavigation<CustomerView>();

        }
    }
}
