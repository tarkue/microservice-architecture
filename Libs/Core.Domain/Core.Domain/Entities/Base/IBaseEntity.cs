namespace Core.Domain.Entities.Base;

public interface IBaseEntity<T>
{
    public T Id { get; init; }
}