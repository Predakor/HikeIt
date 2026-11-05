using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public sealed record RouteVisualizationSetting(
    IReadOnlyList<IFilterConfig> FilterConfigs
) : IAppSetting
{
    [JsonIgnore] public string Name => nameof(RouteVisualizationSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.RouteVisualization;
}
