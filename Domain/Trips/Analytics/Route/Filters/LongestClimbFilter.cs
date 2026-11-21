using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

// The longest continuous climb and descent by distance, with their average grade. Dips below MinChangeM do not end a segment.
public sealed class LongestClimbFilter(LongestClimbFilter.Config config) : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config(double MinChangeM = 3) : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new LongestClimbFilter(this);
    }

    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        var segments = ElevationSegments.Split(context.Gains, config.MinChangeM);

        var climb = LongestBy(segments, segment => segment.ElevationM > 0);
        var descent = LongestBy(segments, segment => segment.ElevationM < 0);

        context.LongestClimbDistanceM = climb?.DistanceM;
        context.LongestClimbPercent = climb is null ? null : GradeOf(climb.Value);
        context.LongestDescentDistanceM = descent?.DistanceM;
        context.LongestDescentPercent = descent is null ? null : GradeOf(descent.Value);

        return context;
    }

    private static ElevationSegment? LongestBy(List<ElevationSegment> segments, Func<ElevationSegment, bool> isWanted)
    {
        var wanted = segments.Where(isWanted).Where(segment => segment.DistanceM > 0).ToList();

        return wanted.Count == 0 ? null : wanted.MaxBy(segment => segment.DistanceM);
    }

    private static double GradeOf(ElevationSegment segment)
    {
        return segment.ElevationM / segment.DistanceM * 100;
    }
}
