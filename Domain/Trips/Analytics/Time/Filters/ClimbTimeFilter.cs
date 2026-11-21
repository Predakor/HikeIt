using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Time.Filters;

// Time spent on the segments the classifier filters claimed as ascent and descent.
public sealed class ClimbTimeFilter : SyncFilter<TimeAnalyticContext>
{
    public sealed record Config : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new ClimbTimeFilter();
    }

    public override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        context.AscentTime = context.TimeOf(SegmentClass.Ascent);
        context.DescentTime = context.TimeOf(SegmentClass.Descent);

        return context;
    }
}
