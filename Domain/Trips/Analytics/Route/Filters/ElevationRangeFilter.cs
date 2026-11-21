using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

public sealed class ElevationRangeFilter : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new ElevationRangeFilter();
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        context.HighestElevationMeters = context.Points.Max(point => point.Ele);
        context.LowestElevationMeters = context.Points.Min(point => point.Ele);

        return context;
    }
}
