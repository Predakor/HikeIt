using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks whose elevation differs by less than ElevationDeltaM, keeping the highest.
public sealed class MergeNearbyByElevationFilter(MergeNearbyByElevationFilter.Config config)
    : IFilter<GpxPointWithDistance>
{
    public sealed record Config(double ElevationDeltaM) : FilterConfigBase<Config>(default!), IPeakFilterConfig;

    public IList<GpxPointWithDistance> Apply(IList<GpxPointWithDistance> values)
    {
        // STUB (Phase 0): implemented by BE-1, wrapping MergeNearbyPeaksByElevation.
        throw new NotImplementedException();
    }
}
