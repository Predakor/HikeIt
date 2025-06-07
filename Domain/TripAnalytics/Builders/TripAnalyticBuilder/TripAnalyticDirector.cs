using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.TripAnalyticBuilder;

public static class TripAnalyticDirector {
    public static TripAnalytic Create(TripAnalyticData data) {
        return new TripAnalyticBuilder(data)
            .WithGains()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithTotalDescent()
            .WithTotalAscent()
            .WithTotalDistance()
            .Build();
    }

    public static TripAnalytic CreateTimedAnalytics(TripAnalyticData data) {
        return new TripAnalyticBuilder(data)
            .WithGains()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithTotalDescent()
            .WithTotalAscent()
            .WithTotalDistance()
            .WithTimeAnalytic()
            .Build();
    }
}
