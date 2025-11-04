using Core.Entities.Base;

namespace Core.Entities;

public interface IGroup: IBaseEntity<Guid>
{
    string Name { get; set; }
}