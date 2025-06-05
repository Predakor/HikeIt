using Domain.Trips.ValueObjects;
using Infrastructure.Parsers;

namespace Domain.Tests;

public class ParserTests {
    [Fact]
    public async Task Parser_ShouldThrow_When_NoFile() {
        var invalidPath = "totally incorrect path trust me";

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(async () => {
            await ParseFromGpxFile(invalidPath);
        });
    }

    [Fact]
    public async Task Parser_Should_Return_ParsedFile() {
        GpxAnalyticData data = await ParseFromGpxFile("data/trip_small.gpx");

        Assert.NotNull(data);
    }

    [Fact]
    public async Task ParsedFiles_ShouldContain_AllGpxParams() {
        GpxAnalyticData data = await ParseFromGpxFile("data/trip_small.gpx");

        Assert.Contains(data.Data, e => e.Lat != 0);
        Assert.Contains(data.Data, e => e.Lon != 0);
        Assert.Contains(data.Data, e => e.Ele != 0);
    }

    [Fact]
    public async Task ParsedFile_WithTimes_ShouldContain_GpxParamWithTime() {
        GpxAnalyticData data = await ParseFromGpxFile("data/trip_small.gpx");

        Assert.Contains(data.Data, e => e.Time != null);
    }

    public static async Task<GpxAnalyticData> ParseFromGpxFile(string relativePath) {
        var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);
        var gpxData = await new GpxParser().ParseAsync(File.OpenRead(fullPath));
        return gpxData;
    }
}
