using Domain.Entities;
using Domain.Interfaces;

namespace Dal.Entities;

public interface IPermissionRepository: IRepository<IPermission, Guid>
{
    public Task<IPermission[]> FindByUser(Guid userId);
}