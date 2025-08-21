using Domain.Common.Result;

namespace Application.Commons.CacheService;

public interface ICache {
    Task<Result<bool>> SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null,
        CancellationToken ct = default
    );
    Task<Result<T>> GetAsync<T>(string key, CancellationToken ct = default);
    Task<Result<bool>> RemoveAsync(string key, CancellationToken ct = default);
}
