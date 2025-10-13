using Core.Entities.Base;

namespace Core.Interfaces;

public interface IRepository<TEntity, in TIdentifier>
    where TEntity: IBaseEntity<TIdentifier>
{
    public Task<TEntity[]> GetAllAsync();
    
    public Task<TEntity?> GetByIdAsync(TIdentifier id);

    public Task CreateAsync(TEntity entity);
    
    public void Update(TEntity entity);
    
    public void Delete(TEntity entity);
    
    public Task SaveAsync();
}