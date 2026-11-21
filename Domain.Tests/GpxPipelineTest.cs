using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Route.Filters;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Time.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests;

public class GpxPipelineTests {
    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public async Task PlainMetrics_ShouldCalculateElevationRangeAndTotals(AnalyticData data) {
        var analytics = await RunRoute(data);

        Assert.Equal(data.Points.Max(p => p.Ele), analytics.HighestElevationMeters);
        Assert.Equal(data.Points.Min(p => p.Ele), analytics.LowestElevationMeters);
        Assert.True(analytics.TotalAscentMeters >= 0);
        Assert.True(analytics.TotalDescentMeters >= 0);
    }

    [Fact]
    public async Task Pipeline_Should_Genereta_TimeAnalytics_For_FileWithTimespams() {
        AnalyticData data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var points = GpxDataFactory.Create(data).Points;
        var route = await RunRoute(new AnalyticData(points), [.. PipelineDefaults.TerrainFilters]);

        var created = TimeAnalyticContext.Create(route, points);
        Assert.True(created.IsSuccess);

        var context = created.Value!;
        foreach (var config in PipelineDefaults.PaceFilters) {
            await config.CreateFilter().ApplyAsync(context);
        }

        Assert.True(context.ToAnalytic().Duration > TimeSpan.Zero);
    }

    static async Task<RouteAnalytic> RunRoute(AnalyticData data, params ITerrainFilterConfig[] configs) {
        var context = new RouteAnalyticContext(data.Points, data.Points.ToGains());

        foreach (var config in configs) {
            await config.CreateFilter().ApplyAsync(context);
        }

        return context.ToAnalytic();
    }
}
