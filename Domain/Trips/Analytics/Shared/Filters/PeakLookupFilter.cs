using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;

namespace Domain.Trips.Analytics.Shared.Filters;

/// <summary>Attaches the nearest peak in the candidates' bounding box (padded by OffsetM) to each candidate; drops none.</summary>
public sealed class PeakLookupFilter(PeakLookupFilter.Config config, IPeakSource peaks) : IPeakFilter
{
    public sealed record Config(double OffsetM) : IPeakFilterConfig
    {
        public IPeakFilter CreateFilter(PeakFilterContext context) => new PeakLookupFilter(this, context.Peaks);
    }

    public async ValueTask<IList<PeakCandidate>> ApplyAsync(IList<PeakCandidate> candidates)
    {
        if (candidates.Count == 0)
        {
            return candidates;
        }

        var bounds = GeoBounds.Around(candidates.Select(candidate => candidate.Point.BasePoint), config.OffsetM);
        var found = await peaks.GetPeaksInBounds(bounds);

        return candidates.Select(candidate => candidate with { Peak = NearestPeak(found, candidate) }).ToList();
    }

    private static Peak? NearestPeak(IReadOnlyList<Peak> found, PeakCandidate candidate)
    {
        return found.MinBy(peak => DistanceHelpers.Distance2D(peak, candidate));
    }
}