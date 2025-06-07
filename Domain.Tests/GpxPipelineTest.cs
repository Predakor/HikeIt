using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.Factories;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class GpxPipelineTests {
    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateMaxAndMinElevation(TripAnalyticData data) {
        var analytics = new TripAnalyticBuilder(data).WithHighestPoint().WithLowestPoint().Build();

        Assert.Equal(data.Data.Max(p => p.Ele), analytics.MaxElevation);
        Assert.Equal(data.Data.Min(p => p.Ele), analytics.MinElevation);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldCalculateTotalAscentAndDescent(TripAnalyticData data) {
        var analytics = new TripAnalyticBuilder(data)
            .WithGains()
            .WithTotalAscent()
            .WithTotalDescent()
            .Build();

        // Just assert ascent >= 0 and descent <= 0 (descent is negative sum)
        Assert.True(analytics.TotalAscent >= 0);
        Assert.True(analytics.TotalDescent <= 0);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void Build_ShouldIncludeReachedPeaks(TripAnalyticData data) {
        var analytics = new TripAnalyticBuilder(data).WithGains().WithClimbedPeaks().Build();

        Assert.NotNull(analytics.ReachedPeaks);
        Assert.NotEmpty(analytics.ReachedPeaks);
    }

    [Fact]
    public async Task Pipeline_Should_Genereta_TimeAnalytics_For_FileWithTimespams() {
        TripAnalyticData data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var gpxData = GpxDataBuilder.ProcessData(data);
        var analytics = TripAnalyticFactory.CreateAnalytics(gpxData);

        Assert.NotNull(analytics.TimeAnalytics);
    }
}
