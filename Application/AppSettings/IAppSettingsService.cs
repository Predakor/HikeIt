using Application.AppSettings.DTOs;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using System.Text.Json;

namespace Application.AppSettings;

public interface IAppSettingsService
{
    Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct);
    Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct) where TSetting : IAppSetting;

    Task<Result<AppSetting>> SetSetting<TSetting>(TSetting setting, CancellationToken ct) where TSetting : IAppSetting;
    Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct);
}