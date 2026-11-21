using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using System.Text.Json.Serialization;

namespace Domain.Trips.Analytics.Time.Filters;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GradeTargets
{
    None = 0,
    Ascent = 1,
    Descent = 2,
    Both = Ascent | Descent,
}

// Claims segments steeper than MinGradePercent in the targeted directions. Add one per direction for asymmetric thresholds.
public sealed class GradeFilter(GradeFilter.Config config) : TimeAnalyticFilter
{
    public sealed record Config(double MinGradePercent, GradeTargets Targets = GradeTargets.Both) : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new GradeFilter(this);
    }

    public override SegmentClass? Classify(GpxGainWithTime gain)
    {
        bool isAscent = config.Targets.HasFlag(GradeTargets.Ascent) && gain.Slope > config.MinGradePercent;
        if (isAscent)
        {
            return SegmentClass.Ascent;
        }

        bool isDescent = config.Targets.HasFlag(GradeTargets.Descent) && gain.Slope < -config.MinGradePercent;

        return isDescent ? SegmentClass.Descent : null;
    }
}
