using Domain.Common.Abstractions;

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
        int start = 0;

        for (int end = 1; end < distance.Length; end++)
        {
            // Largest start that still leaves the window at least WindowM long.
            while (start + 1 < end && distance[end] - distance[start + 1] >= config.WindowM)
            {
                start++;
            }

            double length = distance[end] - distance[start];
            bool isLongEnough = length >= config.WindowM && length > 0;
            if (!isLongEnough)
            {
                continue;
            }

            double grade = (elevation[end] - elevation[start]) / length * 100;

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
