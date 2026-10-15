using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks whose elevation differs by less than ElevationDeltaM, keeping the highest.
public sealed class MergeNearbyByElevationFilter(MergeNearbyByElevationFilter.Config config)
    : IFilter<GpxPointWithDistance>
{
    public sealed record Config(double ElevationDeltaM) : FilterConfigBase<Config>(default!), IPeakFilterConfig;

    public IList<GpxPointWithDistance> Apply(IList<GpxPointWithDistance> values)
    {
        return values.ToList().MergeNearbyPeaksByElevation(config.ElevationDeltaM);
    }
}
