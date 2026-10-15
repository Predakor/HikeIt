using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics.Pipeline.Contracts;

// Data shapes per stage. ElevationProfile reuses ElevationProfileDto and Route reuses RoutePath.
public abstract record StageData
{
    public sealed record Cleanup(int PointCountIn, int PointCountOut) : StageData;

    public sealed record Analytics(RouteAnalytic Route, TimeAnalytic? Time) : StageData;

    public sealed record MatchedPeak(int PeakId, string Name, double DistanceM, double Elevation);

    public sealed record PeakMatching(int CandidateCount, IReadOnlyList<MatchedPeak> Matched) : StageData;
}

public sealed record StageResult(
    StageId Stage,
    bool Ok,
    string? Error,
    bool CacheHit,
    int DurationMs,
    int PointCount,
    int PayloadBytes,
    object? Data
);

// Dictionaries are keyed by StageIds.Key(stage), e.g. "elevationProfile".
public sealed record PipelinePreviewResponse(
    IReadOnlyDictionary<string, StageResult> Results,
    IReadOnlyDictionary<string, StageResult> Baseline
);

public interface IPipelinePreviewService
{
    // Read-only: must never persist or mutate anything.
    Task<PipelinePreviewResponse> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    );
}
