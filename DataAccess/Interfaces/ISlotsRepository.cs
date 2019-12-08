using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface ISlotsRepository
    {
        Task<List<Slot>> GetAllSlotsAsync();

        Task<Slot> GetSlotAsync(Slot slot);

        Task AddSlotAsync(Slot slot);

        Task UpdateSlotAsync(Slot slot);

        Task DeleteSlotAsync(Slot slot);
    }
}
