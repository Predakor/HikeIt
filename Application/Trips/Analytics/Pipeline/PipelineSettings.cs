using Application.AppSettings;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;

namespace Application.Trips.Analytics.Pipeline;

// Reads the pipeline's database settings. A missing service, row or unreadable value yields null,
// and the callers then fall back to the code constants in PipelineDefaults.
public static class PipelineSettings
{
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
