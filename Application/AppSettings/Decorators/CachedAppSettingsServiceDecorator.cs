using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;

namespace Application.AppSettings.Decorators;

internal sealed class CachedAppSettingsServiceDecorator : IAppSettingsService
{
    private readonly IAppSettingsService inner;
    private readonly ICache cache;

    private static string GetPrefix(AppSettingType settingType) => $"{nameof(AppSetting)}-{settingType}";

    public CachedAppSettingsServiceDecorator(IAppSettingsService inner, ICache cache)
    {
        this.inner = inner;
        this.cache = cache;
    }

    public async Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return await cache.GetOrCreateAsync(
            GetPrefix(setting.SettingFor),
            () => inner.GetSetting(setting, ct),
            ct: ct
        );
    }

    public async Task<Result<AppSetting>> SetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return await cache.GetOrCreateAsync(
            GetPrefix(setting.SettingFor),
            () => inner.SetSetting(setting, ct),
            ct: ct
        );
    }

    public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct)
    {
        return inner.DeleteSettingAsync(id, ct);
    }
}
