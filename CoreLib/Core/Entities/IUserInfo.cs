using Core.Entities.Base;

namespace Core.Entities;

public interface IUserInfo: IBaseEntity<Guid>
{
    string Name { get; init; }
    IResource? Photo { get; init; }
}