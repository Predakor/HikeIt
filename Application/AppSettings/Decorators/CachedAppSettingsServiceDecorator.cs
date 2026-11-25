using System.Text.Json;
using Application.AppSettings.DTOs;
using Application.Commons.Abstractions;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;

namespace Application.AppSettings.Decorators;

internal sealed class CachedAppSettingsServiceDecorator : IAppSettingsService, ICacheable<IAppSettingsService>
{
    public const string GetAllKey = $"{nameof(CachedAppSettingsServiceDecorator)}-All";

    private readonly IAppSettingsService inner;
    private readonly ICache cache;

    public static string GetKey(AppSettingType settingType) => $"{nameof(AppSetting)}-{settingType}";

    public CachedAppSettingsServiceDecorator(IAppSettingsService inner, ICache cache)
    {
        this.inner = inner;
        this.cache = cache;
    }

    public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct)
    {
        return cache.GetOrCreateAsync(GetAllKey, () => inner.GetAllAsync(ct), ct: ct);
    }

    public Task<Result<TSetting>> GetSettingAsync<TSetting>(CancellationToken ct)
        where TSetting : IAppSetting
    {
        return cache.GetOrCreateAsync(GetKey(TSetting.SettingFor), () => inner.GetSettingAsync<TSetting>(ct), ct: ct);
    }

    public Task<TSetting> GetSettingOrDefaultAsync<TSetting>(CancellationToken ct)
        where TSetting : class, IAppSetting<TSetting>
    {
        return GetSettingAsync<TSetting>(ct)
            .MatchAsync(
                setting => setting,
                _ => TSetting.GetDefault()
            );
    }

    public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return inner
            .SetSetting(setting, ct)
            .TapAsync(_ => Invalidate(TSetting.SettingFor, ct));
    }

    public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct)
    {
        return inner
            .Update(id, jsonSetting, ct)
            .TapAsync(updated => Invalidate(updated.Type, ct));
    }

    public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct)
    {
        return inner
            .DeleteSettingAsync(id, ct)
            .TapAsync(deleted => Invalidate(deleted.SettingType, ct));
    }

    private async Task Invalidate(AppSettingType settingType, CancellationToken ct)
    {
        await cache.RemoveAsync(GetKey(settingType), ct);
        await cache.RemoveAsync(GetAllKey, ct);
    }
}
