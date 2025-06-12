using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

public static class GpxDataFactory {
    public static AnalyticData Create<T>(T data) =>
        data switch {
            List<GpxPoint> points => GpxDataDirector.AnalyticData(points),
            AnalyticData analyticData => GpxDataDirector.AnalyticData(analyticData.Points),
            ElevationProfileData elevationData => GpxDataDirector.ElevationProfile(elevationData),
            _ => throw new Exception($"unsuported data type: {data}"),
        };
}
