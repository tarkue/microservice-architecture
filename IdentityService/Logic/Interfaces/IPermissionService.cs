using Dal.Models;
using Core.Entities;
using Core.Interfaces;

namespace Logic.Interfaces;

public interface IPermissionService: IService<PermissionDal, Guid>
{
    public Task<List<PermissionDal>> FindByUserAsync(Guid userId);
    public Task CreateAsync(Guid userId, ICreatePermission createPermission);
    public Task DeleteAsync(Guid userId, Guid permissionId);
}