using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Time.Filters;

// Claims segments slower than SpeedKph. Put it first so a standing wobble never counts as climbing.
public sealed class IdleFilter(IdleFilter.Config config) : TimeAnalyticFilter
{
    public sealed record Config(double SpeedKph) : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new IdleFilter(this);
    }

    public override SegmentClass? Classify(GpxGainWithTime gain)
    {
        bool hasNoDuration = gain.TimeDelta <= 0;
        if (hasNoDuration)
        {
            return null;
        }

        double speedKph = gain.DistanceDelta / gain.TimeDelta * 3.6;

        return speedKph < config.SpeedKph
            ? SegmentClass.Idle
            : null;
    }
}
