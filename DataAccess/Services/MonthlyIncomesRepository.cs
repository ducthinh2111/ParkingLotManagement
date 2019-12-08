using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.DataModels;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class MonthlyIncomesRepository : IMonthlyIncomesRepository
    {

        public async Task AddMonthlyIncomeAsync(MonthlyIncome monthlyIncome)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.MonthlyIncome.Add(monthlyIncome);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteMonthlyIncomeAsync(MonthlyIncome monthlyIncome)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                MonthlyIncome income = await context.MonthlyIncome.FindAsync(monthlyIncome.ID);
                context.MonthlyIncome.Remove(income);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<MonthlyIncome>> GetMonthlyIncomesAsync()
        {
            using(ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.MonthlyIncome.ToListAsync();
            }
        }

        public async Task<MonthlyIncome> GetMonthlyIncomeAsync(MonthlyIncome monthlyIncome)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.MonthlyIncome.FindAsync(monthlyIncome.ID);
            }
        }

        public async Task UpdateMonthlyIncomeAsync(MonthlyIncome monthlyIncome)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(monthlyIncome).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
