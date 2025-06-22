namespace Domain.Trips.Config;

public abstract record ConfigBase() {
    public record GpxFile(
        float MaxElevationSpike = 10f,
        float EmaSmoothingAlpha = 0.88f,
        int MedianFilterWindowSize = 7,
        int RoundingDecimalsCount = 1
    ) : ConfigBase;

    public record ElevationProfile(
        float MaxElevationSpike = 10f,
        float EmaSmoothingAlpha = 0.88f,
        int MedianFilterWindowSize = 7,
        int RoundingDecimalsCount = 1,
        int DownsamplingFactor = 10
    ) : ConfigBase;

    public record Nullable(
        float? MaxElevationSpike = null,
        float? EmaSmoothingAlpha = null,
        int? MedianFilterWindowSize = null,
        int? RoundingDecimalsCount = null,
        int? DownsamplingFactor = null
    ) : ConfigBase;
}

public static class GpxDataConfigs {
    public static ConfigBase.GpxFile GpxFile => new();
    public static ConfigBase.ElevationProfile ElevationProfile => new();
    public static ConfigBase.Nullable Nullable => new();
}
