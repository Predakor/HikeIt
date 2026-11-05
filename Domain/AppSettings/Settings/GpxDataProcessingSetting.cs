using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.AppSettings.Settings;

public sealed record GpxDataProcessingSetting(
    IReadOnlyList<IPointFilterConfig> Filters
) : IAppSetting<GpxDataProcessingSetting>, IFilterSetting<IPointFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.GpxDataProcessing;
    public static GpxDataProcessingSetting GetDefault() => new(PipelineDefaults.CleanupFilters);
}
