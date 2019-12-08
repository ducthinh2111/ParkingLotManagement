using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using System.Collections.ObjectModel;
using Prism.Commands;
using System.Windows;

namespace Account.ViewModels
{
    public class ManageStaffAccountViewModel : BindableBase, INavigationAware
    {
        private IStaffsRepository _staffsRepository;
        private IRegionManager _regionManager;
        private List<Staff> _staffsList;
        private ObservableCollection<Staff> _staffsView;
        private double _maxHeight;
        private Window _window;

        public ObservableCollection<Staff> StaffsView
        {
            get
            {
                return _staffsView;
            }
            set
            {
                SetProperty(ref _staffsView, value);
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

        public DelegateCommand<Staff> EditStaffCommand { get; private set; }

        public DelegateCommand<Staff> DeleteStaffCommand { get; private set; }

        public DelegateCommand OrderByFirstNameCommand { get; private set; }

        public DelegateCommand OrderByLastNameCommand { get; private set; }

        public DelegateCommand OrderByMoneyCommand { get; private set; }

        public ManageStaffAccountViewModel(IStaffsRepository staffsRepository, IRegionManager regionManager)
        {
            _staffsRepository = staffsRepository;
            _regionManager = regionManager;
            _window = Application.Current.MainWindow;

            EditStaffCommand = new DelegateCommand<Staff>(EditStaff);

            DeleteStaffCommand = new DelegateCommand<Staff>(DeleteStaff);

            OrderByFirstNameCommand = new DelegateCommand(OrderByFirstName);

            OrderByLastNameCommand = new DelegateCommand(OrderByLastName);

            OrderByMoneyCommand = new DelegateCommand(OrderByMoney);

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

        }

        private void OrderByMoney()
        {
            StaffsView = new ObservableCollection<DataAccess.DataModels.Staff>();
            var sort = from staff in _staffsList orderby staff.Salary descending select staff;

            foreach (var staff in sort)
            {
                StaffsView.Add(staff);
            }
        }

        private void OrderByLastName()
        {
            StaffsView = new ObservableCollection<DataAccess.DataModels.Staff>();
            var sort = from staff in _staffsList orderby staff.LastName select staff;

            foreach (var staff in sort)
            {
                StaffsView.Add(staff);
            }
        }

        private void OrderByFirstName()
        {
            StaffsView = new ObservableCollection<DataAccess.DataModels.Staff>();
            var sort = from staff in _staffsList orderby staff.FirstName select staff;

            foreach (var staff in sort)
            {
                StaffsView.Add(staff);
            }
        }

        private void DeleteStaff(Staff obj)
        {
            Task.WaitAll(Task.Run(async () => await _staffsRepository.DeleteStaffAsync(obj)));

            LoadUI();
        }

        private void EditStaff(Staff obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Staff", obj);

            if (obj != null)
            {
                _regionManager.RequestNavigate("ManagerWorkspaceRegion", "EditStaffView", parameters);

            }
        }

        private void LoadUI()
        {
            StaffsView = new ObservableCollection<Staff>();
            _staffsList = new List<Staff>();
            var task = Task.Run(async () => _staffsList = await _staffsRepository.GetAllStaffsAsync());

            task.ContinueWith((t) =>
            {
                if(task.IsCompleted)
                {
                    foreach (var staff in _staffsList)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            StaffsView.Add(staff);
                        });
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
