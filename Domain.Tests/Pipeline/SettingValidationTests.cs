using Domain.AppSettings.Interfaces;
using Application.Trips.Analytics.Pipeline;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.Tests.Pipeline;

// Settings are validated when they are written, so an invalid value never reaches the database.
public class SettingValidationTests
{
    [Fact]
    public void GpxDataProcessing_SeededValues_AreValid()
    {
        IAppSetting setting = new GpxDataProcessingSetting(8, 0.88f, 6, 1);

        Assert.Empty(setting.Validate());
    }

    [Theory]
    [InlineData(0f, 0.88f, 6, 1)]
    [InlineData(-1f, 0.88f, 6, 1)]
    [InlineData(8f, 0f, 6, 1)]
    [InlineData(8f, 1.5f, 6, 1)]
    [InlineData(8f, -0.1f, 6, 1)]
    [InlineData(8f, 0.88f, 0, 1)]
    [InlineData(8f, 0.88f, 6, -1)]
    [InlineData(8f, 0.88f, 6, 16)]
    public void GpxDataProcessing_OutOfRangeValue_ReportsAnError(float spike, float alpha, int window, int decimals)
    {
        IAppSetting setting = new GpxDataProcessingSetting(spike, alpha, window, decimals);

        Assert.NotEmpty(setting.Validate());
    }

    [Fact]
    public void GpxDataProcessing_AlphaOfOne_IsValid()
    {
        IAppSetting setting = new GpxDataProcessingSetting(8, 1f, 6, 1);

        Assert.Empty(setting.Validate());
    }

    [Fact]
    public void GpxDataProcessing_EveryBadField_IsReported()
    {
        IAppSetting setting = new GpxDataProcessingSetting(0, 0, 0, -1);

        Assert.Equal(4, setting.Validate().Count());
    }

    [Fact]
    public void AppSetting_SettingAnInvalidValue_IsRejectedAndKeepsTheOldValue()
    {
        var stored = AppSetting.Create(new GpxDataProcessingSetting(8, 0.88f, 6, 1));
        var before = stored.JsonValue!.RootElement.GetRawText();

        var result = stored.SetSetting(new GpxDataProcessingSetting(8, 1.5f, 6, 1));

        Assert.False(result.IsSuccess);
        Assert.Equal(before, stored.JsonValue!.RootElement.GetRawText());
    }

    [Fact]
    public void AppSetting_SettingAnInvalidValueByType_IsRejected()
    {
        var stored = AppSetting.Create(new GpxDataProcessingSetting(8, 0.88f, 6, 1));

        var result = stored.SetSetting(new GpxDataProcessingSetting(8, 0f, 6, 1), typeof(GpxDataProcessingSetting));

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void PeakMatching_SeededValues_AreValid()
    {
        IAppSetting setting = new PeakMatchingSetting(100, 50, 50);

        Assert.Empty(setting.Validate());
    }

    [Theory]
    [InlineData(0f, 50f, 50f)]
    [InlineData(100f, -1f, 50f)]
    [InlineData(100f, 50f, -1f)]
    public void PeakMatching_NonPositiveValue_ReportsAnError(float radius, float distance, float elevation)
    {
        IAppSetting setting = new PeakMatchingSetting(radius, distance, elevation);

        Assert.NotEmpty(setting.Validate());
    }

    [Fact]
    public void PeakMatching_AsPipelineFilters_UseTheSettingValues()
    {
        var setting = new PeakMatchingSetting(120, 30, 40);

        var filters = PipelineDefaults.PeakFiltersFrom(setting);

        Assert.Equal(2, filters.Count);
        Assert.Equal(30, ((MergeNearbyByDistanceFilter.Config)filters[0]).Value.DistanceM);
        Assert.Equal(40, ((MergeNearbyByElevationFilter.Config)filters[1]).Value.ElevationDeltaM);
    }
}
