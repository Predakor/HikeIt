using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;

namespace Application.AppSettings;

internal sealed class AppSettingsService : IAppSettingsService
{
    private readonly IAppSettingsRepository repository;

    public AppSettingsService(IAppSettingsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result<AppSetting>> SetSetting<TSetting>(
        TSetting setting,
        CancellationToken ct
    )
        where TSetting : IAppSetting
    {
        return await repository
            .GetBySettingType(setting.SettingFor, ct)
            .MatchAsync(
                found => found.SetSetting(setting),
                notFound => repository.AddAsync(AppSetting.Create(setting))
            )
            .TapAsync(_ => repository.SaveChangesAsync(ct));
    }

    public async Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return await repository
            .GetBySettingType(setting.SettingFor, ct)
            .BindAsync(r => r.GetSetting<TSetting>());
    }
}
