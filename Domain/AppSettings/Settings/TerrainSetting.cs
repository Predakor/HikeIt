using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Route.Filters;
using Domain.Trips.Analytics.Shared;

namespace Domain.AppSettings.Settings;

public sealed record TerrainSetting(IReadOnlyList<ITerrainFilterConfig> Filters)
    : IAppSetting<TerrainSetting>, IFilterSetting<ITerrainFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.Terrain;
    public static TerrainSetting GetDefault() => new(PipelineDefaults.TerrainFilters);

    public IEnumerable<string> Validate()
    {
        foreach (var filter in Filters ?? [])
        {
            if (filter is ElevationGainFilter.Config { MinChangeM: <= 0 })
            {
                yield return "ElevationGainFilter MinChangeM must be greater than 0.";
            }

            if (filter is LongestClimbFilter.Config { MinChangeM: <= 0 })
            {
                yield return "LongestClimbFilter MinChangeM must be greater than 0.";
            }

            if (filter is SteepestSegmentFilter.Config { WindowM: <= 0 })
            {
                yield return "SteepestSegmentFilter WindowM must be greater than 0.";
            }
        }
    }
}
