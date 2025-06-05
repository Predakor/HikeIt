using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Builders.TripAnalyticBuilder;

public static class TripAnalyticDirector {
    public static TripAnalytic Create(GpxAnalyticData data) {
        return new TripAnalyticBuilder(data)
            .WithGains()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithTotalDescent()
            .WithTotalAscent()
            .WithTotalDistance()
            .Build();
    }

    public static TripAnalytic CreateTimedAnalytics(GpxAnalyticData data) {
        return new TripAnalyticBuilder(data)
            .WithGains()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithTotalDescent()
            .WithTotalAscent()
            .WithTotalDistance()
            .Build();
    }
}
