using Domain.TripAnalytics.Configs;
using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

public record PointsToPreserve(List<GpxPoint> Maximas, List<GpxPoint> Minimas);

public record ElevationProfileData(AnalyticData Data, PointsToPreserve? Points = null);

public record ElevationDataWithConfig(ElevationProfileData Data, ElevationProfileConfig Config);

record DefaultConfig(
    float MaxElevationSpike = 10f,
    float EmaSmoothingAlpha = 0.88f,
    int MedianFilterWindowSize = 7,
    int RoundingDecimalsCount = 1
) : IConfig<GpxDataBuilder>;

internal static class GpxDataDirector {
    public static AnalyticData AnalyticData(List<GpxPoint> points) {
        var config = new DefaultConfig();
        return new GpxDataBuilder(points)
            .ClampElevationSpikes(config.MaxElevationSpike)
            .ApplyMedianFilter(config.MedianFilterWindowSize)
            .ApplyEmaSmoothing(config.EmaSmoothingAlpha)
            .RoundElevation(config.RoundingDecimalsCount)
            .Build();
    }

    public static AnalyticData ElevationProfile(ElevationProfileData data) {
        var config = new DefaultConfig();

        return new GpxDataBuilder(data.Data.Points).DownSample(10).Build();
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
        Console.WriteLine("im here and i see ");
        Console.WriteLine(DownsamplingFactor);

        if (MaxElevationSpike != null) {
            builder.ClampElevationSpikes((double)MaxElevationSpike);
        }
        if (MedianFilterWindowSize.HasValue) {
            builder.ApplyMedianFilter(MedianFilterWindowSize.Value);
        }
        if (EmaSmoothingAlpha.HasValue) {
            builder.ApplyEmaSmoothing(EmaSmoothingAlpha.Value);
        }
        if (DownsamplingFactor.HasValue) {
            builder.DownSample(DownsamplingFactor.Value);
        }
        if (RoundingDecimalsCount.HasValue) {
            builder.RoundElevation(RoundingDecimalsCount.Value);
        }
        return builder.Build();
    }
}
