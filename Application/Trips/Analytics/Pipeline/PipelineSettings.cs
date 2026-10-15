using Application.AppSettings;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;

namespace Application.Trips.Analytics.Pipeline;

// Reads the pipeline's database settings. A missing service, row or unreadable value yields null,
// and the callers then fall back to the code constants in PipelineDefaults.
public static class PipelineSettings
{
    public static async Task<GpxDataProcessingSetting?> GpxDataProcessingAsync(
        IAppSettingsService? appSettings,
        CancellationToken cancellationToken
    )
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

    // What a null config resolves to for every stage, so the bench can show and customise the real defaults.
    public static async Task<PipelineDefaultsDto> DefaultsAsync(
        IAppSettingsService? appSettings,
        CancellationToken cancellationToken
    )
    {
        var cleanup = await GpxDataProcessingAsync(appSettings, cancellationToken);
        var peaks = await PeakMatchingAsync(appSettings, cancellationToken);

        var cleanupFilters = cleanup is null ? PipelineDefaults.CleanupFilters : PipelineDefaults.CleanupFiltersFrom(cleanup);
        var peakFilters = peaks is null ? PipelineDefaults.PeakFilters : PipelineDefaults.PeakFiltersFrom(peaks);

        return new PipelineDefaultsDto(
            cleanupFilters.Select(DefaultFilterDto.From).ToList(),
            [],
            peakFilters.Select(DefaultFilterDto.From).ToList(),
            peaks?.ProximityRadiusM ?? PipelineDefaults.PeakProximityRadiusM,
            PipelineDefaults.RouteFilters.Select(DefaultFilterDto.From).ToList()
        );
    }

    public static async Task<PeakMatchingSetting?> PeakMatchingAsync(
        IAppSettingsService? appSettings,
        CancellationToken cancellationToken
    )
    {
        if (appSettings is null)
        {
            return null;
        }

        var result = await appSettings.GetSettingAsync<PeakMatchingSetting>(
            AppSettingType.PeakMatching,
            cancellationToken
        );

        return result.IsSuccess ? result.Value : null;
    }
}
