using Core;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace Logic.Extensions;

public static class PaginatedResultExtensions
{
    public static async Task<IPaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (short)Math.Ceiling(totalCount / (double)pageSize);

        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        var meta = new PaginatedResultMeta
        {
            TotalPages = Math.Max((short)1, totalPages), // Исправлен cast
            CurrentPage = (short)pageIndex,
            PageSize = (short)pageSize,
            HasNextPage = pageIndex < totalPages
        };

        return new PaginatedResult<T>
        {
            Data = data,
            Meta = meta
        };
    }
}