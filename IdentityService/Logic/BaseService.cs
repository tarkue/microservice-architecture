using Domain.Entities.Base;
using Domain.Interfaces;
using Logic.Extensions;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic;

public class BaseService<TEntity, TIdentifier>(DbSet<TEntity> entity, DbContext context) : IService<TEntity, TIdentifier>
    where TEntity : class, IBaseEntity<TIdentifier>
{

    public async Task<IPaginatedResult<TEntity>> GetAllAsync(int pageIndex, int pageSize)
    {
        return await entity.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToPaginatedResultAsync(pageIndex, pageSize);
    }

    public async Task<TEntity?> FindByIdAsync(TIdentifier id)
    {
        return await entity.FindAsync(id);
    }

    public async Task CreateAsync(TEntity newEntity)
    {
        entity.Add(newEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity currentEntity)
    {
        entity.Update(currentEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity currentEntity)
    {
        entity.Remove(currentEntity);
        await context.SaveChangesAsync();
    }
}