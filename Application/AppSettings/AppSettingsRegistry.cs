using Core.Serialization;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using System.Text.Json.Nodes;

namespace Application.AppSettings;

public static class AppSettingsRegistry
{

    private static readonly Dictionary<AppSettingType, Type> map = new()
    {
        { AppSettingType.GpxDataProcessing, typeof(GpxDataProcessingSetting) },
        { AppSettingType.PeakMatching, typeof(PeakMatchingSetting) },
        { AppSettingType.ElevationProfile, typeof(ElevationProfileSetting) },
        { AppSettingType.Route, typeof(RouteSetting) },
    };

    public static JsonNode GetSchema(AppSettingType type)
    {
        if (!map.TryGetValue(type, out var settingType))
        {
            throw new InvalidOperationException($"Setting type {type} not registered.");
        }

        return settingType.ToJsonSchema();
    }

    public static Type GetType(AppSettingType type)
    {
        if (!map.TryGetValue(type, out var settingType))
        {
            throw new InvalidOperationException($"Setting type {type} not registered.");
        }

        return settingType;
    }
}