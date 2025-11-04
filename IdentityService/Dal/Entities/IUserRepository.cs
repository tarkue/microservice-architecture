using Core.Entities;
using Core.Interfaces;

namespace Dal.Entities;

public interface IUserRepository: IRepository<IUser, Guid>
{
    public Task<IUser> FindByEmailAsync(string email);
}