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

    // Measuring filters read what the earlier ones set: classifiers, then durations and climb times, then activity time, then speeds.
    private static int StageOf(IPaceFilterConfig filter)
    {
        return filter switch
        {
            IdleFilter.Config or GradeFilter.Config => 0,
            DurationFilter.Config or ClimbTimeFilter.Config => 1,
            ActivityTimeFilter.Config => 2,
            ClimbSpeedFilter.Config => 3,
            _ => throw new InvalidOperationException($"No order defined for {filter.GetType().Name}."),
        };
    }

    public IEnumerable<string> Validate()
    {
        var stages = Filters
            .Select(StageOf)
            .ToList();

        bool isOutOfOrder = stages
            .Zip(
                stages.Skip(1),
                (current, next) => next < current)
            .Any(isBefore => isBefore);

        if (isOutOfOrder)
        {
            yield return "Time filters must run in order: Idle/Grade, then Duration/ClimbTime, then ActivityTime, then ClimbSpeed.";
        }

        foreach (var filter in Filters)
        {
            if (filter is IdleFilter.Config { SpeedKph: < 0 })
            {
                yield return "IdleFilter SpeedKph must not be negative.";
            }

            if (filter is GradeFilter.Config { MinGradePercent: < 0 })
            {
                yield return "GradeFilter MinGradePercent must not be negative.";
            }

            if (filter is GradeFilter.Config { Targets: GradeTargets.None })
            {
                yield return "GradeFilter must target Ascent, Descent or both.";
            }
        }
    }
}
