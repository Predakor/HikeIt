using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.AppSettings.Settings;

public sealed record RouteVisualizationSetting(
    IReadOnlyList<IFilterConfig> FilterConfigs
) : IAppSetting<RouteVisualizationSetting>
{
    public static AppSettingType SettingFor => AppSettingType.RouteVisualization;
    public static RouteVisualizationSetting GetDefault() => new(PipelineDefaults.RouteFilters);
}
