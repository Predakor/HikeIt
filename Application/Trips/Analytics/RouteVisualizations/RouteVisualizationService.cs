using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.RouteVisualizations;

internal sealed class RouteVisualizationService : IRouteVisualizationService
{
    private readonly IPointsFilterRunner _pointsRunner;

    public RouteVisualizationService(IPointsFilterRunner pointsRunner)
    {
        _pointsRunner = pointsRunner;
    }

    public Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points)
    {
        return _pointsRunner
            .ApplyAsync(points, StageId.Route)
            .MapAsync(filtered => new RoutePath(filtered));
    }

    public Task<Result<RoutePath>> GetRouteVisualizationAsync(
        IEnumerable<GpxPoint> points,
        IReadOnlyList<IPointFilterConfig> configs
    )
    {
        return _pointsRunner
            .ApplyAsync(points, StageId.Route, configs)
            .MapAsync(filtered => new RoutePath(filtered));
    }
}