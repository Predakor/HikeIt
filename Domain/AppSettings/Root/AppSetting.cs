using Domain.AppSettings.Interfaces;
using Domain.Common.AggregateRoot;
using System.Text.Json;

namespace Domain.AppSettings.Root;

public enum AppSettingType
{
    GpxDataProcessing = 1,
    PeakMatching = 3,
    ElevationProfile = 4,
    Route = 5,
}

public class AppSetting : AggregateRoot<int, AppSetting>
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public string Name { get; protected set; } = string.Empty;
    public JsonDocument? JsonValue { get; protected set; }
    public AppSettingType SettingType { get; protected set; }

    public Result<AppSetting> SetSetting<TSetting>(TSetting setting)
        where TSetting : IAppSetting
    {
        if (TSetting.SettingFor != SettingType)
        {
            return Errors.Unknown("setting type doesn't match");
        }

        var validationErrors = setting.Validate().ToList();
        if (validationErrors.Count > 0)
        {
            return Errors.BadRequest(string.Join(" ", validationErrors));
        }

        JsonValue = JsonSerializer.SerializeToDocument(setting, SerializerOptions);
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

        var validationErrors = st.Validate().ToList();
        if (validationErrors.Count > 0)
        {
            return Errors.BadRequest(string.Join(" ", validationErrors));
        }

        JsonValue = JsonSerializer.SerializeToDocument(setting, type, SerializerOptions);
        AddDomainEvent(new AppSettingEvents.JsonValueUpdated(SettingType));
        return this;
    }

    public Result<TSetting> GetSetting<TSetting>()
        where TSetting : IAppSetting
    {
        var parsedSetting = JsonSerializer.Deserialize<TSetting>(JsonValue.RootElement.GetRawText(), SerializerOptions);

        return parsedSetting is null
            ? Errors.ParsingError<TSetting>(JsonValue.RootElement.GetRawText())
            : parsedSetting;
    }

    public static AppSetting Create<TSetting>(TSetting setting)
        where TSetting : IAppSetting
    {
        return new AppSetting()
        {
            Id = 0,
            Name = typeof(TSetting).Name,
            JsonValue = JsonSerializer.SerializeToDocument(setting, SerializerOptions),
            SettingType = TSetting.SettingFor,
        };
    }
}