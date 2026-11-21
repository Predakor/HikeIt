using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

// Replaces the plain ascent and descent sums: a climb or descent counts only once the track has turned by MinChangeM, so GPS wobble adds nothing.
public sealed class ElevationGainFilter(ElevationGainFilter.Config config) : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config(double MinChangeM = 3) : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new ElevationGainFilter(this);
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        var segments = ElevationSegments.Split(context.Gains, config.MinChangeM);

        context.TotalAscentMeters = segments
            .Where(segment => segment.ElevationM > 0)
            .Sum(segment => segment.ElevationM);

        context.TotalDescentMeters = -segments
            .Where(segment => segment.ElevationM < 0)
            .Sum(segment => segment.ElevationM);

        return context;
    }
}
