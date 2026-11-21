using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Peaks;
using Domain.Trips.Analytics.Shared;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface IPeaksFilterRunner
{
    /// <summary> Pass steps to collect per-filter timings; leave it null for a normal, untimed run. </summary>
    Task<Result<PeakChain>> ApplyAsync(
        IList<PeakCandidate> candidates,
        IReadOnlyList<IPeakFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class PeaksFilterRunner(IPeakSource peaks, IPipelineSettingsService settings) : IPeaksFilterRunner
{
    public async Task<Result<PeakChain>> ApplyAsync(
        IList<PeakCandidate> candidates,
        IReadOnlyList<IPeakFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<IPeakFilterConfig>(StageId.PeakMatching, cancellationToken);

        return await candidates
            .EnsureNotEmpty("The trip has no points to match peaks against")
            .BindAsync(nonEmpty => FilterChain.RunAsync(new PeakChain(nonEmpty, peaks), configs, steps));
    }
}
