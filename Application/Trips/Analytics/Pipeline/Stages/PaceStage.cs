using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class PaceStage(IPaceRunner runner)
    : FilterListStageBase<IPaceFilterConfig>
{
    public override StageId Id => StageId.Pace;

    public override IReadOnlyList<StageId> DependsOn => [StageId.Cleanup, StageId.Terrain];

    protected override IReadOnlyList<IPaceFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Pace?.Filters;
    }

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var points = CleanupStage.CleanedDataFrom(context).Points;
        var route = TerrainStage.RouteFrom(context);
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(route, points, RequestFilters(config), steps, cancellationToken)
            .MapAsync(time => new Stage.Output(time, steps));
    }
}
