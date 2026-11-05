using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;

namespace Domain.Trips.Analytics.Shared;

/// <summary>Peak stays null until a lookup step attaches the nearest known peak.</summary>
public sealed record PeakCandidate(GpxPointWithDistance Point, Peak? Peak = null) : IGeoPoint
{
    public double Lat => Point.Lat;
    public double Lon => Point.Lon;
    public double Ele => Point.Ele;
}
