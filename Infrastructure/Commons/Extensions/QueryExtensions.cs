using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.Extensions;

public static class QueryExtensions
{
    public static async Task<Result<T[]>> ToResultArrayAsync<T>(
        this IQueryable<T> query,
        string emptyCollectionMessage,
        CancellationToken ct)
    {
        var items = await query.ToArrayAsync(ct);
        return items.NullOrEmpty()
            ? Errors.EmptyCollection(emptyCollectionMessage)
            : items;
    }

    public static async Task<Result<List<T>>> ToResultListAsync<T>(
        this IQueryable<T> query,
        string emptyCollectionMessage,
        CancellationToken ct)
    {
        var items = await query.ToListAsync(ct);
        return items.NullOrEmpty()
            ? Errors.EmptyCollection(emptyCollectionMessage)
            : items;
    }

    public static async Task<Result<T>> FirstOrFailureAsync<T>(
        this IQueryable<T> query,
        string emptyCollectionMessage,
        CancellationToken ct)
        where T : class
    {
        var item = await query.FirstOrDefaultAsync(ct);
        return item is null
            ? Errors.NotFound(emptyCollectionMessage)
            : item;
    }

}
