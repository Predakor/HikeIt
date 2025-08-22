namespace Domain.Common.Abstractions;

public interface IEntity<out TKey> {
    TKey Id { get; }
}

public interface IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> { }

public interface IReadRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}

public interface IWriteRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<bool> AddAsync(TEntity entity);
    TEntity Add(TEntity entity);
    Task<bool> RemoveAsync(TKey id);
    Task<bool> SaveChangesAsync();
}

public interface ICrudRepository<TEntity, TKey>
    : IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> { }
