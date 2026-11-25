using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;

namespace Domain.Trips.Analytics.Shared.Filters;

/// <summary>Attaches the nearest peak in the candidates' bounding box (padded by OffsetM) to each candidate; drops none.</summary>
public sealed class PeakLookupFilter(PeakLookupFilter.Config config) : IFilter<PeakChain>
{
    private static Peak? NearestPeak(IReadOnlyList<Peak> found, PeakCandidate candidate)
    {
        return found.MinBy(peak => DistanceHelpers.Distance2D(peak, candidate));
    }

    public sealed record Config(double OffsetM) : IPeakFilterConfig
    {
        public IFilter<PeakChain> CreateFilter() => new PeakLookupFilter(this);
    }

    public async ValueTask<PeakChain> ApplyAsync(PeakChain chain)
    {
        chain.MarkLookupStarted();

        var candidates = chain.Candidates;
        if (candidates.Count == 0)
        {
            return chain;
        }

        var bounds = GeoBounds.Around(candidates.Select(candidate => candidate.Point.BasePoint), config.OffsetM);
        var found = await chain.Peaks.GetPeaksInBounds(bounds);

        chain.Candidates = candidates
            .Select(candidate => candidate with { Peak = NearestPeak(found, candidate) })
            .ToList();

        return chain;
    }
}
