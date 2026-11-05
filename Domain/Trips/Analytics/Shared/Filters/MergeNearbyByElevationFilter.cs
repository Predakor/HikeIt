using Domain.Common.Abstractions;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Trips.Analytics.Shared.Filters;

// Merges candidate peaks whose elevation differs by less than ElevationDeltaM, keeping the highest.
public sealed class MergeNearbyByElevationFilter(MergeNearbyByElevationFilter.Config config) : IPeakFilter
{
    public sealed record Config(double ElevationDeltaM) : IPeakFilterConfig
    {
        public IPeakFilter CreateFilter(PeakFilterContext context) => new MergeNearbyByElevationFilter(this);
    }

    public ValueTask<IList<PeakCandidate>> ApplyAsync(IList<PeakCandidate> candidates)
    {
        return ValueTask.FromResult<IList<PeakCandidate>>(candidates.ToList().MergeNearbyPeaksByElevation(config.ElevationDeltaM));
    }
}
