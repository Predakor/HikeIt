namespace HikeIt.Api.Repository;

public interface IRepositoryObject { }

public interface IRepository<T>
    where T : class, IRepositoryObject {
    Task<T?> GetByIDAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(int id, T updatedEntity);
    Task RemoveAsync(int id);
}
