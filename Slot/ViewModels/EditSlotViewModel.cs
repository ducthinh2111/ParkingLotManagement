using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using Prism.Commands;
using DataAccess.Interfaces;
using System.Windows;

namespace Slot.ViewModels
{
    public class EditSlotViewModel : BindableBase, INavigationAware
    {
        private DataAccess.DataModels.Slot _selectedSlot;
        private ISlotsRepository _slotsRepository;
        private IRegionManager _regionManager;
        private string _name;
        private string _status;
        private string _availability;
        private string _block;

        public DataAccess.DataModels.Slot SelectedSlot
        {
            get
            {
                return _selectedSlot;
            }
            set
            {
                SetProperty(ref _selectedSlot, value);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        public string Availability
        {
            get
            {
                return _availability;
            }
            set
            {
                SetProperty(ref _availability, value);
            }
        }

        public string Block
        {
            get
            {
                return _block;
            }
            set
            {
                SetProperty(ref _block, value);
            }
        }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand DoneCommand { get; private set; }

        public EditSlotViewModel(ISlotsRepository slotsRepository, IRegionManager regionManager)
        {
            _slotsRepository = slotsRepository;
            _regionManager = regionManager;

            SaveChangesCommand = new DelegateCommand(Save);

            DoneCommand = new DelegateCommand(Done);
        }

        private void Done()
        {
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageSlotView");
            _regionManager.RequestNavigate("StaffWorkspaceRegion", "ManageSlotView");
        }

        private void Save()
        {
            SelectedSlot.Status = Status;
            if(SelectedSlot.Status == "Active")
            {
                SelectedSlot.Availability = "Yes";
            }
            else
            {
                SelectedSlot.Availability = "No";
            }
            Task.WaitAll(Task.Run(async () => await _slotsRepository.UpdateSlotAsync(SelectedSlot)));
            MessageBox.Show("Data have been saved!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
          
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var slot = navigationContext.Parameters["Slot"] as DataAccess.DataModels.Slot;
            if (slot != null)
            {
                SelectedSlot = slot;
                Name = SelectedSlot.Name.Substring(1);
                Status = SelectedSlot.Status;
                Availability = SelectedSlot.Availability;
                Block = SelectedSlot.BlockID.Last().ToString();
            }
        }

    }
}
