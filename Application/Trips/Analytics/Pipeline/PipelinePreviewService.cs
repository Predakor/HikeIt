using Application.Trips.Analytics.Pipeline.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Application.Trips.Analytics.Pipeline;

// Runs the requested stages and the baseline (all-default config), caching each stage's output.
public sealed class PipelinePreviewService(
    IPipelineSource source,
    IEnumerable<IPipelineStage> stages,
    IMemoryCache cache
) : IPipelinePreviewService
{
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(10);

    // The untouched file: cleanup with no filters, every other section left at its default.
    private static readonly PipelineConfig RawConfig = new(Cleanup: new PipeConfig.Cleanup([]));

    // Filter configs have a FilterType property (a System.Type) that clashes with the polymorphic discriminator of the same name.
    // The discriminator already identifies the config, so the property is left out of the key.
    private static readonly JsonSerializerOptions CacheKeyOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { RemoveFilterTypeProperty } },
    };

    private readonly IReadOnlyDictionary<StageId, IPipelineStage> _stagesById = stages.ToDictionary(stage => stage.Id);

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
        var raw = request.IncludeRaw ? await RunPlanAsync(input, RawConfig, plan, cancellationToken) : null;

        return new PipelinePreviewResponse(
            Select(preview, requested),
            Select(baseline, requested),
            raw is null ? null : Select(raw, requested)
        );
    }

    private async Task<Dictionary<StageId, StageRun>> RunPlanAsync(
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

    private async Task<StageRun> RunStageAsync(
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
            var discriminator = await handler.CacheDiscriminatorAsync(cancellationToken);
            var key = CacheKey(input.CacheScope, stage, handler.SelectConfig(config), discriminator, upstreamKeys);

            if (cache.TryGetValue(key, out StageOutput? cached) && cached is not null)
            {
                return new StageRun(ToResult(stage, cached, cacheHit: true, durationMs: 0), cached, key);
            }

            var upstream = dependencies.ToDictionary(dependency => dependency, dependency => upstreamRuns[dependency].Output!);

            var watch = Stopwatch.StartNew();
            var run = await handler.RunAsync(new StageContext(input, upstream), config, cancellationToken);
            watch.Stop();

            return run.Match(
                output => CacheRun(stage, key, output, (int)watch.ElapsedMilliseconds),
                error => new StageRun(Failed(stage, error.Message), null, null)
            );
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new StageRun(Failed(stage, ex.Message), null, null);
        }
    }

    private StageRun CacheRun(StageId stage, string key, StageOutput output, int durationMs)
    {
        cache.Set(
            key,
            output,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheLifetime,
                Size = output.PointCount + 1,
            }
        );

        return new StageRun(ToResult(stage, output, cacheHit: false, durationMs), output, key);
    }

    private static IReadOnlyList<StageId> WithDependencies(IEnumerable<StageId> requested)
    {
        var plan = new List<StageId>();

        foreach (var stage in requested)
        {
            AddWithDependencies(stage, plan);
        }

        return plan;
    }

    private static void AddWithDependencies(StageId stage, List<StageId> plan)
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

    private static IReadOnlyDictionary<string, StageResult> Select(
        IReadOnlyDictionary<StageId, StageRun> runs,
        IEnumerable<StageId> requested
    ) => requested.ToDictionary(StageIds.Key, stage => runs[stage].Result);

    private static string CacheKey(
        string scope,
        StageId stage,
        object? selectedConfig,
        string discriminator,
        IEnumerable<string> upstreamKeys
    )
    {
        var material = string.Join(
            "\n",
            scope,
            StageIds.Key(stage),
            JsonSerializer.Serialize(selectedConfig, CacheKeyOptions),
            discriminator,
            string.Join(",", upstreamKeys)
        );

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(material)));
    }

    private static StageResult ToResult(StageId stage, StageOutput output, bool cacheHit, int durationMs) =>
        new(stage, true, null, cacheHit, durationMs, output.PointCount, PayloadBytes(output.Data), output.Data);

    private static StageResult Failed(StageId stage, string error) => new(stage, false, error, false, 0, 0, 0, null);

    private static int PayloadBytes(object? data) => data is null ? 0 : JsonSerializer.SerializeToUtf8Bytes(data).Length;

    // Result is what the client sees; Output and CacheKey feed downstream stages and are null when the stage failed.
    private sealed record StageRun(StageResult Result, StageOutput? Output, string? CacheKey);

    private static void RemoveFilterTypeProperty(JsonTypeInfo typeInfo)
    {
        var filterTypeProperties = typeInfo.Properties.Where(property => property.Name == "FilterType").ToList();

        foreach (var property in filterTypeProperties)
        {
            typeInfo.Properties.Remove(property);
        }
    }
}