using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.RouteVisualizations;

public interface IRouteVisualizationService : IService
{
    Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points);
    Task<Result<RoutePath>> GetRouteVisualizationAsync(
        IEnumerable<GpxPoint> points,
        IReadOnlyList<IPointFilterConfig> configs
    );
}
