using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.DataModels;

namespace Report.ViewModels
{
    public class ReportViewModel : BindableBase
    {
        private IMonthlyIncomesRepository _monthlyIncomeRepository;
        private List<MonthlyIncome> _monthlyIncomesList;
        private double _totalIncome;

        public double TotalIncome
        {
            get
            {
                return _totalIncome;
            }
            set
            {
                SetProperty(ref _totalIncome, value);
            }
        }

        public ReportViewModel(IMonthlyIncomesRepository monthlyIncomesRepository)
        {
            _monthlyIncomeRepository = monthlyIncomesRepository;

            _monthlyIncomesList = new List<MonthlyIncome>();

            Task.WaitAll(Task.Run(async () => _monthlyIncomesList = await _monthlyIncomeRepository.GetMonthlyIncomesAsync()));

            _monthlyIncomesList.ForEach(param => TotalIncome += (double)param.Income);
        }
        
    }
}
