using Domain.AppSettings.Root;
using Domain.Common.Abstractions;

namespace Domain.AppSettings.Interfaces;
public interface IAppSettingsRepository : ICrudResultRepository<AppSetting, int>
{
    Task<Result<AppSetting>> GetBySettingType(AppSettingType settingType, CancellationToken ct);
}
