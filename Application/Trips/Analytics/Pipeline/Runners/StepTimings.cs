using Application.Trips.Analytics.Dtos;
using Domain.Common.Abstractions;
using System.Collections.Concurrent;

namespace Application.Trips.Analytics.Pipeline.Runners;

internal static class StepTimings
{
    private static readonly ConcurrentDictionary<Type, string> Names = new();

    public static PipeStageDto.StepTiming Of(IFilterConfig config, TimeSpan elapsed)
    {
        var name = Names.GetOrAdd(
            config.GetType(),
            configType => configType.DeclaringType?.Name ?? configType.Name
        );

        return new PipeStageDto.StepTiming(name, elapsed.TotalMilliseconds);
    }

    // Configs are nested records (IdleFilter.Config), so the declaring type is the filter's name.
}
