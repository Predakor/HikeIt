using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.Pipeline.Stages;

// Route path of the cleaned points. No filters configured means the production default, which is no filters at all.
public sealed class RouteStage(IRouteVisualizationService routeVisualizations) : IPipelineStage
{
    public StageId Id => StageId.Route;

    public object? SelectConfig(PipelineConfig config) => config.Route;

    public async Task<StageOutput> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var points = CleanupStage.CleanedDataFrom(context).Points;
        var filters = config.Route?.Filters ?? PipelineDefaults.RouteFilters;

        var result = routeVisualizations.GetRouteVisualization(points, filters);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException(result.Error!.Message);
        }

        var path = result.Value!;
        return new StageOutput(path, path.Points.Count);
    }
}
