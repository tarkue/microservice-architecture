using Core.Entities;

namespace Domain.Interfaces.Connections;

public interface IGetPermission
{
    public Task<IPermission> GetAsync(Guid userId);
}