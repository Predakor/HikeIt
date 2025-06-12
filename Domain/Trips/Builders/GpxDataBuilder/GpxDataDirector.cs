using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

public record PointsToPreserve(List<GpxPoint> Maximas, List<GpxPoint> Minimas);
public record ElevationProfileData(AnalyticData Data, PointsToPreserve? Points);


internal static class GpxDataDirector {
    public static AnalyticData AnalyticData(List<GpxPoint> points) {
        return new GpxDataBuilder(points)
            .RoundElevation()
            .ClampElevationSpikes()
            .ApplyMedianFilter()
            .ApplyEmaSmoothing()
            .Build();
    }

    public static AnalyticData ElevationProfile(ElevationProfileData data) {
        return new GpxDataBuilder(data.Data.Points)
            .DownSample()
            .RoundElevation()
            .ClampElevationSpikes()
            .ApplyMedianFilter()
            .ApplyEmaSmoothing()
            .Build();
    }
}
