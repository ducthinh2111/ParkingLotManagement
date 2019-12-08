using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;

namespace Dashboard.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private ICustomersRepository _customersRepository;
        private IStaffsRepository _staffsRepository;
        private IBlocksRepository _blocksRepository;
        private List<Customer> _customersList;
        private List<Staff> _staffsList;
        private List<Block> _blocksList;
        private double _totalBlocks;
        private double _totalSlots;
        private double _totalStaffs;
        private double _totalCustomers;
        private double _totalVIPMember;

        public double TotalBlocks
        {
            get
            {
                return _totalBlocks;
            }
            set
            {
                SetProperty(ref _totalBlocks, value);
            }
        }

        public double TotalSlots
        {
            get
            {
                return _totalSlots;
            }
            set
            {
                SetProperty(ref _totalSlots, value);
            }
        }

        public double TotalStaffs
        {
            get
            {
                return _totalStaffs;
            }
            set
            {
                SetProperty(ref _totalStaffs, value);
            }
        }

        public double TotalCustomers
        {
            get
            {
                return _totalCustomers;
            }
            set
            {
                SetProperty(ref _totalCustomers, value);
            }
        }

        public double TotalVIPMember
        {
            get
            {
                return _totalVIPMember;
            }
            set
            {
                SetProperty(ref _totalVIPMember, value);
            }
        }


        public DashboardViewModel(ICustomersRepository customersRepository, IStaffsRepository staffsRepository, IBlocksRepository blocksRepository)
        {
            _customersRepository = customersRepository;
            _staffsRepository = staffsRepository;
            _blocksRepository = blocksRepository;

            LoadUI();
        }


        private void LoadUI()
        {
            TotalBlocks = 0;
            TotalCustomers = 0;
            TotalSlots = 0;
            TotalStaffs = 0;
            TotalVIPMember = 0;
            _customersList = new List<Customer>();
            var task1 = Task.Run(async () => _customersList = await _customersRepository.GetAllCustomersAsync());

            _staffsList = new List<Staff>();
            var task2 = Task.Run(async () => _staffsList = await _staffsRepository.GetAllStaffsAsync());

            _blocksList = new List<Block>();
            var task3 = Task.Run(async () => _blocksList = await _blocksRepository.GetAllBlocksAsync());

            var result = Task.WhenAll(task1, task2, task3);
            result.ContinueWith((t) =>
            {
                if(result.IsCompleted)
                {
                    TotalBlocks = _blocksList.Count;
                    _blocksList.ForEach(param => TotalSlots += (int)param.NumberOfSlots);
                    TotalStaffs = _staffsList.Count;
                    TotalCustomers = _customersList.Count;
                    foreach (var cus in _customersList)
                    {
                        if (cus.isVIP == "Yes")
                        {
                            TotalVIPMember++;
                        }
                    }
                }
            });

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
            LoadUI();
        }
    }
}
