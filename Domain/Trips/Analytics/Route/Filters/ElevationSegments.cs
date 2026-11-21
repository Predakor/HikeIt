using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Route.Filters;

// One continuous climb (positive ElevationM) or descent (negative) between two turning points.
public readonly record struct ElevationSegment(double DistanceM, double ElevationM);

public static class ElevationSegments
{
    // Turning points are confirmed once the track reverses by at least minChangeM, so smaller wobble never splits a segment.
    public static List<ElevationSegment> Split(IReadOnlyList<GpxGain> gains, double minChangeM)
    {
        var segments = new List<ElevationSegment>();

        double distance = 0;
        double elevation = 0;
        double pivotDistance = 0;
        double pivotElevation = 0;
        double extremeDistance = 0;
        double extremeElevation = 0;
        int direction = 0;

        foreach (var gain in gains)
        {
            distance += gain.DistanceDelta;
            elevation += gain.ElevationDelta;

            bool isStarting = direction == 0;
            bool isExtending = direction * (elevation - extremeElevation) > 0;
            double pullback = direction * (extremeElevation - elevation);
            bool hasReversed = pullback > 0 && pullback >= minChangeM;

            if (isStarting)
            {
                double change = elevation - pivotElevation;
                bool hasMovedEnough = Math.Abs(change) >= minChangeM && change != 0;

                if (hasMovedEnough)
                {
                    direction = Math.Sign(change);
                    extremeDistance = distance;
                    extremeElevation = elevation;
                }
            }
            else if (isExtending)
            {
                extremeDistance = distance;
                extremeElevation = elevation;
            }
            else if (hasReversed)
            {
                segments.Add(new ElevationSegment(extremeDistance - pivotDistance, extremeElevation - pivotElevation));
                pivotDistance = extremeDistance;
                pivotElevation = extremeElevation;
                direction = -direction;
                extremeDistance = distance;
                extremeElevation = elevation;
            }
        }

        if (direction != 0)
        {
            segments.Add(new ElevationSegment(extremeDistance - pivotDistance, extremeElevation - pivotElevation));
        }

        return segments;
    }
}
