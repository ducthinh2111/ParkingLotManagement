using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;
using DataAccess.Interfaces;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class ParkingsRepository : IParkingsRepository
    {

        public async Task AddParkingAsync(Parking parking)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Parking.Add(parking);
                await context.SaveChangesAsync();
                context.Entry(parking).State = EntityState.Detached;
            }
        }

        public async Task DeleteParkingAsync(Parking parking)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                Parking pk = await context.Parking.FindAsync(parking.LicensePlate);
                context.Parking.Remove(pk);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Parking>> GetAllParkingsAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Parking.ToListAsync();
            }
        }

        public async Task<Parking> GetParkingAsync(Parking parking)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Parking.FindAsync(parking.LicensePlate);
            }
        }

        public async Task UpdateParkingAsync(Parking parking)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(parking).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
