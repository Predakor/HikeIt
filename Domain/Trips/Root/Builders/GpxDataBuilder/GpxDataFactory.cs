using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.Config;

namespace Domain.Trips.Root.Builders.GpxDataBuilder;

public static class GpxDataFactory {
    public static AnalyticData Create<T>(T data) =>
        data switch {
            List<GpxPoint> points => GpxDataDirector.AnalyticData(points),
            AnalyticData analyticData => GpxDataDirector.AnalyticData(analyticData.Points),
            //ElevationProfileData elevationData => GpxDataDirector.ElevationProfile(elevationData),
            _ => throw new Exception($"unsuported data type: {data}"),
        };

    public static AnalyticData CreateFromConfig(ElevationDataWithConfig data) {
        //var (analyticData, config) = data;
        var points = data.Data.Points;

        return data switch {
            (_, DataProccesConfig.GpxFile) => GpxDataDirector.AnalyticData(points),
            (_, DataProccesConfig.ElevationProfile) => GpxDataDirector.ElevationProfile(data.Data),
            (_, DataProccesConfig.Partial config) => GpxDataDirector.FromConfig(data.Data, config),

            _ => throw new Exception($"unsuported data type: {data}"),
        };
    }
}
