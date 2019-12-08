using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class StaffsRepository : IStaffsRepository
    {

        public async Task AddStaffAsync(Staff staff)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Staff.Add(staff);
                await context.SaveChangesAsync();
                context.Entry(staff).State = EntityState.Detached;
            }
        }

        public async Task DeleteStaffAsync(Staff staff)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                Staff st = await context.Staff.FindAsync(staff.ID);
                context.Staff.Remove(st);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Staff>> GetAllStaffsAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Staff.ToListAsync();
            }
        }

        public async Task<Staff> GetStaffAsync(Staff staff)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Staff.FindAsync(staff.ID);
            }
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(staff).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
