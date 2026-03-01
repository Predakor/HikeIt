using Domain.AppSettings.Root;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Application.AppSettings.DTOs;

public sealed record UpdateAppSettingDto(int Id, JsonDocument Value);

public sealed record AppSettingDto
{
    public static IList<AppSettingDto> FromAppSettingList(IEnumerable<AppSetting> setting) => [.. setting.Select(FromAppSetting)];
    public static AppSettingDto FromAppSetting(AppSetting setting)
    {
        return new()
        {
            Id = setting.Id,
            Name = setting.Name,
            Type = setting.SettingType,
            Value = setting.JsonValue!,
            Schema = AppSettingsRegistry.GetSchema(setting.SettingType)
        };
    }

    public int Id { get; init; }
    public required string Name { get; init; }
    public required JsonDocument Value { get; init; }
    public required JsonNode Schema { get; init; }
    public AppSettingType Type { get; init; }

}

