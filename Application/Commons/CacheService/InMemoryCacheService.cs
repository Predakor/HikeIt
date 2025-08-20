using Domain.Common;
using Domain.Common.Result;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Commons.CacheService;

internal class InMemoryCacheService : ICacheService {
    readonly IMemoryCache _cache;

    public InMemoryCacheService(IMemoryCache cache) {
        _cache = cache;
    }

    public async Task<Result<T?>> GetAsync<T>(string key, CancellationToken ct = default) {
        await Task.CompletedTask;
        if (_cache.TryGetValue<T>(key, out var value)) {
            return value;
        }
        return Errors.NotFound(nameof(T), key);
    }

    public async Task<Result<bool>> RemoveAsync(string key, CancellationToken ct = default) {
        _cache.Remove(key);
        return await Task.FromResult(true);
    }

    public async Task<Result<bool>> SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null,
        CancellationToken ct = default
    ) {
        _cache.Set(key, value, ttl ?? TimeSpan.FromMinutes(5));
        return await Task.FromResult(true);
    }
}
