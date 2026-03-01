namespace Domain.Common.Abstractions;
public interface IReadResultRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    Task<Result<TEntity>> GetByIdAsync(TKey id, CancellationToken ct = default);
    Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken ct = default);
}

public interface IWriteResultRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<bool>> RemoveAsync(TKey id);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result<bool>> SaveChangesAsync(CancellationToken ct);
}

public interface ICrudResultRepository<TEntity, TKey>
    : IReadResultRepository<TEntity, TKey>,
        IWriteResultRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{ }
