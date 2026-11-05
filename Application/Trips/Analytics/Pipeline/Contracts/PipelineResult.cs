using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics.Pipeline.Contracts;

// Data shapes per stage. ElevationProfile reuses ElevationProfileDto and Route reuses RoutePath.
public abstract record StageData
{
    public sealed record Cleanup(int PointCountIn, int PointCountOut) : StageData;

    public sealed record Analytics(RouteAnalytic Route, TimeAnalytic? Time) : StageData;

    // DistanceM is the distance in metres from the candidate (local maximum) to the matched peak's location.
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
// Baseline = the current production settings (cleanup values come from the GpxDataProcessing app setting).
// Raw = the same stages on the untouched file (cleanup with no filters); only present when the request asked for it.
public sealed record PipelinePreviewResponse(
    IReadOnlyDictionary<string, StageResult> Results,
    IReadOnlyDictionary<string, StageResult> Baseline,
    IReadOnlyDictionary<string, StageResult>? Raw = null
);

public interface IPipelinePreviewService
{
    // Read-only: must never persist or mutate anything. Returns null when the trip file does not exist.
    // Stages a requested stage depends on are always computed but only requested stages appear in the response.
    // A failing stage yields Ok = false with Error; stages that depend on it fail without running.
    Task<PipelinePreviewResponse?> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    );
}