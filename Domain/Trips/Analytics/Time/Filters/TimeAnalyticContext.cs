using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;

namespace Domain.Trips.Analytics.Time.Filters;

public enum SegmentClass
{
    Flat,
    Idle,
    Ascent,
    Descent,
}

// The state the pace filters fill in: classifier filters claim segments, and the times and speeds are measured from the claims.
public sealed class TimeAnalyticContext
{
    private readonly SegmentClass?[] _claimed;

    public TimeAnalyticContext(RouteAnalytic route, IReadOnlyList<GpxGainWithTime> gains, DateTime start, DateTime end)
    {
        Route = route;
        Gains = gains;
        Start = start;
        End = end;
        _claimed = new SegmentClass?[gains.Count];
    }

    public RouteAnalytic Route { get; }
    public IReadOnlyList<GpxGainWithTime> Gains { get; }
    public DateTime Start { get; }
    public DateTime End { get; }

    // Unclaimed segments are flat.
    public IReadOnlyList<SegmentClass> Classes => _claimed.Select(claimed => claimed ?? SegmentClass.Flat).ToList();

    public double? FastestSpeedKph { get; set; }
    public double? SlowestSpeedKph { get; set; }

    public static bool HasUsableTimestamps(List<GpxPoint> points)
    {
        return points.Count >= 2 && points.All(point => point.Time is not null);
    }

    // Fails when the track has no usable timestamps.
    public static Result<TimeAnalyticContext> Create(RouteAnalytic route, List<GpxPoint> points)
    {
        if (!HasUsableTimestamps(points))
        {
            return Errors.EmptyCollection("timestamps");
        }

        var pointsWithTime = points.MapToTimed();

        return new TimeAnalyticContext(route, pointsWithTime.ToGains(), pointsWithTime.First().Time, pointsWithTime.Last().Time);
    }

    public bool IsClaimed(int index) => _claimed[index].HasValue;

    public void Claim(int index, SegmentClass segmentClass)
    {
        _claimed[index] = segmentClass;
    }

    public TimeSpan TimeOf(SegmentClass segmentClass)
    {
        double seconds = Gains
            .Where((_, i) => (_claimed[i] ?? SegmentClass.Flat) == segmentClass)
            .Sum(gain => gain.TimeDelta);

        return TimeSpan.FromSeconds(seconds);
    }

    public TimeAnalytic ToAnalytic()
    {
        var startTime = Start.ToUniversalTime();
        var endTime = End.ToUniversalTime();
        var duration = endTime - startTime;

        var idleTime = TimeOf(SegmentClass.Idle);
        var ascentTime = TimeOf(SegmentClass.Ascent);
        var descentTime = TimeOf(SegmentClass.Descent);
        var activeTime = duration - idleTime;

        return new TimeAnalytic
        {
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration,

            ActiveTime = activeTime,
            IdleTime = idleTime,
            AscentTime = ascentTime,
            DescentTime = descentTime,

            AverageSpeedKph = ToKph(activeTime, Route.TotalDistanceMeters),
            AverageAscentKph = ToKph(ascentTime, Route.TotalAscentMeters),
            AverageDescentKph = ToKph(descentTime, Route.TotalDescentMeters),

            FastestSpeedKph = FastestSpeedKph,
            SlowestSpeedKph = SlowestSpeedKph,
        };
    }

    private static double ToKph(TimeSpan time, double distanceMeters)
    {
        return time.TotalHours > 0 ? distanceMeters / 1000 / time.TotalHours : 0;
    }
}

// First filter that claims a segment wins, so filter order decides ties.
public abstract class TimeAnalyticFilter : SyncFilter<TimeAnalyticContext>
{
    // Null leaves the segment to the next filter.
    public abstract SegmentClass? Classify(GpxGainWithTime gain);

    public sealed override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        for (int i = 0; i < context.Gains.Count; i++)
        {
            if (context.IsClaimed(i))
            {
                continue;
            }

            var claimed = Classify(context.Gains[i]);
            if (claimed.HasValue)
            {
                context.Claim(i, claimed.Value);
            }
        }

        return context;
    }
}
