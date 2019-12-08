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
    public class ParkingLotsRepository : IParkingLotsRepository
    {

        public async Task AddParkingLotAsync(ParkingLot parkingLot)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.ParkingLot.Add(parkingLot);
                await context.SaveChangesAsync();
            }
        }

        public async Task<ParkingLot> GetParkingLotAsync(ParkingLot parkingLot)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.ParkingLot.FindAsync(parkingLot.ID);
            }
        }

        public async Task<List<ParkingLot>> GetAllParkingLotsAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.ParkingLot.ToListAsync();
            }
        }

        public async Task UpdateParkingLotAsync(ParkingLot parkingLot)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(parkingLot).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteParkingLotAsync(ParkingLot parkingLot)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                ParkingLot lot = await context.ParkingLot.FindAsync(parkingLot.ID);
                context.ParkingLot.Remove(lot);
                await context.SaveChangesAsync();
            }
        }
    }
}
