using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared.Filters;
using System.Text.Json;

namespace Domain.Tests.Pipeline;

public class SettingValidationTests
{
    [Fact]
    public void GpxDataProcessing_ColdRead_OfTheOrderAppSettingActuallyWrites_Succeeds()
    {
        var coldWriteOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var written = JsonSerializer.Serialize(new GpxDataProcessingSetting(CleanupFilters()), coldWriteOptions);

        var coldReadOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var setting = JsonSerializer.Deserialize<GpxDataProcessingSetting>(written, coldReadOptions);

        Assert.NotNull(setting);
        Assert.Equal(4, setting.Filters.Count);
        Assert.Equal(8, ((MaxSpikeFilter.Config)setting.Filters[0].GetValue()).MaxSpike);
    }

    [Fact]
    public void PeakMatching_SeededValues_AreValid()
    {
        IAppSetting setting = new PeakMatchingSetting(100, PeakFilters());

        Assert.Empty(setting.Validate());
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    public void PeakMatching_NonPositiveRadius_ReportsAnError(float radius)
    {
        IAppSetting setting = new PeakMatchingSetting(radius, PeakFilters());

        Assert.NotEmpty(setting.Validate());
    }

    [Fact]
    public void AppSetting_SettingAnInvalidValue_IsRejectedAndKeepsTheOldValue()
    {
        var stored = AppSetting.Create(new PeakMatchingSetting(100, PeakFilters()));
        var before = stored.JsonValue!.RootElement.GetRawText();

        var result = stored.SetSetting(new PeakMatchingSetting(0, PeakFilters()));

        Assert.False(result.IsSuccess);
        Assert.Equal(before, stored.JsonValue!.RootElement.GetRawText());
    }

    [Fact]
    public void AppSetting_SettingAnInvalidValueByType_IsRejected()
    {
        var stored = AppSetting.Create(new PeakMatchingSetting(100, PeakFilters()));

        var result = stored.SetSetting(new PeakMatchingSetting(0, PeakFilters()), typeof(PeakMatchingSetting));

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void GpxDataProcessing_RoundTripsThroughJson_WithTheSameFilters()
    {
        var stored = AppSetting.Create(new GpxDataProcessingSetting(CleanupFilters()));

        var result = stored.GetSetting<GpxDataProcessingSetting>();

        Assert.True(result.IsSuccess);
        var filters = result.Value!.Filters;
        Assert.Equal(4, filters.Count);
        Assert.Equal(8, ((MaxSpikeFilter.Config)filters[0]).Value.MaxSpike);
        Assert.Equal(6, ((MedianSmoothingFilter.Config)filters[1]).Value.WindowSize);
        Assert.Equal(0.88, ((EmaSmoothingFilter.Config)filters[2]).Value.Alpha);
        Assert.Equal(1, ((RoundingPrecisionFilter.Config)filters[3]).Value.Decimals);
    }

    [Fact]
    public void PeakMatching_RoundTripsThroughJson_WithTheSameFilters()
    {
        var stored = AppSetting.Create(new PeakMatchingSetting(120, PeakFilters(distance: 30, elevation: 40)));

        var result = stored.GetSetting<PeakMatchingSetting>();

        Assert.True(result.IsSuccess);
        Assert.Equal(120, result.Value!.ProximityRadiusM);
        var filters = result.Value.Filters;
        Assert.Equal(2, filters.Count);
        Assert.Equal(30, ((MergeNearbyByDistanceFilter.Config)filters[0]).Value.DistanceM);
        Assert.Equal(40, ((MergeNearbyByElevationFilter.Config)filters[1]).Value.ElevationDeltaM);
    }

    [Fact]
    public void ElevationProfile_RoundTripsThroughJson_WithTheSameFilters()
    {
        var stored = AppSetting.Create(new ElevationProfileSetting(PinAndSimplify(enabled: false, tolerance: 5)));

        var result = stored.GetSetting<ElevationProfileSetting>();

        Assert.True(result.IsSuccess);
        var filters = result.Value!.Filters;
        Assert.Equal(2, filters.Count);
        Assert.False(((PinLocalMaximaFilter.Config)filters[0]).Value.Enabled);
        Assert.Equal(5, ((SimplifyFilter.Config)filters[1]).Value.ToleranceM);
    }

    [Fact]
    public void Route_RoundTripsThroughJson_WithTheSameFilters()
    {
        var stored = AppSetting.Create(new RouteSetting(PinAndSimplify(enabled: true, tolerance: 7)));

        var result = stored.GetSetting<RouteSetting>();

        Assert.True(result.IsSuccess);
        var filters = result.Value!.Filters;
        Assert.Equal(2, filters.Count);
        Assert.True(((PinLocalMaximaFilter.Config)filters[0]).Value.Enabled);
        Assert.Equal(7, ((SimplifyFilter.Config)filters[1]).Value.ToleranceM);
    }

    [Fact]
    public void GpxDataProcessing_DeserialisesFromThePutRequestWireShape()
    {
        const string json = """
            {
              "Filters": [
                { "FilterType": "MaxSpikeFilter", "value": { "MaxSpike": 12 } },
                { "FilterType": "MedianSmoothingFilter", "value": { "WindowSize": 4 } },
                { "FilterType": "EmaSmoothingFilter", "value": { "Alpha": 0.5 } },
                { "FilterType": "RoundingPrecisionFilter", "value": { "Decimals": 2 } }
              ]
            }
            """;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var setting = JsonSerializer.Deserialize<GpxDataProcessingSetting>(json, options)!;

        Assert.Equal(4, setting.Filters.Count);
        Assert.Equal(12, ((MaxSpikeFilter.Config)setting.Filters[0].GetValue()).MaxSpike);
        Assert.Equal(4, ((MedianSmoothingFilter.Config)setting.Filters[1].GetValue()).WindowSize);
        Assert.Equal(0.5, ((EmaSmoothingFilter.Config)setting.Filters[2].GetValue()).Alpha);
        Assert.Equal(2, ((RoundingPrecisionFilter.Config)setting.Filters[3].GetValue()).Decimals);

        var stored = AppSetting.Create(setting);
        var reread = stored.GetSetting<GpxDataProcessingSetting>();
        Assert.True(reread.IsSuccess);
        Assert.Equal(12, ((MaxSpikeFilter.Config)reread.Value!.Filters[0]).Value.MaxSpike);
    }

    private static IReadOnlyList<IPointFilterConfig> CleanupFilters() =>
    [
        new MaxSpikeFilter.Config(8) with { Value = new MaxSpikeFilter.Config(8) },
        new MedianSmoothingFilter.Config(6) with { Value = new MedianSmoothingFilter.Config(6) },
        new EmaSmoothingFilter.Config(0.88) with { Value = new EmaSmoothingFilter.Config(0.88) },
        new RoundingPrecisionFilter.Config(1) with { Value = new RoundingPrecisionFilter.Config(1) },
    ];

    private static IReadOnlyList<IPeakFilterConfig> PeakFilters(double distance = 50, double elevation = 50) =>
    [
        new MergeNearbyByDistanceFilter.Config(distance) with { Value = new MergeNearbyByDistanceFilter.Config(distance) },
        new MergeNearbyByElevationFilter.Config(elevation) with { Value = new MergeNearbyByElevationFilter.Config(elevation) },
    ];

    private static IReadOnlyList<IPointFilterConfig> PinAndSimplify(bool enabled = true, double tolerance = 3) =>
    [
        new PinLocalMaximaFilter.Config(enabled) with { Value = new PinLocalMaximaFilter.Config(enabled) },
        new SimplifyFilter.Config(tolerance) with { Value = new SimplifyFilter.Config(tolerance) },
    ];
}