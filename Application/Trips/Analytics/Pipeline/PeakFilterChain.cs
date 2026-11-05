using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

namespace Application.Trips.Analytics.Pipeline;

public sealed class PeakFilterChain(IPeakSource peaks)
{
    public static List<PeakCandidate> CandidatesFrom(AnalyticData data)
    {
        if (data.Points.Count == 0)
        {
            return [];
        }

        var gains = data.Gains ?? (data.Points.Count > 1 ? data.Points.ToGains() : []);
        var candidates = new List<PeakCandidate>(data.Points.Count) { new(new GpxPointWithDistance(data.Points[0], 0)) };

        double distanceFromStart = 0;
        for (int i = 1; i < data.Points.Count; i++)
        {
            distanceFromStart += gains[i - 1].DistanceDelta;
            candidates.Add(new PeakCandidate(new GpxPointWithDistance(data.Points[i], (float)distanceFromStart)));
        }

        return candidates;
    }

    public Task<Result<PeakChainResult>> ApplyAsync(
        IList<PeakCandidate> candidates,
        IEnumerable<IPeakFilterConfig> configs
    )
    {
        return candidates
            .EnsureNotEmpty("The trip has no points to match peaks against")
            .BindAsync(nonEmpty => RunFiltersAsync(nonEmpty, configs));
    }

    private async Task<Result<PeakChainResult>> RunFiltersAsync(
        IList<PeakCandidate> candidates,
        IEnumerable<IPeakFilterConfig> configs
    )
    {
        var context = new PeakFilterContext(peaks);
        IList<PeakCandidate> result = candidates;
        int? countBeforeLookup = null;

        foreach (var config in configs)
        {
            if (config is PeakLookupFilter.Config)
            {
                countBeforeLookup ??= result.Count;
            }

            result = await config.CreateFilter(context).ApplyAsync(result);
        }

        return new PeakChainResult(result, countBeforeLookup ?? result.Count);
    }
}

public sealed record PeakChainResult(IList<PeakCandidate> Candidates, int CandidatesBeforeLookup)
{
    public IEnumerable<PeakCandidate> ToMatched() => Candidates.Where(candidate => candidate.Peak is not null);
}
