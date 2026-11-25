using Domain.Common.Abstractions;
using Domain.Common.Geography;

namespace Domain.Trips.Analytics.Shared.Filters;

/// <summary>Keeps candidates whose attached peak is within MaxDistanceM. Without it, any peak in the lookup box matches.</summary>
public sealed class MatchWithinDistanceFilter(MatchWithinDistanceFilter.Config config) : SyncFilter<PeakChain>
{
    public sealed record Config(double MaxDistanceM) : IPeakFilterConfig
    {
        public IFilter<PeakChain> CreateFilter() => new MatchWithinDistanceFilter(this);
    }

    public override PeakChain Apply(PeakChain chain)
    {
        chain.Candidates = chain
            .Candidates.Where(IsWithinDistance)
            .ToList();

        return chain;
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
