namespace Core.Entities;

public interface IPhoto<T>
    where T : IResource
{
    Guid? PhotoResourceId { get; init; }
    T? Photo { get; init; }
}