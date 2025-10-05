using Dal.Models;
using Domain.Entities;

namespace Logic.Interfaces;

public interface IGroupService: IService<GroupDal, Guid>
{
    public Task<List<GroupDal>> FindByUserAsync(Guid userId);
}