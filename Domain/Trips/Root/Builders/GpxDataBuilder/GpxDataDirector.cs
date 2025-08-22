using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.Config;

namespace Domain.Trips.Root.Builders.GpxDataBuilder;

public record ElevationDataWithConfig(AnalyticData Data, DataProccesConfig Config);

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

    public static AnalyticData ElevationProfile(AnalyticData data) {
        var config = GpxDataConfigs.ElevationProfile;

        return new GpxDataBuilder(data.Points)
            //.DownSample(config.DownsamplingFactor)
            .Build();
    }

    public static AnalyticData FromConfig(AnalyticData data, DataProccesConfig.Partial config) {
        var builder = new GpxDataBuilder(data.Points);
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
