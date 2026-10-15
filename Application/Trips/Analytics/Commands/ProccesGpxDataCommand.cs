using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.Config;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Application.Trips.Analytics.Commands;

internal class ProccesGpxDataCommand(AnalyticData data, IGpxDataProcessingSettings? settings = null) : ICommand<AnalyticData> {
    readonly AnalyticData _data = data;
    readonly IGpxDataProcessingSettings? _settings = settings;

    public static ICommand<AnalyticData> Create(AnalyticData data, IGpxDataProcessingSettings? settings = null) {
        return new ProccesGpxDataCommand(data, settings);
    }

    public static Result<AnalyticData> Run(AnalyticData data, IGpxDataProcessingSettings? settings = null) {
        return new ProccesGpxDataCommand(data, settings).Execute();
    }

    public Result<AnalyticData> Execute() {
        var points = GpxDataFactory.Create(_data, _settings).Points;
        var gains = points.ToGains();
        if (points == null || gains == null) {
            return CommandResult.Failure(Errors.Unknown("invalid data"));
        }
        AnalyticData result = new(points, gains);
        return Result<AnalyticData>.Success(result);
    }

    static class CommandResult {
        public static Result<AnalyticData> Success<T>(AnalyticData res) {
            return Result<AnalyticData>.Success(res);
        }

        public static Result<AnalyticData> Failure(ResultError err) {
            return Result<AnalyticData>.Failure(err);
        }
    }
}
