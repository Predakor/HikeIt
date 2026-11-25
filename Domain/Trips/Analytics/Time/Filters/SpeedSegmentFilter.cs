using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.Trips.Analytics.Time.Filters;

// Fastest and slowest average speed over any stretch at least WindowM long. Leaves both null when the track is shorter.
public sealed class SpeedSegmentFilter(SpeedSegmentFilter.Config config) : SyncFilter<TimeAnalyticContext>
{
    public sealed record Config(double WindowM) : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new SpeedSegmentFilter(this);
    }

    public override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        var gains = context.Gains;
        var distance = new double[gains.Count + 1];
        var seconds = new double[gains.Count + 1];

        for (int i = 0; i < gains.Count; i++)
        {
            distance[i + 1] = distance[i] + gains[i].DistanceDelta;
            seconds[i + 1] = seconds[i] + gains[i].TimeDelta;
        }

        double? fastest = null;
        double? slowest = null;

        foreach (var (start, end) in DistanceWindows.Of(distance, config.WindowM))
        {
            double elapsed = seconds[end] - seconds[start];
            if (elapsed <= 0)
            {
                continue;
            }

            double speedKph = (distance[end] - distance[start]) / elapsed * 3.6;

            fastest = fastest is null
                ? speedKph
                : Math.Max(fastest.Value, speedKph);
            slowest = slowest is null
                ? speedKph
                : Math.Min(slowest.Value, speedKph);
        }

        context.FastestSpeedKph = fastest;
        context.SlowestSpeedKph = slowest;

        return context;
    }
}
