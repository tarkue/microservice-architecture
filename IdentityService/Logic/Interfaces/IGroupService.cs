using Dal.Models;
using Core.Entities;
using Core.Interfaces;

namespace Logic.Interfaces;

public interface IGroupService : IService<GroupDal, Guid>
{
    public Task<List<GroupDal>> FindByUserAsync(Guid userId);
}