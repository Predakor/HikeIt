using System.Text.Json;
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
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

// The cleanup values come from the GpxDataProcessing setting when it is available, the code constants otherwise.
public class CleanupSettingsTests
{
    static readonly GpxDataProcessingSetting SeededSetting = new(8, 0.88f, 6, 1);

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
        var emaSetting = new GpxDataProcessingSetting(8, 0.3f, 6, 1);

        var defaults = ProccesGpxDataCommand.Run(data, null);
        var withSettings = ProccesGpxDataCommand.Run(data, emaSetting);

        Assert.NotEqual(Elevations(defaults.Value!), Elevations(withSettings.Value!));
    }

    [Fact]
    public async Task CleanupStage_SettingsFromService_ChangeTheDefaultFilters()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var medianSetting = new GpxDataProcessingSetting(8, 0.88f, 2, 1);

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
    public async Task CacheDiscriminator_DiffersBetweenSettings_AndIsStableForTheSameSetting()
    {
        var seeded = await DiscriminatorOf(SeededSetting);
        var changed = await DiscriminatorOf(new GpxDataProcessingSetting(8, 0.3f, 6, 1));
        var seededAgain = await DiscriminatorOf(new GpxDataProcessingSetting(8, 0.88f, 6, 1));

        Assert.NotEqual(seeded, changed);
        Assert.Equal(seeded, seededAgain);
    }

    static async Task<string> DiscriminatorOf(GpxDataProcessingSetting setting)
    {
        return await new CleanupStage(new FakeAppSettingsService(setting)).CacheDiscriminatorAsync(default);
    }

    static async Task<List<double>> CleanupElevations(IAppSettingsService service, List<GpxPoint> points)
    {
        var context = new StageContext(new PipelineInput("test", points), new Dictionary<StageId, StageOutput>());
        var output = await new CleanupStage(service).RunAsync(context, new PipelineConfig(), default);

        return Elevations(((AnalyticData)output.Internal!));
    }

    static List<double> Elevations(AnalyticData data) => data.Points.Select(p => p.Ele).ToList();

    // Returns the setting when one is given, otherwise a failed result (the row is missing or unreadable).
    sealed class FakeAppSettingsService(GpxDataProcessingSetting? setting) : IAppSettingsService
    {
        public Task<Result<TSetting>> GetSettingAsync<TSetting>(AppSettingType settingType, CancellationToken ct)
            where TSetting : IAppSetting
        {
            if (setting is null)
            {
                return Task.FromResult(Result<TSetting>.Failure(Errors.NotFound("GpxDataProcessing setting")));
            }

            return Task.FromResult(Result<TSetting>.Success((TSetting)(object)setting));
        }

        public Task<Result<IList<AppSettingDto>>> GetAllAsync(CancellationToken ct) => throw new NotImplementedException();

        public Task<Result<AppSettingDto>> Update(int id, JsonDocument jsonSetting, CancellationToken ct) =>
            throw new NotImplementedException();

        public Task<Result<AppSetting>> SetSetting<TSetting>(TSetting value, CancellationToken ct)
            where TSetting : IAppSetting => throw new NotImplementedException();

        public Task<Result<AppSetting>> DeleteSettingAsync(int id, CancellationToken ct) => throw new NotImplementedException();
    }
}
