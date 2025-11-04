using Core.Interfaces;

namespace Core;

// Реализация метаданных
public record PaginatedResultMeta : IPaginatedResultMeta
{
    public short TotalPages { get; init; }
    public short CurrentPage { get; init; }
    public short PageSize { get; init; }
    public bool HasNextPage { get; init; }
}

// Реализация пагинированного результата
public record PaginatedResult<T> : IPaginatedResult<T>
{
    public IEnumerable<T> Data { get; init; } = [];
    public IPaginatedResultMeta Meta { get; init; } = new PaginatedResultMeta();
}