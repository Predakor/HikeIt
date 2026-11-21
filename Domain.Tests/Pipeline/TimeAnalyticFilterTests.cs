using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Time.Filters;

namespace Domain.Tests.Pipeline;

public class TimeAnalyticFilterTests
{
    // 10 m in 10 s is 3.6 km/h, 0.1 m in 10 s is 0.036 km/h.
    private static GpxGainWithTime Moving(float slope) => new(10, slope / 10, slope, 10);
    private static GpxGainWithTime Standing(float slope) => new(0.1f, slope / 10, slope, 10);

    private static readonly RouteAnalytic EmptyRoute = new()
    {
        TotalDistanceMeters = 0,
        TotalAscentMeters = 0,
        TotalDescentMeters = 0,
        HighestElevationMeters = 0,
        LowestElevationMeters = 0,
        AverageSlopePercent = 0,
        AverageAscentSlopePercent = 0,
        AverageDescentSlopePercent = 0,
    };

    private static IPaceFilterConfig[] Pipe(params IPaceFilterConfig[] configs) => configs;

    private static readonly DateTime Start = new(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc);

    private static async Task<TimeAnalyticContext> Run(IEnumerable<GpxGainWithTime> gains, IPaceFilterConfig[] pipe)
    {
        var gainList = gains.ToList();
        var context = new TimeAnalyticContext(EmptyRoute, gainList, Start, Start.AddSeconds(gainList.Sum(gain => gain.TimeDelta)));

        foreach (var config in pipe)
        {
            await config.CreateFilter().ApplyAsync(context);
        }

        return context;
    }

    private static async Task<SegmentClass> Classify(GpxGainWithTime gain, IPaceFilterConfig[] pipe)
    {
        var context = await Run([gain], pipe);

        return context.Classes[0];
    }

    [Fact]
    public async Task IdleFilterFirst_StandingOnASlope_IsIdleNotClimbing()
    {
        var pipe = Pipe(new IdleFilter.Config(0.5), new GradeFilter.Config(2));

        Assert.Equal(SegmentClass.Idle, await Classify(Standing(8), pipe));
    }

    [Fact]
    public async Task IdleFilterLast_StandingOnASlope_CountsAsClimbing()
    {
        var pipe = Pipe(new GradeFilter.Config(2), new IdleFilter.Config(0.5));

        Assert.Equal(SegmentClass.Ascent, await Classify(Standing(8), pipe));
    }

    [Fact]
    public async Task GradeFiltersPerDirection_UseTheirOwnThresholds()
    {
        var pipe = Pipe(
            new GradeFilter.Config(2, GradeTargets.Ascent),
            new GradeFilter.Config(4, GradeTargets.Descent)
        );

        Assert.Equal(SegmentClass.Ascent, await Classify(Moving(3), pipe));
        Assert.Equal(SegmentClass.Flat, await Classify(Moving(-3), pipe));
        Assert.Equal(SegmentClass.Descent, await Classify(Moving(-5), pipe));
    }

    [Fact]
    public async Task Filters_IdleAscentDescentAndFlatTimesAddUpToTheDuration()
    {
        var gains = new List<GpxGainWithTime> { Moving(5), Moving(-5), Standing(5), Moving(0) };
        var context = await Run(gains, [.. PipelineDefaults.PaceFilters]);

        var analytic = context.ToAnalytic();

        Assert.Equal(TimeSpan.FromSeconds(10), analytic.IdleTime);
        Assert.Equal(TimeSpan.FromSeconds(10), analytic.AscentTime);
        Assert.Equal(TimeSpan.FromSeconds(10), analytic.DescentTime);
        Assert.Equal(TimeSpan.FromSeconds(30), analytic.ActiveTime);
    }

    [Fact]
    public async Task SpeedSegment_FindsTheFastestAndSlowestStretchOverTheWindow()
    {
        // 50 m in 10 s is 18 km/h, 50 m in 50 s is 3.6 km/h.
        var gains = new List<GpxGainWithTime>
        {
            new(50, 0, 0, 10), new(50, 0, 0, 10), new(50, 0, 0, 50), new(50, 0, 0, 50),
        };

        var context = await Run(gains, Pipe(new SpeedSegmentFilter.Config(100)));

        Assert.Equal(18, context.FastestSpeedKph!.Value, 3);
        Assert.Equal(3.6, context.SlowestSpeedKph!.Value, 3);
    }
}
