using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

// Counts ascent and descent only once the elevation has moved MinGainM away from the last counted level, so GPS wobble adds nothing.
public sealed class ElevationGainFilter(ElevationGainFilter.Config config) : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config(double MinGainM) : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new ElevationGainFilter(this);
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        double elevation = 0;
        double counted = 0;
        double ascent = 0;
        double descent = 0;

        foreach (var gain in context.Gains)
        {
            elevation += gain.ElevationDelta;
            double change = elevation - counted;

            bool hasClimbedEnough = change >= config.MinGainM;
            bool hasDescendedEnough = -change >= config.MinGainM;

            if (hasClimbedEnough)
            {
                ascent += change;
                counted = elevation;
            }
            else if (hasDescendedEnough)
            {
                descent -= change;
                counted = elevation;
            }
        }

        context.TotalAscentMeters = ascent;
        context.TotalDescentMeters = descent;

        return context;
    }
}
