using Application.Commons.Interfaces;
using Domain.Common;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Commands;

internal class ProccesGpxDataCommand(AnalyticData data) : ICommand<AnalyticData> {
    readonly AnalyticData _data = data;

    public static ICommand<AnalyticData> Create(AnalyticData data) {
        return new ProccesGpxDataCommand(data);
    }

    public static Result<AnalyticData> Run(AnalyticData data) {
        return new ProccesGpxDataCommand(data).Execute();
    }

    public Result<AnalyticData> Execute() {
        var points = GpxDataFactory.Create(_data).Points;
        var gains = points.ToGains();
        if (points == null || gains == null) {
            return CommandResult.Failure(Error.Unknown("invalid data"));
        }
        AnalyticData result = new(points, gains);
        return Result<AnalyticData>.Success(result);
    }

    static class CommandResult {
        public static Result<AnalyticData> Success<T>(AnalyticData res) {
            return Result<AnalyticData>.Success(res);
        }

        public static Result<AnalyticData> Failure(Error err) {
            return Result<AnalyticData>.Failure(err);
        }
    }
}
