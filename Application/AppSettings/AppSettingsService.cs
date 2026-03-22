using Application.AppSettings.DTOs;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.AppSettings;

internal sealed class AppSettingsService : IAppSettingsService
{
    private readonly IAppSettingsRepository repository;
    private readonly TimeProvider dateTimeProvider;
    private readonly ILogger<AppSettingsService> logger;

    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public AppSettingsService(IAppSettingsRepository repository, TimeProvider dateTimeProvider, ILogger<AppSettingsService> logger)
    {
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
        this.logger = logger;
    }

    public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting setting, CancellationToken ct)
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

    public Task<Result<TSetting>> GetSettingAsync<TSetting>(AppSettingType settingType, CancellationToken ct)
        where TSetting : IAppSetting
    {
        return repository
            .GetBySettingTypeAsync(settingType, ct)
            .BindAsync(r => r.GetSetting<TSetting>());
    }

    public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct)
    {
        return repository
            .GetAllAsync(ct)
            .MapAsync(AppSettingDto.FromAppSettingList);
    }

    public Task<Result<AppSettingDto>> Update(int id, JsonDocument? jsonSetting, CancellationToken ct)
    {
        return repository.GetByIdAsync(id, ct)
            .BindAsync(s =>
            {
                if (jsonSetting is null)
                {
                    return Errors.BadRequest("setting is null");
                }

                var type = AppSettingsRegistry.GetType(s.SettingType);
                var source = jsonSetting.RootElement.GetRawText();

                try
                {
                    var deserialized = JsonSerializer.Deserialize(source, type, jsonSerializerOptions);
                    return s.SetSetting(deserialized, type);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error deserializing setting with id {Id} and type {Type}", id, type);
                    return Result<AppSetting>.Failure(Errors.ParsingError(source, type));

                }

            })
            .MapAsync(AppSettingDto.FromAppSetting)
            .TapAsync(_ => repository.SaveChangesAsync(ct));
    }

    public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct)
    {
        return repository
            .GetByIdAsync(id, ct)
            .TapAsync(e => e.Delete(dateTimeProvider.GetUtcNow()))
            .TapAsync(_ => repository.SaveChangesAsync(ct));
    }

}
