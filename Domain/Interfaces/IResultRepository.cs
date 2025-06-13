using Domain.Common;

namespace Domain.Interfaces;
public interface IReadResultRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<Result<TEntity?>> GetByIdAsync(TKey id);
    Task<Result<IEnumerable<TEntity>>> GetAllAsync();
}

public interface IWriteResultRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<bool>> RemoveAsync(TKey id);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
}

public interface ICrudResultRepository<TEntity, TKey>
    : IReadResultRepository<TEntity, TKey>,
        IWriteResultRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> { }
