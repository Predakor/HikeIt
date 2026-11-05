using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public sealed record RouteSetting(
    IReadOnlyList<IPointFilterConfig> Filters
) : IAppSetting
{
    [JsonIgnore] public string Name => nameof(RouteSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.Route;
}
