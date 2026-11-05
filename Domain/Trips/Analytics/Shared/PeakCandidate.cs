using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;

namespace Domain.Trips.Analytics.Shared;

/// <summary>Peak stays null until a lookup step attaches the nearest known peak.</summary>
public sealed record PeakCandidate(GpxPointWithDistance Point, Peak? Peak = null) : IGeoPoint
{
    public double Lat => Point.Lat;
    public double Lon => Point.Lon;
    public double Ele => Point.Ele;

    public static List<PeakCandidate> From(AnalyticData data)
    {
        if (data.Points.Count == 0)
        {
            return [];
        }

        var gains = data.Gains ?? (data.Points.Count > 1 ? data.Points.ToGains() : []);
        var candidates = new List<PeakCandidate>(data.Points.Count) { new(new GpxPointWithDistance(data.Points[0], 0)) };

        double distanceFromStart = 0;
        for (int i = 1; i < data.Points.Count; i++)
        {
            distanceFromStart += gains[i - 1].DistanceDelta;
            candidates.Add(new PeakCandidate(new GpxPointWithDistance(data.Points[i], (float)distanceFromStart)));
        }

        return candidates;
    }
}