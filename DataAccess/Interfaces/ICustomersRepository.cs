using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface ICustomersRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();

        Task<Customer> GetCustomerAsync(Customer customer);

        Task AddCustomerAsync(Customer customer);

        Task UpdateCustomerAsync(Customer customer);

        Task DeleteCustomerAsync(Customer customer);
    }
}
