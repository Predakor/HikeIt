using Domain.Common.Abstractions;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks whose elevation differs by less than ElevationDeltaM, keeping the highest.
public sealed class MergeNearbyByElevationFilter(MergeNearbyByElevationFilter.Config config) : SyncFilter<PeakChain>
{
    public sealed record Config(double ElevationDeltaM) : IPeakFilterConfig
    {
        public IFilter<PeakChain> CreateFilter() => new MergeNearbyByElevationFilter(this);
    }

    public override PeakChain Apply(PeakChain chain)
    {
        chain.Candidates = chain
            .Candidates.ToList()
            .MergeNearbyPeaksByElevation(config.ElevationDeltaM);

        return chain;
    }
}
