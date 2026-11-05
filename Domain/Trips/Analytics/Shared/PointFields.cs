using System.Text.Json.Serialization;

namespace Domain.Trips.Analytics.Shared;

/// <summary>Which numeric fields of a track point a filter acts on.</summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PointFields
{
    None = 0,
    Latitude = 1,
    Longitude = 2,
    Elevation = 4,
    Coordinates = Latitude | Longitude,
    All = Coordinates | Elevation,
}

public static class PointFieldsExtensions
{
    public static void EnsureAny(this PointFields fields, string paramName)
    {
        bool selectsNothing = fields == PointFields.None;
        if (selectsNothing)
        {
            throw new ArgumentException("At least one of Latitude, Longitude or Elevation must be selected.", paramName);
        }
    }
}
