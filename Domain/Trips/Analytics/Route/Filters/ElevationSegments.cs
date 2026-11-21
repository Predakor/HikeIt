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
        double lowDistance = 0;
        double lowElevation = 0;
        double highDistance = 0;
        double highElevation = 0;
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
                if (elevation < lowElevation)
                {
                    lowDistance = distance;
                    lowElevation = elevation;
                }

                if (elevation > highElevation)
                {
                    highDistance = distance;
                    highElevation = elevation;
                }

                double rise = elevation - lowElevation;
                double fall = highElevation - elevation;
                bool hasRisen = rise > 0 && rise >= minChangeM;
                bool hasFallen = fall > 0 && fall >= minChangeM;

                if (hasRisen)
                {
                    direction = 1;
                    pivotDistance = lowDistance;
                    pivotElevation = lowElevation;
                    extremeDistance = distance;
                    extremeElevation = elevation;
                }
                else if (hasFallen)
                {
                    direction = -1;
                    pivotDistance = highDistance;
                    pivotElevation = highElevation;
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
