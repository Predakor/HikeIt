using Domain.TripAnalytics;
using Domain.Users.ValueObjects;

namespace Domain.Users.Extentions;

public static class AnalyticExtentions {
    public static StatsUpdates.All ToStatUpdate(this TripAnalytic analytics, DateOnly tripDay) {
        if (analytics.RouteAnalytics is null) {
            throw new NullReferenceException("NO route analytics ");
        }

        var routeAnalytics = analytics.RouteAnalytics;
        var timeAnalytics = analytics.TimeAnalytics;

        var peakAnalytics = analytics.PeaksAnalytic;
        var newPeaks = peakAnalytics?.Total;

        var newRegions = 0;

        var tripDistanceMeters = routeAnalytics.TotalDistanceMeters.ToSafeUint();

        var totalsUpdate = new StatsUpdates.Totals(
            tripDistanceMeters,
            routeAnalytics.TotalAscentMeters.ToSafeUint(),
            routeAnalytics.TotalDescentMeters.ToSafeUint(),
            newPeaks ?? 0,
            timeAnalytics?.Duration ?? TimeSpan.Zero
        );

        var locationsUpdate = new StatsUpdates.Locations(newPeaks ?? 0, 1);

        var metasUpdate = new StatsUpdates.Metas(tripDay, tripDistanceMeters);

        return new(totalsUpdate, locationsUpdate, metasUpdate);
    }
}

static class Helpers {
    public static uint ToSafeUint(this double? value) {
        return (uint)Math.Max(value ?? 0, 0);
    }

    public static uint ToSafeUint(this double value) {
        return (uint)Math.Max(value, 0);
    }

    public static uint ToSafeUint(this int? value) {
        return (uint)Math.Max(value ?? 0, 0);
    }

    public static uint ToSafeUint(this int value) {
        return (uint)Math.Max(value, 0);
    }
}
