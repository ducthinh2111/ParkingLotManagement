using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IBlocksRepository
    {
        Task<List<Block>> GetAllBlocksAsync();

        Task<Block> GetBlockAsync(Block block);

        Task AddBlockAsync(Block block);

        Task UpdateBlockAsync(Block block);

        Task DeleteBlockAsync(Block block);
    }
}
