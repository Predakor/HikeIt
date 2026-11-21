using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Time.Filters;

namespace Domain.AppSettings.Settings;

public sealed record PaceSetting(IReadOnlyList<IPaceFilterConfig> Filters)
    : IAppSetting<PaceSetting>, IFilterSetting<IPaceFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.Pace;
    public static PaceSetting GetDefault() => new(PipelineDefaults.PaceFilters);

    public IEnumerable<string> Validate()
    {
        foreach (var filter in Filters ?? [])
        {
            if (filter is null)
            {
                yield return "Filters must not contain empty entries.";
            }

            if (filter is IdleFilter.Config { SpeedKph: < 0 })
            {
                yield return "IdleFilter SpeedKph must not be negative.";
            }

            if (filter is GradeFilter.Config { MinGradePercent: < 0 })
            {
                yield return "GradeFilter MinGradePercent must not be negative.";
            }

            if (filter is SpeedSegmentFilter.Config { WindowM: <= 0 })
            {
                yield return "SpeedSegmentFilter WindowM must be greater than 0.";
            }

            if (filter is GradeFilter.Config { Targets: GradeTargets.None })
            {
                yield return "GradeFilter must target Ascent, Descent or both.";
            }
        }
    }
}
