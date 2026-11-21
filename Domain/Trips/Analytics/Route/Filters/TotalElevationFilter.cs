using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

// Plain sums of every climb and descent. ElevationGainFilter is the alternative that ignores GPS wobble.
public sealed class TotalElevationFilter : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new TotalElevationFilter();
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        context.TotalAscentMeters = context.Gains
            .Where(gain => gain.ElevationDelta > 0)
            .Sum(gain => gain.ElevationDelta);

        context.TotalDescentMeters = Math.Abs(context.Gains
            .Where(gain => gain.ElevationDelta < 0)
            .Sum(gain => gain.ElevationDelta));

        return context;
    }
}
