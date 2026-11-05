using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.AppSettings.Settings;

public sealed record ElevationProfileSetting(
    IReadOnlyList<IPointFilterConfig> Filters
) : IAppSetting<ElevationProfileSetting>, IFilterSetting<IPointFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.ElevationProfile;
    public static ElevationProfileSetting GetDefault() => new(PipelineDefaults.ElevationProfileFilters);
}