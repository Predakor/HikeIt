namespace Domain.TripAnalytics.Configs;

public record ElevationProfileConfig(
    float? MaxElevationSpike = 10f,
    float? EmaSmoothingAlpha = .4f,
    float? MedianFilterWindowSize = 5,
    int? RoundingDecimalsCount = 1,
    int? DownsamplingFactor = 10
);
