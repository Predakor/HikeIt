using Domain.TripAnalytics;
using Domain.Users.ValueObjects;

namespace Domain.Users.Extentions;

public static class AnalyticExtentions {
    public static StatsUpdates.All ToStatUpdate(this TripAnalytic analytics, DateOnly tripDay) {
        if (analytics.RouteAnalytics is null) {
            throw new NullReferenceException("No route analytics ");
        }

        var routeAnalytics = analytics.RouteAnalytics;
        var timeAnalytics = analytics.TimeAnalytics;

        var peakAnalytics = analytics.PeaksAnalytic;

        var totalPeaks = peakAnalytics?.Total ?? 0;
        var newPeaks = peakAnalytics?.New ?? 0;

        Console.WriteLine("new peaks added/removed " + newPeaks);
        Console.WriteLine("total peaks added/removed " + totalPeaks);


        var newRegions = 0;

        var tripDistanceMeters = routeAnalytics.TotalDistanceMeters.ToSafeUint();

        var totalsUpdate = new StatsUpdates.Totals(
            tripDistanceMeters,
            routeAnalytics.TotalAscentMeters.ToSafeUint(),
            routeAnalytics.TotalDescentMeters.ToSafeUint(),
            totalPeaks,
            timeAnalytics?.Duration ?? TimeSpan.Zero
        );

        var locationsUpdate = new StatsUpdates.Locations(newPeaks, 1);

        var metasUpdate = new StatsUpdates.Metas(tripDay, tripDistanceMeters);

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
