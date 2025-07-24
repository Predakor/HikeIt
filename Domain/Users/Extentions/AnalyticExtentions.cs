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
        var newPeaks = peakAnalytics?.Summary?.TotalPeaks.ToSafeUint() ?? 0;

        //change when trip summary updates
        var newRegions = 0;

        //To be fixed
        //TotalDistanceKm is actually distance in meters
        var tripDistanceMeters = routeAnalytics.TotalDistanceKm.ToSafeUint();

        var totalsUpdate = new StatsUpdates.Totals(
            tripDistanceMeters,
            routeAnalytics.TotalAscent.ToSafeUint(),
            routeAnalytics.TotalDescent.ToSafeUint(),
            newPeaks,
            timeAnalytics?.Duration ?? TimeSpan.Zero
        );

        var locationsUpdate = new StatsUpdates.Locations(newPeaks, 1);

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
