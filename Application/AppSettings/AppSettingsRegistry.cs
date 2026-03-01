using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace Application.AppSettings;

public static class AppSettingsRegistry
{

    private static readonly Dictionary<AppSettingType, Type> map = new()
    {
        { AppSettingType.GpxDataProcessing, typeof(GpxDataProcessingSetting) },
    };

    private static readonly JsonSerializerOptions options = new(JsonSerializerOptions.Default)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

    };

    private static readonly JsonSchemaExporterOptions exportOptions = new()
    {
        TreatNullObliviousAsNonNullable = true,
    };

    public static JsonNode GetSchema(AppSettingType type)
    {
        if (!map.TryGetValue(type, out var settingType))
        {
            throw new InvalidOperationException($"Setting type {type} not registered.");
        }

        return options.GetJsonSchemaAsNode(settingType, exportOptions)
            ?? throw new InvalidCastException($"Failed to generate schema for {type}");
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
