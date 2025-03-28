using NetArch.Template.Domain.Shared.DTOs;
using NetArch.Template.Domain.Shared.Interfaces;

using System.Linq.Expressions;

namespace NetArch.Template.Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : IEntity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<PagedResultDto<TEntity>> GetPagedAsync(int skipCount, int maxResultCount,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool ascending = true);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
