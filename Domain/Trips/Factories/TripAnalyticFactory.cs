using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.Trips.ValueObjects;
using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Factories;

public interface IFactory<TData, TOutput> {
    TOutput Create(TData data);
}

public class TripAnalyticFactory : IFactory<TripAnalyticData, TripAnalytic> {
    public TripAnalytic Create(TripAnalyticData gpxData) {
        var gpxPointsWithTime = gpxData.Data.Where(e => e.Time != null).ToList();

        bool hasDataForTimeANalytics = gpxPointsWithTime.Count > 0;

        return hasDataForTimeANalytics
            ? TripAnalyticDirector.CreateTimedAnalytics(gpxData)
            : TripAnalyticDirector.Create(gpxData);
    }

    public static TripAnalytic CreateAnalytics(TripAnalyticData data) {
        return new TripAnalyticFactory().Create(data);
    }
}
