using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class RouteStage(IPointsFilterRunner runner) : FilterListStageBase<IPointFilterConfig>
{
    public override StageId Id => StageId.Route;

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var points = CleanupStage.CleanedDataFrom(context).Points;
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(points, Id, RequestFilters(config), steps, cancellationToken)
            .MapAsync(path => new Stage.Output(new RoutePath(path), steps));
    }

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Route?.Filters;
    }
}
