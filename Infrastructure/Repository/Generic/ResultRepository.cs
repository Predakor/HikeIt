using Domain.Common;
using Domain.Common.Result;
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

    public async Task<Result<IEnumerable<T>>> GetAllAsync() {
        var query = DbSet;
        if (!query.Any()) {
            return Result<IEnumerable<T>>.Failure(Errors.EmptyCollection(DbSet.ToQueryString()));
        }

        throw new NotImplementedException();
    }

    public Task<Result<T?>> GetByIdAsync(TKey id) {
        throw new NotImplementedException();
    }
}

public class CrudResultRepository<T, TKey>
    : ResultRepository<T, TKey>,
        ICrudResultRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected CrudResultRepository(TripDbContext context)
        : base(context) { }

    public async Task<Result<T>> AddAsync(T entity) {
        var query = await DbSet.AddAsync(entity);
        return query != null
            ? Result<T>.Success(entity)
            : Result<T>.Failure(Errors.DbError("db error"));
    }

    public async Task<Result<bool>> RemoveAsync(TKey id) {
        var entity = await GetByIdAsync(id);
        return entity.Map(
            found => Result<bool>.Success(DbSet.Remove(found) != null),
            notFound => Result<bool>.Failure(Errors.NotFound(id?.ToString() ?? "")),
            error => Result<bool>.Failure(Errors.Unknown())
        );
    }

    public Task<Result<T>> UpdateAsync(T entity) {
        throw new NotImplementedException("");
    }
}
