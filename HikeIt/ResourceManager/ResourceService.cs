using HikeIt.Api.ApiResolver;
using HikeIt.Api.Repository;

namespace HikeIt.Api.ResourceManager;

public record ResourceServiceConfig<T, TResponse>(
    IRepository<T> Repository,
    IRequestResolver<TResponse> Resolver
) where T : class, IRepositoryObject;

public record ProtectedResourceServiceConfig<T, TResponse>(
    IRepository<T> Repository,
    IRequestResolver<TResponse> Resolver,
    string User
) where T : class, IRepositoryObject;

public class ResourceService<T, TResponse> : IResourceManager<T, TResponse> where T : class, IRepositoryObject {

    readonly IRepository<T> _repository;
    readonly IRequestResolver<TResponse> _resolver;

    public ResourceService(ResourceServiceConfig<T, TResponse> config) {
        _repository = config.Repository;
        _resolver = config.Resolver;
    }

    public async Task<TResponse> Create(T newObject) {
        await _repository.AddAsync(newObject);
        return _resolver.Resolve(newObject);
    }

    public async Task<TResponse> GetAll() {
        var results = await _repository.GetAllAsync();
        return _resolver.Resolve(results);
    }

    public async Task<TResponse> GetById(int id) {
        var result = await _repository.GetByIDAsync(id);
        return _resolver.Resolve(result);
    }

    public async Task<TResponse> Update(int id, T update) {
        var result = await _repository.UpdateAsync(id, update);
        return _resolver.Resolve(result);
    }

    public async Task<TResponse> Delete(int id) {
        await _repository.RemoveAsync(id);
        return _resolver.Resolve(true);
    }
}



public static class ResourceManagerFactory {

}

