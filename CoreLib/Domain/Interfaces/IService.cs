using Domain.Entities.Base;
using Domain.Interfaces;

namespace Logic.Interfaces;

public interface IService<TEntity, in TIdentifier>
    where TEntity: IBaseEntity<TIdentifier>
{
    public Task<IPaginatedResult<TEntity>> GetAllAsync(int pageIndex, int pageSize);
    public Task<TEntity?> FindByIdAsync(TIdentifier id);
    public Task CreateAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
}