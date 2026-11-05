using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public sealed record ElevationProfileSetting(
    IReadOnlyList<IPointFilterConfig> Filters
) : IAppSetting
{
    [JsonIgnore] public string Name => nameof(ElevationProfileSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.ElevationProfile;
}
