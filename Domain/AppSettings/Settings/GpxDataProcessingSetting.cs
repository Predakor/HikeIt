using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Trips.Root.Builders.Config;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public sealed record GpxDataProcessingSetting(
    float MaxElevationSpike,
    float EmaSmoothingAlpha,
    int MedianFilterWindowSize,
    int RoundingDecimalsCount
) : IAppSetting, IGpxDataProcessingSettings
{
    // Math.Round supports at most 15 fractional digits.
    const int MaxRoundingDecimals = 15;

    [JsonIgnore] public string Name => nameof(GpxDataProcessingSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.GpxDataProcessing;

    public IEnumerable<string> Validate()
    {
        var isSpikeValid = MaxElevationSpike > 0;
        var isAlphaValid = EmaSmoothingAlpha > 0 && EmaSmoothingAlpha <= 1;
        var isWindowValid = MedianFilterWindowSize >= 1;
        var isDecimalsValid = RoundingDecimalsCount is >= 0 and <= MaxRoundingDecimals;

        if (!isSpikeValid)
        {
            yield return "MaxElevationSpike must be greater than 0.";
        }

        if (!isAlphaValid)
        {
            yield return "EmaSmoothingAlpha must be in (0, 1].";
        }

        if (!isWindowValid)
        {
            yield return "MedianFilterWindowSize must be at least 1.";
        }

        if (!isDecimalsValid)
        {
            yield return $"RoundingDecimalsCount must be between 0 and {MaxRoundingDecimals}.";
        }
    }
}
