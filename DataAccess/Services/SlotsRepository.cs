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
    public class SlotsRepository : ISlotsRepository
    {

        public async Task AddSlotAsync(Slot slot)
        {
            using(ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Slot.Add(slot);
                await context.SaveChangesAsync();
                context.Entry(slot).State = EntityState.Detached;
            }
        }

        public async Task DeleteSlotAsync(Slot slot)
        {
            using(ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                Slot sl = await context.Slot.FindAsync(slot.ID);
                context.Slot.Remove(sl);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Slot>> GetAllSlotsAsync()
        {
            using(ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Slot.ToListAsync();
            }
        }

        public async Task<Slot> GetSlotAsync(Slot slot)
        {
            using(ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Slot.FindAsync(slot.ID);
            }
        }

        public async Task UpdateSlotAsync(Slot slot)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(slot).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
