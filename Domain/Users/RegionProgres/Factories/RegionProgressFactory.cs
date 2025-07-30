using Domain.Users.RegionProgres;
using Domain.Users.RegionProgresses.ValueObjects;

namespace Domain.Users.RegionProgresses.Factories;

internal static class RegionProgressFactory {
    public static RegionProgress FromProgressUpdate(
        UpdateRegionProgress progressUpdate,
        Guid userId
    ) {
        var newRegionProgress = RegionProgress.Create(userId, progressUpdate.RegionId);
        newRegionProgress.AddPeakVisits(progressUpdate.PeaksIds);
        return newRegionProgress;
    }
}
