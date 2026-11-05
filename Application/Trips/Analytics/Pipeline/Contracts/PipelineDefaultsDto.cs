using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Contracts;

public sealed record PipelineDefaultsDto(
    IReadOnlyList<IPointFilterConfig> Cleanup,
    IReadOnlyList<IPointFilterConfig> ElevationProfile,
    IReadOnlyList<IPeakFilterConfig> Peaks,
    IReadOnlyList<IPointFilterConfig> Route
);
