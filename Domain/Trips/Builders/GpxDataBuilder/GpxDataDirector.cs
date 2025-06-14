using Domain.TripAnalytics.Configs;
using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

public record PointsToPreserve(List<GpxPoint> Maximas, List<GpxPoint> Minimas);

public record ElevationProfileData(AnalyticData Data, PointsToPreserve? Points);

public record ElevationDataWithConfig(ElevationProfileData Data, ElevationProfileConfig Config);

internal static class GpxDataDirector {
    public static AnalyticData AnalyticData(List<GpxPoint> points) {
        return new GpxDataBuilder(points)
            //.RoundElevation()
            .ClampElevationSpikes()
            //.ApplyMedianFilter()
            //.ApplyEmaSmoothing(.1f)
            .Build();
    }

    public static AnalyticData ElevationProfile(ElevationProfileData data) {
        return new GpxDataBuilder(data.Data.Points).DownSample(5).Build();
    }

    public static AnalyticData FromConfig(ElevationProfileData data, ElevationProfileConfig config) {
        var builder = new GpxDataBuilder(data.Data.Points);

        var (
            MaxElevationSpike,
            EmaSmoothingAlpha,
            MedianFilterWindowSize,
            RoundingDecimalsCount,
            DownsamplingFactor
        ) = config;

        if (MaxElevationSpike != null) {
            builder.ClampElevationSpikes((double)MaxElevationSpike);
        }
        if (EmaSmoothingAlpha.HasValue) {
            builder.ApplyEmaSmoothing(EmaSmoothingAlpha.Value);
        }
        if (MedianFilterWindowSize.HasValue) {
            builder.ApplyMedianFilter(MedianFilterWindowSize.Value);
        }
        if (RoundingDecimalsCount.HasValue) {
            builder.RoundElevation(RoundingDecimalsCount.Value);
        }
        if (DownsamplingFactor.HasValue) {
            builder.DownSample(DownsamplingFactor.Value);
        }

        return new GpxDataBuilder(data.Data.Points).DownSample(5).Build();
    }
}
