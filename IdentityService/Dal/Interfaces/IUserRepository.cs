using Domain.Entities;

namespace Dal.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Возвращает пользователя по уникальному идентификатору
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<IUser> GetUserById(Guid userId);
}