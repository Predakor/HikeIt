using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Application.Trips.Analytics.Pipeline.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Trips.Analytics.Pipeline;

// Runs the requested stages and the baseline (all-default config), caching each stage's output.
public sealed class PipelinePreviewService(
    IPipelineSource source,
    IEnumerable<IPipelineStage> stages,
    IMemoryCache cache
) : IPipelinePreviewService
{
    static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(10);

    // Filter configs have a FilterType property (a System.Type) that clashes with the polymorphic discriminator of the same name.
    // The discriminator already identifies the config, so the property is left out of the key.
    static readonly JsonSerializerOptions CacheKeyOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { RemoveFilterTypeProperty } },
    };

    readonly IReadOnlyDictionary<StageId, IPipelineStage> _stagesById = stages.ToDictionary(stage => stage.Id);

    public async Task<PipelinePreviewResponse?> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    )
    {
        var input = await source.LoadAsync(tripId, cancellationToken);
        if (input is null)
        {
            return null;
        }

        var requested = (request.Stages ?? StageIds.All).Distinct().ToList();
        var plan = WithDependencies(requested);

        var preview = await RunPlanAsync(input, request.Config, plan, cancellationToken);
        var baseline = await RunPlanAsync(input, new PipelineConfig(), plan, cancellationToken);

        return new PipelinePreviewResponse(Select(preview, requested), Select(baseline, requested));
    }

    async Task<Dictionary<StageId, StageRun>> RunPlanAsync(
        PipelineInput input,
        PipelineConfig config,
        IReadOnlyList<StageId> plan,
        CancellationToken cancellationToken
    )
    {
        var runs = new Dictionary<StageId, StageRun>();

        foreach (var stage in plan)
        {
            var failedUpstream = StageIds
                .DependenciesOf(stage)
                .Where(dependency => !runs[dependency].Result.Ok)
                .Select(dependency => (StageId?)dependency)
                .FirstOrDefault();

            if (failedUpstream is StageId failed)
            {
                runs[stage] = new StageRun(Failed(stage, $"Upstream stage {StageIds.Key(failed)} failed"), null, null);
                continue;
            }

            runs[stage] = await RunStageAsync(stage, input, config, runs, cancellationToken);
        }

        return runs;
    }

    async Task<StageRun> RunStageAsync(
        StageId stage,
        PipelineInput input,
        PipelineConfig config,
        IReadOnlyDictionary<StageId, StageRun> upstreamRuns,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var handler = _stagesById[stage];
            var dependencies = StageIds.DependenciesOf(stage);
            var upstreamKeys = dependencies.Select(dependency => upstreamRuns[dependency].CacheKey!);
            var key = CacheKey(input.CacheScope, stage, handler.SelectConfig(config), upstreamKeys);

            if (cache.TryGetValue(key, out StageOutput? cached) && cached is not null)
            {
                return new StageRun(ToResult(stage, cached, cacheHit: true, durationMs: 0), cached, key);
            }

            var upstream = dependencies.ToDictionary(dependency => dependency, dependency => upstreamRuns[dependency].Output!);

            var watch = Stopwatch.StartNew();
            var output = await handler.RunAsync(new StageContext(input, upstream), config, cancellationToken);
            watch.Stop();

            cache.Set(
                key,
                output,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheLifetime,
                    Size = output.PointCount + 1,
                }
            );

            return new StageRun(ToResult(stage, output, cacheHit: false, durationMs: (int)watch.ElapsedMilliseconds), output, key);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new StageRun(Failed(stage, ex.Message), null, null);
        }
    }

    static IReadOnlyList<StageId> WithDependencies(IEnumerable<StageId> requested)
    {
        var plan = new List<StageId>();

        foreach (var stage in requested)
        {
            AddWithDependencies(stage, plan);
        }

        return plan;
    }

    static void AddWithDependencies(StageId stage, List<StageId> plan)
    {
        if (plan.Contains(stage))
        {
            return;
        }

        foreach (var dependency in StageIds.DependenciesOf(stage))
        {
            AddWithDependencies(dependency, plan);
        }

        plan.Add(stage);
    }

    static IReadOnlyDictionary<string, StageResult> Select(
        IReadOnlyDictionary<StageId, StageRun> runs,
        IEnumerable<StageId> requested
    ) => requested.ToDictionary(StageIds.Key, stage => runs[stage].Result);

    static string CacheKey(string scope, StageId stage, object? selectedConfig, IEnumerable<string> upstreamKeys)
    {
        var material = string.Join(
            "\n",
            scope,
            StageIds.Key(stage),
            JsonSerializer.Serialize(selectedConfig, CacheKeyOptions),
            string.Join(",", upstreamKeys)
        );

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(material)));
    }

    static StageResult ToResult(StageId stage, StageOutput output, bool cacheHit, int durationMs) =>
        new(stage, true, null, cacheHit, durationMs, output.PointCount, PayloadBytes(output.Data), output.Data);

    static StageResult Failed(StageId stage, string error) => new(stage, false, error, false, 0, 0, 0, null);

    static int PayloadBytes(object? data) => data is null ? 0 : JsonSerializer.SerializeToUtf8Bytes(data).Length;

    // Result is what the client sees; Output and CacheKey feed downstream stages and are null when the stage failed.
    sealed record StageRun(StageResult Result, StageOutput? Output, string? CacheKey);

    static void RemoveFilterTypeProperty(JsonTypeInfo typeInfo)
    {
        var filterTypeProperties = typeInfo.Properties.Where(property => property.Name == "FilterType").ToList();

        foreach (var property in filterTypeProperties)
        {
            typeInfo.Properties.Remove(property);
        }
    }
}
