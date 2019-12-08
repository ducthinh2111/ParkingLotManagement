using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Views;
using Prism.Regions;

namespace Dashboard
{
    public class DashboardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ManagerWorkspaceRegion", typeof(DashboardView));
            regionManager.RegisterViewWithRegion("CustomerWorkspaceRegion", typeof(DashboardView));
            regionManager.RegisterViewWithRegion("StaffWorkspaceRegion", typeof(DashboardView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DashboardView>();
        }
    }
}
