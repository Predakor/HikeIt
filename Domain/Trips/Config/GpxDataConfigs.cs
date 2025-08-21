namespace Domain.Trips.Config;

public abstract record DataProccesConfig {
    public sealed record GpxFile(
        float MaxElevationSpike = 8f,
        float EmaSmoothingAlpha = 0.88f,
        int MedianFilterWindowSize = 6,
        int RoundingDecimalsCount = 1
    ) : DataProccesConfig;

    public sealed record ElevationProfile(
        float MaxElevationSpike = 8f,
        float EmaSmoothingAlpha = 0.88f,
        int MedianFilterWindowSize = 6,
        int RoundingDecimalsCount = 1,
        int DownsamplingFactor = 10
    ) : DataProccesConfig;

    public sealed record Partial(
        float? MaxElevationSpike = null,
        float? EmaSmoothingAlpha = null,
        int? MedianFilterWindowSize = null,
        int? RoundingDecimalsCount = null,
        int? DownsamplingFactor = null
    ) : DataProccesConfig;
}

public static class GpxDataConfigs {
    public static DataProccesConfig.GpxFile GpxFile => new();
    public static DataProccesConfig.ElevationProfile ElevationProfile => new();
    public static DataProccesConfig.Partial Partial => new();
}
