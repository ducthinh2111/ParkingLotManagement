using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IMonthlyIncomesRepository
    {
        Task<List<MonthlyIncome>> GetMonthlyIncomesAsync();

        Task<MonthlyIncome> GetMonthlyIncomeAsync(MonthlyIncome monthlyIncome);

        Task AddMonthlyIncomeAsync(MonthlyIncome monthlyIncome);

        Task UpdateMonthlyIncomeAsync(MonthlyIncome monthlyIncome);

        Task DeleteMonthlyIncomeAsync(MonthlyIncome monthlyIncome);
    }
}
