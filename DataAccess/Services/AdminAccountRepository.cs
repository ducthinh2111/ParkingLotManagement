using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class AdminAccountRepository : IAdminAccountRepository
    {

        public async Task AddAdminAccountAsync(AdminAccount adminAccount)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.AdminAccount.Add(adminAccount);
                await context.SaveChangesAsync();
                context.Entry(adminAccount).State = EntityState.Detached;
            }
        }

        public async Task DeleteAdminAccountAsync(AdminAccount adminAccount)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                AdminAccount admin = await context.AdminAccount.FindAsync(adminAccount.ID);
                context.AdminAccount.Remove(admin);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<AdminAccount>> GetAllAdminAccountsAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.AdminAccount.ToListAsync();
            }
        }

        public async Task<AdminAccount> GetAdminAccountAsync(AdminAccount adminAccount)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.AdminAccount.FindAsync(adminAccount.ID);
            }
        }
    }
}
