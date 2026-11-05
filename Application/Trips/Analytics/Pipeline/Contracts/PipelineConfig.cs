using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Contracts;

public static class PipeConfig
{
    public sealed record Cleanup(IReadOnlyList<IPointFilterConfig>? Filters = null);

    public sealed record Profile(IReadOnlyList<IPointFilterConfig>? Filters = null);

    public sealed record Peaks(IReadOnlyList<IPeakFilterConfig>? Filters = null);

    public sealed record Route(IReadOnlyList<IPointFilterConfig>? Filters = null);
}

public sealed record PipelineConfig(
    PipeConfig.Cleanup? Cleanup = null,
    PipeConfig.Profile? ElevationProfile = null,
    PipeConfig.Peaks? PeakMatching = null,
    PipeConfig.Route? Route = null
);

public sealed record PipelinePreviewRequest(
    PipelineConfig Config,
    IReadOnlyList<StageId>? Stages = null,
    bool IncludeRaw = false
);