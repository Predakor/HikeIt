using Application.AppSettings;
using Application.AppSettings.DTOs;
using Application.Trips.Analytics.Commands;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Stages;
using Core.Results;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using System.Text.Json;

namespace Domain.Tests.Pipeline;

// The cleanup values come from the GpxDataProcessing setting when it is available, the code constants otherwise.
public class CleanupSettingsTests
{
    private static readonly GpxDataProcessingSetting SeededSetting = SettingWith();

    [Fact]
    public async Task Command_WithoutSettings_MatchesGpxDataDirector()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var result = ProccesGpxDataCommand.Run(data, null);

        Assert.Equal(Elevations(GpxDataDirector.AnalyticData(data.Points)), Elevations(result.Value!));
    }

    [Fact]
    public async Task Command_WithEmaAlpha03_DiffersFromDefault()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var emaSetting = SettingWith(ema: 0.3);

        var defaults = ProccesGpxDataCommand.Run(data, null);
        var withSettings = ProccesGpxDataCommand.Run(data, emaSetting);

        Assert.NotEqual(Elevations(defaults.Value!), Elevations(withSettings.Value!));
    }

    [Fact]
    public async Task CleanupStage_SettingsFromService_ChangeTheDefaultFilters()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var medianSetting = SettingWith(medianWindow: 2);

        var actual = await CleanupElevations(new FakeAppSettingsService(medianSetting), data.Points);

        Assert.NotEqual(Elevations(GpxDataDirector.AnalyticData(data.Points)), actual);
    }

    [Fact]
    public async Task CleanupStage_FailedSettingLoad_FallsBackToCodeConstants()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var actual = await CleanupElevations(new FakeAppSettingsService(null), data.Points);

        Assert.Equal(Elevations(GpxDataDirector.AnalyticData(data.Points)), actual);
    }

    [Fact]
    public async Task CleanupStage_StaleSettingWithNullFilters_FallsBackToCodeConstants()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var staleSetting = new GpxDataProcessingSetting(null!);

        var actual = await CleanupElevations(new FakeAppSettingsService(staleSetting), data.Points);
        var discriminator = await DiscriminatorOf(staleSetting);

        Assert.Equal(Elevations(GpxDataDirector.AnalyticData(data.Points)), actual);
        Assert.Equal(await DiscriminatorOf(SettingWith()), discriminator);
    }

    [Fact]
    public async Task CacheDiscriminator_DiffersBetweenSettings_AndIsStableForTheSameSetting()
    {
        var seeded = await DiscriminatorOf(SeededSetting);
        var changed = await DiscriminatorOf(SettingWith(ema: 0.3));
        var seededAgain = await DiscriminatorOf(SettingWith());

        Assert.NotEqual(seeded, changed);
        Assert.Equal(seeded, seededAgain);
    }

    private static GpxDataProcessingSetting SettingWith(double maxSpike = 8, int medianWindow = 6, double ema = 0.88, int rounding = 1)
    {
        IReadOnlyList<IPointFilterConfig> filters =
        [
            new MaxSpikeFilter.Config(maxSpike) with { Value = new MaxSpikeFilter.Config(maxSpike) },
            new MedianSmoothingFilter.Config(medianWindow) with { Value = new MedianSmoothingFilter.Config(medianWindow) },
            new EmaSmoothingFilter.Config(ema) with { Value = new EmaSmoothingFilter.Config(ema) },
            new RoundingPrecisionFilter.Config(rounding) with { Value = new RoundingPrecisionFilter.Config(rounding) },
        ];

        return new GpxDataProcessingSetting(filters);
    }

    private static async Task<string> DiscriminatorOf(GpxDataProcessingSetting setting)
    {
        return await new CleanupStage(new FakeAppSettingsService(setting)).CacheDiscriminatorAsync(default);
    }

    private static async Task<List<double>> CleanupElevations(IAppSettingsService service, List<GpxPoint> points)
    {
        var context = new StageContext(new PipelineInput("test", points), new Dictionary<StageId, StageOutput>());
        var output = await new CleanupStage(service).RunAsync(context, new PipelineConfig(), default);

        return Elevations(((AnalyticData)output.Internal!));
    }

    private static List<double> Elevations(AnalyticData data) => data.Points.Select(p => p.Ele).ToList();

    // Returns the setting when one is given, otherwise a failed result (the row is missing or unreadable).
    private sealed class FakeAppSettingsService(GpxDataProcessingSetting? setting) : IAppSettingsService
    {

        public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct) => throw new NotImplementedException();

        public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct) =>
            throw new NotImplementedException();

        public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting value, CancellationToken ct)
            where TSetting : IAppSetting => throw new NotImplementedException();

        public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct) => throw new NotImplementedException();
    }
}