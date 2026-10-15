using System.Text.Json;
using Application.AppSettings;
using Application.AppSettings.DTOs;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Core.Results;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;

namespace Domain.Tests.Pipeline;

// The defaults endpoint returns what the stages resolve for a null config: DB settings when present, code constants otherwise.
public class PipelineDefaultsEndpointTests
{
    static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task WithoutSettings_ReturnsTheCodeConstants()
    {
        var defaults = await PipelineSettings.DefaultsAsync(null, default);

        Assert.Equal(["MaxSpikeFilter", "MedianSmoothingFilter", "EmaSmoothingFilter", "RoundingPrecisionFilter"],
            defaults.Cleanup.Select(f => f.FilterType));
        Assert.Equal(8d, defaults.Cleanup[0].Value["MaxSpike"]);
        Assert.Empty(defaults.ElevationProfile);
        Assert.Empty(defaults.Route);
        Assert.Equal(100f, defaults.PeakProximityRadiusM);
        Assert.Equal(["MergeNearbyByDistanceFilter", "MergeNearbyByElevationFilter"], defaults.Peaks.Select(f => f.FilterType));
    }

    [Fact]
    public async Task WithSettings_ReturnsTheDatabaseValues()
    {
        var service = new FakeSettings(
            new GpxDataProcessingSetting(5, 0.5f, 3, 2),
            new PeakMatchingSetting(150, 30, 40)
        );

        var defaults = await PipelineSettings.DefaultsAsync(service, default);

        Assert.Equal(5d, defaults.Cleanup[0].Value["MaxSpike"]);
        Assert.Equal(3, defaults.Cleanup[1].Value["WindowSize"]);
        Assert.Equal(0.5d, defaults.Cleanup[2].Value["Alpha"]);
        Assert.Equal(150f, defaults.PeakProximityRadiusM);
        Assert.Equal(30d, defaults.Peaks[0].Value["DistanceM"]);
        Assert.Equal(40d, defaults.Peaks[1].Value["ElevationDeltaM"]);
    }

    [Fact]
    public async Task Serialised_UsesTheWireShapeOfTheRequest()
    {
        var defaults = await PipelineSettings.DefaultsAsync(null, default);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(defaults, Json));
        var first = document.RootElement.GetProperty("cleanup")[0];

        Assert.Equal("MaxSpikeFilter", first.GetProperty("FilterType").GetString());
        Assert.Equal(8, first.GetProperty("value").GetProperty("MaxSpike").GetDouble());
    }

    sealed class FakeSettings(GpxDataProcessingSetting cleanup, PeakMatchingSetting peaks) : IAppSettingsService
    {
        public Task<Result<TSetting>> GetSettingAsync<TSetting>(AppSettingType settingType, CancellationToken ct)
            where TSetting : IAppSetting
        {
            object setting = settingType == AppSettingType.GpxDataProcessing ? cleanup : peaks;
            return Task.FromResult(Result<TSetting>.Success((TSetting)setting));
        }

        public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct) => throw new NotImplementedException();

        public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct) =>
            throw new NotImplementedException();

        public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting value, CancellationToken ct)
            where TSetting : IAppSetting => throw new NotImplementedException();

        public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct) => throw new NotImplementedException();
    }
}
