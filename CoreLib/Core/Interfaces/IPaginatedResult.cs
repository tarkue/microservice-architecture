namespace Domain.Interfaces;

public interface IPaginatedResultMeta
{
    short TotalPages { get; init; }
    short CurrentPage { get; init; }
    short PageSize { get; init; }
    bool HasNextPage { get; init; }
}

public interface IPaginatedResult<T>
{
    IEnumerable<T> Data { get; init; }
    IPaginatedResultMeta Meta { get; init; }
}