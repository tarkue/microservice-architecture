using Domain.Entities.Base;

namespace Domain.Interfaces;

public interface IService<TEntity, in TIdentifier>
    where TEntity: IBaseEntity<TIdentifier>
{
    public Task<IPaginatedResult<TEntity>> GetAllAsync(int pageIndex, int pageSize);
    public Task<TEntity?> FindByIdAsync(TIdentifier id);
    public Task CreateFromDalAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
}