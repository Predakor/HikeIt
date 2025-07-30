using Domain.Users.RegionProgres;

namespace Domain.Tests.User;

public class UserRegionProgressTests {
    const int MockRegionId = 1;

    static readonly Guid MockupGuid = new();
    static RegionProgress MockProggress => RegionProgress.Create(MockupGuid, MockRegionId);


    [Fact]
    public void Create_ShouldInitializeWithEmptyVisits() {
        var userId = Guid.NewGuid();

        var progress = RegionProgress.Create(userId, MockRegionId);

        Assert.Equal(userId, progress.UserId);
        Assert.Equal(MockRegionId, progress.RegionId);
        Assert.Empty(progress.PeakVisits);
    }

    [Fact]
    public void AddPeakVisits_ShouldAddNewPeaksWithCountOne() {
        var progress = MockProggress;

        int[] peaks = [1, 2, 3];
        progress.AddPeakVisits(peaks);

        Assert.Equal(3, progress.PeakVisits.Count);
        foreach (var peakId in peaks) {
            Assert.True(progress.PeakVisits.ContainsKey(peakId));
            Assert.Equal(1, progress.PeakVisits[peakId]);
        }
    }

    [Fact]
    public void AddPeakVisits_ShouldIncrementVisitCountForExistingPeaks() {
        var progress = MockProggress;

        progress.AddPeakVisits([1, 2]);
        progress.AddPeakVisits([1, 2, 3]);

        Assert.Equal(2, progress.PeakVisits[1]);
        Assert.Equal(2, progress.PeakVisits[2]);
        Assert.Equal(1, progress.PeakVisits[3]);
    }

    [Fact]
    public void RemovePeakVisits_ShouldDecrementVisitCount() {
        var progress = MockProggress;

        progress.AddPeakVisits([1, 1, 2]);
        progress.RemovePeakVisits([1]);

        Assert.Equal(1, progress.PeakVisits[1]);
        Assert.Equal(1, progress.PeakVisits[2]);
    }

    [Fact]
    public void RemovePeakVisits_ShouldRemovePeakIfVisitCountReachesZero() {
        var progress = MockProggress;

        progress.AddPeakVisits([1]);
        progress.RemovePeakVisits([1]);

        Assert.False(progress.PeakVisits.ContainsKey(1));
    }

    [Fact]
    public void RemovePeakVisits_ShouldIgnorePeaksNotInDictionary() {
        var progress = MockProggress;

        progress.AddPeakVisits([1]);

        // Should not throw
        progress.RemovePeakVisits([999]);

        Assert.True(progress.PeakVisits.ContainsKey(1));
    }
}
