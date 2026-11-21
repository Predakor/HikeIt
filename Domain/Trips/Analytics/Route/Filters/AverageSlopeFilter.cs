using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Route.Filters;

public sealed class AverageSlopeFilter : SyncFilter<RouteAnalyticContext>
{
    public sealed record Config : ITerrainFilterConfig
    {
        public IFilter<RouteAnalyticContext> CreateFilter() => new AverageSlopeFilter();
    }

    // DefaultIfEmpty: a track with no climbing (or no descending) has no such slopes to average.
    public override RouteAnalyticContext Apply(RouteAnalyticContext context)
    {
        var gains = context.Gains;

        context.AverageSlopePercent = gains.Average(gain => gain.Slope);
        context.AverageAscentSlopePercent = gains.Where(gain => gain.Slope > 0).Select(gain => gain.Slope).DefaultIfEmpty(0).Average();
        context.AverageDescentSlopePercent = (short)gains.Where(gain => gain.Slope < 0).Select(gain => gain.Slope).DefaultIfEmpty(0).Average();

        return context;
    }
}
