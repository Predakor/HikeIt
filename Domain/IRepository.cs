namespace Domain;

public interface IRepositoryObject { }

public interface IRepository<TEntity>
    where TEntity : class, IRepositoryObject {
    Task<TEntity?> GetByIDAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task<bool> UpdateAsync(int id, TEntity updatedEntity);
    Task RemoveAsync(int id);
}
