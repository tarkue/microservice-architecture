using Domain.Entities.Base;

namespace Domain.Entities;

public interface IGroup: IBaseEntity<Guid>
{
    string Name { get; set; }
}