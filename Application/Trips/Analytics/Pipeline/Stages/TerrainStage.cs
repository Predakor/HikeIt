using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Trips.Analytics.Route;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class TerrainStage(ITerrainRunner runner) : FilterListStageBase<ITerrainFilterConfig>
{
    public override StageId Id => StageId.Terrain;

    internal static RouteAnalytic RouteFrom(Stage.Context context)
    {
        return context.Upstream[StageId.Terrain].Data as RouteAnalytic
            ?? throw new InvalidOperationException("Route analytics output is missing its route analytic.");
    }

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var cleaned = CleanupStage.CleanedDataFrom(context);
        var points = cleaned.Points;
        var gains = cleaned.Gains ?? points.ToGains();
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(points, gains, RequestFilters(config), steps, cancellationToken)
            .MapAsync(route => new Stage.Output(route, steps));
    }

    protected override IReadOnlyList<ITerrainFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Terrain?.Filters;
    }
}
