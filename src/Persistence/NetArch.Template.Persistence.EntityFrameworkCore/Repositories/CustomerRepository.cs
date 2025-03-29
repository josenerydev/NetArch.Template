using Microsoft.EntityFrameworkCore;
using NetArch.Template.Domain.Entities;
using NetArch.Template.Domain.Repositories;
using NetArch.Template.Domain.Shared.Enums;

namespace NetArch.Template.Persistence.EntityFrameworkCore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbContext.Customers.FindAsync(id);
        }

        public async Task<IList<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<IList<Customer>> GetActiveAsync()
        {
            return await _dbContext
                .Customers.Where(c => c.Status == CustomerStatus.Active)
                .ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
        }

        public Task UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer != null)
            {
                _dbContext.Customers.Remove(customer);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
