using Domain.Trips.Builders.GpxDataBuilder;

namespace Domain.TripAnalytics.Configs;

public interface IConfig<TFor>;

public record ElevationProfileConfig(
    float? MaxElevationSpike = 10f,
    float? EmaSmoothingAlpha = .4f,
    int? MedianFilterWindowSize = 5,
    int? RoundingDecimalsCount = 1,
    int? DownsamplingFactor = 10
) : IConfig<GpxDataBuilder>;
