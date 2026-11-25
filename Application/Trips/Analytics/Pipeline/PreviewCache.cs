using System.Security.Cryptography;
using System.Text;
using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Core.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Trips.Analytics.Pipeline;

// A comparison target only depends on the trip file and the production settings, so its finished results are reused across edits.
public sealed class PreviewCache(IMemoryCache cache, IPipelineSettingsService settings)
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(5);

    // The defaults are what an empty config resolves to, so a settings change gives a new key.
    private static string KeyOf(
        ComparisonTarget target,
        Guid tripId,
        PipelineConfig defaults,
        IEnumerable<StageId> requested,
        bool includeSteps
    )
    {
        var material = string.Join(
            "\n",
            target,
            tripId,
            defaults.ToJson(),
            string.Join(",", requested.Order()),
            includeSteps
        );

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(material)));
    }

    public async Task<IReadOnlyDictionary<string, PipeStageDto.Result>> GetOrRunAsync(
        ComparisonTarget target,
        Guid tripId,
        IEnumerable<StageId> requested,
        bool includeSteps,
        Func<Task<IReadOnlyDictionary<string, PipeStageDto.Result>>> run,
        CancellationToken cancellationToken
    )
    {
        var defaults = await settings.GetDefaultsAsync(cancellationToken);
        if (!defaults.IsSuccess)
        {
            return await run();
        }

        var key = KeyOf(target, tripId, defaults.Value!, requested, includeSteps);
        if (cache.TryGetValue(key, out IReadOnlyDictionary<string, PipeStageDto.Result>? cached) && cached is not null)
        {
            return cached;
        }

        var results = await run();

        // A failed stage may be transient, so only runs where every stage finished or was skipped are kept.
        if (results.Values.All(result => result.Ok || result.Skipped))
        {
            cache.Set(key, results, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = Lifetime });
        }

        return results;
    }
}
