using HikeIt.Api.Repository;

namespace HikeIt.Api.ResourceManager;

public interface IResourceManager<T, TResponse> where T : class, IRepositoryObject {
    public Task<TResponse> Create(T newObject);

    public Task<TResponse> GetAll();
    public Task<TResponse> GetById(int id);

    public Task<TResponse> Update(int id, T update);

    public Task<TResponse> Delete(int id);
}
