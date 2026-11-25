using Domain.Common.Abstractions;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks closer than DistanceM, keeping the highest.
public sealed class MergeNearbyByDistanceFilter(MergeNearbyByDistanceFilter.Config config) : SyncFilter<PeakChain>
{
    public sealed record Config(double DistanceM) : IPeakFilterConfig
    {
        public IFilter<PeakChain> CreateFilter() => new MergeNearbyByDistanceFilter(this);
    }

    public override PeakChain Apply(PeakChain chain)
    {
        chain.Candidates = chain
            .Candidates.ToList()
            .MergeNearbyPeakByDistance(config.DistanceM);

        return chain;
    }
}
