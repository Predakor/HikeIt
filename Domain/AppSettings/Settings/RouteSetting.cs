using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;

namespace Domain.AppSettings.Settings;

public sealed record RouteSetting(IReadOnlyList<IPointFilterConfig> Filters)
    : IAppSetting<RouteSetting>,
        IFilterSetting<IPointFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.Route;

    public static RouteSetting GetDefault() => new(PipelineDefaults.RouteFilters);

    public IEnumerable<string> Validate() => PointFilterValidation.Validate(Filters);
}
