using Application.AppSettings;
using Application.Commons.Abstractions;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.Config;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Application.Trips.Analytics.Pipeline.Stages;

// Cleans the raw track with the cleanup filters (production list by default). Every other stage reads its output.
public sealed class CleanupStage(IAppSettingsService? appSettings = null) : IPipelineStage
{
    public StageId Id => StageId.Cleanup;

    public object? SelectConfig(PipelineConfig config) => config.Cleanup;

    // Default filters follow the GPX processing setting in the database; the code constants are the fallback.
    public async Task<string> CacheDiscriminatorAsync(CancellationToken cancellationToken)
    {
        var settings = await LoadSettingsAsync(cancellationToken);
        if (settings is null)
        {
            return "defaults";
        }

        return FormattableString.Invariant(
            $"{settings.MaxElevationSpike}|{settings.MedianFilterWindowSize}|{settings.EmaSmoothingAlpha}|{settings.RoundingDecimalsCount}"
        );
    }

    public async Task<StageOutput> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var filters = config.Cleanup?.Filters ?? DefaultFilters(await LoadSettingsAsync(cancellationToken));
        var rawCount = context.Input.RawPoints.Count;
        var cleanedPoints = ApplyPointFilters(context.Input.RawPoints, filters);

        var data = new StageData.Cleanup(rawCount, cleanedPoints.Count);
        return new StageOutput(data, cleanedPoints.Count, new AnalyticData(cleanedPoints));
    }

    // Null when no service is injected or the setting is missing or fails to load.
    async Task<IGpxDataProcessingSettings?> LoadSettingsAsync(CancellationToken cancellationToken)
    {
        if (appSettings is null)
        {
            return null;
        }

        var result = await appSettings.GetSettingAsync<GpxDataProcessingSetting>(
            AppSettingType.GpxDataProcessing,
            cancellationToken
        );

        return result.IsSuccess ? result.Value : null;
    }

    static IReadOnlyList<IPointFilterConfig> DefaultFilters(IGpxDataProcessingSettings? settings)
    {
        return settings is null ? PipelineDefaults.CleanupFilters : PipelineDefaults.CleanupFiltersFrom(settings);
    }

    // Read by the downstream stages. The cleanup stage always runs first, so its Internal is present.
    internal static AnalyticData CleanedDataFrom(StageContext context)
    {
        return context.Upstream[StageId.Cleanup].Internal as AnalyticData
            ?? throw new InvalidOperationException("Cleanup output is missing its cleaned points.");
    }

    // Runs point filters on a copy of the points; the input list is never mutated.
    internal static List<GpxPoint> ApplyPointFilters(IEnumerable<GpxPoint> points, IEnumerable<IFilterConfig> configs)
    {
        var mutablePoints = points.Select(p => p.ToMutable()).ToList();

        foreach (var filter in RouteVisualizationService.GpxFilterFactory.CreateFilterList(configs))
        {
            mutablePoints = [.. filter.Apply(mutablePoints)];
        }

        return mutablePoints.Select(p => p.ToGpxPoint()).ToList();
    }
}
