using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class RouteStage(
    IRouteVisualizationService routeVisualizations,
    IPipelineSettingsService settings
) : FilterListStageBase<IPointFilterConfig>(settings)
{
    public override StageId Id => StageId.Route;

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Route?.Filters;
    }

    public override Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var points = CleanupStage.CleanedDataFrom(context).Points;

        return ResolveFiltersAsync(config, cancellationToken)
            .BindAsync(filters => routeVisualizations.GetRouteVisualization(points, filters))
            .MapAsync(path => new StageOutput(path, path.Points.Count));
    }
}
