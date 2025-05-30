namespace Domain;

public interface IEntity { }

public interface IReadRepository<TEntity>
    where TEntity : class, IEntity {
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}

public interface IWriteRepository<TEntity>
    where TEntity : class, IEntity {
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(int id, TEntity updatedEntity);
    Task<bool> RemoveAsync(int id);
    Task<bool> SaveChangesAsync();
}

public interface ICrudRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>
    where TEntity : class, IEntity { }
