using Domain.Common;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Factories;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class GpxPipelineTests {
    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateMaxAndMinElevation(AnalyticData data) {
        var analytics = CreateBuilder(data).WithHighestPoint().WithLowestPoint().Build();

        Assert.Equal(data.Points.Max(p => p.Ele), analytics.HighestElevation);
        Assert.Equal(data.Points.Min(p => p.Ele), analytics.LowestElevation);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateTotalAscentAndDescent(AnalyticData data) {
        var analytics = CreateBuilder(data).WithTotalAscent().WithTotalDescent().Build();

        // Just assert ascent >= 0 and descent <= 0 (descent is negative sum)
        Assert.True(analytics.TotalAscent >= 0);
        Assert.True(analytics.TotalDescent <= 0);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Builder_WithLowest_AndHighestPoints_Should_ContainThem(AnalyticData data) {
        var analytics = CreateBuilder(data).WithHighestPoint().WithLowestPoint().Build();

        Assert.NotEqual(default, analytics.HighestElevation);
        Assert.NotEqual(default, analytics.LowestElevation);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Builder_With_AverageSlopes_Should_ContainThem(AnalyticData data) {
        var analytics = CreateBuilder(data)
            .WithAverageSlope()
            .WithAverageAscentSlope()
            .WithAverageDescentSlope()
            .Build();

        Assert.NotEqual(default, analytics.AverageSlope);
        Assert.NotEqual(default, analytics.AverageAscentSlope);
        Assert.NotEqual(default, analytics.AverageDescentSlope);
    }

    [Fact]
    public async Task Pipeline_Should_Genereta_TimeAnalytics_For_FileWithTimespams() {
        AnalyticData data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var points = GpxDataFactory.Create(data).Points;
        var routeAnalytics = new RouteAnalyticsBuilder(points, points.ToGains()).Build();

        var analytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytics, points));

        Assert.NotNull(analytics);
    }

    static RouteAnalyticsBuilder CreateBuilder(AnalyticData data) {
        return new RouteAnalyticsBuilder(data.Points, data.Points.ToGains());
    }
}
