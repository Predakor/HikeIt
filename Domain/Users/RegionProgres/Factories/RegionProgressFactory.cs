using Domain.Users.RegionProgresses.ValueObjects;

namespace Domain.Users.RegionProgres.Factories;

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
