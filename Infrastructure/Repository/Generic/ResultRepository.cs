using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Validations.Validators;
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

    static readonly AbstractValidator<T> validator = new AbstractValidator<T>()
        .NotNull()
        .NotDefault();

    static bool NotNullOrDefaul(T? t) => validator.Validate(t!).IsSuccess;

    public virtual async Task<Result<IEnumerable<T>>> GetAllAsync() {
        var query = await DbSet.ToListAsync();
        if (query.Count == 0) {
            return Errors.EmptyCollection(DbSet.ToQueryString());
        }
        return query;
    }

    public virtual async Task<Result<T>> GetByIdAsync(TKey id) {
        var res = await DbSet.FindAsync(id);

        if (NotNullOrDefaul(res)) {
            return Errors.NotFound(nameof(T), id);
        }

        return res!;
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

    public async Task<Result<bool>> SaveChangesAsync() {
        return await Context.SaveChangesAsync() > 0;
    }

    public Task<Result<T>> UpdateAsync(T entity) {
        throw new NotImplementedException("");
    }
}
