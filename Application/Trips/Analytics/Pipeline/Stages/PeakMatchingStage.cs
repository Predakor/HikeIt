using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Trips.Analytics.Shared;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class PeakMatchingStage(IPeaksFilterRunner runner)
    : FilterListStageBase<IPeakFilterConfig>
{
    public override StageId Id => StageId.PeakMatching;

    protected override IReadOnlyList<IPeakFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.PeakMatching?.Filters;
    }

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var candidates = PeakCandidate.From(CleanupStage.CleanedDataFrom(context));
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(candidates, RequestFilters(config), steps, cancellationToken)
            .MapAsync(chain => ToOutput(chain, steps));
    }

    private static Stage.Output ToOutput(PeakChain chain, List<PipeStageDto.StepTiming> steps)
    {
        var matched = chain
            .Matched
            .Select(ToMatchedPeak)
            .ToList();

        return new Stage.Output(new StageData.PeakMatching(chain.CandidatesBeforeLookup, matched), steps);
    }

    private static StageData.MatchedPeak ToMatchedPeak(PeakCandidate candidate)
    {
        var peak = candidate.Peak!;
        var distanceM = DistanceHelpers.Distance2D(peak, candidate);

        return new StageData.MatchedPeak(peak.Id, peak.Name, distanceM, peak.Height);
    }
}