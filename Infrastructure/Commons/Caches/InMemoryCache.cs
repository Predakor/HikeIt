using Application.Commons.Abstractions;
using Domain.Common;
using Domain.Common.Result;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Commons.Caches;

internal class InMemoryCache : ICache {
    readonly IMemoryCache _cache;

    public InMemoryCache(IMemoryCache cache) {
        _cache = cache;
    }

    public async Task<Result<T>> GetAsync<T>(string key, CancellationToken ct = default) {
        await Task.CompletedTask;
        if (_cache.TryGetValue<T>(key, out var value)) {
            return value!;
        }
        return Errors.NotFound(nameof(T), key);
    }

    public async Task<Result<T>> GetOrCreateAsync<T>(
        string key,
        Func<Task<Result<T>>> factory,
        TimeSpan? ttl = null,
        CancellationToken ct = default
    ) {
        await Task.CompletedTask;
        var result = await _cache.GetOrCreateAsync(
            key,
            e => {
                e.SetAbsoluteExpiration(ttl ?? TimeSpan.FromMinutes(5));
                return factory();
            }
        );

        if (result is null) {
            return Errors.Unknown("Failed to use cache");
        }

        return result;
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
