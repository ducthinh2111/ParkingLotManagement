using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IParkingLotsRepository
    {
        Task<List<ParkingLot>> GetAllParkingLotsAsync();

        Task<ParkingLot> GetParkingLotAsync(ParkingLot parkingLot);

        Task AddParkingLotAsync(ParkingLot parkingLot);

        Task UpdateParkingLotAsync(ParkingLot parkingLot);

        Task DeleteParkingLotAsync(ParkingLot parkingLot);
    }
}
