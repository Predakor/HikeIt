using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

public sealed record PeakMatchingSetting(
    float ProximityRadiusM,
    IReadOnlyList<IPeakFilterConfig> Filters
) : IAppSetting
{
    [JsonIgnore] public string Name => nameof(PeakMatchingSetting);
    [JsonIgnore] public AppSettingType SettingFor => AppSettingType.PeakMatching;

    public IEnumerable<string> Validate()
    {
        if (ProximityRadiusM <= 0)
        {
            yield return "ProximityRadiusM must be greater than 0.";
        }
    }
}
