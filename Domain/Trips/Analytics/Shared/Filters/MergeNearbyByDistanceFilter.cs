using Application.Commons.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks closer than DistanceM, keeping the highest.
public sealed class MergeNearbyByDistanceFilter(MergeNearbyByDistanceFilter.Config config)
    : IFilter<GpxPointWithDistance>
{
    public sealed record Config(double DistanceM) : IPeakFilterConfig;

    public IList<GpxPointWithDistance> Apply(IList<GpxPointWithDistance> values)
    {
        return values.ToList().MergeNearbyPeakByDistance(config.DistanceM);
    }
}
