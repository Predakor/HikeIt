using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Stages;

public abstract class FilterListStageBase<TFilter>(IPipelineSettingsService settings) : IPipelineStage
    where TFilter : IFilterConfig
{
    public abstract StageId Id { get; }

    protected abstract IReadOnlyList<TFilter>? RequestFilters(PipelineConfig config);

    public abstract Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    );

    public object? SelectConfig(PipelineConfig config) => RequestFilters(config);

    public async Task<string> CacheDiscriminatorAsync(CancellationToken cancellationToken)
    {
        var filters = await settings.GetFiltersAsync<TFilter>(Id, cancellationToken);

        return string.Join('|', filters.Select(filter => $"{filter.GetType().FullName}:{filter}"));
    }

    protected async Task<Result<IReadOnlyList<TFilter>>> ResolveFiltersAsync(
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        return Result<IReadOnlyList<TFilter>>.Success(
            RequestFilters(config) ?? await settings.GetFiltersAsync<TFilter>(Id, cancellationToken)
        );
    }
}
