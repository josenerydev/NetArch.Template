using NetArch.Template.Application.Contracts.DTOs;

namespace NetArch.Template.Application.Contracts.Services;

public interface ICustomerService
{
    Task<CustomerDto> GetByIdAsync(int id);
    Task<IList<CustomerDto>> GetAllAsync();
    Task<CustomerDto> CreateAsync(CustomerCreateDto input);
    Task<CustomerDto> UpdateAsync(int id, CustomerUpdateDto input);
    Task DeleteAsync(int id);
}
