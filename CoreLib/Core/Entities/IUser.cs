using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities;

public interface IUser : IBaseEntity<Guid>
{
    string Name { get; init; }
    string Email { get; init; }
}

public interface ISecuredUser : IUser
{
    string  Password { get; }
}

public interface IUserCreate
{
    string Name { get; init; }
    string Email { get; init; }
    string Password { get; init; }
}

public interface IUserUpdate
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public Guid? Photo { get; init; }
}
