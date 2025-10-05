using Dal.Models;
using Domain.Entities;

namespace Logic.Interfaces;

public interface IPermissionService: IService<PermissionDal, Guid>
{
    public Task<List<PermissionDal>> FindByUserAsync(Guid userId);
}