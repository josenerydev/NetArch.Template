using NetArch.Template.Domain.Entities;

namespace NetArch.Template.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<IList<Customer>> GetAllAsync();
    Task<IList<Customer>> GetActiveAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
