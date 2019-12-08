using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Diagnostics;
using Prism.Regions;
using DataAccess.Interfaces;
using DataAccess.DataModels;
using System.Windows;

namespace Slot.ViewModels
{
    public class ManageSlotViewModel : BindableBase, INavigationAware
    {

        #region Private Attributes
        private List<DataAccess.DataModels.Block> _blocksList;
        private List<DataAccess.DataModels.Slot> _slotsList;
        private ISlotsRepository _slotsRepository;
        private IBlocksRepository _blocksRepository;
        private IRegionManager _regionManager;
        private ObservableCollection<DataAccess.DataModels.Slot> _slotsView;
        private ObservableCollection<DataAccess.DataModels.Block> _blocksView;
        private object _selectedBlock;
        private double _maxHeight;
        private Window _window;
        #endregion


        #region Public Properties
        public ObservableCollection<DataAccess.DataModels.Slot> SlotsView
        {
            get
            {
                return _slotsView;
            }
            set
            {
                SetProperty(ref _slotsView, value);
            }
        }

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

        public object SelectedBlock
        {
            get
            {
                return _selectedBlock;
            }
            set
            {
                SetProperty(ref _selectedBlock, value);
            }
        }

        public double MaxHeight
        {
            get
            {
                return _maxHeight;
            }
            set
            {
                SetProperty(ref _maxHeight, value);
            }
        }
        #endregion

        #region Command
        public DelegateCommand<DataAccess.DataModels.Slot> EditSlotCommand { get; private set; }

        public DelegateCommand<DataAccess.DataModels.Slot> DeleteSlotCommand { get; private set; }

        public DelegateCommand<object> SelectBlockCommand { get; private set; }

        public DelegateCommand AddSlotCommand { get; private set; }

        public DelegateCommand ViewAllCommand { get; private set; }

        public DelegateCommand OrderByAvailabilityCommand { get; private set; }

        public DelegateCommand OrderByStatusCommand { get; private set; }

        public DelegateCommand OrderByNameCommand { get; private set; }
        #endregion

        public ManageSlotViewModel(IRegionManager regionManager, ISlotsRepository slotsRepository, IBlocksRepository blocksRepository)
        {
            _regionManager = regionManager;
            _slotsRepository = slotsRepository;
            _blocksRepository = blocksRepository;
            _window = Application.Current.MainWindow;

            EditSlotCommand = new DelegateCommand<DataAccess.DataModels.Slot>(EditSlot);
            DeleteSlotCommand = new DelegateCommand<DataAccess.DataModels.Slot>(DeleteSlot);
            SelectBlockCommand = new DelegateCommand<object>(OnBlockChanged);
            AddSlotCommand = new DelegateCommand(AddSlot);
            ViewAllCommand = new DelegateCommand(LoadUI);
            OrderByAvailabilityCommand = new DelegateCommand(OrderByAvailability);
            OrderByStatusCommand = new DelegateCommand(OrderByStatus);
            OrderByNameCommand = new DelegateCommand(OrderByName);

            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            BlocksView = new ObservableCollection<Block>();

            #region Handle Window State Changed Event
            if (_window.WindowState == WindowState.Normal)
            {
                MaxHeight = 450;
            }
            else
            {
                MaxHeight = 700;
            }
            _window.StateChanged += (sender, e) =>
            {
                if (_window.WindowState == WindowState.Normal)
                {
                    MaxHeight = 450;
                }
                else
                {
                    MaxHeight = 700;
                }
            };
            #endregion
        }

        private void OrderByName()
        {
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            var sort = from slot in _slotsList orderby slot.Name select slot;

            foreach (var slot in sort)
            {
                SlotsView.Add(slot);
            }
        }

        private void OrderByStatus()
        {
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            var sort = from slot in _slotsList orderby slot.Status select slot;

            foreach(var slot in sort)
            {
                SlotsView.Add(slot);
            }
        }

        private void OrderByAvailability()
        {
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            var sort = from slot in _slotsList orderby slot.Availability ascending select slot;

            foreach (var slot in sort)
            {
                SlotsView.Add(slot);
            }
        }

        private void LoadUI()
        {
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            BlocksView = new ObservableCollection<Block>();
            SelectedBlock = null;
            _slotsList = new List<DataAccess.DataModels.Slot>();
            _blocksList = new List<DataAccess.DataModels.Block>();

            var task1 = Task.Run(async () => _slotsList = await _slotsRepository.GetAllSlotsAsync());
            var task2 = Task.Run(async () => _blocksList = await _blocksRepository.GetAllBlocksAsync());

            var result = Task.WhenAll(task1, task2);

            result.ContinueWith((t) =>
            {
                if (t.IsCompleted)
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        foreach (var slot in _slotsList)
                        {
                            SlotsView.Add(slot);
                        }
                        foreach (var block in _blocksList)
                        {
                            BlocksView.Add(block);
                        }
                    });
                }
            });

        }

        private void DeleteSlot(DataAccess.DataModels.Slot obj)
        {
            Task.WaitAll(Task.Run(async () => await Load()));
            var updateblock = _blocksList.Find(param => param.ID == obj.BlockID);
            if (updateblock.NumberOfSlots != 0)
            {
                if (MessageBox.Show("Do you want to delete?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Task.WaitAll(Task.Run(async () => await _slotsRepository.DeleteSlotAsync(obj)));
                        updateblock.NumberOfSlots--;
                        Task.WaitAll(Task.Run(async () => await _blocksRepository.UpdateBlockAsync(updateblock)));
                    }
                    catch
                    {
                        MessageBox.Show("This slot has been using!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }

            }

            Task.WaitAll(Task.Run(async () => await Load()));
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            if (SelectedBlock != null)
            {
                SlotsView.AddRange(_slotsList.FindAll(param => param.BlockID == $"HCMUTEPL{SelectedBlock.ToString()}"));
            }
            else
            {
                SlotsView.AddRange(_slotsList.GetRange(0, _slotsList.Count));
            }
        }

        private void AddSlot()
        {
            _regionManager.RequestNavigate("ManagerWorkspaceRegion", "AddSlotView");
        }

        private void EditSlot(DataAccess.DataModels.Slot obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Slot", obj);

            if (obj != null)
            {
                _regionManager.RequestNavigate("ManagerWorkspaceRegion", "EditSlotView", parameters);
            }
        }

        private void OnBlockChanged(object obj)
        {
            SlotsView = new ObservableCollection<DataAccess.DataModels.Slot>();
            if (obj != null)
            {
                SlotsView.AddRange(_slotsList.FindAll(param => param.BlockID == $"HCMUTEPL{obj.ToString()}"));
            }

        }

        private async Task Load()
        {
            _slotsList = new List<DataAccess.DataModels.Slot>();
            _slotsList = await _slotsRepository.GetAllSlotsAsync();

            _blocksList = new List<DataAccess.DataModels.Block>();
            _blocksList = await _blocksRepository.GetAllBlocksAsync();

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadUI();
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
