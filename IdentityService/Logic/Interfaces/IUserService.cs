using Dal.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Logic.Interfaces;

public interface IUserService: IService<UserDal, Guid>
{
    public Task<IPaginatedResult<UserDal>> GetAllAsync(int pageIndex, int pageSize, string? search);
    public Task<UserDal?> FindByEmailAsync(string email);
}