using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Trips.Root.Builders.Config;

namespace Domain.AppSettings.Settings;

public sealed record GpxDataProcessingSetting(
    float MaxElevationSpike,
    float EmaSmoothingAlpha,
    int MedianFilterWindowSize,
    int RoundingDecimalsCount
) : IAppSetting, IGpxDataProcessingSettings
{
    public string Name => nameof(GpxDataProcessingSetting);
    public AppSettingType SettingFor => AppSettingType.GpxDataProcessing;

}
