namespace Domain;

public interface IEntity<TKey> {
    TKey Id { get; }
}

public interface IReadRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}

public interface IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> {
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TKey id, TEntity updatedEntity);
    Task<bool> RemoveAsync(TKey id);
    Task<bool> SaveChangesAsync();
}

public interface ICrudRepository<TEntity, TKey>
    : IReadRepository<TEntity, TKey>,
      IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey> { }
