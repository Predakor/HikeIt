using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Peaks;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface IPeaksFilterRunner
{
    Task<Result<PeakChainResult>> ApplyAsync(
        IList<PeakCandidate> candidates,
        IReadOnlyList<IPeakFilterConfig>? configs = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class PeaksFilterRunner(IPeakSource peaks, IPipelineSettingsService settings) : IPeaksFilterRunner
{
    public async Task<Result<PeakChainResult>> ApplyAsync(
        IList<PeakCandidate> candidates,
        IReadOnlyList<IPeakFilterConfig>? configs = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<IPeakFilterConfig>(StageId.PeakMatching, cancellationToken);

        return await candidates
            .EnsureNotEmpty("The trip has no points to match peaks against")
            .BindAsync(nonEmpty => RunFiltersAsync(
                nonEmpty,
                configs
            ));
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
