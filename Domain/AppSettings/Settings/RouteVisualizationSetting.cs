using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public record FilterConfigBase<T>(string Name, T Value) : IFilterConfig<T>
{
    public Type FilterType => typeof(T);
    public object GetValue() => Value!;
}

public sealed record RouteVisualizationSetting(
    IReadOnlyList<IFilterConfig<object>> FilterConfigs
) : IAppSetting
{
    [JsonIgnore] public string Name => nameof(RouteVisualizationSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.RouteVisualization;
}
