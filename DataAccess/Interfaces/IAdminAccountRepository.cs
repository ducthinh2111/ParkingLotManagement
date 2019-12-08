using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IAdminAccountRepository
    {
        Task<List<AdminAccount>> GetAllAdminAccountsAsync();

        Task<AdminAccount> GetAdminAccountAsync(AdminAccount adminAccount);

        Task AddAdminAccountAsync(AdminAccount adminAccount);

        Task DeleteAdminAccountAsync(AdminAccount adminAccount);
    }
}
