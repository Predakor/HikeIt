using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace Core.Serialization;

// Uses the framework's Web defaults, the same ones ASP.NET applies to request and response bodies.
public static class AppJsonExtensions
{
    private static JsonSerializerOptions Options => JsonSerializerOptions.Web;

    private static readonly JsonSchemaExporterOptions SchemaOptions = new()
    {
        TreatNullObliviousAsNonNullable = true,
    };

    public static string ToJson<T>(this T value) => JsonSerializer.Serialize(value, Options);

    public static JsonDocument ToJsonDocument<T>(this T value) => JsonSerializer.SerializeToDocument(value, Options);

    public static JsonDocument ToJsonDocument(this object value, Type type) => JsonSerializer.SerializeToDocument(value, type, Options);

    public static T? FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json, Options);

    public static object? FromJson(this string json, Type type) => JsonSerializer.Deserialize(json, type, Options);

    public static JsonNode ToJsonSchema(this Type type) => Options.GetJsonSchemaAsNode(type, SchemaOptions);
}
