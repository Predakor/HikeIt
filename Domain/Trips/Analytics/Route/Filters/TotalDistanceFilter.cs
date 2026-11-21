using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

public sealed class TotalDistanceFilter : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new TotalDistanceFilter();
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        context.TotalDistanceMeters = context.Gains.Sum(gain => gain.DistanceDelta);

        return context;
    }
}
