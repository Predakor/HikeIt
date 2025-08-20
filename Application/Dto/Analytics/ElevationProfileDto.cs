using Application.TripAnalytics.Commands;
using Domain.Common;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips.ValueObjects;

namespace Application.Dto.Analytics;

public record GainDto(float Dist, float Ele, float Time);

public record ElevationProfileDto(GpxPoint Start, GainDto[] Gains);

public static class ElevationProfileExtentios {
    public static ElevationProfileDto ToDto(this ElevationProfile profile) {
        return DecodeGainsDataCommand
            .Create(profile.GainsData)
            .Execute()
            .Match(gains => new ElevationProfileDto(profile.Start, gains), error => null);
    }

    public static ElevationProfileDto ToElevationProfileDto(this AnalyticData data) {
        List<GpxGain> gains = data.Gains ?? data.Points.ToGains();
        var graphStart = data.Points[0];
        return new ElevationProfileDto(graphStart, [.. gains.ToGainDtos()]);
    }

    public static GainDto ToGainDto(this GpxGain g) {
        return new GainDto(g.DistanceDelta, g.ElevationDelta, g.TimeDelta ?? 0);
    }

    public static IEnumerable<GainDto> ToGainDtos(this IEnumerable<GpxGain> gains) {
        return gains.Select(g => g.ToGainDto());
    }
}
