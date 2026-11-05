using System.Text.Json;
using Application.AppSettings;
using Application.AppSettings.DTOs;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Core.Results;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared.Filters;

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
        Assert.Equal(["PinLocalMaximaFilter", "SimplifyFilter"], defaults.ElevationProfile.Select(f => f.FilterType));
        Assert.Equal(true, defaults.ElevationProfile[0].Value["Enabled"]);
        Assert.Equal(3d, defaults.ElevationProfile[1].Value["ToleranceM"]);
        Assert.Equal(["PinLocalMaximaFilter", "SimplifyFilter"], defaults.Route.Select(f => f.FilterType));
        Assert.Equal(100f, defaults.PeakProximityRadiusM);
        Assert.Equal(["MergeNearbyByDistanceFilter", "MergeNearbyByElevationFilter"], defaults.Peaks.Select(f => f.FilterType));
    }

    [Fact]
    public async Task WithSettings_ReturnsTheDatabaseValues()
    {
        var service = new FakeSettings(
            new GpxDataProcessingSetting(PointFilters(maxSpike: 5, windowSize: 3, alpha: 0.5)),
            new PeakMatchingSetting(150, PeakFilters(distance: 30, elevation: 40)),
            new ElevationProfileSetting(PinAndSimplify(enabled: false, tolerance: 10)),
            new RouteSetting(PinAndSimplify(enabled: true, tolerance: 20))
        );

        var defaults = await PipelineSettings.DefaultsAsync(service, default);

        Assert.Equal(5d, defaults.Cleanup[0].Value["MaxSpike"]);
        Assert.Equal(3, defaults.Cleanup[1].Value["WindowSize"]);
        Assert.Equal(0.5d, defaults.Cleanup[2].Value["Alpha"]);
        Assert.Equal(150f, defaults.PeakProximityRadiusM);
        Assert.Equal(30d, defaults.Peaks[0].Value["DistanceM"]);
        Assert.Equal(40d, defaults.Peaks[1].Value["ElevationDeltaM"]);
        Assert.Equal(false, defaults.ElevationProfile[0].Value["Enabled"]);
        Assert.Equal(10d, defaults.ElevationProfile[1].Value["ToleranceM"]);
        Assert.Equal(true, defaults.Route[0].Value["Enabled"]);
        Assert.Equal(20d, defaults.Route[1].Value["ToleranceM"]);
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

    static IReadOnlyList<IPointFilterConfig> PointFilters(double maxSpike, int windowSize, double alpha, int roundingDecimals = 1) =>
    [
        new MaxSpikeFilter.Config(maxSpike) with { Value = new MaxSpikeFilter.Config(maxSpike) },
        new MedianSmoothingFilter.Config(windowSize) with { Value = new MedianSmoothingFilter.Config(windowSize) },
        new EmaSmoothingFilter.Config(alpha) with { Value = new EmaSmoothingFilter.Config(alpha) },
        new RoundingPrecisionFilter.Config(roundingDecimals) with { Value = new RoundingPrecisionFilter.Config(roundingDecimals) },
    ];

    static IReadOnlyList<IPeakFilterConfig> PeakFilters(double distance, double elevation) =>
    [
        new MergeNearbyByDistanceFilter.Config(distance) with { Value = new MergeNearbyByDistanceFilter.Config(distance) },
        new MergeNearbyByElevationFilter.Config(elevation) with { Value = new MergeNearbyByElevationFilter.Config(elevation) },
    ];

    static IReadOnlyList<IPointFilterConfig> PinAndSimplify(bool enabled, double tolerance) =>
    [
        new PinLocalMaximaFilter.Config(enabled) with { Value = new PinLocalMaximaFilter.Config(enabled) },
        new SimplifyFilter.Config(tolerance) with { Value = new SimplifyFilter.Config(tolerance) },
    ];

    sealed class FakeSettings(
        GpxDataProcessingSetting cleanup,
        PeakMatchingSetting peaks,
        ElevationProfileSetting? elevationProfile = null,
        RouteSetting? route = null
    ) : IAppSettingsService
    {
        public Task<Result<TSetting>> GetSettingAsync<TSetting>(AppSettingType settingType, CancellationToken ct)
            where TSetting : IAppSetting
        {
            object? setting = settingType switch
            {
                AppSettingType.GpxDataProcessing => cleanup,
                AppSettingType.PeakMatching => peaks,
                AppSettingType.ElevationProfile => elevationProfile,
                AppSettingType.Route => route,
                _ => null,
            };

            return setting is null
                ? Task.FromResult(Result<TSetting>.Failure(Errors.NotFound("setting")))
                : Task.FromResult(Result<TSetting>.Success((TSetting)setting));
        }

        public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct) => throw new NotImplementedException();

        public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct) =>
            throw new NotImplementedException();

        public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting value, CancellationToken ct)
            where TSetting : IAppSetting => throw new NotImplementedException();

        public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct) => throw new NotImplementedException();
    }
}
