using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route.Builders;
using Domain.Trips.Analytics.Time;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests;

public class GpxPipelineTests {
    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateMaxAndMinElevation(AnalyticData data) {
        var analytics = CreateBuilder(data).WithHighestPoint().WithLowestPoint().Build();

        Assert.Equal(data.Points.Max(p => p.Ele), analytics.HighestElevationMeters);
        Assert.Equal(data.Points.Min(p => p.Ele), analytics.LowestElevationMeters);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateTotalAscentAndDescent(AnalyticData data) {
        var analytics = CreateBuilder(data).WithTotalAscent().WithTotalDescent().Build();

        // Just assert ascent >= 0 and descent <= 0 (descent is negative sum)
        Assert.True(analytics.TotalAscentMeters >= 0);

        //Descent is a Math.Abs value 
        Assert.True(analytics.TotalDescentMeters >= 0);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Builder_WithLowest_AndHighestPoints_Should_ContainThem(AnalyticData data) {
        var analytics = CreateBuilder(data).WithHighestPoint().WithLowestPoint().Build();

        Assert.NotEqual(default, analytics.HighestElevationMeters);
        Assert.NotEqual(default, analytics.LowestElevationMeters);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Builder_With_AverageSlopes_Should_ContainThem(AnalyticData data) {
        var analytics = CreateBuilder(data)
            .WithAverageSlope()
            .WithAverageAscentSlope()
            .WithAverageDescentSlope()
            .Build();

        Assert.NotEqual(default, analytics.AverageSlopePercent);
        Assert.NotEqual(default, analytics.AverageAscentSlopePercent);
        Assert.NotEqual(default, analytics.AverageDescentSlopePercent);
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
