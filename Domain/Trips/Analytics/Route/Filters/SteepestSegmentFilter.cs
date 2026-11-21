using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.Trips.Analytics.Route.Filters;

// Steepest climb and descent over any stretch at least WindowM long. Leaves both null when the track is shorter.
public sealed class SteepestSegmentFilter(SteepestSegmentFilter.Config config) : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config(double WindowM) : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new SteepestSegmentFilter(this);
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        var gains = context.Gains;
        var distance = new double[gains.Count + 1];
        var elevation = new double[gains.Count + 1];

        for (int i = 0; i < gains.Count; i++)
        {
            distance[i + 1] = distance[i] + gains[i].DistanceDelta;
            elevation[i + 1] = elevation[i] + gains[i].ElevationDelta;
        }

        double? climb = null;
        double? descent = null;

        foreach (var (start, end) in DistanceWindows.Of(distance, config.WindowM))
        {
            double grade = (elevation[end] - elevation[start]) / (distance[end] - distance[start]) * 100;

            bool isSteeperClimb = grade > 0 && (climb is null || grade > climb);
            bool isSteeperDescent = grade < 0 && (descent is null || grade < descent);

            if (isSteeperClimb)
            {
                climb = grade;
            }

            if (isSteeperDescent)
            {
                descent = grade;
            }
        }

        context.SteepestClimbPercent = climb;
        context.SteepestDescentPercent = descent;

        return context;
    }
}
