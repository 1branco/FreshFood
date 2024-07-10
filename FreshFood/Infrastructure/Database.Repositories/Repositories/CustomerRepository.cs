using Database.Contexts;
using Database.Entities.Customer;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly CoreDbContext context;
        public CustomerRepository(CoreDbContext _context) : base(_context)
        {
            context = _context;
        }

        public async Task<Customer> RegisterNewCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var entity = context.Customers.Add(customer).Entity;

            await SaveChangesAsync();

            return entity;
        }
    }
}
