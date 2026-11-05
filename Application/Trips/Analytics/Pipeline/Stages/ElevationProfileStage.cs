using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class ElevationProfileStage(IPipelineSettingsService settings, IPointsFilterRunner runner)
    : FilterListStageBase<IPointFilterConfig>(settings)
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
        var cleanedPoints = CleanupStage.CleanedDataFrom(context).Points;

        return runner
            .ApplyAsync(cleanedPoints, Id, RequestFilters(config), cancellationToken)
            .MapAsync(ToOutput);
    }

    private static StageOutput ToOutput(List<GpxPoint> points)
    {
        var profile = new AnalyticData(points).ToElevationProfileDto();

        return new StageOutput(profile, points.Count);
    }
}
