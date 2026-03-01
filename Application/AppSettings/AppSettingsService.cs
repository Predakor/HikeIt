using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;

namespace Application.AppSettings;

internal sealed class AppSettingsService : IAppSettingsService
{
    private readonly IAppSettingsRepository repository;
    private readonly TimeProvider dateTimeProvider;

    public AppSettingsService(IAppSettingsRepository repository, TimeProvider dateTimeProvider)
    {
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
    }

    public Task<Result<AppSetting>> SetSetting<TSetting>(
        TSetting setting,
        CancellationToken ct
    )
        where TSetting : IAppSetting
    {
        return repository
            .GetBySettingTypeAsync(setting.SettingFor, ct)
            .MatchAsync(
                found => found.SetSetting(setting),
                notFound => repository.AddAsync(AppSetting.Create(setting))
            )
            .TapAsync(_ => repository.SaveChangesAsync(ct));
    }

    public Task<Result<TSetting>> GetSetting<TSetting>(TSetting setting, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return repository
            .GetBySettingTypeAsync(setting.SettingFor, ct)
            .BindAsync(r => r.GetSetting<TSetting>());
    }

    public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct)
    {
        return repository
            .GetByIdAsync(id, ct)
            .TapAsync(e => e.Delete(dateTimeProvider.GetUtcNow()))
            .TapAsync(_ => repository.SaveChangesAsync(ct));

    }
}
