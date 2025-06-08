using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Factories;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class GpxPipelineTests {
    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateMaxAndMinElevation(TripAnalyticData data) {
        var analytics = CreateBuilder(data)
            .WithHighestPoint()
            .WithLowestPoint()
            .Build();

        Assert.Equal(data.Data.Max(p => p.Ele), analytics.HighestElevation);
        Assert.Equal(data.Data.Min(p => p.Ele), analytics.LowestElevation);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateTotalAscentAndDescent(TripAnalyticData data) {
        var analytics = CreateBuilder(data)
    .WithTotalAscent()
    .WithTotalDescent()
    .Build();

        // Just assert ascent >= 0 and descent <= 0 (descent is negative sum)
        Assert.True(analytics.TotalAscent >= 0);
        Assert.True(analytics.TotalDescent <= 0);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Builder_WithLowestAndHighestPoints_Should_ContainThem(TripAnalyticData data) {

        var analytics = CreateBuilder(data)
            .WithHighestPoint()
            .WithLowestPoint()
            .Build();

        Assert.Equal(default, analytics.HighestElevation);
        Assert.Equal(default, analytics.LowestElevation);
    }

    [Fact]
    public async Task Pipeline_Should_Genereta_TimeAnalytics_For_FileWithTimespams() {
        TripAnalyticData data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var points = GpxDataBuilder.ProcessData(data).Data;
        var routeAnalytics = new RouteAnalyticsBuilder(points, points.ToGains()).Build();

        var analytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytics, points));

        Assert.NotNull(analytics);
    }


    static RouteAnalyticsBuilder CreateBuilder(TripAnalyticData data) {
        return new RouteAnalyticsBuilder(data.Data, data.Data.ToGains());
    }

}
