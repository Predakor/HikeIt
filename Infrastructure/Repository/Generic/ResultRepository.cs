using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Rules;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Generic;

public abstract class ResultRepository<T, TKey>
    : BaseRepository<T, TKey>,
        IReadResultRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected ResultRepository(TripDbContext context)
        : base(context) { }

    public virtual async Task<Result<IEnumerable<T>>> GetAllAsync() {
        var query = await DbSet.ToListAsync();
        if (query.Count == 0) {
            return Errors.EmptyCollection(DbSet.ToQueryString());
        }
        return query;
    }

    public virtual async Task<Result<T>> GetByIdAsync(TKey id) {
        var nullOrDefault = new NotNullOrDefault<TKey>(id, "id").Check();
        if (nullOrDefault.HasErrors(out var err)) {
            return Result<T>.Failure(err);
        }

        var res = await DbSet.FindAsync(id);

        if (res is null) {
            return Errors.NotFound(id?.ToString() ?? "");
        }

        return res;
    }
}

public class CrudResultRepository<T, TKey>
    : ResultRepository<T, TKey>,
        ICrudResultRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected CrudResultRepository(TripDbContext context)
        : base(context) { }

    public virtual async Task<Result<T>> AddAsync(T entity) {
        var query = await DbSet.AddAsync(entity);
        return query != null
            ? Result<T>.Success(entity)
            : Result<T>.Failure(Errors.DbError("db error"));
    }

    public virtual async Task<Result<bool>> RemoveAsync(TKey id) {
        return await GetByIdAsync(id)
            .MatchAsync(
                found => Result<bool>.Success(DbSet.Remove(found) != null),
                error => Result<bool>.Failure(Errors.Unknown())
            );
    }

    public Task<Result<T>> UpdateAsync(T entity) {
        throw new NotImplementedException("");
    }
}
