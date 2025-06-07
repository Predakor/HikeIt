using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Builders.TripAnalyticBuilder;

internal static class TripBuilderMethods {


    public static List<GpxPoint> FindLocalPeaks(List<GpxPoint> data, List<GpxGain> gains) {
        //data is filtered from gps jitter and microjumps
        //so if ele delta gets smaller we probbalby are on local peak

        List<GpxPoint> localPeaks = [];

        for (int i = 0; i < gains.Count; i++) {
            //difference between gpxPoint[i] and gpxPoint[i-1]
            var eleDelta = gains[i].ElevationDelta;
            if (eleDelta > 0) {
                continue;
            }

            localPeaks.Add(data[i]);
        }

        return localPeaks;
    }
}