using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks closer than DistanceM, keeping the highest.
public sealed class MergeNearbyByDistanceFilter(MergeNearbyByDistanceFilter.Config config)
    : IFilter<GpxPointWithDistance>
{
    public sealed record Config(double DistanceM) : FilterConfigBase<Config>(default!), IPeakFilterConfig;

    public IList<GpxPointWithDistance> Apply(IList<GpxPointWithDistance> values)
    {
        // STUB (Phase 0): implemented by BE-1, wrapping MergeNearbyPeakByDistance.
        throw new NotImplementedException();
    }
}
