using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Tests;

public class FindLocalMaximasTests {
    [Fact]
    public void Should_Find_One_Local_Maxima() {
        // peak at 110
        var points = GeneratePointsFromElevation(100, 105, 110, 108, 107, 106, 100);
        var maximas = GetValue(points);

        Assert.Equal(110, maximas[0].Ele);
    }

    [Fact]
    public void Should_Find_Multiple_Local_Maxima() {
        // peaks at 110 and 120
        var points = GeneratePointsFromElevation(100, 110, 100, 120, 100);
        var maximas = GetValue(points);

        Assert.Equal(2, maximas.Count);
        Assert.Equal(110, maximas[0].Ele);
        Assert.Equal(120, maximas[1].Ele);
    }

    [Fact]
    public void Should_Return_OnlyMaximas() {
        var points = GeneratePointsFromElevation(100, 110, 108, 105, 100, 110, 115, 100);
        var maximas = GetValue(points);

        Assert.Equal(2, maximas.Count);
        Assert.Equal(110, maximas[0].Ele);
        Assert.Equal(115, maximas[1].Ele);
    }

    [Fact]
    public void Should_Return_Empty_If_Always_Ascending() {
        var points = GeneratePointsFromElevation(100, 110, 120, 130, 140);
        var maximas = GetValue(points);

        Assert.Empty(maximas);
    }

    [Fact]
    public void Should_Return_Empty_If_Flat() {
        //hmm shouldn't it return a highest maximas still?
        var points = GeneratePointsFromElevation(100, 100, 100, 100);

        var maximas = GetValue(points);

        Assert.Empty(maximas);
    }

    static List<GpxPoint> GetValue(List<GpxPoint> points) {
        return new AnalyticData(points, points.ToGains()).ToLocalMaxima();
    }

    static List<GpxPoint> GeneratePointsFromElevation(params double[] elevations) {
        return elevations
            .Select(
                (e, i) => {
                    return new GpxPoint(
                        50 + i * 0.001,
                        20 + i * 0.001,
                        e,
                        DateTime.UtcNow.AddMinutes(i)
                    );
                }
            )
            .ToList();
    }
}
