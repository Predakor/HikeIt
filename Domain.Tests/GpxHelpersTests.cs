using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class GpxHelpersTests {
    static readonly List<GpxPoint> data =
    [
        new(16, 18, 100),
        new(16.5, 18, 102),
        new(17, 18, 98, DateTime.UtcNow.AddSeconds(-60)), // with timestamp
        new(17.5, 18, 104, DateTime.UtcNow), // with timestamp
    ];

    static readonly List<Trips.ValueObjects.GpxPoint> gpxFileData =
    [
        new(50.67983628, 16.53870464, 487, DateTime.Parse("2025-05-01T07:05:52Z")),
        new(50.67988247, 16.53873224, 488, DateTime.Parse("2025-05-01T07:05:57Z")),
        new(50.6799229, 16.53874797, 491, DateTime.Parse("2025-05-01T07:06:01Z")),
        new(50.68000702, 16.53875604, 493, DateTime.Parse("2025-05-01T07:06:09Z")),
        new(50.68004266, 16.5387713, 497, DateTime.Parse("2025-05-01T07:06:12Z")),
        new(50.6801281, 16.53874078, 491, DateTime.Parse("2025-05-01T07:06:19Z")),
        new(50.6801649, 16.53873323, 493, DateTime.Parse("2025-05-01T07:06:23Z")),
        new(50.68024035, 16.53874175, 496, DateTime.Parse("2025-05-01T07:06:30Z")),
        new(50.68028378, 16.53874053, 496, DateTime.Parse("2025-05-01T07:06:34Z")),
        new(50.68034968, 16.53871821, 498, DateTime.Parse("2025-05-01T07:06:39Z")),
        new(50.68045247, 16.53874486, 498, DateTime.Parse("2025-05-01T07:06:46Z")),
        new(50.68049591, 16.53873433, 499, DateTime.Parse("2025-05-01T07:06:49Z")),
        new(50.68059608, 16.53867165, 498, DateTime.Parse("2025-05-01T07:06:58Z")),
        new(50.68067846, 16.53865193, 503, DateTime.Parse("2025-05-01T07:07:07Z")),
        new(50.68071969, 16.53863437, 503, DateTime.Parse("2025-05-01T07:07:11Z")),
        new(50.68079192, 16.53857404, 503, DateTime.Parse("2025-05-01T07:07:18Z")),
        new(50.68082525, 16.53852922, 505, DateTime.Parse("2025-05-01T07:07:22Z")),
        new(50.68085604, 16.53849554, 504, DateTime.Parse("2025-05-01T07:07:25Z")),
        new(50.6808905, 16.53846957, 503, DateTime.Parse("2025-05-01T07:07:28Z")),
    ];

    [Fact]
    public void GpxGain_ShouldNot_BeNull() {
        var gain = GpxHelpers.ComputeGain(data[1], data[0]);
        Assert.NotNull(gain);
    }

    [Fact]
    public void GpxGain_ShouldHave_ElevationDelta() {
        var gain = GpxHelpers.ComputeGain(data[1], data[0]);
        Assert.Equal(2, gain.ElevationDelta);
    }

    [Fact]
    public void GpxGain_ShouldHave_Slope() {
        GpxPoint[] ascend =
        [
            new GpxPoint(16.0000, 18.0000, 100),
            new GpxPoint(16.0005, 18.0000, 110),
        ];

        var upGain = GpxHelpers.ComputeGain(ascend[1], ascend[0]);
        var downGain = GpxHelpers.ComputeGain(ascend[0], ascend[1]);

        Assert.NotEqual(default, upGain.Slope);
        Assert.NotEqual(default, downGain.Slope);
    }

    [Fact]
    public void AllGpxGains_ShouldHave_Slope() {
        var gains = gpxFileData.ToGains();
        gains.Select(s => s.Slope).ToList();

        foreach (var ga in gains) {

            Assert.InRange(ga.Slope, -100, 100);

        }
    }

    [Fact]
    public void GpxGain_ShouldNotHave_TimeDelta_When_Timestamps_AreMissing() {
        var gain = GpxHelpers.ComputeGain(data[1], data[0]);
        Assert.Null(gain.TimeDelta);
    }

    [Fact]
    public void GpxGain_ShouldHave_TimeDelta_When_Timestamps_ArePresent() {
        var gain = GpxHelpers.ComputeGain(data[3], data[2]);
        Assert.NotNull(gain.TimeDelta);
        Assert.True(gain.TimeDelta > 0);
    }

    [Fact]
    public void GpxGain_ElevationDelta_ShouldBe_Negative_ForDescent() {
        var gain = GpxHelpers.ComputeGain(data[2], data[1]); // 98 - 102 = -4
        Assert.True(gain.ElevationDelta < 0);
    }

    [Fact]
    public void GpxGain_Slope_ShouldBe_Zero_When_HorizontalDistance_IsZero() {
        var point1 = new GpxPoint(0, 0, 100);
        var point2 = new GpxPoint(0, 0, 110);

        var gain = GpxHelpers.ComputeGain(point2, point1);
        Assert.Equal(0, gain.Slope);
    }

    [Fact]
    public void GpxGain_Should_Handle_SamePoint() {
        var point = new GpxPoint(0, 0, 100);
        var gain = GpxHelpers.ComputeGain(point, point);

        Assert.Equal(0, gain.ElevationDelta);
        Assert.Equal(0, gain.Slope);
    }

    [Fact]
    public void GpxGain_Should_Handle_ZeroElevationChange() {
        var point1 = new GpxPoint(16, 18, 100);
        var point2 = new GpxPoint(16.01, 18, 100);

        var gain = GpxHelpers.ComputeGain(point2, point1);
        Assert.Equal(0, gain.ElevationDelta);
        Assert.Equal(0, gain.Slope);
    }

    [Fact]
    public void GainsMapper_Should_Return_SmallerArrayThanOrignal() {
        var gains = data.ToGains();

        Assert.NotNull(gains);
        Assert.NotEmpty(gains);
        Assert.Equal(data.Count - 1, gains.Count);
    }

    [Fact]
    public void MapToTimed_Should_FilterOut_Points_Without_Time() {
        List<GpxPoint> points =
        [
            new(16, 18, 100),
            new(16.5, 18, 102),
            new(17, 18, 98, DateTime.UtcNow.AddSeconds(-60)), // with timestamp
            new(17.5, 18, 104, DateTime.UtcNow), // with timestamp
        ];

        var pointsWithTime = points.MapToTimed();

        Assert.NotNull(pointsWithTime);
        Assert.NotEmpty(pointsWithTime);
        Assert.Equal(2, pointsWithTime.Count);
    }
}
