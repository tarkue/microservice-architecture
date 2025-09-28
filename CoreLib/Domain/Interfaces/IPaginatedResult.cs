namespace Core.Domain.Interfaces;

public interface IPaginatedResultMeta
{
    Int16 TotalPages { get; init; }
    Int16 CurrentPage { get; init; }
    Int16 PageSize { get; init; }
    Boolean HasNextPage { get; init; }
}

public interface IPaginatedResult<T>
{
    T[] Data { get; init; }
    IPaginatedResultMeta Meta { get; init; }
}