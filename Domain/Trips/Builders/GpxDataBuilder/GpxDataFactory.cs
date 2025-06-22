using Domain.Trips.Config;
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

    public static AnalyticData CreateFromConfig(ElevationDataWithConfig data) {
        //var (analyticData, config) = data;
        var points = data.Data.Data.Points;

        return data switch {
            (_, ConfigBase.GpxFile) => GpxDataDirector.AnalyticData(points),
            (_, ConfigBase.ElevationProfile) => GpxDataDirector.ElevationProfile(data.Data),
            (_, ConfigBase.Nullable config) => GpxDataDirector.FromConfig(data.Data, config),

            _ => throw new Exception($"unsuported data type: {data}"),
        };
    }
}
