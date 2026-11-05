using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Contracts;

// The raw stored file of a trip, parsed. CacheScope identifies user + file so cache entries never cross users.
public sealed record PipelineInput(string CacheScope, IReadOnlyList<GpxPoint> RawPoints);

public interface IPipelineSource
{
    // Read-only. Returns null when the trip or its file does not exist.
    Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken);
}

// Data is what is sent to the client; Internal is the in-memory value downstream stages need
// (e.g. cleanup's cleaned points) and is never serialised.
public sealed record StageOutput(object? Data, int PointCount, object? Internal = null);

public sealed record StageContext(
    PipelineInput Input,
    IReadOnlyDictionary<StageId, StageOutput> Upstream
);

// One implementation per StageId. Stages are pure with respect to the database: they must never write.
public interface IPipelineStage
{
    StageId Id { get; }

    // The part of the config this stage depends on; serialised to JSON for the cache key. null = defaults.
    object? SelectConfig(PipelineConfig config);

    // Extra text for the cache key when a stage's output also depends on something outside the request config,
    // e.g. cleanup's default filters come from a database setting. Must change whenever that input changes.
    Task<string> CacheDiscriminatorAsync(CancellationToken cancellationToken) => Task.FromResult(string.Empty);

    Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    );
}
