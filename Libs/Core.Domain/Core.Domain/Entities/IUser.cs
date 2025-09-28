using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public interface IUser: IBaseEntity<Guid>
{
    string Name { get; init; }
    string Email { get; init; }
    string Password { get; init; }
    
    
}