using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppSettings;

internal class AppSettingsRepository : CrudResultRepository<AppSetting, int>, IAppSettingsRepository
{
    public AppSettingsRepository(TripDbContext context)
        : base(context) { }

    public async Task<Result<AppSetting>> GetBySettingType(
        AppSettingType settingType,
        CancellationToken ct
    )
    {
        var result = await DbSet
            .Where(s => s.SettingType == settingType)
            .FirstOrDefaultAsync(ct);

        return result is null
            ? Errors.NotFound(nameof(settingType), "type", settingType)
            : result;
    }
}
