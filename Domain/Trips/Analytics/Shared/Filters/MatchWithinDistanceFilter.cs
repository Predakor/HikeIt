using Domain.Common.Abstractions;
using Domain.Common.Geography;

namespace Domain.Trips.Analytics.Shared.Filters;

/// <summary>Keeps candidates whose attached peak is within MaxDistanceM. Without it, any peak in the lookup box matches.</summary>
public sealed class MatchWithinDistanceFilter(MatchWithinDistanceFilter.Config config) : IPeakFilter
{
    public sealed record Config(double MaxDistanceM) : IPeakFilterConfig
    {
        public IPeakFilter CreateFilter(PeakFilterContext context) => new MatchWithinDistanceFilter(this);
    }

    public ValueTask<IList<PeakCandidate>> ApplyAsync(IList<PeakCandidate> candidates)
    {
        return ValueTask.FromResult<IList<PeakCandidate>>(candidates.Where(IsWithinDistance).ToList());
    }

    private bool IsWithinDistance(PeakCandidate candidate)
    {
        if (candidate.Peak is null)
        {
            return false;
        }

        return DistanceHelpers.Distance2D(candidate.Peak, candidate) <= config.MaxDistanceM;
    }
}
