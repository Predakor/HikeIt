using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Trips.Analytics.Shared;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class PeakMatchingStage(IPeaksFilterRunner runner, IPipelineSettingsService settings)
    : FilterListStageBase<IPeakFilterConfig>(settings)
{
    public override StageId Id => StageId.PeakMatching;

    protected override IReadOnlyList<IPeakFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.PeakMatching?.Filters;
    }

    public override Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var candidates = PeakCandidate.From(CleanupStage.CleanedDataFrom(context));

        return runner
            .ApplyAsync(candidates, RequestFilters(config), cancellationToken)
            .MapAsync(ToOutput);
    }

    private static StageOutput ToOutput(PeakChainResult chain)
    {
        var matched = chain
            .ToMatched()
            .Select(ToMatchedPeak)
            .ToList();

        return new StageOutput(new StageData.PeakMatching(chain.CandidatesBeforeLookup, matched), matched.Count);
    }

    private static StageData.MatchedPeak ToMatchedPeak(PeakCandidate candidate)
    {
        var peak = candidate.Peak!;
        var distanceM = DistanceHelpers.Distance2D(peak, candidate);

        return new StageData.MatchedPeak(peak.Id, peak.Name, distanceM, peak.Height);
    }
}