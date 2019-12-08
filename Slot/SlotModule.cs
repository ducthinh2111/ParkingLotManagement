using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slot.Views;

namespace Slot
{
    public class SlotModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddSlotView>();
            containerRegistry.RegisterForNavigation<ManageSlotView>();
            containerRegistry.RegisterForNavigation<EditSlotView>();
        }
    }
}
