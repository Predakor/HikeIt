using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json.Serialization;

namespace Domain.AppSettings.Settings;

// Peak matching defaults: the database lookup radius and the two merge filters applied to the candidate peaks.
public sealed record PeakMatchingSetting(
    float ProximityRadiusM,
    float MergeDistanceM,
    float MergeElevationM
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

        if (MergeDistanceM <= 0)
        {
            yield return "MergeDistanceM must be greater than 0.";
        }

        if (MergeElevationM <= 0)
        {
            yield return "MergeElevationM must be greater than 0.";
        }
    }
}
