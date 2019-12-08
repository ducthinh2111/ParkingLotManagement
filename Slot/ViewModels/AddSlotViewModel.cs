using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Prism.Commands;
using System.Windows.Controls;
using System.Diagnostics;
using DataAccess.DataModels;
using System.Windows;
using Prism.Regions;

namespace Slot.ViewModels
{
    public class AddSlotViewModel : BindableBase, INavigationAware
    {
        private List<DataAccess.DataModels.Block> _blocksList;
        private List<DataAccess.DataModels.Slot> _slotsList;
        private ISlotsRepository _slotsRepository;
        private IBlocksRepository _blocksRepository;
        private IRegionManager _regionManager;
        private ObservableCollection<DataAccess.DataModels.Block> _blocksView;
        private string _inputName;
        private bool _canInputName = false;
        private string _block = null;
        private string _status = null;

        public ObservableCollection<Block> BlocksView
        {
            get
            {
                return _blocksView;
            }
            set
            {
                SetProperty(ref _blocksView, value);
            }
        }

        public string InputName
        {
            get
            {
                return _inputName;
            }
            set
            {
                SetProperty(ref _inputName, value);
            }
        }

        public bool CanInputName
        {
            get
            {
                return _canInputName;
            }
            set
            {
                SetProperty(ref _canInputName, value);
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

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand<object> SelectBlockCommand { get; private set; }

        public AddSlotViewModel(ISlotsRepository slotsRepository, IBlocksRepository blocksRepository, IRegionManager regionManager)
        {
            _slotsRepository = slotsRepository;
            _blocksRepository = blocksRepository;
            _regionManager = regionManager;

            AddCommand = new DelegateCommand(Add);
            CancelCommand = new DelegateCommand(Cancel);

            SelectBlockCommand = new DelegateCommand<object>(OnSelectBlockChanged);
        }

        private void Cancel()
        {
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "ManageSlotView");
        }

        private void OnSelectBlockChanged(object obj)
        {
            if (obj != null)
            {
                CanInputName = true;
            }
        }

        private async Task Load()
        {
            _slotsList = new List<DataAccess.DataModels.Slot>();
            _slotsList = await _slotsRepository.GetAllSlotsAsync();

            _blocksList = new List<DataAccess.DataModels.Block>();
            _blocksList = await _blocksRepository.GetAllBlocksAsync();
        }

        private void Add()
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            if (Block == null || Status == null || InputName == String.Empty || InputName == null)
            {
                MessageBox.Show("You must fill in all fields first!", "Notification", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            var temp = InputName.Insert(0, Block);
            if (!CheckInputName(temp))
            {
                MessageBox.Show("Name has existed, choose other name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataAccess.DataModels.Slot item = new DataAccess.DataModels.Slot
            {
                ID = $"HCMUTEPL{temp}",
                Name = temp,
                Status = Status,
                Availability = "Yes",
                BlockID = $"HCMUTEPL{Block}"
            };

            var updateblock = _blocksList.Find(param => param.ID == item.BlockID);
            updateblock.NumberOfSlots++;

            var task1 = Task.Run(async () => await _blocksRepository.UpdateBlockAsync(updateblock));

            var task2 = Task.Run(async () => await _slotsRepository.AddSlotAsync(item));

            Task.WaitAll(task1, task2);
            InputName = String.Empty;
            MessageBox.Show("Slot has been added!", "Successful", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _canInputName = false;
            Task.WaitAll(Task.Run(async () => await Load()));
            BlocksView = new ObservableCollection<Block>(_blocksList);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private bool CheckInputName(string name)
        {

            foreach(var item in _slotsList)
            {
                if(name == item.Name)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
