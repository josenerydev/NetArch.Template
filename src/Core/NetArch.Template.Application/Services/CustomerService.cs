using NetArch.Template.Application.Contracts.DTOs;
using NetArch.Template.Application.Contracts.Services;
using NetArch.Template.Domain.Entities;
using NetArch.Template.Domain.Repositories;
using NetArch.Template.Infrastructure.Abstractions.Mapping;

namespace NetArch.Template.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IObjectMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IObjectMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<IList<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<IList<CustomerDto>>(customers);
    }

    public async Task<CustomerDto> CreateAsync(CustomerCreateDto input)
    {
        var entity = _mapper.Map<Customer>(input);
        await _customerRepository.AddAsync(entity);
        await _customerRepository.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(entity);
    }

    public async Task<CustomerDto> UpdateAsync(int id, CustomerUpdateDto input)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        _mapper.Map(input, customer);

        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task DeleteAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
        await _customerRepository.SaveChangesAsync();
    }
}
