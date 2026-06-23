using Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.Extensions;

public static class TypeNameCache<TType>
{
    public static readonly string Name = typeof(TType).Name;
}

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

    /// <summary>
    /// Gets the first element by ID or returns a NotFound error.
    /// Auto-detects entity name from IEntity constraint.
    /// </summary>
    public static async Task<Result<T>> FirstOrFailureAsync<T, TValue>(
        this IQueryable<T> query,
        TValue filterValue,
        CancellationToken ct)
        where T : class, IEntity
    {
        return await query.FirstOrFailureAsync(filterValue, TypeNameCache<T>.Name, ct);
    }

    /// <summary>
    /// Gets the first element by ID or returns a NotFound error.
    /// Use this for projected types where entity name must be explicit.
    /// </summary>
    public static async Task<Result<T>> FirstOrFailureAsync<T, TValue>(
        this IQueryable<T> query,
        TValue filterValue,
        string entityName,
        CancellationToken ct)
        where T : class
    {
        return await query.FirstOrFailureAsync(filterValue, entityName, "id", ct);
    }

    /// <summary>
    /// Gets the first element by custom filter or returns a NotFound error.
    /// Use this for projections with custom filter names (not ID).
    /// </summary>
    public static async Task<Result<T>> FirstOrFailureAsync<T, TValue>(
        this IQueryable<T> query,
        TValue filterValue,
        string entityName,
        string filterName,
        CancellationToken ct)
    where T : class
    {
        var item = await query.FirstOrDefaultAsync(ct);
        return item is null
            ? Errors.NotFound(entityName, filterName, filterValue)
            : item;
    }
}
