using Domain.Users.RegionProgressions.ValueObjects;

namespace Domain.Users.RegionProgressions.Factories;

public static class RegionProgressFactory {
    public static RegionProgress FromProgressUpdate(
        UpdateRegionProgress progressUpdate,
        Guid userId,
        short totalPeaks
    ) {
        var newRegionProgress = RegionProgress.Create(userId, progressUpdate.RegionId, totalPeaks);
        newRegionProgress.AddPeakVisits(progressUpdate.PeaksIds);
        return newRegionProgress;
    }
}
