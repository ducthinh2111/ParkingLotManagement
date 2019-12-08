using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parking.Views;

namespace Parking
{
    public class ParkingModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CheckInView>();
            containerRegistry.RegisterForNavigation<CheckOutView>();
            containerRegistry.RegisterForNavigation<ManageParkingView>();
        }
    }
}
