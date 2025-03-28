using NetArch.Template.Domain.Shared.DTOs;

namespace NetArch.Template.Application.Contracts.Services
{
    public interface IGenericAppService<TEntityDto, TCreateEntityDto, TUpdateEntityDto>
    {
        Task<TEntityDto> GetAsync(Guid id);
        Task<List<TEntityDto>> GetAllAsync();
        Task<PagedResultDto<TEntityDto>> GetPagedAsync(int skipCount, int maxResultCount);
        Task<TEntityDto> CreateAsync(TCreateEntityDto input);
        Task<TEntityDto> UpdateAsync(Guid id, TUpdateEntityDto input);
        Task DeleteAsync(Guid id);
    }
}
