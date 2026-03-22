using Application.Commons.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.RouteVisualizations;

public interface IRouteVisualizationService
{
    Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points);
    Result<RoutePath> GetRouteVisualization(IEnumerable<GpxPoint> points, IEnumerable<IFilterConfig> configs);

}