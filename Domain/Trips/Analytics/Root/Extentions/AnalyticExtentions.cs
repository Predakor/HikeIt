using Domain.Users.Stats.ValueObjects;

namespace Domain.Trips.Analytics.Root.Extentions;

public static class AnalyticExtentions {
    public static UserStatsUpdates.All ToStatUpdate(this TripAnalytic analytics) {
        if (analytics.RouteAnalytics is null) {
            throw new NullReferenceException("No route analytics ");
        }

        var routeAnalytics = analytics.RouteAnalytics;
        var timeAnalytics = analytics.TimeAnalytics;

        var peakAnalytics = analytics.PeaksAnalytic;

        var totalPeaks = peakAnalytics?.Total ?? 0;
        var newPeaks = peakAnalytics?.New ?? 0;


        var tripDistanceMeters = routeAnalytics.TotalDistanceMeters.ToSafeUint();
        var tripDuration = timeAnalytics?.Duration ?? TimeSpan.Zero;

        var totalsUpdate = new UserStatsUpdates.Totals(
            tripDistanceMeters,
            routeAnalytics.TotalAscentMeters.ToSafeUint(),
            routeAnalytics.TotalDescentMeters.ToSafeUint(),
            totalPeaks,
            tripDuration
        );

        var locationsUpdate = new UserStatsUpdates.Locations(newPeaks, 1);


        var metasUpdate = new UserStatsUpdates.Metas(tripDistanceMeters, tripDuration);

        return new(totalsUpdate, locationsUpdate, metasUpdate);
    }
}

static class Helpers {

    const uint Max = uint.MaxValue;
    const uint Min = uint.MinValue;

    public static uint ToSafeUint(this double? value) {
        return (uint)Math.Clamp(value ?? 0, Min, Max);
    }

    public static uint ToSafeUint(this double value) {
        return (uint)Math.Clamp(value, Min, Max);
    }

    public static uint ToSafeUint(this int? value) {
        return (uint)Math.Clamp(value ?? 0, Min, Max);
    }

    public static uint ToSafeUint(this int value) {
        return (uint)Math.Clamp(value, Min, Max);
    }
}
