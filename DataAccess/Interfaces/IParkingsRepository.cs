using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IParkingsRepository
    {
        Task<List<Parking>> GetAllParkingsAsync();

        Task<Parking> GetParkingAsync(Parking parking);

        Task AddParkingAsync(Parking parking);

        Task UpdateParkingAsync(Parking parking);

        Task DeleteParkingAsync(Parking parking);
    }
}
