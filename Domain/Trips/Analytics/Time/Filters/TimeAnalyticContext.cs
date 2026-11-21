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

// The state the time analytic filters fill in. Classifier filters claim segments, measuring filters read the claims.
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

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public TimeSpan ActiveTime { get; set; }
    public TimeSpan IdleTime { get; set; }
    public TimeSpan AscentTime { get; set; }
    public TimeSpan DescentTime { get; set; }

    public double AverageSpeedKph { get; set; }
    public double AverageAscentKph { get; set; }
    public double AverageDescentKph { get; set; }

    // Fails when the track has no usable timestamps.
    public static Result<TimeAnalyticContext> Create(RouteAnalytic route, List<GpxPoint> points)
    {
        bool hasUsableTimestamps = points.Count >= 2 && points.All(point => point.Time is not null);
        if (!hasUsableTimestamps)
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
        return new TimeAnalytic
        {
            StartTime = StartTime,
            EndTime = EndTime,
            Duration = Duration,

            ActiveTime = ActiveTime,
            IdleTime = IdleTime,
            AscentTime = AscentTime,
            DescentTime = DescentTime,

            AverageAscentKph = AverageAscentKph,
            AverageDescentKph = AverageDescentKph,
            AverageSpeedKph = AverageSpeedKph,
        };
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
