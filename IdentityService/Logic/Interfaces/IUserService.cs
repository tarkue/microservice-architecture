using Dal.Models;
using Core.Entities;
using Core.Interfaces;

namespace Logic.Interfaces;

public interface IUserService : IService<UserDal, Guid>
{
    public Task<IPaginatedResult<UserDal>> GetAllAsync(int? pageIndex, int? pageSize, string? search = null);
    public Task<UserDal> GetByIdOrThrow(Guid id);
    public Task UpdateAsync(UserDal user, IUserUpdate updateBody);

    public Task<UserDal?> FindByEmailAsync(string email);
}