using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public interface IUser: IBaseEntity<Guid>
{
    string Name { get; init; }
    string Email { get; init; }
    
    UserRole Role { get; init; }
}

public interface ISecuredUser : IUser
{
    string  Password { get; }
}