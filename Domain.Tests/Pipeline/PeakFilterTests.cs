using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.Tests.Pipeline;

public class PeakFilterTests
{
    // 0.0001 degrees of latitude is about 11 m.
    private const double Step = 0.0001;

    [Fact]
    public async Task LocalMaxima_KeepsEveryPointTheTrackRisesIntoAndFallsFrom()
    {
        var candidates = Candidates(100, 120, 110, 130, 90);

        var result = await new LocalMaximaFilter(new LocalMaximaFilter.Config()).ApplyAsync(candidates);

        Assert.Equal([120d, 130d], Elevations(result));
    }

    [Fact]
    public async Task LocalMaxima_MinClimb_DropsPeaksThatRoseTooLittle()
    {
        var candidates = Candidates(100, 120, 110, 130, 90);

        var result = await new LocalMaximaFilter(new LocalMaximaFilter.Config(MinClimbM: 30)).ApplyAsync(candidates);

        Assert.Equal([130d], Elevations(result));
    }

    [Fact]
    public async Task LocalMaxima_MonotoneTrack_HasNoMaxima()
    {
        var result = await new LocalMaximaFilter(new LocalMaximaFilter.Config()).ApplyAsync(Candidates(100, 110, 120, 130));

        Assert.Empty(result);
    }

    [Fact]
    public async Task MergeNearbyByDistance_CollapsesCloseNeighboursToTheHighest()
    {
        var close = Candidates(100, 120, 110);
        var far = new PeakCandidate(new GpxPointWithDistance(new GpxPoint(50.01, 20, 105), 0));

        var result = await new MergeNearbyByDistanceFilter(new MergeNearbyByDistanceFilter.Config(50))
            .ApplyAsync([.. close, far]);

        Assert.Equal([120d, 105d], Elevations(result));
    }

    [Fact]
    public async Task MergeNearbyByDistance_ZeroDistance_KeepsEveryCandidate()
    {
        var result = await new MergeNearbyByDistanceFilter(new MergeNearbyByDistanceFilter.Config(0)).ApplyAsync(Candidates(100, 120, 110));

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task MergeNearbyByElevation_CollapsesNeighboursWithinTheDeltaToTheHighest()
    {
        var result = await new MergeNearbyByElevationFilter(new MergeNearbyByElevationFilter.Config(50))
            .ApplyAsync(Candidates(100, 110, 300));

        Assert.Equal([110d, 300d], Elevations(result));
    }

    [Fact]
    public async Task MatchWithinDistance_KeepsOnlyCandidatesWithAPeakInRange()
    {
        var near = Candidates(100)[0] with { Peak = PeakAt(50, 20) };
        var farPeak = Candidates(100)[0] with { Peak = PeakAt(50.01, 20) };
        var unmatched = Candidates(100)[0];

        var result = await new MatchWithinDistanceFilter(new MatchWithinDistanceFilter.Config(100))
            .ApplyAsync([near, farPeak, unmatched]);

        Assert.Equal([near], result);
    }

    [Fact]
    public async Task PeakLookup_AttachesTheNearestPeakAndKeepsEveryCandidate()
    {
        var closer = PeakAt(50.0001, 20);
        var further = PeakAt(50.002, 20);
        var source = new FakePeakSource([further, closer]);

        var result = await new PeakLookupFilter(new PeakLookupFilter.Config(100), source).ApplyAsync(Candidates(100, 120));

        Assert.Equal(2, result.Count);
        Assert.All(result, candidate => Assert.Same(closer, candidate.Peak));
    }

    [Fact]
    public async Task PeakLookup_NoPeaksFound_LeavesCandidatesUnmatched()
    {
        var result = await new PeakLookupFilter(new PeakLookupFilter.Config(100), new FakePeakSource([])).ApplyAsync(Candidates(100, 120));

        Assert.Equal(2, result.Count);
        Assert.All(result, candidate => Assert.Null(candidate.Peak));
    }

    [Fact]
    public async Task PeakLookup_NoCandidates_DoesNotQueryTheSource()
    {
        var source = new FakePeakSource([]);

        var result = await new PeakLookupFilter(new PeakLookupFilter.Config(100), source).ApplyAsync([]);

        Assert.Empty(result);
        Assert.Equal(0, source.Calls);
    }

    [Fact]
    public async Task PeakLookup_QueriesABoxPaddedByTheOffset()
    {
        var source = new FakePeakSource([]);

        await new PeakLookupFilter(new PeakLookupFilter.Config(500), source).ApplyAsync(Candidates(100, 120));

        var bounds = source.LastBounds!.Value;
        Assert.True(bounds.MinLat < 50 && bounds.MaxLat > 50 + Step);
    }

    private static List<PeakCandidate> Candidates(params double[] elevations) =>
        [.. elevations.Select((ele, i) => new PeakCandidate(new GpxPointWithDistance(new GpxPoint(50 + i * Step, 20, ele), i)))];

    private static List<double> Elevations(IEnumerable<PeakCandidate> candidates) => [.. candidates.Select(c => c.Ele)];

    private static Peak PeakAt(double lat, double lon) => Peak.Create("Peak", new GpxPoint(lat, lon, 1000), 1);

    private sealed class FakePeakSource(List<Peak> peaks) : IPeakSource
    {
        public int Calls { get; private set; }
        public GeoBounds? LastBounds { get; private set; }

        public Task<List<Peak>> GetPeaksInBounds(GeoBounds bounds)
        {
            Calls++;
            LastBounds = bounds;
            return Task.FromResult(peaks);
        }
    }
}
