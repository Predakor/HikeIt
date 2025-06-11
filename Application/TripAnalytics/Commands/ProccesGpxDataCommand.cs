using Application.Commons.Interfaces;
using Domain.Common;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Commands;

public record AnalyticData(List<GpxPoint> Points, List<GpxGain> Gains);

internal class ProccesGpxDataCommand(TripAnalyticData data) : ICommand<AnalyticData> {
    readonly TripAnalyticData _data = data;

    public Result<AnalyticData> Execute() {
        var points = GpxDataBuilder.ProcessData(_data).Data;
        var gains = points.ToGains();
        if (points == null || gains == null) {
            return Result<AnalyticData>.Failure(Error.Unknown("invalid data"));
        }
        AnalyticData result = new(points, gains);
        return Result<AnalyticData>.Success(result);
    }

    public static Result<AnalyticData> Run(TripAnalyticData data) {
        return new ProccesGpxDataCommand(data).Execute();
    }
}
