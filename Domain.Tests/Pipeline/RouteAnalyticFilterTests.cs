using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route.Filters;

namespace Domain.Tests.Pipeline;

public class RouteAnalyticFilterTests
{
    private static GpxGain Gain(float distance, float elevation) =>
        new(
            distance,
            elevation,
            distance == 0
                ? 0
                : elevation / distance * 100
        );

    private static async Task<RouteAnalyticContext> Run(ITerrainFilterConfig config, params GpxGain[] gains)
    {
        var context = new RouteAnalyticContext([], gains);

        return await config
            .CreateFilter()
            .ApplyAsync(context);
    }

    [Fact]
    public async Task SteepestSegment_FindsTheSteepestClimbAndDescentOverTheWindow()
    {
        var context = await Run(
            new SteepestSegmentFilter.Config(100),
            Gain(50, 0),
            Gain(50, 10),
            Gain(50, 10),
            Gain(50, 0),
            Gain(50, -5),
            Gain(50, -5)
        );

        Assert.Equal(20, context.SteepestClimbPercent!.Value, 3);
        Assert.Equal(-10, context.SteepestDescentPercent!.Value, 3);
    }

    [Fact]
    public async Task SteepestSegment_TrackShorterThanTheWindow_HasNoSegments()
    {
        var context = await Run(new SteepestSegmentFilter.Config(500), Gain(50, 10), Gain(50, 10));

        Assert.Null(context.SteepestClimbPercent);
        Assert.Null(context.SteepestDescentPercent);
    }

    [Fact]
    public async Task ElevationGain_IgnoresWobbleSmallerThanTheThreshold()
    {
        var wobble = Enumerable
            .Range(0, 10)
            .Select(i =>
                Gain(
                    10,
                    i % 2 == 0
                        ? 0.3f
                        : -0.3f
                )
            )
            .ToArray();

        var context = await Run(new ElevationGainFilter.Config(1), wobble);

        Assert.Equal(0, context.TotalAscentMeters);
        Assert.Equal(0, context.TotalDescentMeters);
    }

    [Fact]
    public async Task ElevationGain_CountsRealClimbAndDescent()
    {
        var context = await Run(
            new ElevationGainFilter.Config(1),
            Gain(10, 1.5f),
            Gain(10, 1.5f),
            Gain(10, -2f),
            Gain(10, -2f)
        );

        Assert.Equal(3, context.TotalAscentMeters, 3);
        Assert.Equal(4, context.TotalDescentMeters, 3);
    }

    [Fact]
    public void PlainTotals_CountEveryRiseAndFall()
    {
        var wobble = Enumerable
            .Range(0, 10)
            .Select(i =>
                Gain(
                    10,
                    i % 2 == 0
                        ? 0.3f
                        : -0.3f
                )
            )
            .ToArray();

        var context = new RouteAnalyticContext([], wobble);

        Assert.Equal(1.5, context.TotalAscentMeters, 3);
        Assert.Equal(1.5, context.TotalDescentMeters, 3);
    }

    [Fact]
    public async Task LongestClimb_PicksTheLongestSegmentByDistanceNotTheSteepest()
    {
        var context = await Run(
            new LongestClimbFilter.Config(1),
            Gain(100, 5),
            Gain(100, 5),
            Gain(100, 5),
            Gain(50, -10),
            Gain(20, 8)
        );

        Assert.Equal(300, context.LongestClimbDistanceM!.Value, 3);
        Assert.Equal(5, context.LongestClimbPercent!.Value, 3);
        Assert.Equal(50, context.LongestDescentDistanceM!.Value, 3);
        Assert.Equal(-20, context.LongestDescentPercent!.Value, 3);
    }

    [Fact]
    public async Task LongestClimb_ADipBelowTheThresholdDoesNotSplitTheClimb()
    {
        var context = await Run(new LongestClimbFilter.Config(2), Gain(100, 5), Gain(20, -1), Gain(100, 5));

        Assert.Equal(220, context.LongestClimbDistanceM!.Value, 3);
    }

    [Fact]
    public async Task LongestClimb_ASmallDipBeforeTheFirstClimbIsNotPartOfIt()
    {
        var context = await Run(new LongestClimbFilter.Config(3), Gain(50, -2), Gain(100, 10));

        Assert.Equal(100, context.LongestClimbDistanceM!.Value, 3);
        Assert.Equal(10, context.LongestClimbPercent!.Value, 3);
    }

    [Fact]
    public void AverageSlope_FlatTrack_DoesNotFailOnMissingClimbOrDescent()
    {
        var points = new List<GpxPoint> { new(50, 19.00, 500), new(50, 19.01, 500), new(50, 19.02, 500) };
        var context = new RouteAnalyticContext(points, points.ToGains());

        Assert.Equal(0, context.AverageAscentSlopePercent);
        Assert.Equal(0, context.AverageDescentSlopePercent);
    }
}
