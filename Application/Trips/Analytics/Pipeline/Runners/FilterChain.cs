using Application.Trips.Analytics.Dtos;
using Core.Timing;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Runners;

internal static class FilterChain
{
    // steps is null on normal runs: nothing is recorded.
    public static async Task<Result<TState>> RunAsync<TState>(
        TState state,
        IEnumerable<IFilterConfig<TState>> configs,
        List<PipeStageDto.StepTiming>? steps
    )
    {
        foreach (var config in configs)
        {
            var (elapsed, next) = await Timed.RunAsync(() =>
                config
                    .CreateFilter()
                    .ApplyAsync(state)
            );
            state = next;

            steps?.Add(StepTimings.Of(config, elapsed));
        }

        return state;
    }
}
