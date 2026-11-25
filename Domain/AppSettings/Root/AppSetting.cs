using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Serialization;
using Domain.AppSettings.Interfaces;
using Domain.Common.AggregateRoot;

namespace Domain.AppSettings.Root;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AppSettingType
{
    GpxDataProcessing,
    PeakMatching,
    ElevationProfile,
    Route,
    Pace,
    Terrain,
}

public class AppSetting : AggregateRoot<int, AppSetting>
{
    public string Name { get; protected set; } = string.Empty;
    public JsonDocument? JsonValue { get; protected set; }
    public AppSettingType SettingType { get; protected set; }

    public static AppSetting Create<TSetting>(TSetting setting)
        where TSetting : IAppSetting
    {
        return new AppSetting()
        {
            Id = 0,
            Name = typeof(TSetting).Name,
            JsonValue = setting.ToJsonDocument(),
            SettingType = TSetting.SettingFor,
        };
    }

    public Result<AppSetting> SetSetting<TSetting>(TSetting setting)
        where TSetting : IAppSetting
    {
        if (TSetting.SettingFor != SettingType)
        {
            return Errors.Unknown("setting type doesn't match");
        }

        var validationErrors = setting
            .Validate()
            .ToList();
        if (validationErrors.Count > 0)
        {
            return Errors.BadRequest(string.Join(" ", validationErrors));
        }

        JsonValue = setting.ToJsonDocument();
        AddDomainEvent(new AppSettingEvents.JsonValueUpdated(TSetting.SettingFor));
        return this;
    }

    public Result<AppSetting> SetSetting(object? setting, Type type)
    {
        if (setting is null)
        {
            return Errors.Unknown("setting is null");
        }

        if (setting is not IAppSetting st)
        {
            return Errors.BadRequest("setting type doesn't match");
        }

        var validationErrors = st
            .Validate()
            .ToList();
        if (validationErrors.Count > 0)
        {
            return Errors.BadRequest(string.Join(" ", validationErrors));
        }

        JsonValue = setting.ToJsonDocument(type);
        AddDomainEvent(new AppSettingEvents.JsonValueUpdated(SettingType));
        return this;
    }

    public Result<TSetting> GetSetting<TSetting>()
        where TSetting : IAppSetting
    {
        var parsedSetting = JsonValue
            .RootElement.GetRawText()
            .FromJson<TSetting>();

        return parsedSetting is null
            ? Errors.ParsingError<TSetting>(JsonValue.RootElement.GetRawText())
            : parsedSetting;
    }
}
