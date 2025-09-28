using Domain.Entities.Base;

namespace Domain.Entities;

public interface IUser: IBaseEntity<Guid>
{
    string Name { get; init; }
    string Email { get; init; }
    string Password { get; init; }
    IResource Photo { get; init; }
}