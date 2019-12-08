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
    public class BlocksRepository : IBlocksRepository
    {
        public async Task AddBlockAsync(Block block)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Block.Add(block);
                await context.SaveChangesAsync();
                context.Entry(block).State = EntityState.Detached;
            }

        }

        public async Task DeleteBlockAsync(Block block)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                Block bl = await context.Block.FindAsync(block.ID);
                context.Block.Remove(bl);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Block>> GetAllBlocksAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Block.ToListAsync();
            }
        }

        public async Task<Block> GetBlockAsync(Block block)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Block.FindAsync(block.ID);
            }
        }

        public async Task UpdateBlockAsync(Block block)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(block).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
