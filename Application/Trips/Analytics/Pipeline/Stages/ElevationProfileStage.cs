using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;
using Application.AppSettings;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Geography.ValueObjects;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class ElevationProfileStage(IPipelineSettingsService settings) : FilterListStageBase<IPointFilterConfig>(settings)
{
    public override StageId Id => StageId.ElevationProfile;

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.ElevationProfile?.Filters;
    }

    public override Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        return ResolveFiltersAsync(config, cancellationToken)
            .MapAsync(filters => BuildProfile(CleanupStage.CleanedDataFrom(context).Points, filters));
    }

    private static StageOutput BuildProfile(IEnumerable<GpxPoint> cleanedPoints, IEnumerable<IFilterConfig> filters)
    {
        var points = PointFilters.Apply(cleanedPoints, filters);
        var profile = new AnalyticData(points).ToElevationProfileDto();

        return new StageOutput(profile, points.Count);
    }
}
