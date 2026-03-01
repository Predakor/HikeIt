using Application.AppSettings.DTOs;
using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json;

namespace Application.AppSettings.Decorators;

internal sealed class CachedAppSettingsServiceDecorator : IAppSettingsService
{
    private readonly IAppSettingsService inner;
    private readonly ICache cache;

    private static string GetPrefix(AppSettingType settingType) =>
        $"{nameof(AppSetting)}-{settingType}";

    public CachedAppSettingsServiceDecorator(IAppSettingsService inner, ICache cache)
    {
        this.inner = inner;
        this.cache = cache;
    }
    public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct)
    {
        return cache.GetOrCreateAsync(
            "All",
            () => inner.GetAllAsync(ct),
            ct: ct
        );
    }
    public Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return cache.GetOrCreateAsync(
            GetPrefix(setting.SettingFor),
            () => inner.GetSetting(setting, ct),
            ct: ct
        );
    }

    public Task<Result<AppSetting>> SetSetting<TSetting>(
        TSetting setting,
        CancellationToken ct
    )
        where TSetting : IAppSetting
    {
        return cache.GetOrCreateAsync(
            GetPrefix(setting.SettingFor),
            () => inner.SetSetting(setting, ct),
            ct: ct
        );
    }

    public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct) => inner.Update(id, jsonSetting, ct);
    public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct) => inner.DeleteSettingAsync(id, ct);
}
