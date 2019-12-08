using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;


namespace CustomerReport.ViewModels
{
    public class CustomerReportViewModel : BindableBase, INavigationAware
    {
        private Customer cus;
        private string _id;
        private string _firstName;
        private string _lastName;
        private double _totalUsingTime;
        private double _totalUsingMoney;
        private string _isVip;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetProperty(ref _firstName, value);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        public double TotalUsingTime
        {
            get
            {
                return _totalUsingTime;
            }
            set
            {
                SetProperty(ref _totalUsingTime, value);
            }
        }

        public double TotalUsingMoney
        {
            get
            {
                return _totalUsingMoney;
            }
            set
            {
                SetProperty(ref _totalUsingMoney, value);
            }
        }

        public string IsVIP
        {
            get
            {
                return _isVip;
            }
            set
            {
                SetProperty(ref _isVip, value);
            }
        }

        public CustomerReportViewModel()
        {
            FirstName = "pro";
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
            cus = navigationContext.Parameters["Customer"] as Customer;

            if (cus != null)
            {
                ID = cus.ID;
                FirstName = cus.FirstName;
                LastName = cus.LastName;
                TotalUsingTime = (double)cus.TotalUsingTime;
                TotalUsingMoney = (double)cus.TotalUsingMoney;
                IsVIP = cus.isVIP;
            }
        }
    }
}
