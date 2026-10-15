using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Stages;

// Elevation profile of the cleaned points. Production applies no filters here, so the default keeps every point.
public sealed class ElevationProfileStage : IPipelineStage
{
    public StageId Id => StageId.ElevationProfile;

    public object? SelectConfig(PipelineConfig config) => config.ElevationProfile;

    public Task<StageOutput> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var filters = config.ElevationProfile?.Filters ?? [];
        var points = CleanupStage.ApplyPointFilters(CleanupStage.CleanedDataFrom(context).Points, filters);

        var profile = new AnalyticData(points).ToElevationProfileDto();

        return Task.FromResult(new StageOutput(profile, points.Count));
    }
}
