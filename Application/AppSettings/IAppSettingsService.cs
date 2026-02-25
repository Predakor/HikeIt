using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;

namespace Application.AppSettings;
internal interface IAppSettingsService
{
    Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct) where TSetting : IAppSetting;
    Task<Result<AppSetting>> SetSetting<TSetting>(TSetting setting, CancellationToken ct) where TSetting : IAppSetting;
}