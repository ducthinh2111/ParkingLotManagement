using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IStaffsRepository
    {
        Task<List<Staff>> GetAllStaffsAsync();

        Task<Staff> GetStaffAsync(Staff staff);

        Task AddStaffAsync(Staff staff);

        Task UpdateStaffAsync(Staff staff);

        Task DeleteStaffAsync(Staff staff);
    }
}
