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
    public class CustomersRepository : ICustomersRepository
    {

        public async Task AddCustomerAsync(Customer customer)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Customer.Add(customer);
                await context.SaveChangesAsync();
                context.Entry(customer).State = EntityState.Detached;
            }
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                Customer cus = await context.Customer.FindAsync(customer.ID);
                context.Customer.Remove(cus);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Customer.ToListAsync();
            }
        }

        public async Task<Customer> GetCustomerAsync(Customer customer)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                return await context.Customer.FindAsync(customer.ID);
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using (ParkingLotManagementContext context = new ParkingLotManagementContext())
            {
                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
