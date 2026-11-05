using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Commands;

internal class ProccesGpxDataCommand(AnalyticData data, GpxDataProcessingSetting? settings = null) : ICommand<AnalyticData>
{
    private readonly AnalyticData _data = data;
    private readonly GpxDataProcessingSetting? _settings = settings;

    public static ICommand<AnalyticData> Create(AnalyticData data, GpxDataProcessingSetting? settings = null)
    {
        return new ProccesGpxDataCommand(data, settings);
    }

    public static Result<AnalyticData> Run(AnalyticData data, GpxDataProcessingSetting? settings = null)
    {
        return new ProccesGpxDataCommand(data, settings).Execute();
    }

    public Result<AnalyticData> Execute()
    {
        var filters = _settings?.Filters ?? PipelineDefaults.CleanupFilters;
        var points = RouteVisualizationService.GpxFilterFactory.ApplyPointFilters(_data.Points, filters);
        var gains = points.ToGains();
        if (points == null || gains == null)
        {
            return CommandResult.Failure(Errors.Unknown("invalid data"));
        }
        AnalyticData result = new(points, gains);
        return Result<AnalyticData>.Success(result);
    }

    private static class CommandResult
    {

        public static Result<AnalyticData> Failure(ResultError err)
        {
            return Result<AnalyticData>.Failure(err);
        }
    }
}
