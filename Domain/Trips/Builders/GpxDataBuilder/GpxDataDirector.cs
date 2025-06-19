using Domain.Trips.Config;
using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

public record PointsToPreserve(IEnumerable<GpxPoint> Maximas, IEnumerable<GpxPoint> Minimas);

public record ElevationProfileData(AnalyticData Data, PointsToPreserve? Points = null);

public record ElevationDataWithConfig(ElevationProfileData Data);

internal static class GpxDataDirector {
    public static AnalyticData AnalyticData(List<GpxPoint> points) {
        var config = GpxDataConfigs.GpxFile;
        return new GpxDataBuilder(points)
            .ClampElevationSpikes(config.MaxElevationSpike)
            .ApplyMedianFilter(config.MedianFilterWindowSize)
            .ApplyEmaSmoothing(config.EmaSmoothingAlpha)
            .RoundElevation(config.RoundingDecimalsCount)
            .Build();
    }

    public static AnalyticData ElevationProfile(ElevationProfileData data) {
        var config = GpxDataConfigs.ElevationProfile;

        return new GpxDataBuilder(data.Data.Points).DownSample(config.DownsamplingFactor).Build();
    }

    public static AnalyticData FromConfig(ElevationProfileData data, ConfigBase.Nullable config) {
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
