using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class ElevationProfileStage(IPointsFilterRunner runner)
    : FilterListStageBase<IPointFilterConfig>
{
    public override StageId Id => StageId.ElevationProfile;

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.ElevationProfile?.Filters;
    }

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var cleanedPoints = CleanupStage.CleanedDataFrom(context).Points;
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(cleanedPoints, Id, RequestFilters(config), steps, cancellationToken)
            .MapAsync(points => new Stage.Output(new AnalyticData(points).ToElevationProfileDto(), steps));
    }
}