using Domain.Entities;
using Domain.Interfaces;

namespace Dal.Entities;

public interface IGroupRepository : IRepository<IGroup, Guid>
{
    public Task<IGroup[]> FindByUser(Guid userId);
}