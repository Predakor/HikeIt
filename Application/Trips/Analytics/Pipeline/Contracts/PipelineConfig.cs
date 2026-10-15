using Application.Commons.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Contracts;

// Per-stage configs of the GPX processing pipeline.
// Every field is nullable; null means "use the production default".
// Point-filter lists accept only IPointFilterConfig and the peak list only IPeakFilterConfig,
// so a misplaced filter fails request deserialisation instead of reaching the pipeline.
public static class PipeConfig
{
    // Filters == null means "use the production cleanup list" (MaxSpike, Median, Ema, Rounding).
    public sealed record Cleanup(IReadOnlyList<IPointFilterConfig>? Filters = null);

    // Filters run on the cleaned points before gains are computed, so Pin + Simplify keep summits in the profile.
    // Filters == null means "use the production profile list".
    public sealed record Profile(IReadOnlyList<IPointFilterConfig>? Filters = null);

    // Filters run on the candidate peaks (local maxima) in order, e.g. MergeNearbyByDistance then MergeNearbyByElevation.
    // Filters == null means "use the production list". The DB lookup radius is a scalar because the lookup is not a filter.
    public sealed record Peaks(IReadOnlyList<IPeakFilterConfig>? Filters = null, float? ProximityRadiusM = null);

    // Filters == null means "use the production filter list".
    public sealed record Route(IReadOnlyList<IPointFilterConfig>? Filters = null);
}

public sealed record PipelineConfig(
    PipeConfig.Cleanup? Cleanup = null,
    PipeConfig.Profile? ElevationProfile = null,
    PipeConfig.Peaks? PeakMatching = null,
    PipeConfig.Route? Route = null
);

// IncludeRaw adds the raw run (untouched file) to the response; clients ask for it once per file.
public sealed record PipelinePreviewRequest(
    PipelineConfig Config,
    IReadOnlyList<StageId>? Stages = null,
    bool IncludeRaw = false
);
