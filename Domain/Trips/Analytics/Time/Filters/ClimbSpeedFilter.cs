using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Time.Filters;

// Speeds from the route totals over the measured times, so it must run after the filters that set those times.
public sealed class ClimbSpeedFilter : SyncFilter<TimeAnalyticContext>
{
    public sealed record Config : IPaceFilterConfig
    {
        public IFilter<TimeAnalyticContext> CreateFilter() => new ClimbSpeedFilter();
    }

    public override TimeAnalyticContext Apply(TimeAnalyticContext context)
    {
        var route = context.Route;

        context.AverageSpeedKph = ToKph(context.ActiveTime, route.TotalDistanceMeters);
        context.AverageAscentKph = ToKph(context.AscentTime, route.TotalAscentMeters);
        context.AverageDescentKph = ToKph(context.DescentTime, route.TotalDescentMeters);

        return context;
    }

    private static double ToKph(TimeSpan time, double distanceMeters)
    {
        return time.TotalHours > 0 ? distanceMeters / 1000 / time.TotalHours : 0;
    }
}
