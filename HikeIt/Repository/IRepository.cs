namespace HikeIt.Api.Repository;

public interface IRepositoryObject { }

public interface IRepository<T>
    where T : class, IRepositoryObject
{
    Task<T?> GetByIDAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(Guid id, T updatedEntity);
    Task RemoveAsync(Guid id);
}
