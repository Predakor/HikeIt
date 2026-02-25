using Domain.AppSettings.Interfaces;
using Domain.Common.AggregateRoot;
using System.Text.Json;

namespace Domain.AppSettings.Root;

public enum AppSettingType
{
    GpxDataProcessing = 1,
}

public class AppSetting : AggregateRoot<int, AppSetting>
{
    public string Name { get; protected set; } = string.Empty;
    public string JsonValue { get; protected set; } = string.Empty;
    public AppSettingType SettingType { get; protected set; }

    public Result<AppSetting> SetSetting<TSetting>(TSetting setting)
        where TSetting : IAppSetting
    {
        if (setting.SettingFor != SettingType)
        {
            return Errors.Unknown("setting type doesn't match");
        }

        AddDomainEvent(new AppSettingEvents.JsonValueUpdated(Id));
        return this;
    }

    public Result<TSetting> GetSetting<TSetting>()
    {
        var parsedSetting = JsonSerializer.Deserialize<TSetting>(JsonValue);

        return parsedSetting is null ? Errors.ParsingError<TSetting>(JsonValue) : parsedSetting;
    }

    public static AppSetting Create(IAppSetting setting)
    {
        return new AppSetting()
        {
            Id = 0,
            Name = nameof(setting),
            JsonValue = JsonSerializer.Serialize(setting),
            SettingType = setting.SettingFor
        };
    }
}
