namespace Domain.Trips.Analytics.Shared;

public static class DistanceWindows
{
    // For every end point, the latest start that still leaves the window at least windowM long. Ends with no such start are skipped.
    public static IEnumerable<(int Start, int End)> Of(IReadOnlyList<double> cumulativeDistance, double windowM)
    {
        int start = 0;

        for (int end = 1; end < cumulativeDistance.Count; end++)
        {
            while (start + 1 < end && cumulativeDistance[end] - cumulativeDistance[start + 1] >= windowM)
            {
                start++;
            }

            double length = cumulativeDistance[end] - cumulativeDistance[start];
            bool isLongEnough = length >= windowM && length > 0;

            if (isLongEnough)
            {
                yield return (start, end);
            }
        }
    }
}
