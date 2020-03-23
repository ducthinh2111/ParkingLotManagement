using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ParkingLotManagement.Shell.Views;
using DataAccess.Interfaces;
using DataAccess.Services;

namespace ParkingLotManagement.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<Login.LoginModule>();
            moduleCatalog.AddModule<User.UserModule>();
            moduleCatalog.AddModule<Rate.RateModule>();
            moduleCatalog.AddModule<Slot.SlotModule>();
            moduleCatalog.AddModule<Parking.ParkingModule>();
            moduleCatalog.AddModule<Account.AccountModule>();
            moduleCatalog.AddModule<Search.SearchModule>();
            moduleCatalog.AddModule<Dashboard.DashboardModule>();
            moduleCatalog.AddModule<CustomerReport.CustomerReportModule>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IParkingLotsRepository, ParkingLotsRepository>();
            containerRegistry.Register<ISlotsRepository, SlotsRepository>();
            containerRegistry.Register<IBlocksRepository, BlocksRepository>();
            containerRegistry.Register<IParkingsRepository, ParkingsRepository>();
            containerRegistry.Register<ICustomersRepository, CustomersRepository>();
            containerRegistry.Register<IMonthlyIncomesRepository, MonthlyIncomesRepository>();
            containerRegistry.Register<IStaffsRepository, StaffsRepository>();
            containerRegistry.Register<IAdminAccountRepository, AdminAccountRepository>();
        }
    }
}
