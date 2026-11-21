using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Time.Filters;

// Idle time from the claimed segments; active time is what is left of the duration, so DurationFilter must run first.
public sealed class ActivityTimeFilter : SyncFilter<TimeAnalyticContext>
{
    public sealed record Config : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new ActivityTimeFilter();
    }

    public override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        context.IdleTime = context.TimeOf(SegmentClass.Idle);
        context.ActiveTime = context.Duration - context.IdleTime;

        return context;
    }
}
