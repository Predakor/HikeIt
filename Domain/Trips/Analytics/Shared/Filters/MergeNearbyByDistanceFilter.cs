using Domain.Common.Abstractions;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks closer than DistanceM, keeping the highest.
public sealed class MergeNearbyByDistanceFilter(MergeNearbyByDistanceFilter.Config config) : IPeakFilter
{
    public sealed record Config(double DistanceM) : IPeakFilterConfig
    {
        public IPeakFilter CreateFilter(PeakFilterContext context) => new MergeNearbyByDistanceFilter(this);
    }

    public ValueTask<IList<PeakCandidate>> ApplyAsync(IList<PeakCandidate> candidates)
    {
        return ValueTask.FromResult<IList<PeakCandidate>>(candidates.ToList().MergeNearbyPeakByDistance(config.DistanceM));
    }
}
