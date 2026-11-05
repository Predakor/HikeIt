using System.Text.Json.Serialization;

namespace Domain.Trips.Analytics.Shared;

// Which numeric fields of a track point a filter acts on. Time and pinned flags are never touched.
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
    // Smoothing nothing is a config mistake, so each filter rejects it up front.
    public static void EnsureAny(this PointFields fields, string paramName)
    {
        bool selectsNothing = fields == PointFields.None;
        if (selectsNothing)
        {
            throw new ArgumentException("At least one of Latitude, Longitude or Elevation must be selected.", paramName);
        }
    }
}
