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
    [JsonIgnore] public string Name => nameof(GpxDataProcessingSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.GpxDataProcessing;

}
