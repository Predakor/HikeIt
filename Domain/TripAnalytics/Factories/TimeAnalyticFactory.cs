

using Domain.Common;
using Domain.TripAnalytics.Builders.TripTimeAnalyticBuilder;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Factories;

public interface IFactory<TData, TOutput> {
    TOutput Create(TData data);
}

public record TimeFactoryData(RouteAnalytic RouteAnalytic, List<GpxPoint> Points);

public class TimeAnalyticFactory : IFactory<TimeFactoryData, TimeAnalytic?> {
    public TimeAnalytic? Create(TimeFactoryData data) {
        if (!data.ValidateData()) {
            return null;
        }

        List<GpxPointWithTime> pointsWithTime = data.Points.MapToTimed();
        List<GpxGainWithTime> gainsWithTime = pointsWithTime.ToGains();

        DateTime start = pointsWithTime.First().Time;
        DateTime end = pointsWithTime.Last().Time;
        TimeFrame tripTimeFrame = new(start, end);

        TimeAnalyticData timeAnalyticsData = new(data.RouteAnalytic, gainsWithTime, tripTimeFrame);

        var analytics = TimeAnalyticsDirector.Create(timeAnalyticsData);
        return analytics;
    }

    public static TimeAnalytic? CreateAnalytics(TimeFactoryData data) {
        return new TimeAnalyticFactory().Create(data);
    }
}

internal static class TimeFactoryDataValidator {
    static bool Validate(TimeFactoryData data) {
        if (data.Points.Count == 0) {
            return false;
        }
        if (data.RouteAnalytic == null) {
            return false;
        }
        if (data.Points.Any(p => p.Time == null)) {
            return false;
        }

        return true;
    }

    public static bool ValidateData(this TimeFactoryData data) {
        return Validate(data);
    }
}
