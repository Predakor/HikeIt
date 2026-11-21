using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Time.Filters;

public sealed class DurationFilter : SyncFilter<TimeAnalyticContext>
{
    public sealed record Config : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new DurationFilter();
    }

    public override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        context.StartTime = context.Start.ToUniversalTime();
        context.EndTime = context.End.ToUniversalTime();
        context.Duration = context.EndTime - context.StartTime;

        return context;
    }
}
